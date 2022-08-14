
using System;
using System.Linq;
using System.Text.RegularExpressions;
using static JLPlugin.Interpreter;

namespace JLPlugin
{
    static class ConfigilFunctions
    {


        public static string RemoveApostrophes(string text)
        {
            if (Regex.Matches(text, RegexStrings.WithoutApostrophes) is var TextWithout
               && TextWithout.Cast<Match>().Any(TextWithout => TextWithout.Success))
            {
                return TextWithout[0].Groups[1].Value;
            }
            return text;
        }

        public static string StringContains(string functionContents)
        {
            if (!functionContents.Contains(","))
            {
                throw new Exception("StringContains: Too few function parameters");
            }

            var parameters = functionContents.Split(',');

            if (parameters.Length > 2)
            {
                throw new Exception("StringContains: Too many function parameters");
            }

            if (parameters.Any(elem => string.IsNullOrEmpty(elem)))
            {
                throw new Exception("StringContains: Invalid parameters");
            }

            //you forgot to convert this to lowercase
            return parameters[0].Contains(parameters[1]).ToString().ToLower();
        }

        public static string Random(string functionContents)
        {
            var parameters = functionContents.Split(',');

            Random random = new();

            return RemoveApostrophes(parameters[random.Next(parameters.Length)]);
        }

        public static string CardInSlot(string functionContents)
        {
            if (int.TryParse(functionContents, out int slotIndex))
            {
                throw new Exception($"CardInSlot: {functionContents} is an invalid slot index");
            }

            //TODO: figure the rest of this out later

            throw new Exception("CardInSlot: function is incomplete");
        }

        public static string GetListIndex(string functionContents)
        {
            var parameters = functionContents.Split(',').ToList();

            if (parameters.Count < 1)
            {
                throw new Exception("GetListIndex: Too few function parameters");
            }

            var list = parameters.Skip(1).ToList();
            return RemoveApostrophes(list[int.Parse(parameters[0])]);
        }
    }
}
