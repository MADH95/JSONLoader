using DiskCardGame;
using JLPlugin.Utils;
using System;
using System.Collections.Generic;

namespace JLPlugin.Data
{
    public class DDUtils
    {
        public static DialogueEvent GenerateDialogue( CardData card, DialogueData data )
        {
            DialogueEvent dialogueEvent = new()
            {
                speakers = new List<DialogueEvent.Speaker>() { (DialogueEvent.Speaker) card.speaker },
                id = data.id,
                mainLines = CreateMainLines( data.mainLines ),
                repeatLines = CreateRepeatLines( data.repeatLines )
            };
            return dialogueEvent;
        }

        private static DialogueEvent.LineSet CreateMainLines( List<DialogueLineData> mainLines )
        {
            DialogueEvent.LineSet lineSet = new() { lines = new List<DialogueEvent.Line>() };
            if ( mainLines == null )
            {
                return lineSet;
            }
            mainLines.ForEach( line => lineSet.lines.Add( new()
            {
                text = line.text,
                emotion = CDUtils.Assign( line.emotion, nameof( line.emotion ), Dicts.Emotions ),
                storyCondition = CDUtils.Assign( line.storyCondition, nameof( line.storyCondition ), Dicts.StoryEvents ),
                storyConditionMustBeMet = line.storyConditionMustBeMet
            } ) );
            return lineSet;
        }

        private static List<DialogueEvent.LineSet> CreateRepeatLines( List<List<DialogueLineData>> repeatLines )
        {
            List<DialogueEvent.LineSet> lineSets = new();
            if ( repeatLines == null )
            {
                return lineSets;
            }
            repeatLines.ForEach(lines => {
                DialogueEvent.LineSet lineSet = new() { lines = new List<DialogueEvent.Line>() };
                lineSets.Add(lineSet);
                lines.ForEach( line => lineSet.lines.Add( new()
                {
                    text = line.text,
                    emotion = CDUtils.Assign( line.emotion, nameof( line.emotion ), Dicts.Emotions ),
                    storyCondition = CDUtils.Assign( line.storyCondition, nameof( line.storyCondition ), Dicts.StoryEvents ),
                    storyConditionMustBeMet = line.storyConditionMustBeMet
                } ) );
            });
            return lineSets;
        }

    }
}