
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
            public static string Function = @"([a-zA-Z]+)(?<!if|in)(\(((?>\((?<c>)|[^()]+|\)(?<-c>))*(?(c)(?!)))\))";

            // Detects a variable in the format [variableName]
            public static string Variable = @"\[((?>\[(?<c>)|[^\[\]]+|\](?<-c>))*(?(c)(?!)))\]";

            // Detects a generated variable in the format [variableName.memberName]
            public static string GeneratedVariable = @"\[([^]]*?\.[^[]*?)\]";

            //Detects an Expression in the format (1 + 4)
            public static string Expression = @"\(((?>\((?<c>)|[^()]+|\)(?<-c>))*(?(c)(?!)))\)";
        }

        public static Random random = new Random();


        public static string Process(in string input, AbilityBehaviourData abilityData, bool sendDebug = true)
        {
            string output = input;

            if (Regex.Matches(output, RegexStrings.Expression) is var expressions
            && expressions.Cast<Match>().Any(expressions => expressions.Success))
            {
                foreach (Match expression in expressions)
                {
                    string CalcInput = expression.Groups[0].Value;
                    string CalcContent = expression.Groups[1].Value;

                    //Quick fix for the PlayerSlot and OpponentSlot variables, would be best to change it to actual
                    //functions later if possible
                    if (Regex.Matches(CalcContent, RegexStrings.Function) is var ParContents
                            && ParContents.Cast<Match>().Any(ParContents => ParContents.Success))
                    {
                        foreach (Match ParContent in ParContents)
                        {
                            if (ParContent.Groups[1].Value != "PlayerSlot" && ParContent.Groups[1].Value != "OpponentSlot")
                            {
                                continue;
                            }
                            CalcContent = CalcContent.Replace(ParContent.Groups[3].Value, SigilData.ConvertArgument(ParContent.Groups[2].Value, abilityData, sendDebug));
                        }
                    }

                    //NoCache means the logs don't get spammed but it could cause some performance loss
                    ExtendedExpression e = new ExtendedExpression(CalcContent);
                    e.EvaluateFunction += ConfigilExtensions.Extend;

                    foreach (KeyValuePair<string, string> variable in abilityData.variables)
                    {
                        e.Parameters[variable.Key] = variable.Value;
                    }

                    foreach (KeyValuePair<string, object> generatedVariable in abilityData.generatedVariables)
                    {
                        e.Parameters[generatedVariable.Key] = generatedVariable.Value;
                    }

                    if (Regex.Matches(CalcContent, RegexStrings.GeneratedVariable) is var variables
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
                        Plugin.Log.LogInfo($"input: {CalcInput}");
                    }

                    string CalcOutput = e.Evaluate().ToString();
                    output = output.Replace(CalcInput, CalcOutput);

                    if (output == "True" || output == "False")
                    {
                        output = output.ToLower();
                    }

                    if (sendDebug)
                    {
                        Plugin.Log.LogInfo($"output: {CalcOutput}");
                    }
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
                PropertyInfo property = obj?.GetType().GetProperty(fieldList[i]);

                if (property is null)
                {
                    FieldInfo field = obj?.GetType().GetField(fieldList[i]);

                    if (field is null)
                    {
                        return (object)null;
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
