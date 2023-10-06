using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Reflection;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Helpers;
using InscryptionAPI.Localizing;
using JLPlugin;
using JLPlugin.V2.Data;
using Sirenix.Utilities;
using TinyJson;
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

    public static void ApplyEnumList<T>(ref T cardInfoValue, ref T? serializeInfoValue, bool toCardInfo) where T : struct
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

    public static void ApplyEnumList<T>(ref T cardInfoValue, ref string serializeInfoValue, bool toCardInfo)
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

    public static void ApplyProperty<T,Y>(Func<T> getter, Action<T> setter, ref Y serializeInfoValue, bool toCardInfo)
    {
        if (toCardInfo)
        {
            T t = default;
            ApplyValue(ref t, ref serializeInfoValue, true);
            setter(t);
                
        }
        else
        {
            T t = getter();
            ApplyValue(ref t, ref serializeInfoValue, false);
        }
    }

    public static void ApplyEnumList(ref string cardInfoValue, ref string serializeInfoValue, bool toCardInfo)
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

    public static void ApplyEnumList(ref Sprite cardInfoValue, ref string serializeInfoValue, bool toCardInfo, string type, string fileName)
    {
        if (toCardInfo)
        {
            if (!string.IsNullOrEmpty(serializeInfoValue))
                cardInfoValue = TextureHelper.GetImageAsTexture(serializeInfoValue).ConvertTexture();
        }
        else
        {
            if (cardInfoValue != null)
            {
                string path = Path.Combine(Plugin.ExportDirectory, type, "Assets", fileName + ".png");
                serializeInfoValue = ExportTexture(cardInfoValue.texture, path);
            }
        }
    }
    
    public static void ApplyEnumList(ref Texture cardInfoValue, ref string serializeInfoValue, bool toCardInfo)
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

    public static void ApplyEnumList(ref Texture2D cardInfoValue, ref string serializeInfoValue, bool toCardInfo,
        string type, string fileName)
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
                string path = Path.Combine(Plugin.ExportDirectory, type, "Assets", fileName + ".png");
                serializeInfoValue = ExportTexture(cardInfoValue, path);
            }
        }
    }

    public static void ApplyValue<T, Y>(ref T a, ref Y b, bool toA)
    {
        if (toA)
        {
            ConvertValue(ref b, ref a);
        }
        else
        {
            ConvertValue(ref a, ref b);
        }
    }
    
    private static void ConvertValue<T, Y>(ref T from, ref Y to)
    {
        if (typeof(T) == typeof(Y))
        {
            to = (Y)(object)from;
            return;
        }
        else if (AreNullableTypesEqual(from, to, out object fromValue, out object toValue, out bool fromHasValue, out bool toHasValue))
        {
            //Debug.Log($"Same types and someone is nullable");
            if (fromHasValue)
            {
                to = (Y)fromValue;
            }
            else
            {
                Debug.LogError($"B has no value. {from}=>{fromValue}");
            }
            return;
        }
        else if (typeof(T).IsEnum && typeof(Y) == typeof(string))
        {
            to = (Y)(object)from.ToString();
            return;
        }
        else if (typeof(T) == typeof(string) && typeof(Y).IsEnum)
        {
            object o = typeof(ImportExportUtils)
                .GetMethod(nameof(ParseEnum))
                .MakeGenericMethod(typeof(Y))
                .Invoke(null, new object[] { from });
            Debug.LogError($"string to enum: {from} => {o}");
            to = (Y)o;
            return;
        }
        else if (typeof(T) == typeof(CardInfo) && typeof(Y) == typeof(string))
        {
            if(from != null)
                to = (Y)(object)((from as CardInfo).name);
            return;
        }
        else if (typeof(T) == typeof(string) && typeof(Y) == typeof(CardInfo))
        {
            string s = (string)(object)from;
            if(string.IsNullOrEmpty(s))
                to = (Y)(object)CardLoader.GetCardByName(s);
            return;
        }
        
        Debug.LogError($"Unsupported conversion type: {typeof(T)} to {typeof(Y)}");
    }

    private static bool AreNullableTypesEqual<T, Y>(T t, Y y, out object a, out object b, out bool aHasValue, out bool bHasValue)
    {
        //Debug.Log($"AreNullableTypesEqual: {typeof(T)} to {typeof(Y)}");
        aHasValue = false;
        bHasValue = false;
        a = null;
        b = null;
        
        bool tIsNullable = typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>);
        bool yIsNullable = typeof(Y).IsGenericType && typeof(Y).GetGenericTypeDefinition() == typeof(Nullable<>);
        if (!tIsNullable && !yIsNullable)
        {
            //Debug.Log($"\t Neither are nullable");
            return false;
        }

        Type tInnerType = tIsNullable ? Nullable.GetUnderlyingType(typeof(T)) : typeof(T);
        Type yInnerType = yIsNullable ? Nullable.GetUnderlyingType(typeof(Y)) : typeof(Y);
        if (tInnerType == yInnerType)
        {
            //Debug.Log($"\t Same Inner types: {t}({tInnerType}) {y}({yInnerType})");
            if (tIsNullable)
            {
                a = GetValueFromNullable(t, out aHasValue);
            }
            else
            {
                a = t;
                aHasValue = true;
            }
            
            if (yIsNullable)
            {
                b = GetValueFromNullable(y, out bHasValue);
            }
            else
            {
                b = y;
                bHasValue = true;
            }

            return true;
        }

        Debug.LogError($"Not same types {typeof(T)} {typeof(Y)}");
        return false;
    }

    public static void ApplyList<T,Y>(ref List<T> cardInfoValue, ref List<Y> serializeInfoValue, bool toCardInfo)
    {
        if (typeof(T).IsEnum || typeof(Y).IsEnum)
        {
            Debug.LogError($"Cannot apply list of enums. {typeof(T)} to {typeof(Y)}");
            return;
        }
        
        if (toCardInfo)
        {
            if (serializeInfoValue == null)
            {
                return;
            }

            cardInfoValue = new List<T>();
            for (var i = 0; i < serializeInfoValue.Count; i++)
            {
                Y y = serializeInfoValue[i];
                T t = default;
                ConvertValue(ref y, ref t);
                cardInfoValue.Add(t);
            }
        }
        else
        {
            if (cardInfoValue == null) 
                return;
            
            
            serializeInfoValue = new List<Y>();
            for (var i = 0; i < cardInfoValue.Count; i++)
            {
                Y y = default;
                T t = cardInfoValue[i];
                ConvertValue(ref t, ref y);
                serializeInfoValue.Add(y);
            }
        }
    }
    
    public static void ApplyEnumList<T,Y>(ref List<T> cardInfoValue, ref List<Y> serializeInfoValue, bool toCardInfo)
        where T : unmanaged, Enum
    {
        if (toCardInfo)
        {
            if (serializeInfoValue != null)
            {
                cardInfoValue = new List<T>();
                for (var i = 0; i < serializeInfoValue.Count; i++)
                {
                    Y y = serializeInfoValue[i];
                    T t = ParseEnum<T>(y.ToString());
                    cardInfoValue.Add(t);
                }
            }
        }
        else
        {
            if (cardInfoValue != null)
            {
                serializeInfoValue = new List<Y>();
                for (var i = 0; i < cardInfoValue.Count; i++)
                {
                    T t = cardInfoValue[i];
                    Y y = default;
                    ConvertValue(ref t, ref y);
                    serializeInfoValue.Add(y);
                }
            }
        }
    }

    public static void ApplyEnumList<T>(ref List<T> cardInfoValue, ref string[] serializeInfoValue, bool toCardInfo)
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
            if (cardInfoValue != null)
            {
                serializeInfoValue = cardInfoValue.Select((a) => a.ToString()).ToArray();
            }
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

    public static string[] ExportTextures(IEnumerable<Texture2D> texture, string type, string fileName)
    {
        int i = 0;
        List<string> paths = new List<string>();
        foreach (Texture2D texture2D in texture)
        {
            i++;

            string path = Path.Combine(Plugin.ExportDirectory, type, "Assets", $"{fileName}_{i}.png");
            paths.Add(ExportTexture(texture2D, path));
        }

        return paths.ToArray();
    }

    public static void ApplyLocaleField(string field, ref JSONParser.LocalizableField rows, ref string cardInfoEnglishField, bool toCardInfo)
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

    private static void ImportLocaleField(JSONParser.LocalizableField rows, string cardInfoEnglishField)
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
    private static void ApplyLocaleField(string field, JSONParser.LocalizableField rows, out string cardInfoEnglishField)
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

    private static object GetValueFromNullable<U>(U u, out bool hasValue)
    {
        Type type = typeof(U);
        if (u != null)
        {
            bool v = (bool)type.GetProperty("HasValue", BindingFlags.Instance | BindingFlags.Public).GetValue(u);
            if (v)
            {
                hasValue = true;
                return type.GetProperty("Value", BindingFlags.Instance | BindingFlags.Public).GetValue(u);
            }
        }

        hasValue = false;
        Type underlyingType = Nullable.GetUnderlyingType(type);
        if(underlyingType.IsValueType)
        {
            return Activator.CreateInstance(underlyingType);
        }
        return null;
    }
}