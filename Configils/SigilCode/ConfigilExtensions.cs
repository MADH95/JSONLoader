using JLPlugin.ConfigilFunctions;
using NCalc;
using System;

namespace JLPlugin
{
    public static class ConfigilExtensions
    {
        public static void Extend(string functionName, FunctionArgs functionArgs)
        {
            if (functionArgs == null)
            {
                throw new ArgumentNullException(nameof(functionArgs));
            }

            switch (functionName)
            {
                case "Random":
                    RandomPar.Evaluate(functionArgs);
                    return;
                case "GetSlot":
                    GetSlot.Evaluate(functionArgs);
                    return;
                case "ListContains":
                    ListContains.Evaluate(functionArgs);
                    return;
                case "Ability":
                    AbilityFunction.Evaluate(functionArgs);
                    return;
                case "Tribe":
                    TribeFunction.Evaluate(functionArgs);
                    return;
                case "Trait":
                    TraitFunction.Evaluate(functionArgs);
                    return;
                case "SpecialAbility":
                    SpecialAbilityFunction.Evaluate(functionArgs);
                    return;
                case "HasAbility":
                    HasAbilityFunction.Evaluate(functionArgs);
                    return;
                case "HasTribe":
                    HasTribeFunction.Evaluate(functionArgs);
                    return;
                case "HasTrait":
                    HasTraitFunction.Evaluate(functionArgs);
                    return;
                case "HasSpecialAbility":
                    HasSpecialAbilityFunction.Evaluate(functionArgs);
                    return;
            }
        }

        /*
        [HarmonyPrefix, HarmonyPatch(typeof(Lambda), "Evaluate", typeof(string), typeof(Type))]
        public static bool Evaluate(Object value, ref Object __result, ref Lambda __instance)
        {
            string predicate = (string)AccessTools.Field(typeof(Lambda), "predicate").GetValue(__instance);
            string nCalcString = (string)AccessTools.Field(typeof(Lambda), "nCalcString").GetValue(__instance);
            Dictionary<string, object?> parameters = (Dictionary<string, object?>)AccessTools.Field(typeof(Lambda), "parameters").GetValue(__instance);

            parameters.Remove(predicate);
            parameters.Add(predicate, value!);

            //i need a way to get AbilityData here but i have no idea how i would do that
            Interpreter.Process();

            var ncalc = new ExtendedExpression(nCalcString)
            {
                Parameters = parameters
            };

            __result = ncalc.Evaluate();
            return false;
        }
        */
    }
}
