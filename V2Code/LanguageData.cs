using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BepInEx;
using InscryptionAPI.Localizing;
using JLPlugin;
using TinyJson;
using TMPro;
using UnityEngine;

namespace JSONLoader.V2Code
{
    [Serializable]
    public class LanguageData
    {
        [Serializable]
        public class Fonts
        {
            public LocalizationManager.FontReplacementType Type;
            public string AssetBundlePath;
            public string FontAssetName;
            public string TMPFontAssetName;
        }
        
        public string languageName;
        public string languageCode;
        public string resetButtonText;
        public string stringTablePath;
        public List<Fonts> fontReplacementPaths;
        
        public static void LoadAllLanguages(List<string> files)
        {
            for (int index = 0; index < files.Count; index++)
            {
                string file = files[index];
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                if (!filename.EndsWith("_language.jldr2")) 
                    continue;
                
                files.RemoveAt(index--);
                
                Plugin.VerboseLog($"Loading JLDR2 (language) {filename}");
                LanguageData languageInfo = JSONParser.FromJson<LanguageData>(File.ReadAllText(file));

                string stringTablePath = languageInfo.stringTablePath;
                if (!TryGetFullPath(stringTablePath, out stringTablePath))
                {
                    Plugin.Log.LogError(
                        $"Could not load language. Could not find string table with name {languageInfo.stringTablePath}!");
                    return;
                }


                List<FontReplacement> fontReplacements = null;
                if (languageInfo.fontReplacementPaths != null)
                {
                    fontReplacements = new List<FontReplacement>();
                    foreach (Fonts replacement in languageInfo.fontReplacementPaths)
                    {
                        if (!TryGetFullPath(replacement.AssetBundlePath, out string abPath))
                        {
                            Plugin.Log.LogWarning(
                                $"Could not load font replacement. Could not find file with name {replacement.AssetBundlePath}!");
                            continue;
                        }

                        AssetBundle bundle = AssetBundle.LoadFromFile(abPath);
                        if (bundle == null)
                        {
                            Plugin.Log.LogWarning(
                                $"Could not load asset bundle at path '{abPath}'. Skipping font replacement!");
                            continue;
                        }

                        Font font = bundle.LoadAsset<Font>(replacement.FontAssetName);
                        if (font == null)
                        {
                            Plugin.Log.LogWarning(
                                $"Could not load FontAssetName asset from bundle with name '{replacement.FontAssetName}'. Skipping font replacement!");
                            continue;
                        }

                        TMP_FontAsset asset = bundle.LoadAsset<TMP_FontAsset>(replacement.TMPFontAssetName);
                        if (asset == null)
                        {
                            Plugin.Log.LogWarning(
                                $"Could not load TMPFontAssetName asset from bundle with name '{replacement.TMPFontAssetName}'. Skipping font replacement!");
                            continue;
                        }

                        fontReplacements.Add(
                            LocalizationManager.GetFontReplacementForFont(replacement.Type, font, asset));
                    }
                }

                LocalizationManager.NewLanguage(Plugin.PluginGuid, languageInfo.languageName,
                    languageInfo.languageCode, languageInfo.resetButtonText, stringTablePath, fontReplacements);

                Plugin.Log.LogDebug($"Loaded JSON gramophone tracks from {filename}!");
            }
        }

        private static bool TryGetFullPath(string fileName, out string fullFilePath)
        {
            DirectoryInfo directory = new DirectoryInfo(Paths.PluginPath);
            FileInfo[] filesInDir = directory.GetFiles(fileName, SearchOption.AllDirectories);
            if (filesInDir.Length == 0)
            {
                fullFilePath = null;
                return false;
            }
            else if (filesInDir.Length > 1)
            {
                Plugin.Log.LogWarning($"More than 1 file with the filename {fileName}! Using first one!");
            }

            fullFilePath = filesInDir[0].FullName;
            return true;
        }

        
        public static void ExportAllLanguages()
        {
            Plugin.Log.LogInfo($"Exporting {LocalizationManager.AllLanguages.Count} languages.");
            StringBuilder stringBuilder = new StringBuilder("id");
            for (int i = 0; i < LocalizationManager.AllLanguages.Count; i++)
            {
                var language = LocalizationManager.AllLanguages[i];
                stringBuilder.Append("," + language);
            }

            int keys = Localization.Translations.Count;
            for (var i = 0; i < keys; i++)
            {
                Localization.Translation translation = Localization.Translations[i];
                stringBuilder.Append($"\n{translation.id}");

                for (var j = 0; j < LocalizationManager.AllLanguages.Count; j++)
                {
                    Language language = LocalizationManager.AllLanguages[j].Language;
                    stringBuilder.Append(",");

                    if (language == Language.English)
                    {
                        stringBuilder.Append(translation.englishString);
                    }
                    else if (translation.values.TryGetValue(language, out string word))
                    {
                        stringBuilder.Append(word);
                    }
                }
            }

            string path = Path.Combine(Plugin.ExportDirectory, "Languages");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            File.WriteAllText(Path.Combine(path, "localisation_table.csv"), stringBuilder.ToString());
        }
    }
}