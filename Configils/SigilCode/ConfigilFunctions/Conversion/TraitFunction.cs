using DiskCardGame;
using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using static JLPlugin.V2.Data.CardSerializeInfo;

namespace JLPlugin.ConfigilFunctions
{
    internal static class TraitFunction
    {
        internal static void Evaluate(FunctionArgs functionArgs)
        {
            List<object> parameters = functionArgs.Parameters.Select(x => x.Evaluate()).ToList();

            if (parameters.Count != 1)
            {
                throw new FormatException($"Trait() requires 1 parameter.");
            }

            if (parameters[0] == null)
            {
                functionArgs.Result = (object)null;
                return;
            }

            string traitName = (string)parameters[0];
            functionArgs.Result = ImportExportUtils.ParseEnum<Trait>(traitName);
        }
    }
}
