using NCalc;
using System.Collections.Generic;
using System.Linq;

namespace JLPlugin.ConfigilFunctions
{
    internal static class RandomPar
    {
        internal static void Evaluate(FunctionArgs functionArgs)
        {
            List<object> parameters = functionArgs.Parameters.Select(x => x.Evaluate()).ToList();
            functionArgs.Result = parameters[Interpreter.random.Next(parameters.Count)];
        }
    }
}
