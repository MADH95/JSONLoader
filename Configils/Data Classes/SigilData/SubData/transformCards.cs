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

                PlayableCard card = null;
                if (transformCardsInfo.slot != null)
                {
                    CardSlot slot = slotData.GetSlot(transformCardsInfo.slot, abilitydata, true);
                    if (slot != null)
                    {
                        if (slot.Card != null)
                        {
                            card = slot.Card;
                        }
                    }
                }
                else
                {
                    if (transformCardsInfo.targetCard != null)
                    {
                        if (Regex.Matches(transformCardsInfo.targetCard, RegexStrings.Variable) is var variables
                        && variables.Cast<Match>().Any(variables => variables.Success))
                        {
                            card = (PlayableCard)Interpreter.GeneratedVarToObj(variables[0], abilitydata);
                        }
                    }
                    else
                    {
                        card = abilitydata.self;
                    }
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