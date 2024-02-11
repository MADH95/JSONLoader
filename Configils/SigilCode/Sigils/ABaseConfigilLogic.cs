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
    public abstract PlayableCard PlayableCard { get; }
    public abstract Card Card { get; }
    public abstract IEnumerator LearnAbility(float startDelay = 0.0f);
    
    private SigilData abilityData;

    public ABaseConfigilLogic(SigilData abilityData)
    {
        this.abilityData = abilityData;
    }
    

    public IEnumerator Activate()
    {
        int BloodCost = abilityData.activationCost?.bloodCost ?? 0;
        if (BloodCost > 0)
        {
            List<CardSlot> occupiedSlots =
                Singleton<BoardManager>.Instance.PlayerSlotsCopy.FindAll((CardSlot x) =>
                    x.Card != null && x.Card != PlayableCard);

            if (Singleton<BoardManager>.Instance != null &&
                Singleton<BoardManager>.Instance.AvailableSacrificeValueInSlots(occupiedSlots) >= BloodCost)
            {
                Singleton<BoardManager>.Instance.CancelledSacrifice = false;
                yield return SacrificeHelper.ChooseSacrificesForCard(Singleton<BoardManager>.Instance, occupiedSlots,
                    PlayableCard, BloodCost);
                if (Singleton<BoardManager>.Instance.CancelledSacrifice)
                {
                    yield break;
                }
            }
            else
            {
                PlayableCard.Anim.LightNegationEffect();
                AudioController.Instance.PlaySound2D("toneless_negate", MixerGroup.GBCSFX, 0.2f, 0f, null, null, null,
                    null, false);
                yield return new WaitForSeconds(0.25f);
                yield break;
            }
        }

        List<GemType> GemCost =
            abilityData.activationCost?.gemsCost?.Select(s => ImportExportUtils.ParseEnum<GemType>(s)).ToList() ??
            new List<GemType>();
        foreach (GemType Gem in GemCost)
        {
            if (!Singleton<ResourcesManager>.Instance.HasGem(Gem))
            {
                PlayableCard.Anim.LightNegationEffect();
                AudioController.Instance.PlaySound2D("toneless_negate", MixerGroup.GBCSFX, 0.2f, 0f, null, null, null,
                    null, false);
                yield return new WaitForSeconds(0.25f);
                yield break;
            }
        }

        yield return TriggerSigil("OnActivate");
        yield break;
    }


    public IEnumerator Start()
    {
        /* Adding this check since this seems to be the root of the null exceptions in Start()!!
         * According to Debug.Assert(). >< */
        if (abilityData?.abilityBehaviour == null) yield break;

        foreach (AbilityBehaviourData behaviourData in abilityData.abilityBehaviour)
        {
            behaviourData.TurnsInPlay = 0;

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
                                variables
                            && variables.Cast<Match>().Any(variables => variables.Success))
                        {
                            behaviourData.variables[variables[0].Groups[1].Value] = property.Value;
                        }
                    }
                }
            }

            SigilData.UpdateVariables(behaviourData, PlayableCard);
        }

        yield return TriggerSigil("OnLoad");
    }

    public bool RespondsToOtherCardResolve(PlayableCard otherCard)
    {
        return true;
    }

    public IEnumerator OnOtherCardResolve(PlayableCard otherCard)
    {
        if (otherCard.Slot.opposingSlot.Card == PlayableCard)
        {
            yield return TriggerSigil("OnDetect", null, otherCard.Slot.opposingSlot.Card);
        }

        yield return TriggerSigil("OnResolveOnBoard", null, otherCard);
        yield break;
    }

    public bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
    {
        return true;
    }

    // Token: 0x0600157B RID: 5499 RVA: 0x000497BE File Offset: 0x000479BE
    public IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
    {
        if (otherCard.Slot.opposingSlot.Card == PlayableCard)
        {
            yield return TriggerSigil("OnDetect", null, otherCard.Slot.opposingSlot.Card);
        }

        yield break;
    }

    public bool RespondsToTurnEnd(bool playerTurnEnd)
    {
        return true;
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
            for (int i = 0; i < abilityData.abilityBehaviour.Count; i++)
            {
                abilityData.abilityBehaviour[i].TurnsInPlay++;
            }
            yield return TriggerSigil("OnEndOfTurn");
        }
        yield break;
    }

    public bool RespondsToUpkeep(bool playerUpkeep)
    {
        return true;
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

        yield break;
    }

    public bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
    {
        return true;
    }

    public IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
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

            MatchCollection OnHealthLevelMatch =
                Regex.Matches(behaviourData.trigger?.triggerType, @"OnHealthLevel\((.*?)\)");
            if (OnHealthLevelMatch.Cast<Match>().ToList().Count > 0)
            {
                int healthLevel = int.Parse(OnHealthLevelMatch.Cast<Match>().ToList()[0].Groups[1].Value);
                if (target.Health <= healthLevel)
                {
                    yield return TriggerBehaviour(behaviourData,
                        new Dictionary<string, object>() { ["AttackerCard"] = attacker }, target);
                }
            }
        }

        yield return TriggerSigil("OnStruck",
            new Dictionary<string, object>() { ["AttackerCard"] = attacker, ["DamageAmount"] = amount }, target);
        yield return TriggerSigil("OnDamage",
            new Dictionary<string, object>() { ["VictimCard"] = target, ["DamageAmount"] = amount }, attacker);
        yield break;
    }

    public bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
    {
        return true;
    }

    public IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
    {
        yield return new WaitForSeconds(0.3f);
        if (fromCombat)
        {
            yield return TriggerSigil("OnDie",
                new Dictionary<string, object>() { ["AttackerCard"] = killer, ["DeathSlot"] = deathSlot }, card);
            if (killer != null)
            {
                yield return TriggerSigil("OnKill",
                    new Dictionary<string, object>() { ["VictimCard"] = card, ["DeathSlot"] = deathSlot }, killer);
            }
        }

        yield break;
    }

    public bool RespondsToSacrifice()
    {
        return true;
    }

    // Token: 0x06001553 RID: 5459 RVA: 0x000494CF File Offset: 0x000476CF
    public IEnumerator OnSacrifice()
    {
        yield return TriggerSigil("OnSacrifice",
            new Dictionary<string, object>()
                { ["SacrificeTargetCard"] = Singleton<BoardManager>.Instance.CurrentSacrificeDemandingCard });
        yield break;
    }

    public bool RespondsToOtherCardPreDeath(CardSlot deathSlot, bool fromCombat, PlayableCard killer)
    {
        return fromCombat;
    }

    // Token: 0x06001B49 RID: 6985 RVA: 0x0005A16A File Offset: 0x0005836A
    public IEnumerator OnOtherCardPreDeath(CardSlot deathSlot, bool fromCombat, PlayableCard killer)
    {
        if (deathSlot.Card != null)
        {
            yield return TriggerSigil("OnPreDeath",
                new Dictionary<string, object>() { ["AttackerCard"] = killer, ["DeathSlot"] = deathSlot },
                deathSlot.Card);
        }

        yield return TriggerSigil("OnPreKill",
            new Dictionary<string, object>() { ["VictimCard"] = deathSlot.Card, ["DeathSlot"] = deathSlot }, killer);
        yield break;
    }

    public bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
    {
        return true;
    }

    public IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
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

    public bool RespondsToCardDealtDamageDirectly(PlayableCard attacker, CardSlot opposingSlot, int damage)
    {
        return true;
    }

    public IEnumerator OnCardDealtDamageDirectly(PlayableCard attacker, CardSlot opposingSlot, int damage)
    {
        yield return TriggerSigil("OnDamageDirectly",
            new Dictionary<string, object>() { ["HitSlot"] = opposingSlot, ["DamageAmount"] = damage }, attacker);
    }

    public IEnumerator TriggerSigil(string trigger, Dictionary<string, object> variableList = null,
        PlayableCard cardToCheck = null)
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
                foreach (PlayableCard card in Singleton<BoardManager>.Instance.AllSlots.Select(x => x.Card)
                             .OfType<PlayableCard>().ToList())
                {
                    yield return TriggerBehaviour(behaviourData, variableList, card);
                }
            }
            else
            {
                yield return TriggerBehaviour(behaviourData, variableList, cardToCheck ?? PlayableCard);
            }
        }

        yield break;
    }

    public IEnumerator TriggerBehaviour(AbilityBehaviourData behaviourData,
        Dictionary<string, object> variableList = null, PlayableCard cardToCheck = null)
    {
        //this is to prevent errors relating to the sigil trying to access
        //the card that it's on after it has been removed from said card
        if (PlayableCard == null)
        {
            yield break;
        }

        SigilData.UpdateVariables(behaviourData, PlayableCard);

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
        yield return SigilData.RunActions(behaviourData, PlayableCard, ability);
        yield break;
    }

    public bool CheckCard(ref AbilityBehaviourData behaviourData, PlayableCard Card)
    {
        behaviourData.generatedVariables["TriggerCard"] = Card;
        string condition =
            SigilData.ConvertArgument(behaviourData.trigger?.activatesForCardsWithCondition, behaviourData);
        return condition == "true";
    }

    public bool CanActivate()
    {
        AbilityBehaviourData behaviourData = abilityData.abilityBehaviour.Where(x => x.trigger?.triggerType == "OnActivate").ToList()[0];
        if (behaviourData == null)
        {
            return false;
        }

        SigilData.UpdateVariables(behaviourData, PlayableCard);
        return (SigilData.ConvertArgument(behaviourData.trigger?.activatesForCardsWithCondition, behaviourData) ?? "true") == "true";
    }
}