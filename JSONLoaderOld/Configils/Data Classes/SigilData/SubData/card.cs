using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static JLPlugin.Interpreter;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class card
    {
        public string name;
        public string retainMods;
        public string randomCardOnCondition;
        public string targetCard;

        public static CardInfo getCard(card cardInfo, AbilityBehaviourData abilitydata)
        {
            CardInfo card = null;

            if (cardInfo == null)
            {
                return null;
            }

            if (AConfigilData.ConvertArgument(cardInfo.name, abilitydata) == "None")
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(cardInfo.name))
            {
                card = CardLoader.GetCardByName(AConfigilData.ConvertArgument(cardInfo.name, abilitydata));
            }
            else if (cardInfo.randomCardOnCondition != null)
            {
                List<CardInfo> cardsWithCondition = new List<CardInfo>();

                foreach (CardInfo randomCard in CardLoader.allData)
                {
                    abilitydata.generatedVariables["RandomCardInfo"] = randomCard;
                    if (SaveManager.SaveFile.IsPart1)
                    {
                        if (AConfigilData.ConvertArgument(cardInfo.randomCardOnCondition, abilitydata) == "true" && randomCard.metaCategories.Contains(CardMetaCategory.TraderOffer))
                        {
                            cardsWithCondition.Add(randomCard);
                        }
                    }
                    else if (SaveManager.SaveFile.IsPart2)
                    {
                        if (AConfigilData.ConvertArgument(cardInfo.randomCardOnCondition, abilitydata) == "true" && randomCard.metaCategories.Contains(CardMetaCategory.GBCPlayable))
                        {
                            cardsWithCondition.Add(randomCard);
                        }
                    }
                    else if (SaveManager.SaveFile.IsPart3)
                    {
                        if (AConfigilData.ConvertArgument(cardInfo.randomCardOnCondition, abilitydata) == "true" && randomCard.metaCategories.Contains(CardMetaCategory.Part3Random))
                        {
                            cardsWithCondition.Add(randomCard);
                        }
                    }
                }

                if (cardsWithCondition.Count > 0)
                {
                    Random random = new Random();
                    card = cardsWithCondition[random.Next(cardsWithCondition.Count)];
                }
            }
            else if (!string.IsNullOrEmpty(cardInfo.targetCard))
            {
                card = ((Card)AConfigilData.ConvertArgumentToType(cardInfo.targetCard, abilitydata, typeof(Card))).Info;
            }
            if (AConfigilData.ConvertArgument(cardInfo.retainMods, abilitydata) == "true")
            {
                ModifyCard(card, abilitydata);
            }
            return card;
        }

        private static void ModifyCard(CardInfo card, AbilityBehaviourData abilitydata)
        {
            if (abilitydata.self == null)
            {
                return;
            }

            foreach (CardModificationInfo mod in abilitydata.self.Info.Mods.FindAll((CardModificationInfo x) => !x.nonCopyable))
            {
                CardModificationInfo NewCardMod = (CardModificationInfo)mod.Clone();
                card.Mods.Add(NewCardMod);
            }
        }
    }
}