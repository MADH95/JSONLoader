// Using Inscryption
using DiskCardGame;
using JLPlugin.Data;
// Modding Inscryption
using System.Collections;
using System.Collections.Generic;
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
            foreach (AbilityBehaviourData behaviourData in abilityData.abilityBehaviour)
            {
                SigilData.UpdateVariables(behaviourData, base.Card);
            }
        }

        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return true;
        }

        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            yield return TriggerSigil("OnResolveOnBoard", null, otherCard);
            yield break;
        }

        public override bool RespondsToResolveOnBoard()
        {
            return true;
        }

        // Token: 0x06001301 RID: 4865 RVA: 0x000433D2 File Offset: 0x000415D2
        public override IEnumerator OnResolveOnBoard()
        {
            yield return TriggerSigil("OnResolveOnBoard");
            yield break;
        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return true;
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            if (base.Card.OpponentCard != playerUpkeep)
            {
                yield return TriggerSigil("OnStartOfTurn");
            }
            if (base.Card.OpponentCard == playerUpkeep)
            {
                yield return TriggerSigil("OnEndOfTurn");
            }
            yield break;
        }

        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            return true;
        }

        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            foreach (AbilityBehaviourData behaviourData in abilityData.abilityBehaviour)
            {
                MatchCollection OnHealthLevelMatch = Regex.Matches(behaviourData.trigger.triggerType, @"OnHealthLevel\((.*?)\)");
                if (OnHealthLevelMatch.Cast<Match>().ToList().Count > 0)
                {
                    int healthLevel = int.Parse(OnHealthLevelMatch.Cast<Match>().ToList()[0].Groups[1].Value);
                    if (target.Health <= healthLevel)
                    {
                        TriggerBehaviour(behaviourData, "OnHealthLevel", new Dictionary<string, object>() { ["AttackerCard"] = attacker }, target);
                    }
                }
            }
            yield return TriggerSigil("OnStruck", new Dictionary<string, object>() { ["AttackerCard"] = attacker }, target);
            yield return TriggerSigil("OnDamage", new Dictionary<string, object>() { ["VictimCard"] = target }, attacker);
            yield break;
        }

        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return true;
        }

        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return TriggerSigil("OnDie", new Dictionary<string, object>() { ["AttackerCard"] = killer }, card);
            yield return TriggerSigil("OnDie", new Dictionary<string, object>() { ["VictimCard"] = card }, killer);
            yield break;
        }

        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            return true;
        }

        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            yield return TriggerSigil("OnAttack", new Dictionary<string, object>() { ["HitSlot"] = slot }, attacker);
            yield break;
        }

        public IEnumerator TriggerSigil(string trigger, Dictionary<string, object> variableList = null, PlayableCard cardToCheck = null)
        {
            //cardToCheck kan elke kaart zijn ook base.Card
            //het is alleen voor wanneer de methods gesplits zijn zovan OnResolveOnBoard en OnOtherResolveOnBoard

            foreach (AbilityBehaviourData behaviourData in abilityData.abilityBehaviour)
            {
                yield return TriggerBehaviour(behaviourData, trigger, variableList, cardToCheck);
            }
            yield break;
        }

        public IEnumerator TriggerBehaviour(AbilityBehaviourData behaviourData, string trigger, Dictionary<string, object> variableList = null, PlayableCard cardToCheck = null)
        {
            if (behaviourData.trigger.triggerType.Contains(trigger))
            {
                //de user wil alle kaarten checken
                if (behaviourData.trigger.activatesForCardsWithCondition != null)
                {
                    //er is een kaart om te checken dus doe dat
                    if (cardToCheck != null)
                    {
                        if (!CheckCard(behaviourData, cardToCheck))
                        {
                            yield break;
                        }
                    }
                }
                else
                {
                    //de user wil alleen base.Card triggeren
                    //dus check je of cardToCheck base.Card is
                    //of dat het null is want dat is het alleen wanneer je zeker weet dat het base.Card is
                    //bijvoorbeeld met OnResolveOnBoard
                    if (cardToCheck != base.Card && cardToCheck != null)
                    {
                        yield break;
                    }
                }

                SigilData.UpdateVariables(behaviourData, base.Card);

                if (variableList != null)
                {
                    foreach (var variable in variableList)
                    {
                        //Plugin.Log.LogInfo("set and set to: " + variable.Key.ToString() + " " + variable.Value.ToString());
                        behaviourData.generatedVariables[variable.Key] = variable.Value;
                    }
                }
                yield return SigilData.RunActions(behaviourData, base.Card, ability);
            }
            yield break;
        }

        public bool CheckCard(AbilityBehaviourData behaviourData, PlayableCard Card)
        {
            if (Card == null || behaviourData.trigger.activatesForCardsWithCondition == null)
            {
                return false;
            }

            behaviourData.generatedVariables["TriggerCard"] = Card;
            string condition = SigilData.ConvertArgument(behaviourData.trigger.activatesForCardsWithCondition, behaviourData);
            return condition == "true";
        }
    }
}