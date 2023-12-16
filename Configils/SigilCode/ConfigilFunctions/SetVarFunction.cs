using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JLPlugin.Data
{
    internal static class SetVarFunction
    {
        internal static void Evaluate(FunctionArgs functionArgs, AbilityBehaviourData abilitydata)
        {
            List<object> parameters = functionArgs.Parameters.Select(x => x.Evaluate()).ToList();

            if (parameters.Count != 2)
            {
                throw new FormatException($"SetVar() requires 2 parameters.");
            }

            abilitydata.generatedVariables[(string)parameters[0]] = parameters[1];
            functionArgs.Result = true;
        }
    }
}
