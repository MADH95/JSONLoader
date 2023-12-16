using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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