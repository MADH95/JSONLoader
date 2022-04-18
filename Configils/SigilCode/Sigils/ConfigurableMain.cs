// Using Inscryption
using DiskCardGame;
using JLPlugin.Data;
// Modding Inscryption
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;

namespace JLPlugin.SigilCode
{
    public class ConfigurableMain : ConfigurableBase
    {
        public Ability sigilOverride;

        public override Ability Ability
        {
            get
            {
                return sigilOverride;
            }
        }

        public override int Priority
        {
            get
            {
                return abilityData.priority ?? 0;
            }
        }

        public void Start()
        {
            SigilData.UpdateVariables(abilityData, base.Card);
        }

        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return abilityData.trigger.triggerType == "OnResolveOnBoard" && CheckCard(otherCard);
        }

        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            yield return SigilData.RunActions(abilityData, base.Card, ability);
            yield break;
        }

        public override bool RespondsToResolveOnBoard()
        {
            bool noAllCards = abilityData.trigger.activatesForCardsWithCondition == null;
            return abilityData.trigger.triggerType == "OnResolveOnBoard" && noAllCards;
        }

        // Token: 0x06001301 RID: 4865 RVA: 0x000433D2 File Offset: 0x000415D2
        public override IEnumerator OnResolveOnBoard()
        {
            yield return SigilData.RunActions(abilityData, base.Card, ability);
            yield break;
        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return abilityData.trigger.triggerType == "OnStartOfTurn" || abilityData.trigger.triggerType == "OnEndOfTurn";
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            if (base.Card.OpponentCard != playerUpkeep && abilityData.trigger.triggerType == "OnStartOfTurn")
            {
                yield return SigilData.RunActions(abilityData, base.Card, ability);
            }
            if (base.Card.OpponentCard == playerUpkeep && abilityData.trigger.triggerType == "OnEndOfTurn")
            {
                yield return SigilData.RunActions(abilityData, base.Card, ability);
            }
            yield break;
        }

        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            MatchCollection OnHealthLevelMatch = Regex.Matches(abilityData.trigger.triggerType, @"OnHealthLevel\((.*?)\)");
            if (OnHealthLevelMatch.Cast<Match>().ToList().Count > 0)
            {
                int healthLevel = int.Parse(OnHealthLevelMatch.Cast<Match>().ToList()[0].Groups[1].Value);
                if (target.Health <= healthLevel)
                {
                    if (abilityData.trigger.activatesForCardsWithCondition == null)
                    {
                        if (target == base.Card)
                        {
                            abilityData.generatedVariables["AttackerCard"] = attacker;
                            return true;
                        }
                    }
                    if (CheckCard(target))
                    {
                        abilityData.generatedVariables["AttackerCard"] = attacker;
                        return true;
                    }
                }
            }

            if (abilityData.trigger.triggerType == "OnStruck")
            {
                if (abilityData.trigger.activatesForCardsWithCondition == null)
                {
                    if (target == base.Card)
                    {
                        abilityData.generatedVariables["AttackerCard"] = attacker;
                        return true;
                    }
                }
                if (CheckCard(target))
                {
                    abilityData.generatedVariables["AttackerCard"] = attacker;
                    return true;
                }
            }
            if (abilityData.trigger.triggerType == "OnDamage" && target != null)
            {
                if (abilityData.trigger.activatesForCardsWithCondition == null)
                {
                    if (attacker == base.Card)
                    {
                        abilityData.generatedVariables["VictimCard"] = target;
                        return true;
                    }
                }
                if (CheckCard(attacker))
                {
                    abilityData.generatedVariables["VictimCard"] = target;
                    return true;
                }
            }
            return false;
        }

        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            yield return SigilData.RunActions(abilityData, base.Card, ability);
            yield break;
        }

        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            if (abilityData.trigger.triggerType == "OnDie")
            {
                if (abilityData.trigger.activatesForCardsWithCondition == null)
                {
                    if (card == base.Card)
                    {
                        abilityData.generatedVariables["AttackerCard"] = killer;
                        return true;
                    }
                }
                if (CheckCard(card))
                {
                    abilityData.generatedVariables["AttackerCard"] = killer;
                    return true;
                }
            }
            if (abilityData.trigger.triggerType == "OnKill")
            {
                if (abilityData.trigger.activatesForCardsWithCondition == null)
                {
                    if (card == base.Card)
                    {
                        abilityData.generatedVariables["VictimCard"] = card;
                        return true;
                    }
                }
                if (CheckCard(killer))
                {
                    abilityData.generatedVariables["VictimCard"] = card;
                    return true;
                }
            }
            return false;
        }

        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return SigilData.RunActions(abilityData, base.Card, ability);
            yield break;
        }

        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            if (abilityData.trigger.triggerType == "OnAttack")
            {
                if (abilityData.trigger.activatesForCardsWithCondition == null)
                {
                    if (attacker == base.Card)
                    {
                        abilityData.generatedVariables["HitSlot"] = slot;
                        return true;
                    }
                }
                if (CheckCard(attacker))
                {
                    abilityData.generatedVariables["HitSlot"] = slot;
                    return true;
                }
            }
            return false;
        }

        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            yield return SigilData.RunActions(abilityData, base.Card, ability);
            yield break;
        }

        public bool CheckCard(PlayableCard Card)
        {
            if (abilityData.trigger.activatesForCardsWithCondition == null || Card == null)
            {
                return false;
            }

            abilityData.generatedVariables["TriggerCard"] = Card;
            string condition = SigilData.ConvertArgument(abilityData.trigger.activatesForCardsWithCondition, abilityData);
            return condition == "true";
        }
    }
}