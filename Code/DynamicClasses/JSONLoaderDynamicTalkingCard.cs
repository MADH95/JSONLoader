using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Text;

namespace JSONLoader.DynamicClasses
{
    public class JSONLoaderDynamicTalkingCard : PaperTalkingCard
    {

        public override DialogueEvent.Speaker SpeakerType => TalkingCards.GetSpeaker(GetComponent<Card>().Info.name);

        protected override string OnDrawnDialogueId => "JSONLoader_DynamicTalkingCard_OnDrawn_" + GetComponent<Card>().Info.name;

        protected override string OnDrawnFallbackDialogueId => "JSONLoader_DynamicTalkingCard_OnDrawnFallback_" + GetComponent<Card>().Info.name;

        protected override string OnPlayFromHandDialogueId => "JSONLoader_DynamicTalkingCard_OnPlayFromHand_" + GetComponent<Card>().Info.name;

        protected override string OnAttackedDialogueId => "JSONLoader_DynamicTalkingCard_OnAttacked_" + GetComponent<Card>().Info.name;

        protected override string OnBecomeSelectablePositiveDialogueId => "JSONLoader_DynamicTalkingCard_OnBecomeSelectablePositive_" + GetComponent<Card>().Info.name;

        protected override string OnBecomeSelectableNegativeDialogueId => "JSONLoader_DynamicTalkingCard_OnBecomeSelectableNegative_" + GetComponent<Card>().Info.name;

        protected override string OnSacrificedDialogueId => "JSONLoader_DynamicTalkingCard_OnSacrificed_" + GetComponent<Card>().Info.name;

        protected override string OnSelectedForCardMergeDialogueId => "JSONLoader_DynamicTalkingCard_OnSelectedForCardMerge_" + GetComponent<Card>().Info.name;

        protected override string OnSelectedForCardRemoveDialogueId => "JSONLoader_DynamicTalkingCard_OnSelectedForCardRemove_" + GetComponent<Card>().Info.name;

        protected override string OnSelectedForDeckTrialDialogueId => "JSONLoader_DynamicTalkingCard_OnSelectedForDeckTrial_" + GetComponent<Card>().Info.name;

        protected override string OnDiscoveredInExplorationDialogueId => "JSONLoader_DynamicTalkingCard_OnDiscoveredInExploration_" + GetComponent<Card>().Info.name;

        protected override Dictionary<Opponent.Type, string> OnDrawnSpecialOpponentDialogueIds => new Dictionary<Opponent.Type, string>();
    }
}
