
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using DiskCardGame;

using BepInEx;

using TinyJson;

using Random = System.Random;

namespace JLPlugin.Data
{
    using System.Text.RegularExpressions;

	using InscryptionAPI.Helpers;
    using InscryptionAPI.Card;

	using NCalc;
    
    using SigilCode;
    
    using static V2.Data.CardSerializeInfo;

    using static InscryptionAPI.Card.SpecialTriggeredAbilityManager;

    using SigilTuple = Tuple<Type, SigilData>;

    public partial class SigilData
    {
        public void GenerateNew()
        {
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

                SigilDicts.SpecialArgumentList.Add(specialAbility.Id, new SigilTuple(SigilType, this));

                return;
            }

            // Is this section necessary? reflection is quite expensive so doing this for every ability will slow some things
            // Still refactored it to use less functions to make it as quick as possible.
            var fields = this.GetType()
                     .GetFields();

            var values = fields.Select(field => field.GetValue(this)).ToList();

            List<string> fieldsinfo = new();

            for (int i = 0; i < fields.Length; ++i )
            {
                Plugin.Log.LogWarning( $"{fields[ i ].Name}: {values[ i ]}\n" );
            }

            //There probably a more API oriented way of handling this, I'm just very confused how that all works atm
            Texture2D sigilPixelTexture = new(17, 17);
            if (this.pixelTexture != null)
            {
                sigilPixelTexture.LoadImage(TextureHelper.ReadArtworkFileAsBytes(this.pixelTexture));
                sigilPixelTexture.filterMode = FilterMode.Point;
            }

            AbilityInfo info = AbilityManager.New(
                    this.GUID ?? Plugin.PluginGuid,
                    this.name ?? "",
                    this.description ?? "",
                    SigilType,
                    this.texture
                );

            info.SetPixelAbilityIcon(sigilPixelTexture);
            info.powerLevel = this.powerLevel ?? 3;

            info.AddMetaCategories(this.metaCategories.Select( elem => ParseEnum<AbilityMetaCategory>( elem ) ).ToArray());
            if (info.metaCategories.Count < 1)
			{
                info.SetDefaultPart1Ability();
			}

            info.canStack = true; // Maybe make this configurable by user?
            info.opponentUsable = this.opponentUsable ?? false;

            //Is this custom dialogue events? can we not use API?
            info.abilityLearnedDialogue = SetAbilityInfoDialogue(this.abilityLearnedDialogue) ?? new DialogueEvent.LineSet();

            info.activated = this.abilityBehaviour.Any(x => x.trigger?.triggerType == "OnActivate");

            // Below is an example of the TriggerType enum being used. This current implementation doesn't make good use of it as the enum
            // would need converted from the string each time. Perhaps there's a method where the list of strings are converted and stored
            // but this is something for down the line. None the less, the TriggerType enum exists in the same file as the Trigger class.

            //info.activated = this.abilityBehaviour.Any( x => ParseEnum<TriggerType>( x.trigger?.triggerType ) == TriggerType.OnActivate );

            SigilDicts.ArgumentList.Add(info.ability, new SigilTuple(SigilType, this));
        }

        public static SigilData GetAbilityArguments(Ability ability)
        {
            return SigilDicts.ArgumentList[ability].Item2;
        }

        public static SigilData GetAbilityArguments(SpecialTriggeredAbility ability)
        {
            return SigilDicts.SpecialArgumentList[ability].Item2;
        }

        public static void LoadAllSigils()
        {
            foreach (string file in Directory.EnumerateFiles(Paths.PluginPath, "_sigil.jldr2", SearchOption.AllDirectories))
            {
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                Plugin.Log.LogDebug($"Loading JLDR2 (sigil) {filename}");
                SigilData sigilInfo = JSONParser.FromJson<SigilData>(File.ReadAllText(file));
                Plugin.Log.LogInfo(sigilInfo.name);
                sigilInfo.GenerateNew();
                Plugin.Log.LogDebug($"Loaded JSON sigil {sigilInfo.name}");
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

        //This could be split up, would make for more readable and usable code. I will give it a go next refactor.
        public static string ConvertString(string value, AbilityBehaviourData abilitydata)
        {
            var random = new Random();

            if (value.Contains("|"))
            {
                //regex instead of splitting so it does not mistake the or operator (||) for randomization
                MatchCollection randomMatchList = Regex.Matches(value, @"(?:(?:\((?>[^()]+|\((?<number>)|\)(?<-number>))*(?(number)(?!))\))|[^|])+");
                List<string> StringList = randomMatchList.Cast<Match>().Select(match => match.Value).ToList();
                value = StringList[random.Next(StringList.Count)];
            }

            //function code, so e.g playerSlot(1 + 1)
            MatchCollection functionMatchList = Regex.Matches(value, @"([a-zA-Z]+)\((.*?)\)");
            List<Match> functionlist = functionMatchList.Cast<Match>().ToList();
            //Plugin.Log.LogInfo(value + " FUNCTIONS!!!: " + functionMatchList.Cast<Match>().ToList().Count);

            foreach (Match function in functionlist)
            {
                string fullFunction = function.Groups[0].Value;
                string functionName = function.Groups[1].Value;
                string functionContents = function.Groups[2].Value;

                if (functionName != "if" && functionName != "in")
                {
                    //custom functions
                    if (functionName == "StringContains")
                    {
                        if (functionContents.Contains(','))
                        {
                            value = value.Replace(fullFunction, ConvertString(Regex.Split(functionContents, ", ")[0], abilitydata).Contains(ConvertString(Regex.Split(functionContents, ", ")[1].Replace("'", ""), abilitydata)).ToString());
                            continue;
                        }
                    }
                    //------------------------------

                    //Plugin.Log.LogInfo("FUNCTION: " + functionName + " " + functionContents);
                    string converted = ConvertString(functionContents, abilitydata);
                    value = value.Replace(functionContents, converted);
                }
            }
            //-------------------------------

            //regex for vars, it matches anything in between [ ]
            MatchCollection matchList = Regex.Matches(value, "\\[.*?\\]");
            List<string> variablelist = matchList.Cast<Match>().Select(match => match.Value).ToList();
            //Plugin.Log.LogInfo("variables amount + contents: " + variablelist.Count + " " + string.Join(", ", variablelist));

            foreach (string variable in variablelist)
            {
                List<string> fieldList = variable.Replace("[", "").Replace("]", "").Split('.').ToList();

                object generatedReplaceValue = null;
                if (abilitydata.generatedVariables.TryGetValue(fieldList[0], out generatedReplaceValue))
                {
                    fieldList.RemoveAt(0);
                    foreach (string field in fieldList)
                    {
                        //Plugin.Log.LogInfo("properties: " + string.Join(", ", generatedReplaceValue.GetType().GetProperties().Select(x => x.Name).ToList()));
                        PropertyInfo propertyOfField = generatedReplaceValue.GetType().GetProperty(field);

                        //Plugin.Log.LogInfo("fields: " + string.Join(", ", generatedReplaceValue.GetType().GetFields().Select(x => x.Name).ToList()));
                        //Plugin.Log.LogInfo("beforeNULL: " + generatedReplaceValue.ToString());

                        try
                        {
                            if (propertyOfField == null)
                            {
                                FieldInfo fieldOfField = generatedReplaceValue.GetType().GetField(field);
                                generatedReplaceValue = fieldOfField.GetValue(generatedReplaceValue);
                            }
                            else
                            {
                                generatedReplaceValue = propertyOfField.GetValue(generatedReplaceValue);
                            }
                        }
                        catch
                        {

                            generatedReplaceValue = "null";
                            break;
                        }

                        if (generatedReplaceValue == null)
                        {
                            generatedReplaceValue = "null";
                            break;
                        }
                    }

                    Plugin.Log.LogInfo("generatedReplaceValue: " + generatedReplaceValue.ToString());
                    if (generatedReplaceValue is IList)
                    {
                        //Plugin.Log.LogInfo("before tribes!");
                        generatedReplaceValue = "'" + string.Join("', '", ((IEnumerable)generatedReplaceValue).Cast<object>().ToList().Select(x => x.ToString())) + "'";
                    }
                    value = value.Replace(variable, generatedReplaceValue.ToString());
                }
                else
                {
                    string replaceValue = null;
                    if (!abilitydata.variables.TryGetValue(variable, out replaceValue))
                    {
                        return null;
                    }
                    value = value.Replace(variable, replaceValue);
                }
            }
            //Plugin.Log.LogInfo(value);

            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                Expression e = new Expression(value);
                value = e.Evaluate().ToString();
            }

            if (value == "True" || value == "False")
            {
                value = value.ToLower();
            }
            return value;
        }

        public static string ConvertArgument(string value, AbilityBehaviourData abilitydata)
        {
            if (value == null)
            {
                return null;
            }

            value = ConvertString(value, abilitydata);
            //Plugin.Log.LogInfo("LastValue: " + value);
            return value;
        }
        public static List<string> ConvertArgument(List<string> value, AbilityBehaviourData abilitydata)
        {
            if (value == null)
            {
                return null;
            }

            return value.Select(x => ConvertString(x, abilitydata)).ToList();
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

        public static IEnumerator RunActions(AbilityBehaviourData abilitydata, PlayableCard self, Ability ability = new Ability())
        {

            abilitydata.self = self;
            abilitydata.ability = ability;

            if (abilitydata.chooseSlots != null)
            {
                foreach (chooseSlot chooseslotdata in abilitydata.chooseSlots)
                {
                    CoroutineWithData chosenslotdata = new CoroutineWithData(Data.chooseSlot.ChooseSlot(abilitydata, chooseslotdata, self.slot));
                    yield return Data.chooseSlot.ChooseSlot(abilitydata, chooseslotdata, self.slot);

                    abilitydata.generatedVariables.Add("ChosenSlot(" + (abilitydata.chooseSlots.IndexOf(chooseslotdata) + 1).ToString() + ")", (chosenslotdata.result as CardSlot) ?? self.slot);
                }
            }
            foreach (var variable in abilitydata.generatedVariables)
            {
                if (variable.Key.Contains("ChosenSlot") && variable.Value == null)
                {
                    yield break;
                }
            }

            if (abilitydata.dealScaleDamage != null)
            {
                int damage = int.Parse(ConvertArgument(abilitydata.dealScaleDamage, abilitydata));
                yield return Singleton<LifeManager>.Instance.ShowDamageSequence(damage, damage, false, 0.125f, null, 0f, true);
            }

            if (abilitydata.drawCards != null)
            {
                yield return drawCards.DrawCards(abilitydata);
            }

            if (abilitydata.placeCards != null)
            {
                yield return placeCards.PlaceCards(abilitydata);
            }

            if (abilitydata.transformCards != null)
            {
                yield return transformCards.TransformCards(abilitydata);
            }

            if (abilitydata.buffCards != null)
            {
                yield return buffCards.BuffCards(abilitydata);
            }

            if (abilitydata.moveCards != null)
            {
                yield return moveCards.MoveCards(abilitydata);
            }

            if (abilitydata.damageSlots != null)
            {
                yield return damageSlots.DamageSlots(abilitydata);
            }

            if (abilitydata.attackSlots != null)
            {
                yield return attackSlots.AttackSlots(abilitydata);
            }

            yield return new WaitForSeconds(0.6f);
            if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            }
            yield break;
        }

        public static void UpdateVariables(AbilityBehaviourData abilitydata, PlayableCard self)
        {
            abilitydata.variables = new Dictionary<string, string>()
            {
                { "[EnergyAmount]", Singleton<ResourcesManager>.Instance.PlayerBones.ToString() },
                { "[BoneAmount]", Singleton<ResourcesManager>.Instance.PlayerEnergy.ToString() },
                { "[Turn]", Singleton<TurnManager>.Instance.TurnNumber.ToString() },
                { "[TurnsInPlay]", (abilitydata.TurnsInPlay ?? 0).ToString() }
            };

            abilitydata.generatedVariables = new Dictionary<string, object>()
            {
                { "DeathSlot", null },
                { "HitSlot", null },
                { "AttackerCard", null },
                { "VictimCard", null },
                { "ChooseableSlot", null },
                { "RandomCardInfo", null },
                { "TriggerCard", null },
                { "BaseCard", self },
                { "PlayerSlot(0)", Singleton<BoardManager>.Instance.playerSlots[0] },
                { "PlayerSlot(1)", Singleton<BoardManager>.Instance.playerSlots[1] },
                { "PlayerSlot(2)", Singleton<BoardManager>.Instance.playerSlots[2] },
                { "PlayerSlot(3)", Singleton<BoardManager>.Instance.playerSlots[3] },
                { "OpponentSlot(0)", Singleton<BoardManager>.Instance.opponentSlots[0] },
                { "OpponentSlot(1)", Singleton<BoardManager>.Instance.opponentSlots[1] },
                { "OpponentSlot(2)", Singleton<BoardManager>.Instance.opponentSlots[2] },
                { "OpponentSlot(3)", Singleton<BoardManager>.Instance.opponentSlots[3] }
            };
        }
    }
}
