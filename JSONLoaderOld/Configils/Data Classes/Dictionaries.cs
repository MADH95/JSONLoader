using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JLPlugin.Data
{
    //type alias to reduce repetition of long tuple type
    using SigilTuple = Tuple<Type, SigilData>;
    using StatTuple = Tuple<Type, SigilData, SpecialStatIcon>;

    public static class SigilDicts
    {
        public static readonly Dictionary<string, AbilityMetaCategory> AbilityMetaCategory
            = Enum.GetValues(typeof(AbilityMetaCategory))
                    .Cast<AbilityMetaCategory>()
                    .ToDictionary(t => t.ToString(), t => t);

        public static readonly Dictionary<string, Emotion> Emotion
            = Enum.GetValues(typeof(Emotion))
                    .Cast<Emotion>()
                    .ToDictionary(t => t.ToString(), t => t);

        public static readonly Dictionary<string, TextDisplayer.LetterAnimation> LetterAnimation
            = Enum.GetValues(typeof(TextDisplayer.LetterAnimation))
                    .Cast<TextDisplayer.LetterAnimation>()
                    .ToDictionary(t => t.ToString(), t => t);

        public static readonly Dictionary<string, DialogueEvent.Speaker> Speaker
            = Enum.GetValues(typeof(DialogueEvent.Speaker))
                    .Cast<DialogueEvent.Speaker>()
                    .ToDictionary(t => t.ToString(), t => t);

        public static IDictionary<Ability, SigilTuple> ArgumentList
            = new Dictionary<Ability, SigilTuple>();

        public static IDictionary<SpecialTriggeredAbility, SigilTuple> SpecialArgumentList
            = new Dictionary<SpecialTriggeredAbility, SigilTuple>();

        public static IDictionary<SpecialTriggeredAbility, StatTuple> PowerStatArgumentList
            = new Dictionary<SpecialTriggeredAbility, StatTuple>();

        public static IDictionary<string, ItemData> ConsumableItemList = new Dictionary<string, ItemData>();

    }
}