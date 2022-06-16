using DiskCardGame;
using System.Collections;
using UnityEngine;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class placeCards
    {
        public string runOnCondition;
        public slotData slot;
        public card card;
        public string replace;

        public static IEnumerator PlaceCards(AbilityBehaviourData abilitydata)
        {
            yield return new WaitForSeconds(0.3f);
            if (Singleton<ViewManager>.Instance.CurrentView != View.Board)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.3f);
            }

            foreach (placeCards placecardinfo in abilitydata.placeCards)
            {
                if (SigilData.ConvertArgument(placecardinfo.runOnCondition, abilitydata) == "false")
                {
                    continue;
                }

                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                bool replace = SigilData.ConvertArgument(placecardinfo.replace, abilitydata) == "true";
                CardSlot slot;
                if (replace)
                {
                    slot = slotData.GetSlot(placecardinfo.slot, abilitydata);
                }
                else
                {
                    slot = slotData.GetSlot(placecardinfo.slot, abilitydata, false);
                }
                Plugin.Log.LogInfo("slot is null?: " + (slot == null).ToString());
                if (slot != null)
                {
                    Plugin.Log.LogInfo("test: " + (slot.Card != null).ToString() + " " + (SigilData.ConvertArgument(placecardinfo.replace, abilitydata) == "true").ToString());
                    if (slot.Card != null && replace)
                    {
                        slot.Card.ExitBoard(0, new Vector3(0, 0, 0));
                    }
                    if (slot.Card == null || slot.Card.Dead)
                    {
                        CardInfo cardinfo = card.getCard(placecardinfo.card, abilitydata);
                        if (cardinfo != null)
                        {
                            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardinfo, slot, 0.15f);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.3f);
            yield break;
        }
    }
}