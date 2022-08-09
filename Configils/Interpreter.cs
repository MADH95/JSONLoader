
using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;

namespace JLPlugin
{
    using Data;
    using DiskCardGame;
    using NCalc;
    using System.Collections.Generic;
    using System.Reflection;

    static class Interpreter
    {
        public static class RegexStrings
        {
            // Detects functions in the format name(params)
            public static string Function = @"([a-zA-Z]+)(?<!if|in)\(((?>\((?<c>)|[^()]+|\)(?<-c>))*(?(c)(?!)))\)";

            // Detects a variable in the format [variableName] or [variableName.memberName]
            public static string Variable = @"\[.*?\]";

            //Detects an Expression in the format (1 + 4)
            public static string Expression = @"^\((.*?)\)$";

            // Detects all whitespace in a string
            public static string Whitespace = @"\s+";

            // Detects text in the format 'text'
            public static string WithoutApostrophes = @"'(.*?)'";
        }


        public static string Process(in string input, AbilityBehaviourData abilityData, bool sendDebug = true)
        {
            string output = input;

            if (Regex.Matches(output, RegexStrings.Variable) is var variables
                && variables.Cast<Match>().Any(variables => variables.Success))
            {
                foreach (Match variable in variables)
                {
                    output = output.Replace(variable.Value, ProcessVariable(variable, abilityData));
                }
            }

            if (Regex.Match(output, RegexStrings.Function) is var function && function.Success)
            {
                output = output.Replace(function.Value, ProcessFunction(function, abilityData));
            }

            //this should stay as it's very useful for debugging for people
            if (sendDebug)
            {
                Plugin.Log.LogInfo($"output before NCalc: {output}");
            }

            if (Regex.Match(output, RegexStrings.Expression) is var expression && expression.Success)
            {
                Expression e = new(expression.Groups[0].Value);
                output = e.Evaluate().ToString();
            }

            if (output == "True" || output == "False")
            {
                output = output.ToLower();
            }

            return output;
        }

        private static string ProcessFunction(Match function, AbilityBehaviourData abilityData)
        {
            string fullFunction = function.Groups[0].Value;
            string functionName = function.Groups[1].Value;

            //I readded the auto process of the function contents
            //as it's now compatible due to the new regex
            string functionContents = Regex.Replace(SigilData.ConvertArgument(function.Groups[2].Value, abilityData), RegexStrings.Whitespace, string.Empty);

            switch (functionName)
            {
                case "StringContains":
                    {
                        return ConfigilFunctions.StringContains(functionContents);
                    }
                case "Random":
                    {
                        return ConfigilFunctions.Random(functionContents);
                    }
                case "CardInSlot":
                    {
                        return ConfigilFunctions.CardInSlot(functionContents);
                    }
                case "GetListIndex":
                    {
                        return ConfigilFunctions.GetListIndex(functionContents);
                    }
                default:
                    {
                        throw new Exception($"{functionName} - is an invalid function name");
                    }

                    //Suggested functions

                    //GetSlot( Player/Opponent, Index) might be easier to get slot in real time rather than updating a list every frame, would only get the required slots
            }
        }

        public static string ProcessVariable(Match variable, AbilityBehaviourData abilityData)
        {
            if (!variable.Value.Contains('.'))
            {
                bool validVariable = abilityData.variables.TryGetValue(variable.Value, out string value);

                if (!validVariable)
                {
                    throw new Exception($"{ variable.Value } is an invalid variable");
                }

                return value;
            }

            object obj = GeneratedVarToObj(variable, abilityData);

            string output = obj.ToString();

            //for some reason it thinks a string is an IEnumerable
            if (obj is List<Ability> AbilityList)
            {
                output = $"'{ string.Join("', '", AbilityList.Select(x => AbilitiesUtil.GetInfo(x).rulebookName)) }'";
            }
            else if (obj is IEnumerable collection && obj is not string)
            {
                List<string> list = new List<string>();
                foreach (object item in collection)
                {
                    list.Add(item.ToString());
                }

                if (list.Count == 0)
                {
                    output = "";
                }
                else
                {
                    output = $"'{ string.Join("', '", list) }'";
                }
            }

            //bool conversion to allow for compatibility with NCalc
            if (output == "True" || output == "False")
            {
                output = output.ToLower();
            }

            return output;
        }

        public static object GeneratedVarToObj(Match variable, AbilityBehaviourData abilityData)
        {
            var fieldList = variable.Value.Trim('[', ']').Split('.').ToList();

            object obj;

            bool validGeneratedVariable = abilityData.generatedVariables.TryGetValue(fieldList[0], out obj);

            if (!validGeneratedVariable)
            {
                throw new Exception($"{ fieldList[0] } is an invalid generated variable");
            }

            for (int i = 1; i < fieldList.Count; ++i)
            {
                PropertyInfo property = obj.GetType().GetProperty(fieldList[i]);

                if (property is null)
                {
                    var field = obj.GetType().GetField(fieldList[i]);

                    if (field is null)
                    {
                        throw new Exception($"{ fieldList[i] } is an invalid field/property of { string.Join(".", fieldList.Where(x => fieldList.IndexOf(x) < i)) }");
                    }

                    obj = field.GetValue(obj);
                    continue;
                }

                if (property.GetIndexParameters().Length < 1)
                {
                    obj = property.GetValue(obj);
                    continue;
                }

                //If we decide to do index lookup it will be handled here.
                //Convert the fieldList[i+1] value to an integer and call GetValue(obj, new(){ convertedInteger } )
                //***I THINK***

                break;
            }
            return obj;
        }
    }
}
