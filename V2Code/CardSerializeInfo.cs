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

namespace JLPlugin.V2.Data
{
    public class CardSerializeInfo
    {
        public const string DEFAULT_MOD_PREFIX = "JSON";
        private static FieldInfo[] PUBLIC_FIELD_INFOS = typeof(CardSerializeInfo).GetFields(BindingFlags.Instance | BindingFlags.Public);

        public string name;

        public string modPrefix;

        public string[] decals;

        public string displayedName;
        public string displayedName_fr;
        public string displayedName_it;
        public string displayedName_de;
        public string displayedName_es;
        public string displayedName_pt;
        public string displayedName_tr;
        public string displayedName_ru;
        public string displayedName_ja;
        public string displayedName_ko;
        public string displayedName_zhcn;
        public string displayedName_zhtw;

        public string description;
        public string description_fr;
        public string description_it;
        public string description_de;
        public string description_es;
        public string description_pt;
        public string description_tr;
        public string description_ru;
        public string description_ja;
        public string description_ko;
        public string description_zhcn;
        public string description_zhtw;

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

        public static T ParseEnum<T>(string value) where T : unmanaged, System.Enum
        {
            T result;
            if (Enum.TryParse<T>(value, out result))
                return result;

            int idx = Math.Max(value.LastIndexOf('_'), value.LastIndexOf('.'));

            if (idx < 0)
                throw new InvalidCastException($"Cannot parse {value} as {typeof(T).FullName}");

            string guid = value.Substring(0, idx);
            string name = value.Substring(idx + 1);
            return GuidManager.GetEnumValue<T>(guid, name);
        }

        private void ApplyCardInfo(CardInfo card)
        {
            if (this.decals != null && this.decals.Length > 0)
                card.AddDecal(this.decals);

            ApplyLocalisations(card);

            if (this.baseAttack.HasValue)
                card.baseAttack = this.baseAttack.Value;

            if (this.baseHealth.HasValue)
                card.baseHealth = this.baseHealth.Value;

            if (this.bloodCost.HasValue)
                card.cost = this.bloodCost.Value;

            if (this.bonesCost.HasValue)
                card.bonesCost = this.bonesCost.Value;

            if (this.energyCost.HasValue)
                card.energyCost = this.energyCost.Value;

            if (this.gemsCost != null)
                card.gemsCost = this.gemsCost.Select(s => ParseEnum<GemType>(s)).ToList();

            if (this.abilities != null && this.abilities.Length > 0)
                card.abilities = new(this.abilities.Select(s => ParseEnum<Ability>(s)).ToArray());

            if (this.specialAbilities != null && this.specialAbilities.Length > 0)
                card.specialAbilities = new(this.specialAbilities.Select(s => ParseEnum<SpecialTriggeredAbility>(s)).ToArray());

            if (!string.IsNullOrEmpty(this.specialStatIcon))
                card.specialStatIcon = ParseEnum<SpecialStatIcon>(this.specialStatIcon);

            if (this.metaCategories != null && this.metaCategories.Length > 0)
                card.metaCategories = new(this.metaCategories.Select(s => ParseEnum<CardMetaCategory>(s)).ToArray());

            if (!string.IsNullOrEmpty(this.cardComplexity))
                card.cardComplexity = ParseEnum<CardComplexity>(this.cardComplexity);

            if (this.onePerDeck.HasValue)
                card.onePerDeck = this.onePerDeck.Value;

            if (!string.IsNullOrEmpty(this.temple))
                card.temple = ParseEnum<CardTemple>(this.temple);

            if (!string.IsNullOrEmpty(this.titleGraphic))
                card.titleGraphic = TextureHelper.GetImageAsTexture(this.titleGraphic);

            if (this.hideAttackAndHealth.HasValue)
                card.hideAttackAndHealth = this.hideAttackAndHealth.Value;

            if (this.appearanceBehaviour != null && this.appearanceBehaviour.Length > 0)
                card.appearanceBehaviour = new(this.appearanceBehaviour.Select(s => ParseEnum<CardAppearanceBehaviour.Appearance>(s)).ToArray());

            if (!string.IsNullOrEmpty(this.texture))
                card.SetPortrait(this.texture);
            
            if (!string.IsNullOrEmpty(this.altTexture))
                card.SetAltPortrait(this.altTexture);

            if (!string.IsNullOrEmpty(this.emissionTexture))
                card.SetEmissivePortrait(this.emissionTexture);

            if (!string.IsNullOrEmpty(this.holoPortraitPrefab))
                card.holoPortraitPrefab = Resources.Load<GameObject>(this.holoPortraitPrefab);

            if (!string.IsNullOrEmpty(this.animatedPortrait))
                card.animatedPortrait = Resources.Load<GameObject>(this.animatedPortrait);

            if (!string.IsNullOrEmpty(this.altEmissionTexture))
                card.SetEmissiveAltPortrait(this.altEmissionTexture);

            if (!string.IsNullOrEmpty(this.pixelTexture))
                card.SetPixelPortrait(this.pixelTexture);

            if (this.tribes != null && this.tribes.Length > 0)
                card.tribes = new(this.tribes.Select(s => ParseEnum<Tribe>(s)).ToArray());

            if (this.traits != null && this.traits.Length > 0)
                card.traits = new(this.traits.Select(s => ParseEnum<Trait>(s)).ToArray());

            if (!string.IsNullOrEmpty(this.evolveIntoName))
                card.SetEvolve(this.evolveIntoName, this.evolveTurns.HasValue ? this.evolveTurns.Value : 1);

            if (!string.IsNullOrEmpty(this.defaultEvolutionName))
                card.defaultEvolutionName = this.defaultEvolutionName;

            if (!string.IsNullOrEmpty(this.tailName))
                card.SetTail(this.tailName, this.tailLostPortrait);

            if (!string.IsNullOrEmpty(this.iceCubeName))
                card.SetIceCube(this.iceCubeName);

            if (this.flipPortraitForStrafe.HasValue)
                card.flipPortraitForStrafe = this.flipPortraitForStrafe.Value;


            if (this.extensionProperties != null)
                foreach (var item in this.extensionProperties)
                    card.SetExtendedProperty(item.Key, item.Value);


            card.SetExtendedProperty("JSONFilePath", this.filePath);
        }

        private void ApplyLocalisations(CardInfo card)
        {
            ApplyLocaleField("displayedName", displayedName, out card.displayedName);
            ApplyLocaleField("description", description, out card.description);
        }

        private void ApplyLocaleField(string field, string englishValue, out string cardInfoEnglishField)
        {
            // English
            cardInfoEnglishField = englishValue;
            
            // Go through our public fields and look for a field with the same prefix
            // Get what code it should be and apply that
            for (int i = 0; i < PUBLIC_FIELD_INFOS.Length; i++)
            {
                FieldInfo fieldInfo = PUBLIC_FIELD_INFOS[i];
                string fieldName = fieldInfo.Name;
                if (!fieldName.StartsWith(field, StringComparison.Ordinal))
                    continue;

                string translatedValue = fieldInfo.GetValue(this) as string;
                if (string.IsNullOrEmpty(translatedValue))
                    continue;
                
                int indexOf = fieldName.LastIndexOf("_", StringComparison.Ordinal);
                if (indexOf >= 0)
                {
                    // Translations
                    int length = fieldName.Length - indexOf - 1;
                    string code = fieldName.Substring(indexOf + 1, length);
                    Language language = LocalizationManager.CodeToLanguage(code);
                    if (language < Language.NUM_LANGUAGES)
                    {
                        Plugin.Log.LogDebug($"{displayedName} has translation for {field} in {language} with {translatedValue}");
                        LocalizationManager.New(Plugin.PluginGuid, null, englishValue, translatedValue, language);
                    }
                    else
                    {
                        Plugin.Log.LogDebug($"Unknown language code {code} for card {displayedName} in field {field}");
                    }
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
                Plugin.Log.LogDebug($"Modifying {this.name} using {this.ToJSON()}");
                this.ApplyCardInfo(existingCard);
            }
            else
            {
                string localModPrefix = this.modPrefix ?? DEFAULT_MOD_PREFIX;
                CardInfo newCard = ScriptableObject.CreateInstance<CardInfo>();
                newCard.name = this.name.StartsWith($"{localModPrefix}_") ? this.name : $"{localModPrefix}_{this.name}";
                this.ApplyCardInfo(newCard);
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
                string localModPrefix = this.modPrefix ?? DEFAULT_MOD_PREFIX;
                CardInfo newCard = ScriptableObject.CreateInstance<CardInfo>();
                newCard.name = this.name.StartsWith($"{localModPrefix}_") ? this.name : $"{localModPrefix}_{this.name}";
                this.ApplyCardInfo(newCard);

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
                this.ApplyCardInfo(baseGameCard);
                return baseGameCard;
            }
            else
            {
                string localModPrefix = this.modPrefix ?? DEFAULT_MOD_PREFIX;
                CardInfo newCard = ScriptableObject.CreateInstance<CardInfo>();
                newCard.name = this.name.StartsWith($"{localModPrefix}_") ? this.name : $"{localModPrefix}_{this.name}";
                this.ApplyCardInfo(newCard);
                return newCard;
            }
        }

        internal string WriteToFile(string filename, bool overwrite = true)
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
                        retval += $"\t\"{field.Name}\": {fieldVal.Value},\n";
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
}