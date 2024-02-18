using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using DiskCardGame;
using InscryptionAPI.Card;
using JLPlugin;
using JLPlugin.Data;
using JLPlugin.SigilCode;
using JLPlugin.V2.Data;
using TinyJson;
using UnityEngine;

public abstract class ABaseConfigilLogic
{
    public abstract object ability { get; }
    public abstract object Instance { get; }
    public abstract PlayableCard PlayableCard { get; }
    public abstract Card Card { get; }
    
    private readonly AConfigilData data;
    private readonly Dictionary<string, List<AbilityBehaviourData>> abilityBehaviours;

    public ABaseConfigilLogic(AConfigilData data)
    {
        this.data = data;

        abilityBehaviours = new Dictionary<string, List<AbilityBehaviourData>>();
        for (var i = 0; i < data.abilityBehaviour.Count; i++)
        {
            AbilityBehaviourData behaviourData = data.abilityBehaviour[i];
            string triggerType = behaviourData.trigger?.triggerType;
            if (string.IsNullOrEmpty(triggerType))
            {
                Plugin.Log.LogError($"AbilityBehaviourData {i} has no triggerType {data.Name}!");
                continue;
            }

            // Some trigger types have arguments after them. This is to catch just the trigger type.
            if (triggerType.StartsWith("OnHealthLevel"))
            {
                triggerType = "OnHealthLevel";
            }
            
            if (!abilityBehaviours.ContainsKey(triggerType))
            {
                abilityBehaviours[triggerType] = new List<AbilityBehaviourData>();
            }

            abilityBehaviours[triggerType].Add(behaviourData);
        }
    }
    

    public IEnumerator Activate()
    {
        int BloodCost = data.activationCost?.bloodCost ?? 0;
        if (BloodCost > 0)
        {
            List<CardSlot> occupiedSlots = Singleton<BoardManager>.Instance.PlayerSlotsCopy.FindAll(x => x.Card != null && x.Card != PlayableCard);

            if (Singleton<BoardManager>.Instance != null &&
                Singleton<BoardManager>.Instance.AvailableSacrificeValueInSlots(occupiedSlots) >= BloodCost)
            {
                Singleton<BoardManager>.Instance.CancelledSacrifice = false;
                yield return Singleton<BoardManager>.Instance.ChooseSacrificesForCard(occupiedSlots,
                    PlayableCard, BloodCost);
                if (Singleton<BoardManager>.Instance.CancelledSacrifice)
                {
                    yield break;
                }
            }
            else
            {
                PlayableCard.Anim.LightNegationEffect();
                AudioController.Instance.PlaySound2D("toneless_negate", MixerGroup.GBCSFX, 0.2f);
                yield return new WaitForSeconds(0.25f);
                yield break;
            }
        }

        var GemCost = data.activationCost?.gemsCost?.Select(ImportExportUtils.ParseEnum<GemType>).ToList() ?? new List<GemType>();
        foreach (GemType Gem in GemCost)
        {
            if (!Singleton<ResourcesManager>.Instance.HasGem(Gem))
            {
                PlayableCard.Anim.LightNegationEffect();
                AudioController.Instance.PlaySound2D("toneless_negate", MixerGroup.GBCSFX, 0.2f);
                yield return new WaitForSeconds(0.25f);
                yield break;
            }
        }

        yield return TriggerSigil("OnActivate");
    }


    public IEnumerator Start()
    {
        /* Adding this check since this seems to be the root of the null exceptions in Start()!!
         * According to Debug.Assert(). >< */
        if (data?.abilityBehaviour == null) yield break;

        foreach (AbilityBehaviourData behaviourData in data.abilityBehaviour)
        {
            behaviourData.TurnsInPlay = 0;

            if (PlayableCard != null)
            {
                string filepath = PlayableCard.Info.GetExtendedProperty("JSONFilePath");
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
                            data: JSONParser.FromFilePath<CardSerializeInfo>(filepath)
                        );
                    }

                    CardSerializeInfo cardinfo = CachedCardData.Get(filepath);

                    if (cardinfo.extensionProperties != null)
                    {
                        foreach (KeyValuePair<string, string> property in cardinfo.extensionProperties)
                        {
                            if (Regex.Matches(property.Key, $"variable: {Interpreter.RegexStrings.Variable}") is var
                                    variables && variables.Cast<Match>().Any(v => v.Success))
                            {
                                behaviourData.variables[variables[0].Groups[1].Value] = property.Value;
                            }
                        }
                    }
                }
            }

            AConfigilData.UpdateVariables(behaviourData, PlayableCard);
        }

        yield return TriggerSigil("OnLoad");
    }

    public bool RespondsToOtherCardResolve(PlayableCard otherCard)
    {
        return abilityBehaviours.ContainsKey("OnDetect") || abilityBehaviours.ContainsKey("OnResolveOnBoard");
    }

    public IEnumerator OnOtherCardResolve(PlayableCard otherCard)
    {
        if (otherCard.Slot.opposingSlot.Card == PlayableCard)
        {
            yield return TriggerSigil("OnDetect", null, otherCard.Slot.opposingSlot.Card);
        }

        yield return TriggerSigil("OnResolveOnBoard", null, otherCard);
    }

    public bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
    {
        return abilityBehaviours.ContainsKey("OnDetect");
    }

    // Token: 0x0600157B RID: 5499 RVA: 0x000497BE File Offset: 0x000479BE
    public IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
    {
        if (otherCard.Slot.opposingSlot.Card == PlayableCard)
        {
            yield return TriggerSigil("OnDetect", null, otherCard.Slot.opposingSlot.Card);
        }
    }

    public bool RespondsToTurnEnd(bool playerTurnEnd)
    {
        return abilityBehaviours.ContainsKey("OnPlayerEndOfTurn") || 
               abilityBehaviours.ContainsKey("OnOpponentEndOfTurn") || 
               abilityBehaviours.ContainsKey("OnEndOfTurn");
    }

    public IEnumerator OnTurnEnd(bool playerTurnEnd)
    {
        if (playerTurnEnd)
        {
            yield return TriggerSigil("OnPlayerEndOfTurn");
        }
        else
        {
            yield return TriggerSigil("OnOpponentEndOfTurn");
        }

        if (PlayableCard.OpponentCard != playerTurnEnd)
        {
            for (int i = 0; i < data.abilityBehaviour.Count; i++)
            {
                data.abilityBehaviour[i].TurnsInPlay++;
            }
            yield return TriggerSigil("OnEndOfTurn");
        }
    }

    public bool RespondsToUpkeep(bool playerUpkeep)
    {
        return abilityBehaviours.ContainsKey("OnPlayerStartOfTurn") || 
               abilityBehaviours.ContainsKey("OnOpponentStartOfTurn") || 
               abilityBehaviours.ContainsKey("OnStartOfTurn");
    }

    public IEnumerator OnUpkeep(bool playerUpkeep)
    {
        if (playerUpkeep)
        {
            yield return TriggerSigil("OnPlayerStartOfTurn");
        }
        else
        {
            yield return TriggerSigil("OnOpponentStartOfTurn");
        }

        if (PlayableCard.OpponentCard != playerUpkeep)
        {
            yield return TriggerSigil("OnStartOfTurn");
        }
    }

    public bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
    {
        return abilityBehaviours.ContainsKey("OnStruck") || 
               abilityBehaviours.ContainsKey("OnDamage") || 
               abilityBehaviours.ContainsKey("OnHealthLevel");
    }

    public IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
    {
        if (abilityBehaviours.TryGetValue("OnHealthLevel", out List<AbilityBehaviourData> healthLevelBehaviours))
        {
            foreach (AbilityBehaviourData behaviourData in healthLevelBehaviours)
            {
                MatchCollection OnHealthLevelMatch = Regex.Matches(behaviourData.trigger.triggerType, @"OnHealthLevel\((.*?)\)");
                if (OnHealthLevelMatch.Cast<Match>().ToList().Count <= 0) 
                    continue;
                
                int healthLevel = int.Parse(OnHealthLevelMatch.Cast<Match>().ToList()[0].Groups[1].Value);
                if (target.Health <= healthLevel)
                {
                    yield return TriggerBehaviour(behaviourData, ("AttackerCard", attacker), target);
                }
            }
        }

        yield return TriggerSigil("OnStruck", ("AttackerCard", attacker, "DamageAmount", amount), target);
        yield return TriggerSigil("OnDamage", ("VictimCard", target, "DamageAmount", amount), attacker);
    }

    public bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
    {
        return abilityBehaviours.ContainsKey("OnDie") || 
               abilityBehaviours.ContainsKey("OnKill");
    }

    public IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
    {
        yield return new WaitForSeconds(0.3f);
        if (fromCombat)
        {
            yield return TriggerSigil("OnDie", ("AttackerCard", killer, "DeathSlot", deathSlot), card);
            if (killer != null)
            {
                yield return TriggerSigil("OnKill", ("VictimCard",card, "DeathSlot", deathSlot), killer);
            }
        }
    }

    public bool RespondsToSacrifice()
    {
        return abilityBehaviours.ContainsKey("OnSacrifice");
    }

    // Token: 0x06001553 RID: 5459 RVA: 0x000494CF File Offset: 0x000476CF
    public IEnumerator OnSacrifice()
    {
        yield return TriggerSigil("OnSacrifice", ("SacrificeTargetCard", Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard));
    }

    public bool RespondsToOtherCardPreDeath(CardSlot deathSlot, bool fromCombat, PlayableCard killer)
    {
        return fromCombat && 
               (abilityBehaviours.ContainsKey("OnPreDeath") || abilityBehaviours.ContainsKey("OnPreKill"));
    }

    // Token: 0x06001B49 RID: 6985 RVA: 0x0005A16A File Offset: 0x0005836A
    public IEnumerator OnOtherCardPreDeath(CardSlot deathSlot, bool fromCombat, PlayableCard killer)
    {
        if (deathSlot.Card != null)
        {
            yield return TriggerSigil("OnPreDeath", ("AttackerCard", killer, "DeathSlot", deathSlot), deathSlot.Card);
        }

        yield return TriggerSigil("OnPreKill", ("VictimCard", deathSlot.Card, "DeathSlot", deathSlot), killer);
    }

    public bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
    {
        return abilityBehaviours.ContainsKey("OnAttack");
    }

    public IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
    {
        yield return TriggerSigil("OnAttack", ("HitSlot", slot), attacker);
    }

    public bool RespondsToBellRung(bool playerCombatPhase)
    {
        return abilityBehaviours.ContainsKey("OnCombatStart") || 
               abilityBehaviours.ContainsKey("OnEnemyCombatStart");
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
    }

    public bool RespondsToOtherCardAddedToHand(PlayableCard card)
    {
        return abilityBehaviours.ContainsKey("OnAddedToHand");
    }

    public IEnumerator OnOtherCardAddedToHand(PlayableCard card)
    {
        yield return TriggerSigil("OnAddedToHand", null, card);
    }

    public bool RespondsToCardAssignedToSlotContext(PlayableCard card, CardSlot oldSlot, CardSlot newSlot)
    {
        return abilityBehaviours.ContainsKey("OnMove");
    }

    public IEnumerator OnCardAssignedToSlotContext(PlayableCard card, CardSlot oldSlot, CardSlot newSlot)
    {
        if (oldSlot != null)
        {
            yield return TriggerSigil("OnMove", ("OldSlot", oldSlot), card);
        }
    }

    public bool RespondsToCardDealtDamageDirectly(PlayableCard attacker, CardSlot opposingSlot, int damage)
    {
        return abilityBehaviours.ContainsKey("OnDamageDirectly");
    }

    public IEnumerator OnCardDealtDamageDirectly(PlayableCard attacker, CardSlot opposingSlot, int damage)
    {
        yield return TriggerSigil("OnDamageDirectly", ("HitSlot", opposingSlot, "DamageAmount", damage), attacker);
    }

    public IEnumerator TriggerSigil(string trigger, TriggerVariables variableList = null, PlayableCard cardToCheck = null)
    {
        if (!abilityBehaviours.TryGetValue(trigger, out List<AbilityBehaviourData> behaviours))
        {
            yield break;
        }
        
        foreach (AbilityBehaviourData behaviourData in behaviours)
        {
            if (behaviourData.trigger.activatesForCardsWithCondition != null && cardToCheck == null)
            {
                foreach (PlayableCard card in Singleton<BoardManager>.Instance.AllSlots.Select(x => x.Card).OfType<PlayableCard>().ToList())
                {
                    yield return TriggerBehaviour(behaviourData, variableList, card);
                }
            }
            else
            {
                yield return TriggerBehaviour(behaviourData, variableList, cardToCheck ?? PlayableCard);
            }
        }
    }

    public IEnumerator TriggerBehaviour(AbilityBehaviourData behaviourData, TriggerVariables variableList = null, PlayableCard cardToCheck = null)
    {
        //this is to prevent errors relating to the sigil trying to access
        //the card that it's on after it has been removed from said card
        if (Instance == null)
        {
            yield break;
        }

        AConfigilData.UpdateVariables(behaviourData, PlayableCard);

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
            if (cardToCheck != PlayableCard && cardToCheck != null)
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

        yield return LearnAbility(0f);
        yield return AConfigilData.RunActions(behaviourData, PlayableCard, ability);
    }

    private bool CheckCard(ref AbilityBehaviourData behaviourData, PlayableCard card)
    {
        behaviourData.generatedVariables["TriggerCard"] = card;
        string condition = AConfigilData.ConvertArgument(behaviourData.trigger?.activatesForCardsWithCondition, behaviourData);
        return condition == "true";
    }

    public bool CanActivate()
    {
        if (!abilityBehaviours.TryGetValue("OnActivate", out List<AbilityBehaviourData> onActivateBehaviours))
        {
            return false;
        }
        
        AbilityBehaviourData behaviourData = onActivateBehaviours[0];
        AConfigilData.UpdateVariables(behaviourData, PlayableCard);
        return (AConfigilData.ConvertArgument(behaviourData.trigger?.activatesForCardsWithCondition, behaviourData) ?? "true") == "true";
    }

    public int[] GetStatValues()
    {
        int[] result = new int[]{0, 0};
        if (!abilityBehaviours.TryGetValue("GetStatValues", out List<AbilityBehaviourData> getStatValues))
        {
            return result;
        }

        foreach (AbilityBehaviourData value in getStatValues)
        {
            AConfigilData.UpdateVariables(value, PlayableCard);
            if (value.trigger?.activatesForCardsWithCondition != null)
            {
                string condition = AConfigilData.ConvertArgument(value.trigger?.activatesForCardsWithCondition, value);
                if (condition != "true")
                {
                    continue;
                }
            }

            if (!string.IsNullOrEmpty(value.getStatValues?.health))
            {
                string healthResult = AConfigilData.ConvertArgument(value.getStatValues.health, value);
                Plugin.Log.LogInfo($"Health result: {healthResult} from {value.getStatValues.health}");
                if (int.TryParse(healthResult, out int h))
                {
                    result[1] += h;
                }
            }

            if (!string.IsNullOrEmpty(value.getStatValues?.attack))
            {
                string attackResult = AConfigilData.ConvertArgument(value.getStatValues.attack, value);
                Plugin.Log.LogInfo($"Attack result: {attackResult} from {value.getStatValues.attack}");
                if (int.TryParse(attackResult, out int a))
                {
                    result[0] += a;
                }
            }
            else
            {
                Plugin.Log.LogInfo($"Attack is empty: {value.getStatValues?.attack}");
            }
        }

        return result;
    }
    
    public virtual IEnumerator LearnAbility(float startDelay = 0)
    {
        yield break;
    }
}