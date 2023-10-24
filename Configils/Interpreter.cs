
using System;
using System.Linq;
using System.Text.RegularExpressions;
using MonoMod.Utils;

namespace JLPlugin
{
    using Data;
    using DiskCardGame;
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


        public static object Process(in string input, AbilityBehaviourData abilityData, Type type = null, bool sendDebug = true, Dictionary<string, object?> additionalParameters = null)
        {
            object output = input;

            if (Regex.Matches(input, RegexStrings.Expression) is var expressions
            && expressions.Cast<Match>().Any(expressions => expressions.Success))
            {
                foreach (Match expression in expressions)
                {
                    string CalcInput = expression.Groups[0].Value;
                    string CalcContent = expression.Groups[1].Value;

                    ExtendedExpression e = new ExtendedExpression(CalcContent);
                    e.EvaluateFunction += ConfigilExtensions.Extend;

                    if (additionalParameters != null)
                    {
                        e.Parameters.AddRange(additionalParameters);
                    }

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
                        Plugin.Log.LogDebug($"input: {CalcInput}");
                    }

                    object CalcOutput = e.Evaluate();

                    //by default it will handle it as a string
                    if (type == null || expressions.Count > 1)
                    {
                        if (CalcOutput.GetType() == typeof(bool))
                        {
                            CalcOutput = CalcOutput.ToString().ToLower();
                        }
                        //this looks very messy but casting here unfortunately causes an error
                        output = output.ToString().Replace(CalcInput, CalcOutput.ToString());
                    }
                    else
                    {
                        output = CalcOutput;
                    }

                    if (sendDebug)
                    {
                        Plugin.Log.LogDebug($"output: {CalcOutput}");
                    }
                }
            }
            return output;
        }

        public static object ProcessGeneratedVariable(string contents, AbilityBehaviourData abilityData = null, object variable = null)
        {
            var fieldList = contents.Split('.').ToList();

            object obj = variable;

            if (variable == null && abilityData != null)
            {
                bool validGeneratedVariable = abilityData.generatedVariables.TryGetValue(fieldList[0], out obj);

                if (!validGeneratedVariable)
                {
                    throw new Exception($"{ fieldList[0] } is an invalid generated variable");
                }
            }

            for (int i = 1; i < fieldList.Count; ++i)
            {
                if (obj.GetType() == typeof(PlayableCard))
                {
                    if (fieldList[i] == "TemporaryAbilities")
                    {
                        List<Ability> abilities = new List<Ability>();
                        foreach (List<Ability> abilityList in ((PlayableCard)obj).TemporaryMods.Select(x => x.abilities).ToList())
                        {
                            abilities.AddRange(abilityList);
                        }
                        obj = abilities;
                        break;
                    }
                    if (fieldList[i] == "AllAbilities")
                    {
                        List<Ability> abilities = new List<Ability>();
                        foreach (List<Ability> abilityList in ((PlayableCard)obj).TemporaryMods.Select(x => x.abilities).ToList())
                        {
                            abilities.AddRange(abilityList);
                        }
                        abilities.AddRange(((PlayableCard)obj).Info.Abilities);
                        obj = abilities;
                        break;
                    }
                }

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
