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

        public static CardInfo getCard(card cardInfo, AbilityBehaviourData abilitydata)
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
            if (SigilData.ConvertArgument(cardInfo.retainMods, abilitydata) == "true")
            {
                ModifySpawnedCard(card, abilitydata);
            }
            return card;
        }

        private static void ModifySpawnedCard(CardInfo card, AbilityBehaviourData abilitydata)
        {
            List<Ability> abilities = new List<Ability>();
            foreach (CardModificationInfo cardMod in abilitydata.self.TemporaryMods)
            {
                abilities.AddRange(cardMod.abilities);
            }
            abilities.RemoveAll((Ability x) => x == abilitydata.ability);
            if (abilities.Count == 0)
            {
                return;
            }
            if (abilities.Count > 4)
            {
                abilities.RemoveRange(3, abilities.Count - 4);
            }
            CardModificationInfo newcardMod = new CardModificationInfo();
            newcardMod.fromCardMerge = true;
            newcardMod.abilities = abilities;
            card.Mods.Add(newcardMod);
        }
    }
}