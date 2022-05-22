using DiskCardGame;
using System.Collections;
using UnityEngine;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class moveCards
    {
        public string runOnCondition;
        public slotData moveFromSlot;
        public slotData moveToSlot;
        public string replace;
        public string self;

        public static IEnumerator MoveCards(AbilityBehaviourData abilitydata)
        {
            yield return new WaitForSeconds(0.3f);
            if (Singleton<ViewManager>.Instance.CurrentView != View.Board)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.3f);
            }

            foreach (moveCards movecardinfo in abilitydata.moveCards)
            {
                if (SigilData.ConvertArgument(movecardinfo.runOnCondition, abilitydata) == "false")
                {
                    continue;
                }

                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                CardSlot slotFrom = slotData.GetSlot(movecardinfo.moveFromSlot, abilitydata);
                CardSlot slotTo = slotData.GetSlot(movecardinfo.moveToSlot, abilitydata);
                if (slotFrom != null && slotTo != null)
                {
                    if (slotTo.Card != null && SigilData.ConvertArgument(movecardinfo.replace, abilitydata) == "true")
                    {
                        slotTo.Card.ExitBoard(0, new Vector3(0, 0, 0));
                    }
                    if (slotTo.Card == null)
                    {
                        CardInfo cardinfo = slotFrom.Card.Info;
                        if (cardinfo != null)
                        {
                            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(cardinfo, slotTo);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.3f);
            if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            }
            yield break;
        }
    }
}