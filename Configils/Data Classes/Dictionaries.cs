using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JLPlugin.Data
{
    public static class SigilDicts
    {
        public static readonly Dictionary<string, AbilityMetaCategory> AbilityMetaCategory
            = Enum.GetValues(typeof(AbilityMetaCategory))
                    .Cast<AbilityMetaCategory>()
                    .ToDictionary(t => t.ToString(), t => t);

        public static IDictionary<Ability, Tuple<Type, Data.SigilData>> ArgumentList = new Dictionary<Ability, Tuple<Type, SigilData>>();

    }
}