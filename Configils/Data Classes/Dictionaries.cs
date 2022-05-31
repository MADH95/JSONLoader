using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JLPlugin.Data
{
    //type alias to reduce repetition of long tuple type
    using SigilTuple = Tuple<Type, SigilData>;

    public static class SigilDicts
    {
        public static readonly Dictionary<string, AbilityMetaCategory> AbilityMetaCategory
            = Enum.GetValues(typeof(AbilityMetaCategory))
                    .Cast<AbilityMetaCategory>()
                    .ToDictionary(t => t.ToString(), t => t);

        public static IDictionary<Ability, SigilTuple> ArgumentList
            = new Dictionary<Ability, SigilTuple>();

        public static IDictionary<SpecialTriggeredAbility, SigilTuple> SpecialArgumentList
            = new Dictionary<SpecialTriggeredAbility, SigilTuple>();

    }
}