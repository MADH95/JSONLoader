using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InscryptionAPI.Guid;
using InscryptionAPI.Helpers;
using InscryptionAPI.Localizing;
using JLPlugin;
using JLPlugin.V2.Data;
using UnityEngine;

public static class ImportExportUtils
{
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

    public static void ApplyValue<T>(ref T cardInfoValue, ref T? serializeInfoValue, bool toCardInfo) where T : struct
    {
        if (toCardInfo)
        {
            if (serializeInfoValue.HasValue)
                cardInfoValue = serializeInfoValue.Value;
        }
        else
        {
            serializeInfoValue = cardInfoValue;
        }
    }

    public static void ApplyValue<T>(ref T cardInfoValue, ref string serializeInfoValue, bool toCardInfo)
        where T : unmanaged, Enum
    {
        if (toCardInfo)
        {
            if (!string.IsNullOrEmpty(serializeInfoValue))
                cardInfoValue = ParseEnum<T>(serializeInfoValue);
        }
        else
        {
            serializeInfoValue = cardInfoValue.ToString(); // TODO: Change to show the actual guid_name
        }
    }

    public static void ApplyValue(ref string cardInfoValue, ref string serializeInfoValue, bool toCardInfo)
    {
        if (toCardInfo)
        {
            if (!string.IsNullOrEmpty(serializeInfoValue))
                cardInfoValue = serializeInfoValue;
        }
        else
        {
            serializeInfoValue = cardInfoValue;
        }
    }

    public static void ApplyValue(ref Texture cardInfoValue, ref string serializeInfoValue, bool toCardInfo)
    {
        if (toCardInfo)
        {
            if (!string.IsNullOrEmpty(serializeInfoValue))
                cardInfoValue = TextureHelper.GetImageAsTexture(serializeInfoValue);
        }
        else
        {
            if (cardInfoValue != null)
                serializeInfoValue = cardInfoValue.ToString(); // TODO: Export as image and save the path
        }
    }

    public static void ApplyValue(ref Texture2D cardInfoValue, ref string serializeInfoValue, bool toCardInfo,
        string fileName)
    {
        if (toCardInfo)
        {
            if (!string.IsNullOrEmpty(serializeInfoValue))
                cardInfoValue = TextureHelper.GetImageAsTexture(serializeInfoValue);
        }
        else
        {
            if (cardInfoValue != null)
            {
                string path = Path.Combine(Plugin.ExportDirectory, "Cards", "Assets", fileName + ".png");
                serializeInfoValue = ExportTexture(cardInfoValue, path);
            }
        }
    }

    public static void ApplyValue<T>(ref List<T> cardInfoValue, ref string[] serializeInfoValue, bool toCardInfo)
        where T : unmanaged, Enum
    {
        if (toCardInfo)
        {
            if (serializeInfoValue != null)
            {
                if (typeof(T).IsEnum)
                {
                    cardInfoValue = serializeInfoValue.Select(s => ParseEnum<T>(s)).ToList();
                }
                else
                {
                    cardInfoValue = serializeInfoValue.Select(s => Convert.ChangeType(s, typeof(T))).Cast<T>().ToList();
                }
            }
        }
        else
        {
            serializeInfoValue = cardInfoValue.Select((a) => a.ToString()).ToArray();
        }
    }

    private static string ExportTexture(Texture2D texture, string path)
    {
        if (!texture.isReadable)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                texture.width,
                texture.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);

            Graphics.Blit(texture, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(texture.width, texture.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            texture = readableText;
        }

        byte[] bytes = texture.EncodeToPNG();
        var dirPath = Path.GetDirectoryName(path);
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        File.WriteAllBytes(path, bytes);
        return Path.GetFileName(path);
    }

    public static string[] ExportTextures(IEnumerable<Texture2D> texture, string fileName)
    {
        int i = 0;
        List<string> paths = new List<string>();
        foreach (Texture2D texture2D in texture)
        {
            i++;

            string path = Path.Combine(Plugin.ExportDirectory, "Cards", "Assets", $"{fileName}_{i}.png");
            paths.Add(ExportTexture(texture2D, path));
        }

        return paths.ToArray();
    }

    public static void ApplyLocaleField(string field, ref LocalizableField rows, ref string cardInfoEnglishField, bool toCardInfo)
    {
        if (toCardInfo)
        {
            ApplyLocaleField(field, rows, out cardInfoEnglishField);
        }
        else
        {
            string s = cardInfoEnglishField;
            cardInfoEnglishField = s;
            ImportLocaleField(rows, cardInfoEnglishField);
        }
    }

    private static void ImportLocaleField(LocalizableField rows, string cardInfoEnglishField)
    {
        // From game to LocalizableField
        rows.rows.Clear();
        rows.Initialize(cardInfoEnglishField);
        
        var translation = Localization.Translations.Find((a) => a.englishStringFormatted == cardInfoEnglishField);
        if (translation != null)
        {
            foreach (KeyValuePair<Language, string> pair in translation.values)
            {
                string code = LocalizationManager.LanguageToCode(pair.Key);
                rows.SetValue($"{rows.englishFieldName}_{code}", pair.Value);
            }
        }
    }
    
    /// <summary>
    /// From SerializeCardInfo to cardInfo
    /// </summary>
    /// <param name="field"></param>
    /// <param name="rows"></param>
    /// <param name="cardInfoEnglishField"></param>
    private static void ApplyLocaleField(string field, LocalizableField rows, out string cardInfoEnglishField)
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
                Plugin.Log.LogDebug($"Unknown language code {code} for card {cardInfoEnglishField} in field {field}");
            }
        }
    }
}