using DiskCardGame;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class slotData
    {
        public string index;
        public string isOpponentSlot;

        public static CardSlot GetSlot(slotData slotdata, AbilityBehaviourData abilitydata)
        {
            if (slotdata == null)
            {
                return null;
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