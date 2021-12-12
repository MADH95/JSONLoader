using JLPlugin.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace JSONLoader.DynamicClasses
{

    public class TalkingCards
    {
        public enum DialogueID
        {
            OnDrawn,
            OnDrawnFallback,
            OnPlayFromHand,
            OnAttacked,
            OnBecomeSelectablePositive,
            OnBecomeSelectableNegative,
            OnSacrificed,
            OnSelectedForCardMerge,
            OnSelectedForCardRemove,
            OnSelectedForDeckTrial,
            OnDiscoveredInExploration
        }

        public static List<CardData> talkingCards = new();
        public static DialogueEvent.Speaker GetSpeaker( string name )
        {
            return ( DialogueEvent.Speaker ) talkingCards.Find( card => card.name == name ).speaker;
        }
    }
}
