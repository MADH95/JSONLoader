using System.Collections.Generic;
using JLPlugin.V2.Data;

#nullable enable
namespace JLPlugin.SigilCode
{
    public static class CachedCardData
    {
        private static Dictionary<string, CardSerializeInfo> CardDataCache = new();

        public static CardSerializeInfo? Get(string filePath)
        {
            if (filePath == null) return null;
            if (!CardDataCache.ContainsKey(filePath)) return null;

            return CardDataCache[filePath];
        }

        public static void Add(string filePath, CardSerializeInfo data)
            => CardDataCache[filePath] = data;

        public static bool Contains(string? filePath)
            => filePath != null && CardDataCache.ContainsKey(filePath);
    }
}
