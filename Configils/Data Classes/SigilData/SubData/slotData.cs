using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class slotData
    {
        public string index;
        public string isOpponentSlot;

        public static CardSlot GetSlot(slotData slotdata, AbilityBehaviourData abilitydata, bool? slotShouldContainCard = null)
        {
            if (slotdata == null)
            {
                return null;
            }

            if (slotdata.index.Contains("|") && slotShouldContainCard != null)
            {
                //regex instead of splitting so it does not mistake the or operator (||) for randomization
                var random = new Random();
                MatchCollection randomMatchList = Regex.Matches(slotdata.index, @"(?:(?:\((?>[^()]+|\((?<number>)|\)(?<-number>))*(?(number)(?!))\))|[^|])+");
                List<string> StringList = randomMatchList.Cast<Match>().Select(match => match.Value).ToList();

                List<CardSlot> SlotList = StringList.Select(x => ConvertIntToSlot(slotdata, abilitydata, int.Parse(SigilData.ConvertArgument(x, abilitydata)))).ToList();

                if (slotShouldContainCard == true)
                {
                    SlotList = SlotList.Where(x => x.Card != null).ToList();
                }
                else
                {
                    SlotList = SlotList.Where(x => x.Card == null).ToList();
                }
                return SlotList[random.Next(SlotList.Count)];
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