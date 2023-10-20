using System.Collections.Generic;
using System.Linq;

namespace JLPlugin
{
    public static class Extensions
    {
        public static void Append<K, V>(this Dictionary<K, V> first, Dictionary<K, V> second)
        {
            second.ToList()
                .ForEach(pair => first[pair.Key] = pair.Value);
        }
    }
}