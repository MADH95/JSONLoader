// Using Inscryption
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using JLPlugin.Data;
using JLPlugin.V2.Data;
// Modding Inscryption
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TinyJson;
using UnityEngine;
using static JLPlugin.Interpreter;

namespace JLPlugin.SigilCode
{
    public class ConfigurableSpecial : ConfigurableSpecialBase, IOnBellRung, IOnOtherCardAddedToHand
    {
        public override int Priority
        {
            get
            {
                return abilityData.priority ?? 0;
            }
        }

        public void Start()
        {
            /* Adding this check since this seems to be the root of the null exceptions in Start()!! 
             * According to Debug.Assert(). >< */
            if (abilityData?.abilityBehaviour == null) return;

            foreach (AbilityBehaviourData behaviourData in abilityData.abilityBehaviour)
            {
                behaviourData.TurnsInPlay = 0;

                string filepath = base.PlayableCard.Info.GetExtendedProperty("JSONFilePath");
                if (filepath != null)
                {
                    /* Load from cache first. avoid reading a file and parsing JSON every single
                     * time this method is called (which will be MULTIPLE TIMES throughout the
                     * game). >< */
                    /* if it doesn't exist in cache, *THEN* you can read from the file. */
                    if (!CachedCardData.Contains(filepath))
                    {
                        CachedCardData.Add(
                                filePath: filepath,
                                data: JSONParser.FromJson<CardSerializeInfo>(File.ReadAllText(filepath))
                            );
                    }
                    CardSerializeInfo cardinfo = CachedCardData.Get(filepath);

                    if (cardinfo.extensionProperties != null)
                    {
                        foreach (KeyValuePair<string, string> property in cardinfo.extensionProperties)
                        {
                            if (Regex.Matches(property.Key, $"variable: {RegexStrings.Variable}") is var variables
                                    && variables.Cast<Match>().Any(variables => variables.Success))
                            {
                                behaviourData.variables[variables[0].Groups[1].Value] = property.Value;
                            }
                        }
                    }
                }
                SigilData.UpdateVariables(behaviourData, base.PlayableCard);
            }
            TriggerSigil("OnLoad");
        }

        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return true;
        }

        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            if (otherCard.Slot.opposingSlot.Card == base.PlayableCard)
            {
                yield return TriggerSigil("OnDetect", null, otherCard.Slot.opposingSlot.Card);
            }
            yield return TriggerSigil("OnResolveOnBoard", null, otherCard);
            yield break;
        }

        public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            return true;
        }

        // Token: 0x0600157B RID: 5499 RVA: 0x000497BE File Offset: 0x000479BE
        public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            if (otherCard.Slot.opposingSlot.Card == base.PlayableCard)
            {
                yield return TriggerSigil("OnDetect", null, otherCard.Slot.opposingSlot.Card);
            }
            yield break;
        }

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return true;
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            if (base.PlayableCard.OpponentCard != playerTurnEnd)
            {
                for (int i = 0; i < abilityData.abilityBehaviour.Count; i++)
                {
                    abilityData.abilityBehaviour[i].TurnsInPlay++;
                }
                yield return TriggerSigil("OnEndOfTurn");
            }
            yield break;
        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return true;
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            if (base.PlayableCard.OpponentCard != playerUpkeep)
            {
                yield return TriggerSigil("OnStartOfTurn");
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
                if (behaviourData.trigger?.triggerType == null)
                {
                    continue;
                }

                if (!behaviourData.trigger.triggerType.Contains("OnHealthLevel"))
                {
                    continue;
                }

                MatchCollection OnHealthLevelMatch = Regex.Matches(behaviourData.trigger?.triggerType, @"OnHealthLevel\((.*?)\)");
                if (OnHealthLevelMatch.Cast<Match>().ToList().Count > 0)
                {
                    int healthLevel = int.Parse(OnHealthLevelMatch.Cast<Match>().ToList()[0].Groups[1].Value);
                    if (target.Health <= healthLevel)
                    {
                        TriggerBehaviour(behaviourData, new Dictionary<string, object>() { ["AttackerCard"] = attacker }, target);
                    }
                }
            }
            yield return TriggerSigil("OnStruck", new Dictionary<string, object>() { ["AttackerCard"] = attacker, ["DamageAmount"] = amount }, target);
            yield return TriggerSigil("OnDamage", new Dictionary<string, object>() { ["VictimCard"] = target, ["DamageAmount"] = amount }, attacker);
            yield break;
        }

        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return true;
        }

        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return new WaitForSeconds(0.3f);
            if (fromCombat)
            {
                yield return TriggerSigil("OnDie", new Dictionary<string, object>() { ["AttackerCard"] = killer, ["DeathSlot"] = deathSlot }, card);
                if (killer != null)
                {
                    yield return TriggerSigil("OnKill", new Dictionary<string, object>() { ["VictimCard"] = card, ["DeathSlot"] = deathSlot }, killer);
                }
            }
            yield break;
        }

        public override bool RespondsToSacrifice()
        {
            return true;
        }

        // Token: 0x06001553 RID: 5459 RVA: 0x000494CF File Offset: 0x000476CF
        public override IEnumerator OnSacrifice()
        {
            yield return TriggerSigil("OnSacrifice", new Dictionary<string, object>() { ["SacrificeTargetCard"] = Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard });
            yield break;
        }

        public override bool RespondsToOtherCardPreDeath(CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return fromCombat;
        }

        // Token: 0x06001B49 RID: 6985 RVA: 0x0005A16A File Offset: 0x0005836A
        public override IEnumerator OnOtherCardPreDeath(CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            if (deathSlot.Card != null)
            {
                yield return TriggerSigil("OnPreDeath", new Dictionary<string, object>() { ["AttackerCard"] = killer }, deathSlot.Card);
            }
            yield return TriggerSigil("OnPreKill", new Dictionary<string, object>() { ["VictimCard"] = deathSlot.Card }, killer);
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
        public bool RespondsToBellRung(bool playerCombatPhase)
        {
            return true;
        }

        public IEnumerator OnBellRung(bool playerCombatPhase)
        {
            //Plugin.Log.LogInfo("COMBAT STARTED");
            if (playerCombatPhase)
            {
                //Plugin.Log.LogInfo("PLAYER COMBAT STARTED");
                yield return TriggerSigil("OnCombatStart");
            }
            else
            {
                //Plugin.Log.LogInfo("OPPONENT COMBAT STARTED");
                yield return TriggerSigil("OnEnemyCombatStart");
            }
            yield break;
        }

        public bool RespondsToOtherCardAddedToHand(PlayableCard card)
        {
            return true;
        }

        public IEnumerator OnOtherCardAddedToHand(PlayableCard card)
        {
            yield return TriggerSigil("OnAddedToHand", null, card);
            yield break;
        }

        public bool RespondsToCardAssignedToSlotContext(PlayableCard card, CardSlot oldSlot, CardSlot newSlot)
        {
            return true;
        }

        public IEnumerator OnCardAssignedToSlotContext(PlayableCard card, CardSlot oldSlot, CardSlot newSlot)
        {
            if (oldSlot != null)
            {
                yield return TriggerSigil("OnMove", new Dictionary<string, object>() { ["OldSlot"] = oldSlot }, card);
            }
            yield break;
        }

        public IEnumerator TriggerSigil(string trigger, Dictionary<string, object> variableList = null, PlayableCard cardToCheck = null)
        {
            foreach (AbilityBehaviourData behaviourData in abilityData.abilityBehaviour)
            {
                if (behaviourData.trigger?.triggerType == null)
                {
                    continue;
                }

                //Plugin.Log.LogInfo($"{behaviourData.trigger.triggerType}, {trigger}");
                if (behaviourData.trigger.triggerType != trigger)
                {
                    continue;
                }

                if (behaviourData.trigger.activatesForCardsWithCondition != null && cardToCheck == null)
                {
                    foreach (PlayableCard card in Singleton<BoardManager>.Instance.AllSlots.Select(x => x.Card).OfType<PlayableCard>().ToList())
                    {
                        yield return TriggerBehaviour(behaviourData, variableList, card);
                    }
                }
                else
                {
                    yield return TriggerBehaviour(behaviourData, variableList, cardToCheck ?? base.PlayableCard);
                }
            }
            yield break;
        }

        public IEnumerator TriggerBehaviour(AbilityBehaviourData behaviourData, Dictionary<string, object> variableList = null, PlayableCard cardToCheck = null)
        {
            //this is to prevent errors relating to the sigil trying to access
            //the card that it's on after it has been removed from said card
            if (base.PlayableCard == null)
            {
                yield break;
            }

            SigilData.UpdateVariables(behaviourData, base.PlayableCard);

            if (behaviourData.trigger.activatesForCardsWithCondition != null)
            {
                if (cardToCheck != null)
                {
                    if (!CheckCard(ref behaviourData, cardToCheck))
                    {
                        yield break;
                    }
                }
            }
            else
            {
                if (cardToCheck != base.PlayableCard && cardToCheck != null)
                {
                    yield break;
                }
            }

            if (variableList != null)
            {
                foreach (var variable in variableList)
                {
                    //Plugin.Log.LogInfo("set and set to: " + variable.Key.ToString() + " " + variable.Value.ToString());
                    behaviourData.generatedVariables[variable.Key] = variable.Value;
                }
            }
            yield return SigilData.RunActions(behaviourData, base.PlayableCard);
            yield break;
        }

        public bool CheckCard(ref AbilityBehaviourData behaviourData, PlayableCard Card)
        {
            behaviourData.generatedVariables["TriggerCard"] = Card;
            string condition = SigilData.ConvertArgument(behaviourData.trigger?.activatesForCardsWithCondition, behaviourData);
            return condition == "true";
        }
    }
}
