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

        public static CardSlot GetSlot(slotData slotdata, AbilityBehaviourData abilitydata)
        {
            if (slotdata == null)
            {
                return null;
            }

            if (slotdata.randomSlotOnCondition != null)
            {
                var random = new Random();

                List<CardSlot> SlotsWithCondition = new List<CardSlot>();
                foreach (CardSlot slot in Singleton<BoardManager>.Instance.AllSlots)
                {
                    abilitydata.generatedVariables["RandomSlot"] = slot;
                    if (SigilData.ConvertArgument(slotdata.randomSlotOnCondition, abilitydata) == "true")
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

            return ConvertIntToSlot(slotdata, abilitydata, int.Parse(SigilData.ConvertArgument(slotdata.index, abilitydata)));
        }

        public static CardSlot ConvertIntToSlot(slotData slotdata, AbilityBehaviourData abilitydata, int index)
        {
            if (index < 0 || index >= Singleton<BoardManager>.Instance.PlayerSlotsCopy.Count)
            {
                return null;
            }

            CardSlot slot = Singleton<BoardManager>.Instance.playerSlots[index];
            if (slotdata.isOpponentSlot != null)
            {
                if (SigilData.ConvertArgument(slotdata.isOpponentSlot, abilitydata) == "true")
                {
                    slot = Singleton<BoardManager>.Instance.opponentSlots[index];
                }
            }
            return slot;
        }
    }
}