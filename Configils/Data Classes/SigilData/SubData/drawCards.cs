using DiskCardGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class drawCards
    {
        public string runOnCondition;
        public card card;

        public static IEnumerator DrawCards(AbilityBehaviourData abilitydata)
        {
            foreach (drawCards drawcardsinfo in abilitydata.drawCards)
            {
                if (SigilData.ConvertArgument(drawcardsinfo.runOnCondition, abilitydata) == "false")
                {
                    continue;
                }

                // yield return new WaitForSeconds(0.3f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);

                card cardInfo = drawcardsinfo.card;
                CardInfo cardinfo = Data.card.getCard(cardInfo, abilitydata);
                if (cardinfo != null)
                {
                    PlayableCard CardInHand = CardSpawner.SpawnPlayableCard(cardinfo);
                    yield return Singleton<PlayerHand>.Instance.AddCardToHand(CardInHand, new Vector3(0f, 0f, 0f), 0);

                    abilitydata.generatedVariables["LastDrawnCard"] = CardInHand;
                    yield return new WaitForSeconds(0.45f);
                }
            }
            yield break;
        }
    }
}
