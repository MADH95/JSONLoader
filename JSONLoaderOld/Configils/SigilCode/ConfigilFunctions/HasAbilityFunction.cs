using DiskCardGame;
using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JLPlugin.ConfigilFunctions
{
    internal static class HasAbilityFunction
    {
        internal static void Evaluate(FunctionArgs functionArgs)
        {
            List<object> parameters = functionArgs.Parameters.Select(x => x.Evaluate()).ToList();

            if (parameters.Count != 2)
            {
                throw new FormatException($"HasAbility() requires 2 parameters.");
            }

            if (parameters[0] == null || parameters[1] == null)
            {
                functionArgs.Result = (object)null;
                return;
            }

            PlayableCard card = (PlayableCard)parameters[0];
            Ability ability = (Ability)parameters[1];
            functionArgs.Result = card.HasAbility(ability);
        }
    }
}
