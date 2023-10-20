
using BepInEx;
using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TinyJson;
using UnityEngine;

namespace JLPlugin.Data
{
    using InscryptionAPI.Card;
    using InscryptionAPI.Helpers;
    using SigilCode;
    using static InscryptionAPI.Card.SpecialTriggeredAbilityManager;
    using SigilTuple = Tuple<Type, SigilData>;

    public partial class SigilData
    {
        public void GenerateNew()
        {
            //It might be a good idea to add a check here to see if the trigger is valid
            //and then send an error message if it isn't?

            //Type SigilType = GetType("JLPlugin.SigilCode", this.sigilBase);
            Type SigilType = typeof(ConfigurableMain);

            if (this.isSpecialAbility == true)
            {
                SigilType = typeof(ConfigurableSpecial);

                FullSpecialTriggeredAbility specialAbility = SpecialTriggeredAbilityManager.Add(
                    this.GUID ?? Plugin.PluginGuid,
                    this.name ?? "",
                    SigilType
                    );

                SigilDicts.SpecialArgumentList[specialAbility.Id] = new SigilTuple(SigilType, this);

                return;
            }

            // This is for debugging it should be removed before release
            //var fields = this.GetType()
            //         .GetFields();
            //
            //var values = fields.Select(field => field.GetValue(this)).ToList();
            //
            //List<string> fieldsinfo = new();
            //
            //for (int i = 0; i < fields.Length; ++i)
            //{
            //    Plugin.Log.LogWarning($"{fields[i].Name}: {values[i]}\n");
            //}

            AbilityInfo info = AbilityManager.New(
                    this.GUID ?? Plugin.PluginGuid,
                    this.name ?? "",
                    this.description ?? "",
                    SigilType,
                    this.texture.IsNullOrWhiteSpace() ? new Texture2D(49, 49) : TextureHelper.GetImageAsTexture(this.texture, FilterMode.Point)
                );

            info.SetPixelAbilityIcon(this.pixelTexture.IsNullOrWhiteSpace()
                                    ? new Texture2D(17, 17) :
                                    TextureHelper.GetImageAsTexture(this.pixelTexture, FilterMode.Point));

            info.powerLevel = this.powerLevel ?? 3;

            if (this.metaCategories != null)
            {
                info.AddMetaCategories(this.metaCategories.Select(ImportExportUtils.ParseEnum<AbilityMetaCategory>).ToArray());
            }
            else
            {
                info.SetDefaultPart1Ability();
            }

            info.canStack = this.canStack ?? false;
            info.opponentUsable = this.opponentUsable ?? false;

            //Is this custom dialogue events? can we not use API?
            info.abilityLearnedDialogue = SetAbilityInfoDialogue(this.abilityLearnedDialogue) ?? new DialogueEvent.LineSet();

            info.activated = this.abilityBehaviour.Any(x => x.trigger?.triggerType == "OnActivate");

            // Below is an example of the TriggerType enum being used. This current implementation doesn't make good use of it as the enum
            // would need converted from the string each time. Perhaps there's a method where the list of strings are converted and stored
            // but this is something for down the line. None the less, the TriggerType enum exists in the same file as the Trigger class.

            //info.activated = this.abilityBehaviour.Any( x => ParseEnum<TriggerType>( x.trigger?.triggerType ) == TriggerType.OnActivate );

            SigilDicts.ArgumentList[info.ability] = new SigilTuple(SigilType, this);
        }

        public static SigilData GetAbilityArguments(Ability ability)
        {
            SigilTuple data;
            return SigilDicts.ArgumentList.TryGetValue(ability, out data) ? data.Item2 : null;
        }

        public static SigilData GetAbilityArguments(SpecialTriggeredAbility ability)
        {
            SigilTuple data;
            return SigilDicts.SpecialArgumentList.TryGetValue(ability, out data) ? data.Item2 : null;
        }

        public static void LoadAllSigils(List<string> files)
        {
            for (int index = 0; index < files.Count; index++)
            {
                string file = files[index];
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (!filename.EndsWith("_sigil.jldr2"))
                    continue;
                
                files.RemoveAt(index--);
                
                Plugin.VerboseLog($"Loading JLDR2 (sigil) {filename}");
                SigilData sigilInfo = JSONParser.FromJson<SigilData>(File.ReadAllText(file));
                sigilInfo.GenerateNew();
                Plugin.VerboseLog($"Loaded JSON sigil {sigilInfo.name}");
            }
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

        public static object ConvertArgumentToType(string value, AbilityBehaviourData abilitydata, Type type, bool sendDebug = true)
        {
            if (value == null)
            {
                return null;
            }

            object output = Interpreter.Process(value, abilitydata, type, sendDebug);
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
            if (value == null)
            {
                return null;
            }

            //if (value.Contains("|"))
            //{
            //regex instead of splitting so it does not mistake the or operator (||) for randomization
            //    var random = new Random();
            //    MatchCollection randomMatchList = Regex.Matches(value, @"(?:(?:\((?>[^()]+|\((?<number>)|\)(?<-number>))*(?(number)(?!))\))|[^|])+");
            //    List<string> StringList = randomMatchList.Cast<Match>().Select(match => match.Value).ToList();
            //    value = StringList[random.Next(StringList.Count)];
            //}

            return Interpreter.Process(value, abilitydata, null, sendDebug).ToString();
        }

        public static List<string> ConvertArgument(List<string> value, AbilityBehaviourData abilitydata, bool sendDebug = true)
        {
            if (value == null)
            {
                return null;
            }

            return value.Select(x => Interpreter.Process(x, abilitydata, null, sendDebug).ToString()).ToList();
        }

        public static Type GetType(string nameSpace, string typeName)
        {
            string text = nameSpace + "." + typeName;
            Type type = Type.GetType(text);
            if (type != null)
            {
                return type;
            }
            if (text.Contains("."))
            {
                Assembly assembly = Assembly.Load(text.Substring(0, text.IndexOf('.')));
                if (assembly == null)
                {
                    return null;
                }
                type = assembly.GetType(text);
                if (type != null)
                {
                    return type;
                }
            }
            AssemblyName[] referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            for (int i = 0; i < referencedAssemblies.Length; i++)
            {
                Assembly assembly2 = Assembly.Load(referencedAssemblies[i]);
                if (assembly2 != null)
                {
                    type = assembly2.GetType(text);
                    if (type != null)
                    {
                        return type;
                    }
                }
            }
            return null;
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
            "buffCards",
            "moveCards",
            "damageSlots",
            "attackSlots"
        };

        /* the delays in runactions() seem misplaced! removing them seems to help a lot with the
         * delay issues people were having.
         *
         * i think keeping delays action-specific rather than putting them here is probably the
         * best thing to do! */

        public static IEnumerator RunActions(AbilityBehaviourData abilitydata, PlayableCard self, Ability ability = new Ability())
        {
            //Plugin.Log.LogInfo($"This behaviour has the trigger: {abilitydata.trigger.triggerType}");

            abilitydata.self = self;
            abilitydata.ability = ability;

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
                                CoroutineWithData chosenslotdata = new CoroutineWithData(Data.chooseSlot.ChooseSlot(abilitydata, chooseslotdata, self.slot));
                                yield return Data.chooseSlot.ChooseSlot(abilitydata, chooseslotdata, self.slot);

                                abilitydata.generatedVariables["ChosenSlot(" + (abilitydata.chooseSlots.IndexOf(chooseslotdata) + 1).ToString() + ")"] = (chosenslotdata.result as CardSlot);
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

                    default:
                        break;
                }
            }

            // yield return new WaitForSeconds(0.6f);
            Singleton<ViewManager>.Instance.SwitchToView(OriginalView, false, false);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            yield break;
        }

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

            Dictionary<string, object> GeneratedVariableDictionary = new Dictionary<string, object>()
            {
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
    }
}
