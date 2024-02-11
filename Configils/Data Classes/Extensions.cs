using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JLPlugin
{
    public static class Extensions
    {
        public static void Append<K, V>(this IDictionary<K, V> first, IDictionary<K, V> second)
        {
            second.ToList()
                .ForEach(pair => first[pair.Key] = pair.Value);
        }
    }
}