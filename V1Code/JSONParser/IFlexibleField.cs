using System;
using System.Collections.Generic;
using JLPlugin;

namespace TinyJson
{
    public interface IFlexibleField
    {
        bool ContainsKey(string key);
        void SetValue(string key, string value);
        string ToJSON(string prefix);
    }

    public interface IInitializable
    {
        public void Initialize();
    }

    [Serializable]
    public class LocalizableField : IFlexibleField
    {
        public string EnglishValue
        {
            get
            {
                if (rows.TryGetValue(englishFieldName, out var englishValue))
                {
                    return englishValue;
                }

                Plugin.Log.LogError($"Field has not been initialized {englishFieldName}!");
                return englishFieldName;
            }
        }

        public Dictionary<string, string> rows;

        public string englishFieldName;
        public string englishFieldNameLower;

        public LocalizableField(string EnglishFieldName)
        {
            rows = new Dictionary<string, string>();
            englishFieldName = EnglishFieldName;
            englishFieldNameLower = EnglishFieldName.ToLower();
        }

        public void Initialize(string englishValue)
        {
            rows[englishFieldName] = englishValue;
        }

        public bool ContainsKey(string key)
        {
            return key.StartsWith(englishFieldNameLower);
        }

        public void SetValue(string key, string value)
        {
            rows[key] = value;
        }

        public string ToJSON(string prefix)
        {
            string json = "";

            int index = 0;
            foreach (KeyValuePair<string, string> pair in rows)
            {
                json += $"\n{prefix}\"{pair.Key}\": \"{pair.Value}\"";
                if (index++ < rows.Count - 1)
                {
                    json += $",";
                }
                // else
                // {
                //     json += $"\n";
                // }
            }

            return json;
        }

        public override string ToString()
        {
            return rows.ToString();
        }
    }
}