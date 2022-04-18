using DiskCardGame;
using System;
using System.Collections.Generic;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class card
    {
        public string name;
        public string retainMods;
        public string randomCardOnCondition;

        public static CardInfo getCard(card cardInfo, SigilData abilitydata)
        {
            CardInfo card = null;

            if (SigilData.ConvertArgument(cardInfo.name, abilitydata) == "None")
            {
                return null;
            }

            if (cardInfo.name != null)
            {
                card = CardLoader.GetCardByName(SigilData.ConvertArgument(cardInfo.name, abilitydata));
            }
            else if (cardInfo.randomCardOnCondition != null)
            {
                List<CardInfo> cardsWithCondition = new List<CardInfo>();

                foreach (CardInfo randomCard in CardLoader.allData)
                {
                    abilitydata.generatedVariables["RandomCardInfo"] = randomCard;
                    if (SaveManager.SaveFile.IsPart1)
                    {
                        if (SigilData.ConvertArgument(cardInfo.randomCardOnCondition, abilitydata) == "true" && randomCard.metaCategories.Contains(CardMetaCategory.TraderOffer))
                        {
                            cardsWithCondition.Add(randomCard);
                        }
                    }
                    else if (SaveManager.SaveFile.IsPart2)
                    {
                        if (SigilData.ConvertArgument(cardInfo.randomCardOnCondition, abilitydata) == "true" && randomCard.metaCategories.Contains(CardMetaCategory.GBCPlayable))
                        {
                            cardsWithCondition.Add(randomCard);
                        }
                    }
                    else if (SaveManager.SaveFile.IsPart3)
                    {
                        if (SigilData.ConvertArgument(cardInfo.randomCardOnCondition, abilitydata) == "true" && randomCard.metaCategories.Contains(CardMetaCategory.Part3Random))
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
            if (cardInfo.retainMods == "true")
            {
                ModifySpawnedCard(card, abilitydata);
            }
            return card;
        }

        private static void ModifySpawnedCard(CardInfo card, SigilData abilitydata)
        {
            List<Ability> abilities = abilitydata.self.Info.Abilities;
            foreach (CardModificationInfo cardModificationInfo in abilitydata.self.TemporaryMods)
            {
                abilities.AddRange(cardModificationInfo.abilities);
            }
            abilities.RemoveAll((Ability x) => x == abilitydata.ability);
            if (abilities.Count > 4)
            {
                abilities.RemoveRange(3, abilities.Count - 4);
            }
            CardModificationInfo cardModificationInfo2 = new CardModificationInfo();
            cardModificationInfo2.fromCardMerge = true;
            cardModificationInfo2.abilities = abilities;
            card.Mods.Add(cardModificationInfo2);
        }
    }
}