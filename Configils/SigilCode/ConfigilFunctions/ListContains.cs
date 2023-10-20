using NCalc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JLPlugin.ConfigilFunctions
{
    internal static class ListContains
    {
        internal static void Evaluate(FunctionArgs functionArgs)
        {
            List<object> parameters = functionArgs.Parameters.Select(x => x.Evaluate()).ToList();

            if (parameters.Count != 2)
            {
                throw new FormatException($"ListContains() requires 2 parameters.");
            }

            if (parameters[0] == null || parameters[1] == null)
            {
                functionArgs.Result = (object)null;
                return;
            }

            List<object> list = ((IList)parameters[0]).Cast<object>().ToList();
            object obj = parameters[1];
            functionArgs.Result = list.Contains(obj);
        }
    }
}
