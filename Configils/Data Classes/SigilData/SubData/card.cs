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

            if (cardInfo == null)
            {
                return null;
            }

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
                if (NewCardMod.HasAbility(Ability.Evolve))
                {
                    NewCardMod.abilities.Remove(Ability.Evolve);
                }
                card.Mods.Add(NewCardMod);
            }
        }
    }
}