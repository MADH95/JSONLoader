using DiskCardGame;
using System.Collections;
using UnityEngine;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class transformCards
    {
        public string runOnCondition;
        public slotData slot;
        public card card;
        public string self;
        public string noRetainDamage;

        public static IEnumerator TransformCards(AbilityBehaviourData abilitydata)
        {
            yield return new WaitForSeconds(0.3f);
            if (Singleton<ViewManager>.Instance.CurrentView != View.Board)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.3f);
            }

            foreach (transformCards transformCardsInfo in abilitydata.transformCards)
            {
                if (SigilData.ConvertArgument(transformCardsInfo.runOnCondition, abilitydata) == "false")
                {
                    continue;
                }

                CardSlot slot = slotData.GetSlot(transformCardsInfo.slot, abilitydata);
                PlayableCard card = null;
                if (slot != null)
                {
                    if (slot.Card != null)
                    {
                        card = slot.Card;
                    }
                }
                else if (transformCardsInfo.self != null)
                {
                    card = abilitydata.self;
                }
                if (card != null)
                {
                    CardInfo cardinfo = Data.card.getCard(transformCardsInfo.card, abilitydata);

                    yield return card.TransformIntoCard(cardinfo);
                    if (SigilData.ConvertArgument(transformCardsInfo.noRetainDamage, abilitydata) == "true")
                    {
                        card.HealDamage(card.Status.damageTaken);
                    }
                }
            }

            yield return new WaitForSeconds(0.3f);
            yield break;
        }
    }
}