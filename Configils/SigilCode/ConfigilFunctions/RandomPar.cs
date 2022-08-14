using NCalc;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace JLPlugin.ConfigilFunctions
{
    internal static class RandomPar
    {
        internal static void Evaluate(FunctionArgs functionArgs)
        {
            List<object> parameters = functionArgs.Parameters.Select(x => x.Evaluate()).ToList();
            Random random = new Random();
            functionArgs.Result = parameters[random.Next(parameters.Count)];
        }
    }
}
