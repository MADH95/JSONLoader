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
            }
        }
    }
}
