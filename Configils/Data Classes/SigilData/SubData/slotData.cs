using DiskCardGame;
using System;
using System.Collections.Generic;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class slotData
    {
        public string randomSlotOnCondition;
        public string index;
        public string isOpponentSlot;

        public static CardSlot GetSlot(slotData slotdata, AbilityBehaviourData abilitydata, bool sendDebug = true)
        {
            if (slotdata == null) return null;
            if (string.IsNullOrWhiteSpace(slotdata.index)) return null;

            if (!string.IsNullOrWhiteSpace(slotdata.randomSlotOnCondition))
            {
                var random = new Random();

                List<CardSlot> SlotsWithCondition = new List<CardSlot>();
                foreach (CardSlot slot in Singleton<BoardManager>.Instance.AllSlots)
                {
                    abilitydata.generatedVariables["RandomSlot"] = slot;
                    if (SigilData.ConvertArgument(slotdata.randomSlotOnCondition, abilitydata, sendDebug) == "true")
                    {
                        SlotsWithCondition.Add(slot);
                    }
                }
                if (SlotsWithCondition.Count == 0)
                {
                    return null;
                }
                return SlotsWithCondition[random.Next(SlotsWithCondition.Count)];
            }

            return ConvertIntToSlot(slotdata, abilitydata, int.Parse(SigilData.ConvertArgument(slotdata.index, abilitydata, sendDebug)), sendDebug);
        }

        public static CardSlot ConvertIntToSlot(slotData slotdata, AbilityBehaviourData abilitydata, int index, bool sendDebug = true)
        {
            if (index < 0 || index >= Singleton<BoardManager>.Instance.PlayerSlotsCopy.Count)
            {
                return null;
            }

            CardSlot slot = Singleton<BoardManager>.Instance.playerSlots[index];
            if (!string.IsNullOrWhiteSpace(slotdata.isOpponentSlot))
            {
                if (SigilData.ConvertArgument(slotdata.isOpponentSlot, abilitydata, sendDebug) == "true")
                {
                    slot = Singleton<BoardManager>.Instance.opponentSlots[index];
                }
            }
            return slot;
        }
    }
}