using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Localizing;
using JSONLoader.API;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using TinyJson;
using UnityEngine;

namespace JLPlugin.V2.Data
{
    public class CardSerializeInfo : IInitializable
    {
        public const string DEFAULT_MOD_PREFIX = "JSON";

        public string name;

        public string modPrefix;

        public string[] decals;

        public LocalizableField displayedName;

        public LocalizableField description;

        public int? baseAttack;

        public int? baseHealth;

        public int? bloodCost;

        public int? bonesCost;

        public int? energyCost;

        public string[] gemsCost;

        public string[] abilities;

        public string[] specialAbilities;

        public string specialStatIcon;

        public string[] metaCategories;

        public string cardComplexity;

        public bool? onePerDeck;

        public string temple;

        public string titleGraphic;

        public bool? hideAttackAndHealth;

        public string[] appearanceBehaviour;

        public string texture;

        public string emissionTexture;

        public string holoPortraitPrefab;

        public string animatedPortrait;

        public string altTexture;

        public string altEmissionTexture;

        public string pixelTexture;

        public string[] tribes;

        public string[] traits;

        public string evolveIntoName;

        public int? evolveTurns;

        public string defaultEvolutionName;

        public string tailName;

        public string tailLostPortrait;

        public string iceCubeName;

        public bool? flipPortraitForStrafe;

        public Dictionary<string, string> extensionProperties;

        public string filePath;

        public CardSerializeInfo()
        {
            Initialize();
        }

        public void Initialize()
        {
            displayedName = new("displayedName"); // displayedName, displayedName_es... etc
            description = new("description"); // description, description_ko... etc
        }

        public static void Apply(CardInfo cardInfo, CardSerializeInfo serializeInfo, bool toCardInfo, string cardName)
        {
            ImportExportUtils.SetID(cardName);

            ImportExportUtils.ApplyLocaleField("displayedName", ref serializeInfo.displayedName, ref cardInfo.displayedName, toCardInfo);
            ImportExportUtils.ApplyLocaleField("description", ref serializeInfo.description, ref cardInfo.description, toCardInfo);

            ImportExportUtils.ApplyProperty(() => cardInfo.name, (a) => cardInfo.name = a, ref serializeInfo.name, toCardInfo, "Cards", "name");
            ImportExportUtils.ApplyValue(ref cardInfo.baseAttack, ref serializeInfo.baseAttack, toCardInfo, "Cards", "baseAttack");
            ImportExportUtils.ApplyValue(ref cardInfo.baseHealth, ref serializeInfo.baseHealth, toCardInfo, "Cards", "baseHealth");
            ImportExportUtils.ApplyValue(ref cardInfo.cost, ref serializeInfo.bloodCost, toCardInfo, "Cards", "cost");
            ImportExportUtils.ApplyValue(ref cardInfo.bonesCost, ref serializeInfo.bonesCost, toCardInfo, "Cards", "bonesCost");
            ImportExportUtils.ApplyValue(ref cardInfo.energyCost, ref serializeInfo.energyCost, toCardInfo, "Cards", "energyCost");
            ImportExportUtils.ApplyValue(ref cardInfo.gemsCost, ref serializeInfo.gemsCost, toCardInfo, "Cards", "gemsCost");
            ImportExportUtils.ApplyValue(ref cardInfo.abilities, ref serializeInfo.abilities, toCardInfo, "Cards", "abilities");
            ImportExportUtils.ApplyValue(ref cardInfo.specialAbilities, ref serializeInfo.specialAbilities, toCardInfo, "Cards", "specialAbilities");
            ImportExportUtils.ApplyValue(ref cardInfo.specialStatIcon, ref serializeInfo.specialStatIcon, toCardInfo, "Cards", "specialStatIcon");
            ImportExportUtils.ApplyValue(ref cardInfo.metaCategories, ref serializeInfo.metaCategories, toCardInfo, "Cards", "metaCategories");
            ImportExportUtils.ApplyValue(ref cardInfo.cardComplexity, ref serializeInfo.cardComplexity, toCardInfo, "Cards", "cardComplexity");
            ImportExportUtils.ApplyValue(ref cardInfo.onePerDeck, ref serializeInfo.onePerDeck, toCardInfo, "Cards", "onePerDeck");
            ImportExportUtils.ApplyValue(ref cardInfo.temple, ref serializeInfo.temple, toCardInfo, "Cards", "temple");
            ImportExportUtils.ApplyValue(ref cardInfo.titleGraphic, ref serializeInfo.titleGraphic, toCardInfo, "Cards", "titleGraphic");
            ImportExportUtils.ApplyValue(ref cardInfo.hideAttackAndHealth, ref serializeInfo.hideAttackAndHealth, toCardInfo, "Cards", "hideAttackAndHealth");
            ImportExportUtils.ApplyValue(ref cardInfo.appearanceBehaviour, ref serializeInfo.appearanceBehaviour, toCardInfo, "Cards", "appearanceBehaviour");
            ImportExportUtils.ApplyValue(ref cardInfo.tribes, ref serializeInfo.tribes, toCardInfo, "Cards", "tribes");
            ImportExportUtils.ApplyValue(ref cardInfo.traits, ref serializeInfo.traits, toCardInfo, "Cards", "traits");
            ImportExportUtils.ApplyValue(ref cardInfo.defaultEvolutionName, ref serializeInfo.defaultEvolutionName, toCardInfo, "Cards", "defaultEvolutionName");
            ImportExportUtils.ApplyValue(ref cardInfo.flipPortraitForStrafe, ref serializeInfo.flipPortraitForStrafe, toCardInfo, "Cards", "flipPortraitForStrafe");
            ImportExportUtils.ApplyValue(ref cardInfo.decals, ref serializeInfo.decals, toCardInfo, "Cards", "decals");
            ImportExportUtils.ApplyValue(ref cardInfo.portraitTex, ref serializeInfo.texture, toCardInfo, "Cards", $"texture");
            ImportExportUtils.ApplyValue(ref cardInfo.alternatePortrait, ref serializeInfo.altTexture, toCardInfo, "Cards", $"altTexture");
            ImportExportUtils.ApplyValue(ref cardInfo.pixelPortrait, ref serializeInfo.pixelTexture, toCardInfo, "Cards", $"pixelTexture");

            // Emissions
            Sprite emissivePortrait = cardInfo.GetEmissivePortrait();
            Sprite emissiveAltPortrait = cardInfo.GetEmissiveAltPortrait();
            ImportExportUtils.ApplyValue(ref emissivePortrait, ref serializeInfo.emissionTexture, toCardInfo, "Cards", $"emissionTexture");
            ImportExportUtils.ApplyValue(ref emissiveAltPortrait, ref serializeInfo.altEmissionTexture, toCardInfo, "Cards", $"altEmissionTexture");
            if (cardInfo.portraitTex != null && emissivePortrait != null)
                cardInfo.SetEmissivePortrait(emissivePortrait);
            if (cardInfo.alternatePortrait != null && emissiveAltPortrait != null)
                cardInfo.SetEmissiveAltPortrait(emissiveAltPortrait);

            // Evolutions
            if (toCardInfo)
            {
                if (!string.IsNullOrEmpty(serializeInfo.evolveIntoName))
                    cardInfo.SetEvolve(serializeInfo.evolveIntoName, serializeInfo.evolveTurns.HasValue ? serializeInfo.evolveTurns.Value : 1);
            }
            else
            {
                if (cardInfo.evolveParams != null)
                {
                    serializeInfo.evolveIntoName = cardInfo.evolveParams.evolution?.name;
                    serializeInfo.evolveTurns = cardInfo.evolveParams.turnsToEvolve;
                }
            }

            // Tails
            if (toCardInfo)
            {
                if (!string.IsNullOrEmpty(serializeInfo.tailName))
                    cardInfo.SetTail(serializeInfo.tailName, serializeInfo.tailLostPortrait);
            }
            else
            {
                if (cardInfo.tailParams != null)
                {
                    serializeInfo.tailName = cardInfo.tailParams.tail?.name;
                    ImportExportUtils.ApplyValue(ref cardInfo.tailParams.tailLostPortrait, ref serializeInfo.tailLostPortrait, toCardInfo, "Cards", $"tailLostPortrait");
                }
            }

            // IceCube
            if (toCardInfo)
            {
                if (!string.IsNullOrEmpty(serializeInfo.iceCubeName))
                    cardInfo.SetIceCube(serializeInfo.iceCubeName);
            }
            else
            {
                if (cardInfo.iceCubeParams != null)
                {
                    serializeInfo.iceCubeName = cardInfo.iceCubeParams.creatureWithin.name;
                }
            }

            // Extension Properties
            if (toCardInfo)
            {
                if (serializeInfo.extensionProperties != null)
                    foreach (var item in serializeInfo.extensionProperties)
                        cardInfo.SetExtendedProperty(item.Key, item.Value);

                cardInfo.SetExtendedProperty("JSONFilePath", serializeInfo.filePath);
            }
            else
            {
                Dictionary<string, string> dictionary = CardManager.GetCardExtensionTable(cardInfo);
                if (dictionary != null && dictionary.Count > 0)
                {
                    foreach (KeyValuePair<string, string> pair in dictionary)
                    {
                        if (pair.Key == "ModPrefix")
                        {
                            serializeInfo.modPrefix = pair.Value;
                        }
                    }
                    serializeInfo.extensionProperties = dictionary;
                }
            }

            // Holo Prefabs
            if (toCardInfo)
            {
                if (!string.IsNullOrEmpty(serializeInfo.holoPortraitPrefab))
                    cardInfo.holoPortraitPrefab = Resources.Load<GameObject>(serializeInfo.holoPortraitPrefab);
            }
            else
            {
                // TODO: Prefabs
                if (cardInfo.holoPortraitPrefab != null)
                    serializeInfo.holoPortraitPrefab = cardInfo.holoPortraitPrefab.ToString();
            }

            // Animated Portraits
            if (toCardInfo)
            {
                if (!string.IsNullOrEmpty(serializeInfo.animatedPortrait))
                    cardInfo.animatedPortrait = Resources.Load<GameObject>(serializeInfo.animatedPortrait);
            }
            else
            {
                // TODO: Prefabs
                if (cardInfo.animatedPortrait != null)
                    serializeInfo.holoPortraitPrefab = cardInfo.animatedPortrait.ToString();
            }
        }

        private void ApplyLocaleField(string field, LocalizableField rows, out string cardInfoEnglishField)
        {
            if (rows.rows.TryGetValue(rows.englishFieldName, out string english))
            {
                cardInfoEnglishField = english;
            }
            else if (rows.rows.Count > 0)
            {
                cardInfoEnglishField = rows.rows.First().Value;
            }
            else
            {
                cardInfoEnglishField = null;
                return;
            }

            foreach (KeyValuePair<string, string> pair in rows.rows)
            {
                if (pair.Key == rows.englishFieldName)
                    continue;

                int indexOf = pair.Key.LastIndexOf("_", StringComparison.Ordinal);
                if (indexOf < 0)
                    continue;

                // Translations
                int length = pair.Key.Length - indexOf - 1;
                string code = pair.Key.Substring(indexOf + 1, length);
                Language language = LocalizationManager.CodeToLanguage(code);
                if (language != Language.NUM_LANGUAGES)
                {
                    LocalizationManager.Translate(Plugin.PluginGuid, null, cardInfoEnglishField, pair.Value, language);
                }
                else
                {
                    Plugin.Log.LogDebug($"Unknown language code {code} for card {displayedName} in field {field}");
                }
            }
        }


        internal void Apply(bool UpdateCard = false)
        {
            if (string.IsNullOrEmpty(this.name))
                throw new InvalidOperationException("Card cannot have an empty name!");

            CardInfo existingCard = UpdateCard ? ScriptableObjectLoader<CardInfo>.AllData.Find((CardInfo x) => x.name == this.name) : CardManager.BaseGameCards.CardByName(this.name);
            if (existingCard != null)
            {
                Plugin.VerboseLog($"Modifying {this.name}");
                Apply(existingCard, this, true, existingCard.name);
            }
            else
            {
                Plugin.VerboseLog($"New Card {this.name}");
                string localModPrefix = this.modPrefix ?? DEFAULT_MOD_PREFIX;
                CardInfo newCard = ScriptableObject.CreateInstance<CardInfo>();
                newCard.name = this.name.StartsWith($"{localModPrefix}_") ? this.name : $"{localModPrefix}_{this.name}";
                Apply(newCard, this, true, newCard.name);
                CardManager.Add(localModPrefix, newCard);
            }
        }

        internal void Remove()
        {
            if (string.IsNullOrEmpty(this.name))
                throw new InvalidOperationException("Card cannot have an empty name!");
            CardInfo baseGameCard = CardManager.BaseGameCards.CardByName(this.name);
            if (baseGameCard != null)
            {
                throw new InvalidOperationException("Base game cards cannot be removed!");
            }
            else
            {
                // Remove from NewCards using reflection
                ObservableCollection<CardInfo> NewCards = (ObservableCollection<CardInfo>)typeof(CardManager)
                    .GetField("NewCards", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);

                CardInfo card = NewCards.FirstOrDefault(x => x.name == this.name);
                if (card != default)
                {
                    CardManager.Remove(card);
                }
                else
                {
                    Plugin.Log.LogWarning($"Cannot remove {this.name}");
                }
            }
        }

        internal CardInfo ToCardInfo()
        {
            if (string.IsNullOrEmpty(this.name))
                throw new InvalidOperationException("Card cannot have an empty name!");

            CardInfo baseGameCard = (CardInfo)CardManager.BaseGameCards.CardByName(this.name).Clone();
            if (baseGameCard != null)
            {
                Plugin.Log.LogDebug($"Modifying {this.name}");
                Apply(baseGameCard, this, true, name);
                return baseGameCard;
            }
            else
            {
                string localModPrefix = this.modPrefix ?? DEFAULT_MOD_PREFIX;
                CardInfo newCard = ScriptableObject.CreateInstance<CardInfo>();
                newCard.name = this.name.StartsWith($"{localModPrefix}_") ? this.name : $"{localModPrefix}_{this.name}";
                Apply(newCard, this, true, name);
                return newCard;
            }
        }

        public string WriteToFile(string filename, bool overwrite = true)
        {
            Plugin.Log.LogDebug($"Writing card {this.name ?? "Unnamed"} to {filename}");
            if (!filename.EndsWith("2")) // we now play with jldr2 files
                filename = filename + "2";

            if (overwrite || !File.Exists(filename))
                File.WriteAllText(filename, JSONParser.ToJSON(this));

            return filename;
        }

        public static List<string> fileExtensionExceptions = new List<string>()
        {
            "_encounter",
            "_tribe",
            "_tribes",
            "_sigil",
            "_deck",
            "_gram",
            "_language",
            "_mask",
            "_region",
            "_trait",
            "_item",
            "_talk",
            "_appearence"
        };

        /// <summary>
        /// Assumes files is only cards
        /// </summary>
        /// <param name="files"></param>
        public static void LoadAllJLDR2(List<string> files)
        {
            for (var index = 0; index < files.Count; index++)
            {
                string file = files[index];
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                bool shouldLoad = true;
                List<string> allFileExtensionExceptions = fileExtensionExceptions;
                allFileExtensionExceptions.AddRange(JSONLoaderAPI.customFileExtensionExceptions);
                foreach (string extension in fileExtensionExceptions)
                {
                    if (filename.ToLower().EndsWith($"{extension}.jldr2"))
                    {
                        shouldLoad = false;
                        break;
                    }
                }

                if (!shouldLoad)
                {
                    continue;
                }

                files.RemoveAt(index--);
                ImportExportUtils.SetDebugPath(file);

                try
                {
                    Plugin.VerboseLog($"Loading JLDR2 Card {filename}");
                    CardSerializeInfo cardInfo = JSONParser.FromFilePath<CardSerializeInfo>(file);
                    cardInfo.filePath = file;
                    cardInfo.Apply();
                    Plugin.VerboseLog($"Loaded JSON card from {file}");
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError($"Failed to load card {filename}");
                    Plugin.Log.LogError(ex);
                }
            }
        }

        public static void ExportAllCards()
        {
            Plugin.Log.LogInfo($"Exporting {CardManager.AllCardsCopy.Count} cards.");
            foreach (CardInfo card in CardManager.AllCardsCopy)
            {
                string path = Path.Combine(Plugin.ExportDirectory, "Cards", card.name + ".jldr2");
                ImportExportUtils.SetDebugPath(path);

                CardSerializeInfo info = new CardSerializeInfo();
                info.Initialize();
                Apply(card, info, false, card.name);

                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                info.WriteToFile(path, true);
            }
        }
    }
}