using BepInEx;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using InscryptionAPI.Localizing;
using JSONLoader.V2Code;
using TinyJson;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace JLPlugin.V2.Data
{
    public class CardSerializeInfo
    {
        public const string DEFAULT_MOD_PREFIX = "JSON";
        private static FieldInfo[] PUBLIC_FIELD_INFOS = typeof(CardSerializeInfo).GetFields(BindingFlags.Instance | BindingFlags.Public);

        public string name;

        public string modPrefix;

        public string[] decals;

        public LocalizableField displayedName = new("displayedName"); // displayedName, displayedName_es... etc

        public LocalizableField description = new("description"); // description, description_ko... etc

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

        
        public static void Apply(CardInfo cardInfo, CardSerializeInfo serializeInfo, bool toCardInfo, string cardName)
        {
            ImportExportUtils.ApplyLocaleField("displayedName", ref serializeInfo.displayedName, ref cardInfo.displayedName, toCardInfo);
            ImportExportUtils.ApplyLocaleField("description", ref serializeInfo.description, ref cardInfo.description, toCardInfo);

            ImportExportUtils.ApplyValue(ref cardInfo.baseAttack, ref serializeInfo.baseAttack, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.baseHealth, ref serializeInfo.baseHealth, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.cost, ref serializeInfo.bloodCost, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.bonesCost, ref serializeInfo.bonesCost, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.energyCost, ref serializeInfo.energyCost, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.gemsCost, ref serializeInfo.gemsCost, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.abilities, ref serializeInfo.abilities, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.specialAbilities, ref serializeInfo.specialAbilities, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.specialStatIcon, ref serializeInfo.specialStatIcon, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.metaCategories, ref serializeInfo.metaCategories, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.cardComplexity, ref serializeInfo.cardComplexity, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.onePerDeck, ref serializeInfo.onePerDeck, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.temple, ref serializeInfo.temple, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.titleGraphic, ref serializeInfo.titleGraphic, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.hideAttackAndHealth, ref serializeInfo.hideAttackAndHealth, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.appearanceBehaviour, ref serializeInfo.appearanceBehaviour, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.tribes, ref serializeInfo.tribes, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.traits, ref serializeInfo.traits, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.defaultEvolutionName, ref serializeInfo.defaultEvolutionName, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.flipPortraitForStrafe, ref serializeInfo.flipPortraitForStrafe, toCardInfo);
            ImportExportUtils.ApplyValue(ref cardInfo.onePerDeck, ref serializeInfo.onePerDeck, toCardInfo);

            if (toCardInfo)
            {
                if (serializeInfo.decals != null && serializeInfo.decals.Length > 0)
                    cardInfo.AddDecal(serializeInfo.decals);
            }
            else
            {
                // TODO: Image
                if (cardInfo.decals != null)
                    serializeInfo.decals = ImportExportUtils.ExportTextures(cardInfo.decals.Cast<Texture2D>(), $"{cardName}_decals");
            }

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
                    serializeInfo.tailLostPortrait = cardInfo.tailParams.tailLostPortrait?.ToString(); // TODO: Image
                }
            }

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

            if (toCardInfo)
            {
                if (serializeInfo.extensionProperties != null)
                    foreach (var item in serializeInfo.extensionProperties)
                        cardInfo.SetExtendedProperty(item.Key, item.Value);
                
                cardInfo.SetExtendedProperty("JSONFilePath", serializeInfo.filePath);
            }
            else
            {
                Dictionary<string,string> dictionary = CardManager.GetCardExtensionTable(cardInfo);
                if (dictionary != null && dictionary.Count > 0)
                {
                    serializeInfo.extensionProperties = dictionary;
                }
            }

            if (toCardInfo)
            {
                if (!string.IsNullOrEmpty(serializeInfo.texture))
                    cardInfo.SetPortrait(serializeInfo.texture);
            }
            else
            {
                if (cardInfo.portraitTex != null)
                {
                    Texture2D t = cardInfo.portraitTex.texture;
                    ImportExportUtils.ApplyValue(ref t, ref serializeInfo.texture, false, $"{cardName}_texture");
                }
            }
            
            if (toCardInfo)
            {
                if (!string.IsNullOrEmpty(serializeInfo.altTexture))
                    cardInfo.SetAltPortrait(serializeInfo.altTexture);
            }
            else
            {
                if (cardInfo.alternatePortrait != null)
                {
                    Texture2D t = cardInfo.alternatePortrait.texture;
                    ImportExportUtils.ApplyValue(ref t, ref serializeInfo.altTexture, false, $"{cardName}_altTexture");
                }
            }

            if (toCardInfo)
            {
                if (!string.IsNullOrEmpty(serializeInfo.emissionTexture))
                    cardInfo.SetEmissivePortrait(serializeInfo.emissionTexture);
            }
            else
            {
                Sprite sprite = cardInfo.GetEmissivePortrait();
                if (sprite != null)
                {
                    Texture2D t = sprite.texture;
                    ImportExportUtils.ApplyValue(ref t, ref serializeInfo.emissionTexture, false, $"{cardName}_emissionTexture");
                }
            }

            if (toCardInfo)
            {
                if (!string.IsNullOrEmpty(serializeInfo.holoPortraitPrefab))
                    cardInfo.holoPortraitPrefab = Resources.Load<GameObject>(serializeInfo.holoPortraitPrefab);
            }
            else
            {
                // TODO: Prefabs
                if(cardInfo.holoPortraitPrefab != null)
                    serializeInfo.holoPortraitPrefab = cardInfo.holoPortraitPrefab.ToString();
            }

            if (toCardInfo)
            {
                if (!string.IsNullOrEmpty(serializeInfo.animatedPortrait))
                    cardInfo.animatedPortrait = Resources.Load<GameObject>(serializeInfo.animatedPortrait);
            }
            else
            {
                // TODO: Prefabs
                if(cardInfo.animatedPortrait != null)
                    serializeInfo.holoPortraitPrefab = cardInfo.animatedPortrait.ToString();
            }

            if (toCardInfo)
            {
                if (!string.IsNullOrEmpty(serializeInfo.altEmissionTexture))
                    cardInfo.SetEmissiveAltPortrait(serializeInfo.altEmissionTexture);
            }
            else
            {
                Sprite sprite = cardInfo.GetEmissiveAltPortrait();
                if (sprite != null)
                {
                    Texture2D t = sprite.texture;
                    ImportExportUtils.ApplyValue(ref t, ref serializeInfo.altEmissionTexture, false, $"{cardName}_altEmissionTexture");
                }
            }

            if (toCardInfo)
            {
                if (!string.IsNullOrEmpty(serializeInfo.pixelTexture))
                    cardInfo.SetPixelPortrait(serializeInfo.pixelTexture);
            }
            else
            {
                if (cardInfo.pixelPortrait != null)
                {
                    Texture2D t = cardInfo.pixelPortrait.texture;
                    ImportExportUtils.ApplyValue(ref t, ref serializeInfo.texture, false, $"{cardName}_pixelTexture");
                }
            }
        }

        private void ApplyLocaleField(string field, LocalizableField rows, out string cardInfoEnglishField)
        {
            if (rows.rows.TryGetValue(rows.englishFieldName, out string english))
            {
                cardInfoEnglishField = english;
            }
            else if(rows.rows.Count > 0)
            {
                cardInfoEnglishField = rows.rows.First().Value;
            }
            else
            {
                cardInfoEnglishField = null;
                return;
            }

            foreach (KeyValuePair<string,string> pair in rows.rows)
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
                Plugin.VerboseLog($"Modifying {this.name} using {this.ToJSON()}");
                Apply(existingCard, this, true, existingCard.name);
            }
            else
            {
                Plugin.VerboseLog($"New Card {this.name} using {this.ToJSON()}");
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
                Plugin.Log.LogDebug($"Modifying {this.name} using {this.ToJSON()}");
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
                File.WriteAllText(filename, this.ToJSON());

            return filename;
        }

        private string ToJSON()
        {
            string retval = "{\n";

            foreach (FieldInfo field in PUBLIC_FIELD_INFOS)
            {
                if (field.FieldType == typeof(string))
                {
                    string fieldVal = (string)field.GetValue(this);
                    if (!string.IsNullOrEmpty(fieldVal))
                        retval += $"\t\"{field.Name}\": \"{fieldVal}\",\n";
                }
                else if (field.FieldType == typeof(string[]))
                {
                    string[] fieldVal = (string[])field.GetValue(this);
                    if (fieldVal != null && fieldVal.Length > 0)
                        retval += $"\t\"{field.Name}\": [{string.Join(",", fieldVal.Select(v => $"\"{v}\""))}],\n";
                }
                else if (field.FieldType == typeof(int?))
                {
                    int? fieldVal = (int?)field.GetValue(this);
                    if (fieldVal.HasValue)
                        retval += $"\t\"{field.Name}\": {fieldVal.Value},\n";
                }
                else if (field.FieldType == typeof(bool?))
                {
                    bool? fieldVal = (bool?)field.GetValue(this);
                    if (fieldVal.HasValue)
                        retval += $"\t\"{field.Name}\": {(fieldVal.Value ? "true" : "false")},\n";
                }
                else if (field.FieldType == typeof(Dictionary<string, string>))
                {
                    Dictionary<string, string> fieldVal = (Dictionary<string, string>)field.GetValue(this);
                    if (fieldVal != null && fieldVal.Count > 0)
                    {
                        retval += $"\t\"{field.Name}\": {{\n";
                        foreach (var val in fieldVal.Where(kvp => !string.IsNullOrEmpty(kvp.Key) && !string.IsNullOrEmpty(kvp.Value)))
                            retval += $"\t\t\"{val.Key}\": \"{val.Value}\",\n";
                        retval += $"\t}},\n";
                    }
                }
                else if (field.FieldType.IsAssignableFrom(typeof(JSONParser.IFlexibleField)))
                {
                    object value = field.GetValue(this);
                    if (value != null)
                    {
                        retval += ((JSONParser.IFlexibleField)value).ToJSON();
                    }

                    Dictionary<string, string> fieldVal = ((LocalizableField)field.GetValue(this)).rows;
                    foreach (KeyValuePair<string,string> pair in fieldVal)
                    {
                        retval += $"\t\"{pair.Key}\": {pair.Value},\n";
                    }
                }
            }
            retval = retval.TrimEnd('\n', ',') + "\n}";
            return retval;
        }

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

                files.RemoveAt(index--);

                Plugin.Log.LogDebug($"Loading JLDR2 Card {filename}");
                try
                {
                    CardSerializeInfo cardInfo = JSONParser.FromJson<CardSerializeInfo>(File.ReadAllText(file));
                    cardInfo.filePath = file;
                    cardInfo.Apply();
                    Plugin.Log.LogDebug($"Loaded JSON card {cardInfo.name}");
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError($"Failed to load {filename}: {ex.Message}");
                    Plugin.Log.LogError(ex);
                }
            }
        }

    }

    [Serializable]
    public class LocalizableField : JSONParser.IFlexibleField
    {
        public Dictionary<string, string> rows;

        public string englishFieldName;

        public LocalizableField(string EnglishFieldName)
        {
            rows = new Dictionary<string, string>();
            englishFieldName = EnglishFieldName;
        }

        public void Initialize(string englishValue)
        {
            rows[englishFieldName] = englishValue;
        }
        
        public bool ContainsKey(string key)
        {
            return key.StartsWith(englishFieldName);
        }

        public void SetValue(string key, string value)
        {
            rows[key] = value;
        }

        public string ToJSON()
        {
            string json = "";
            foreach (KeyValuePair<string,string> pair in rows)
            {
                json += $"\t\"{pair.Key}\": \"{pair.Value}\",\n";
            }
            
            return json;
        }

        public override string ToString()
        {
            return rows.ToString();
        }
    }
}