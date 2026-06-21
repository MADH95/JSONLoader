using DiskCardGame;
using NCalc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JLPlugin.ConfigilFunctions
{
    internal static class GetSlot
    {
        internal static void Evaluate(FunctionArgs functionArgs)
        {
            List<object> parameters = functionArgs.Parameters.Select(x => x.Evaluate()).ToList();

            if (parameters.Count < 1 || parameters.Count > 3)
            {
                throw new FormatException($"GetSlot() requires between 1 and 3 parameters.");
            }

            if (parameters[0] == null)
            {
                functionArgs.Result = (object)null;
                return;
            }

            int index = (int)parameters[0];
            if (parameters.Count == 1)
            {
                functionArgs.Result = Singleton<BoardManager>.Instance.playerSlots.ElementAtOrDefault(index);
                return;
            }

            bool isOpponentSlot = (bool)parameters[1];
            CardSlot slot = (isOpponentSlot) ? Singleton<BoardManager>.Instance.opponentSlots.ElementAtOrDefault(index) : Singleton<BoardManager>.Instance.playerSlots.ElementAtOrDefault(index);
            if (parameters.Count == 2)
            {
                functionArgs.Result = slot;
                return;
            }

            string fieldlist = $"slot.{(string)parameters[2]}";
            if (parameters.Count == 3)
            {
                functionArgs.Result = Interpreter.ProcessGeneratedVariable(fieldlist, null, slot);
            }
        }
    }
}
