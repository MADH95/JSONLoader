
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace JLPlugin
{
    using Data;
    using PanoramicData.NCalcExtensions;
    using System.Collections.Generic;
    using System.Reflection;

    static class Interpreter
    {
        public static class RegexStrings
        {
            // Detects functions in the format name(params)
            // This regex is not needed anymore but i just left it in because it's super cool lol
            public static string Function = @"([a-zA-Z]+)(?<!if|in)\(((?>\((?<c>)|[^()]+|\)(?<-c>))*(?(c)(?!)))\)";

            // Detects a variable in the format [variableName]
            public static string Variable = @"\[(.*?)\]";

            // Detects a generated variable in the format [variableName.memberName]
            public static string GeneratedVariable = @"\[([^]]*?\.[^[]*?)\]";

            //Detects an Expression in the format (1 + 4)
            public static string Expression = @"^\((.*?)\)$";
        }

        public static Random random = new Random();


        public static string Process(in string input, AbilityBehaviourData abilityData, bool sendDebug = true)
        {
            string output = input;

            if (Regex.Match(output, RegexStrings.Expression) is var expression && expression.Success)
            {
                //NoCache means the logs don't get spammed but it could cause some performance loss
                ExtendedExpression e = new ExtendedExpression(expression.Groups[1].Value);
                e.EvaluateFunction += ConfigilExtensions.Extend;

                foreach (KeyValuePair<string, string> variable in abilityData.variables)
                {
                    e.Parameters[variable.Key] = variable.Value;
                }

                foreach (KeyValuePair<string, object> generatedVariable in abilityData.generatedVariables)
                {
                    e.Parameters[generatedVariable.Key] = generatedVariable.Value;
                }

                if (Regex.Matches(output, RegexStrings.GeneratedVariable) is var variables
                && variables.Cast<Match>().Any(variables => variables.Success))
                {
                    foreach (Match variable in variables)
                    {
                        string contents = variable.Groups[1].Value;
                        e.Parameters[contents] = ProcessGeneratedVariable(contents, abilityData);
                    }
                }

                //this should stay as it's very useful for debugging for people
                if (sendDebug)
                {
                    Plugin.Log.LogInfo($"input: {output}");
                }

                output = e.Evaluate().ToString();

                if (output == "True" || output == "False")
                {
                    output = output.ToLower();
                }

                if (sendDebug)
                {
                    Plugin.Log.LogInfo($"output: {output}");
                }
            }

            return output;
        }

        public static object ProcessGeneratedVariable(string contents, AbilityBehaviourData abilityData)
        {
            var fieldList = contents.Split('.').ToList();

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
                    FieldInfo field = obj.GetType().GetField(fieldList[i]);

                    if (field is null)
                    {
                        return null;
                        //throw new Exception($"{ fieldList[i] } is an invalid field/property of { string.Join(".", fieldList.Where(x => fieldList.IndexOf(x) < i)) }");
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
