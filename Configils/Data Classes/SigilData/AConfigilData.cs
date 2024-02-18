using System;
using System.Collections;
using DiskCardGame;
using System.Collections.Generic;
using JLPlugin.Data;
using JSONLoader.API;
using TinyJson;
using JLPlugin;

public abstract class AConfigilData : JSONParser.IInitializable
{
    public abstract string Name { get; }

    public activationCost activationCost;

    public List<AbilityBehaviourData> abilityBehaviour;


    protected AConfigilData()
    {
        // Required for JSONParser to work properly with Localisation
        Initialize();
    }

    public abstract void Initialize();

    public static void UpdateVariables(AbilityBehaviourData abilitydata, PlayableCard self)
    {
        if (abilitydata.variables == null)
        {
            abilitydata.variables = new Dictionary<string, string>();
        }

        if (abilitydata.generatedVariables == null)
        {
            abilitydata.generatedVariables = new Dictionary<string, object>();
        }

        Dictionary<string, string> VariableDictionary = new Dictionary<string, string>()
        {
            { "EnergyAmount", Singleton<ResourcesManager>.Instance.PlayerEnergy.ToString() },
            { "BoneAmount", Singleton<ResourcesManager>.Instance.PlayerBones.ToString() },
            { "Turn", Singleton<TurnManager>.Instance.TurnNumber.ToString() },
            { "TurnsInPlay", (abilitydata.TurnsInPlay ?? 0).ToString() },
            { "ScaleBalance", Singleton<LifeManager>.Instance.Balance.ToString() }
        };
        abilitydata.variables.Append(VariableDictionary);
        abilitydata.variables = JSONLoaderAPI.GetModifiedVariableList(abilitydata.variables);

        Dictionary<string, object> GeneratedVariableDictionary = new Dictionary<string, object>()
        {
            { "LastDrawnCard", null },
            { "DamageAmount", null },
            { "DeathSlot", null },
            { "HitSlot", null },
            { "AttackerCard", null },
            { "VictimCard", null },
            { "ChooseableSlot", null },
            { "RandomCardInfo", null },
            { "TriggerCard", null },
            { "BaseCard", self }
        };
        abilitydata.generatedVariables.Append(GeneratedVariableDictionary);
    }

    public static object ConvertArgumentToType(string value, AbilityBehaviourData abilitydata, Type type,
        bool sendDebug = true)
    {
        if (value == null)
        {
            return null;
        }

        object output = null;
        try
        {
            output = Interpreter.Process(value, abilitydata, type, sendDebug);
        }
        catch (Exception)
        {
            Plugin.Log.LogError($"[{abilitydata.GetType()}] Error converting argument '{value}' to '{type}'");
            throw;
        }

        //using IsAssignableFrom because it has the added benefit of also detecting subclasses
        if (type.IsAssignableFrom(output.GetType()))
        {
            return output;
        }
        else
        {
            Plugin.Log.LogError($"{output} is not of type {type}");
            return null;
        }
    }

    public static string ConvertArgument(string value, AbilityBehaviourData abilitydata, bool sendDebug = true)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        try
        {
            return Interpreter.Process(value, abilitydata, null, sendDebug).ToString();
        }
        catch (Exception)
        {
            Plugin.Log.LogError($"[{abilitydata.GetType()}] Error converting argument '{value}' to string");
            throw;
        }
    }

    public static List<string> DefaultActionOrder = new List<string>()
    {
        "chooseSlots",
        "showMessage",
        "gainCurrency",
        "dealScaleDamage",
        "drawCards",
        "placeCards",
        "transformCards",
        "changeAppearance",
        "buffCards",
        "moveCards",
        "damageSlots",
        "attackSlots",
        "customActions"
    };

    /* the delays in runactions() seem misplaced! removing them seems to help a lot with the
     * delay issues people were having.
     *
     * i think keeping delays action-specific rather than putting them here is probably the
     * best thing to do! */

    public static IEnumerator RunActions(AbilityBehaviourData abilitydata, PlayableCard self, Object ability = null)
    {
        //Plugin.Log.LogInfo($"This behaviour has the trigger: {abilitydata.trigger.triggerType}");

        abilitydata.self = self;

        if (ability.GetType().IsEnum)
        {
            if (ability is Ability)
            {
                abilitydata.ability = (Ability)ability;
            }
            else if (ability is SpecialTriggeredAbility)
            {
                abilitydata.specialAbility = (SpecialTriggeredAbility)ability;
            }
            else
            {
                abilitydata.specialStatIcon = (SpecialStatIcon?)ability;
            }
        }
        else
        {
            abilitydata.consumableItem = (string)ability;
        }

        List<string> CompleteActionOrder = DefaultActionOrder;
        if (abilitydata.actionOrder != null)
        {
            for (int i = 0; i < (abilitydata.actionOrder.Count - 1); i++)
            {
                string currentAction = abilitydata.actionOrder[i];
                string nextAction = abilitydata.actionOrder[i + 1];

                if (CompleteActionOrder.IndexOf(currentAction) > CompleteActionOrder.IndexOf(nextAction))
                {
                    CompleteActionOrder.Remove(currentAction);
                    CompleteActionOrder.Insert(CompleteActionOrder.IndexOf(nextAction), currentAction);
                }
            }
        }

        // yield return new WaitForSeconds(0.3f);
        View OriginalView = Singleton<ViewManager>.Instance.CurrentView;
        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;

        foreach (string action in CompleteActionOrder)
        {
            switch (action)
            {
                case nameof(AbilityBehaviourData.chooseSlots):

                    if (abilitydata.chooseSlots != null)
                    {
                        foreach (chooseSlot chooseslotdata in abilitydata.chooseSlots)
                        {
                            CoroutineWithData chosenslotdata = new CoroutineWithData(
                                JLPlugin.Data.chooseSlot.ChooseSlot(abilitydata, chooseslotdata, self.slot));
                            yield return JLPlugin.Data.chooseSlot.ChooseSlot(abilitydata, chooseslotdata, self.slot);

                            abilitydata.generatedVariables[
                                "ChosenSlot(" + (abilitydata.chooseSlots.IndexOf(chooseslotdata) + 1).ToString() +
                                ")"] = (chosenslotdata.result as CardSlot);
                        }
                    }

                    break;

                case nameof(AbilityBehaviourData.showMessage):

                    if (abilitydata.showMessage != null)
                    {
                        yield return messageData.showMessage(abilitydata);
                    }

                    break;

                case nameof(AbilityBehaviourData.gainCurrency):

                    if (abilitydata.gainCurrency != null)
                    {
                        yield return gainCurrency.GainCurrency(abilitydata);
                    }

                    break;

                case nameof(AbilityBehaviourData.dealScaleDamage):

                    if (abilitydata.dealScaleDamage != null)
                    {
                        yield return dealScaleDamage.DealScaleDamage(abilitydata);
                    }

                    break;

                case nameof(AbilityBehaviourData.drawCards):

                    if (abilitydata.drawCards != null)
                    {
                        yield return drawCards.DrawCards(abilitydata);
                    }

                    break;

                case nameof(AbilityBehaviourData.placeCards):

                    if (abilitydata.placeCards != null)
                    {
                        yield return placeCards.PlaceCards(abilitydata);
                    }

                    break;

                case nameof(AbilityBehaviourData.transformCards):

                    if (abilitydata.transformCards != null)
                    {
                        yield return transformCards.TransformCards(abilitydata);
                    }

                    break;

                case nameof(AbilityBehaviourData.changeAppearance):

                    if (abilitydata.changeAppearance != null)
                    {
                        yield return changeAppearance.ChangeAppearance(abilitydata);
                    }

                    break;

                case nameof(AbilityBehaviourData.buffCards):

                    if (abilitydata.buffCards != null)
                    {
                        yield return buffCards.BuffCards(abilitydata);
                    }

                    break;

                case nameof(AbilityBehaviourData.moveCards):

                    if (abilitydata.moveCards != null)
                    {
                        yield return moveCards.MoveCards(abilitydata);
                    }

                    break;

                case nameof(AbilityBehaviourData.damageSlots):

                    if (abilitydata.damageSlots != null)
                    {
                        yield return damageSlots.DamageSlots(abilitydata);
                    }

                    break;

                case nameof(AbilityBehaviourData.attackSlots):

                    if (abilitydata.attackSlots != null)
                    {
                        yield return attackSlots.AttackSlots(abilitydata);
                    }

                    break;

                case nameof(AbilityBehaviourData.customActions):

                    yield return customActions.runCustomActions(abilitydata);
                    break;

                default:
                    break;
            }
        }

        // yield return new WaitForSeconds(0.6f);
        Singleton<ViewManager>.Instance.SwitchToView(OriginalView, false, false);
        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
        yield break;
    }

    public static DialogueEvent.LineSet SetAbilityInfoDialogue(string dialogue)
    {
        return new DialogueEvent.LineSet(new List<DialogueEvent.Line>
        {
            new DialogueEvent.Line
            {
                text = dialogue
            }
        });
    }
}