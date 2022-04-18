using DiskCardGame;
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class slotData
    {
        public string index;
        public string isOpponentSlot;

        public static CardSlot GetSlot(slotData slotdata, SigilData abilitydata)
        {
            if (slotdata.index.Contains('|'))
            {
                List<CardSlot> randomiseableslots = new List<CardSlot>();
                foreach (int index in slotdata.index.Split('|').ToList().Select(int.Parse).ToList())
                {
                    CardSlot slot = ConvertIntToSlot(slotdata, abilitydata, index);
                    if (slot != null)
                    {
                        randomiseableslots.Add(ConvertIntToSlot(slotdata, abilitydata, index));
                    }
                }
                if (randomiseableslots.Count > 0)
                {
                    Random random = new Random();
                    return randomiseableslots[random.Next(randomiseableslots.Count)];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                CardSlot slot = ConvertIntToSlot(slotdata, abilitydata, int.Parse(SigilData.ConvertArgument(slotdata.index, abilitydata)));
                return slot;
            }
        }

        public static CardSlot ConvertIntToSlot(slotData slotdata, SigilData abilitydata, int index)
        {
            if (index < 0 || index > 3)
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