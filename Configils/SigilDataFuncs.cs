using BepInEx;
using DiskCardGame;
using InscryptionAPI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TinyJson;
using UnityEngine;
using Random = System.Random;

namespace JLPlugin.Data
{
    using JLPlugin.SigilCode;
    using NCalc;
    using System.Text.RegularExpressions;
    using Utils;

    public partial class SigilData
    {
        public void GenerateNew()
        {
            //Type SigilType = GetType("JLPlugin.SigilCode", this.sigilBase);
            Type SigilType = typeof(ConfigurableMain);

            var fields = this.GetType()
                     .GetFields()
                     .Select(field => field.GetValue(this))
                     .ToList();

            Plugin.Log.LogInfo(string.Join(",", fields));

            Texture2D sigilTexture = new Texture2D(49, 49);
            if (this.texture != null)
            {
                sigilTexture = CDUtils.Assign(this.texture, nameof(this.texture));
            }
            sigilTexture.filterMode = FilterMode.Point;

            Texture2D sigilPixelTexture = new Texture2D(17, 17);
            if (this.pixelTexture != null)
            {
                sigilPixelTexture = CDUtils.Assign(this.pixelTexture, nameof(this.pixelTexture));
            }
            sigilPixelTexture.filterMode = FilterMode.Point;

            AbilityInfo info = AbilityManager.New(
                    this.GUID ?? "MADH.inscryption.JSONLoader",
                    this.name ?? "",
                    this.description ?? "",
                    SigilType,
                    sigilTexture
                );
            info.SetPixelAbilityIcon(sigilPixelTexture);
            info.powerLevel = this.powerLevel ?? 3;
            info.metaCategories = CDUtils.Assign(this.metaCategories, nameof(this.metaCategories), SigilDicts.AbilityMetaCategory) ?? new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = this.opponentUsable ?? false;
            info.abilityLearnedDialogue = SetAbilityInfoDialogue(this.abilityLearnedDialogue) ?? new DialogueEvent.LineSet();

            SigilDicts.ArgumentList.Add(info.ability, new Tuple<Type, SigilData>(SigilType, this));
        }

        public static SigilData GetAbilityArguments(Ability ability)
        {
            return SigilDicts.ArgumentList[ability].Item2;
        }

        public static void LoadAllSigils()
        {
            foreach (string file in Directory.EnumerateFiles(Paths.PluginPath, "*.jldr2", SearchOption.AllDirectories))
            {
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (filename.EndsWith("_sigil_example.jldr2"))
                {
                    Plugin.Log.LogDebug($"Skipping {filename}");
                    continue;
                }

                if (filename.EndsWith("_sigil.jldr2"))
                {
                    Plugin.Log.LogDebug($"Loading JLDR2 (sigil) {filename}");
                    SigilData sigilInfo = JSONParser.FromJson<SigilData>(File.ReadAllText(file));
                    Plugin.Log.LogInfo(sigilInfo.name);
                    sigilInfo.GenerateNew();
                    Plugin.Log.LogDebug($"Loaded JSON sigil {sigilInfo.name}");
                }
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

        public static string ConvertString(string value, SigilData abilitydata)
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
            Plugin.Log.LogInfo(value + " FUNCTIONS!!!: " + functionMatchList.Cast<Match>().ToList().Count);

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

                    Plugin.Log.LogInfo("FUNCTION: " + functionName + " " + functionContents);
                    string converted = ConvertString(functionContents, abilitydata);
                    value = value.Replace(functionContents, converted);
                }
            }
            //-------------------------------

            //regex for vars, it matches anything in between [ ]
            MatchCollection matchList = Regex.Matches(value, "\\[.*?\\]");
            List<string> variablelist = matchList.Cast<Match>().Select(match => match.Value).ToList();
            Plugin.Log.LogInfo("variables amount + contents: " + variablelist.Count + " " + string.Join(", ", variablelist));

            foreach (string variable in variablelist)
            {
                List<string> fieldList = variable.Replace("[", "").Replace("]", "").Split('.').ToList();

                object generatedReplaceValue = null;
                if (abilitydata.generatedVariables.TryGetValue(fieldList[0], out generatedReplaceValue))
                {
                    fieldList.RemoveAt(0);
                    foreach (string field in fieldList)
                    {
                        Plugin.Log.LogInfo("properties: " + string.Join(", ", generatedReplaceValue.GetType().GetProperties().Select(x => x.Name).ToList()));
                        PropertyInfo propertyOfField = generatedReplaceValue.GetType().GetProperty(field);

                        Plugin.Log.LogInfo("fields: " + string.Join(", ", generatedReplaceValue.GetType().GetFields().Select(x => x.Name).ToList()));
                        Plugin.Log.LogInfo("beforeNULL: " + generatedReplaceValue.ToString());

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
                        Plugin.Log.LogInfo("before tribes!");
                        generatedReplaceValue = "'" + string.Join("', '", ((IEnumerable)generatedReplaceValue).Cast<object>().ToList().Select(x => x.ToString())) + "'";
                    }
                    if (generatedReplaceValue is string)
                    {
                        generatedReplaceValue = "'" + generatedReplaceValue + "'";
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
            Plugin.Log.LogInfo(value);

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

        public static string ConvertArgument(string value, SigilData abilitydata)
        {
            if (value == null)
            {
                return null;
            }

            value = ConvertString(value, abilitydata);
            Plugin.Log.LogInfo("LastValue: " + value);
            return value;
        }
        public static List<string> ConvertArgument(List<String> value, SigilData abilitydata)
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

        public static IEnumerator RunActions(SigilData abilitydata, PlayableCard self, Ability ability)
        {
            Plugin.Log.LogInfo("cardStats: " + self.Info.Attack + "/" + self.Info.Health);
            UpdateVariables(abilitydata, self);

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

            if (abilitydata.placeCards != null)
            {
                yield return Data.placeCards.PlaceCards(abilitydata);
            }

            if (abilitydata.transformCards != null)
            {
                yield return Data.transformCards.TransformCards(abilitydata);
            }

            if (abilitydata.buffCards != null)
            {
                yield return Data.buffCards.BuffCards(abilitydata);
            }

            if (abilitydata.drawCards != null)
            {
                yield return Data.drawCards.DrawCards(abilitydata);
            }
            yield break;
        }

        public static void UpdateVariables(SigilData abilitydata, PlayableCard self)
        {
            abilitydata.variables = new Dictionary<string, string>()
            {
                { "[EnergyAmount]", Singleton<ResourcesManager>.Instance.PlayerBones.ToString() },
                { "[BoneAmount]", Singleton<ResourcesManager>.Instance.PlayerEnergy.ToString() },
                { "[Turn]", Singleton<TurnManager>.Instance.TurnNumber.ToString() }
            };

            abilitydata.generatedVariables = new Dictionary<string, object>()
            {
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
