using DiskCardGame;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using static JLPlugin.Interpreter;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class transformCards
    {
        public string runOnCondition;
        public slotData slot;
        public string targetCard;
        public card card;
        public string noRetainDamage;

        public static IEnumerator TransformCards(AbilityBehaviourData abilitydata)
        {
            foreach (transformCards transformCardsInfo in abilitydata.transformCards)
            {
                if (SigilData.ConvertArgument(transformCardsInfo.runOnCondition, abilitydata) == "false")
                {
                    continue;
                }

                PlayableCard CardToReplace = null;
                if (transformCardsInfo.slot != null)
                {
                    CardSlot slot = slotData.GetSlot(transformCardsInfo.slot, abilitydata);
                    if (slot != null)
                    {
                        if (slot.Card != null)
                        {
                            CardToReplace = slot.Card;
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(transformCardsInfo.targetCard))
                    {
                        CardToReplace = (PlayableCard)SigilData.ConvertArgumentToType(transformCardsInfo.targetCard, abilitydata, typeof(PlayableCard));
                    }
                    else
                    {
                        CardToReplace = abilitydata.self;
                    }
                }

                if (CardToReplace != null)
                {
                    bool CardIsInHand = Singleton<PlayerHand>.Instance.CardsInHand.Contains(CardToReplace);
                    Singleton<ViewManager>.Instance.SwitchToView(CardIsInHand ? View.Hand : View.Board, false, false);

                    CardInfo cardinfo = Data.card.getCard(transformCardsInfo.card, abilitydata);

                    yield return CardToReplace.TransformIntoCard(cardinfo);
                    if (SigilData.ConvertArgument(transformCardsInfo.noRetainDamage, abilitydata) == "true")
                    {
                        CardToReplace.HealDamage(CardToReplace.Status.damageTaken);
                    }
                }
            }

            // yield return new WaitForSeconds(0.3f);
            yield break;
        }
    }
}
