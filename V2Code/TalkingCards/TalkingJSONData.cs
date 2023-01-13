using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DiskCardGame;
using InscryptionAPI.TalkingCards.Create;
using InscryptionAPI.TalkingCards.Helpers;
using InscryptionAPI.TalkingCards.Animation;

#nullable enable
namespace JSONLoader.Data.TalkingCards
{
    [Serializable]
    public class TalkingJSONData
    {
        public string cardName { get; set; }
        public string faceSprite { get; set; }
        public FaceImages? eyeSprites { get; set; }
        public FaceImages? mouthSprites { get; set; }

        // No longer in use, but kept here for backwards compatibility.
        // 'emissionSprites' should be used instead.
        public string? emissionSprite { get; set; }
        public FaceImages? emissionSprites { get; set; }

        public EmotionImages[]? emotions { get; set; }

        public FaceInfo? faceInfo { get; set; }
        public DialogueEventStrings[] dialogueEvents { get; set; }

        public TalkingJSONData(string cardName, string faceSprite,
            FaceImages? eyeSprites = null, FaceImages? mouthSprites = null,
            string? emissionSprite = null, EmotionImages[]? emotions = null,
            FaceInfo? faceInfo = null, DialogueEventStrings[]? dialogueEvents = null,
            FaceImages? emissionSprites = null)
        {
            this.cardName = cardName;
            this.faceSprite = faceSprite;
            this.eyeSprites = eyeSprites;
            this.mouthSprites = mouthSprites;
            this.emissionSprite = emissionSprite;
            this.emotions = emotions;
            this.faceInfo = faceInfo;
            this.dialogueEvents = dialogueEvents ?? new DialogueEventStrings[0];
            this.emissionSprites = emissionSprites;
        }

        public List<EmotionData> GetEmotions()
        {
            List<EmotionData> emotionList = new();

            #region NeutralEmotion
            EmotionData neutral = new(
                    "Neutral",
                    faceSprite,
                    eyeSprites?.AsTuple(),
                    mouthSprites?.AsTuple(),
                    (emissionSprite != null ? (emissionSprite, "_") : emissionSprites?.AsTuple())
                );;

            emotionList.Add(neutral);
            #endregion

            IEnumerable<EmotionData?>? moreEmotions = emotions
                ?.Select(x => x.MakeEmotion(neutral));
            if (moreEmotions == null) return emotionList;

            foreach (EmotionData? data in moreEmotions)
            {
                if (data != null) emotionList.Add(data);
            }
            return emotionList;
        }

        public FaceData GetFaceData() => new(cardName, GetEmotions(), faceInfo);
        public List<DialogueEvent?> MakeDialogueEvents()
            => dialogueEvents.Select(x => x?.CreateEvent(cardName)).ToList();
    }

    [Serializable]
    public class FaceImages
    {
        public string? open { get; set; }
        public string? closed { get; set; }

        public FaceImages(string open, string? closed)
        {
            this.open = open;
            this.closed = closed;
        }

        public (string? open, string? closed) AsTuple()
            => (open, closed);

        public FaceAnim GetSprites()
            => new FaceAnim(this.open, this.closed);

        public static implicit operator FaceImages((string open, string closed) x)
            => new FaceImages(x.open, x.closed);
    }

    [Serializable]
    public class EmotionImages
    {
        public string emotion { get; set; }
        public string? faceSprite { get; set; }
        public FaceImages? eyeSprites { get; set; }
        public FaceImages? mouthSprites { get; set; }
        public string? emissionSprite { get; set; }

        public EmotionImages(string emotion, string? faceSprite = null, FaceImages? eyeSprites = null, FaceImages? mouthSprites = null, string? emissionSprite = null)
        {
            this.emotion = emotion;
            this.faceSprite = faceSprite;
            this.eyeSprites = eyeSprites;
            this.mouthSprites = mouthSprites;
            this.emissionSprite = emissionSprite;
        }

        public EmotionData? MakeEmotion(EmotionData neutralEmotion)
        {
            // Emotion
            Emotion emotionValue = AssetHelpers.ParseAsEnumValue<Emotion>(emotion);
            if (emotionValue == Emotion.Neutral) return null;

            // Sprites
            Sprite face = AssetHelpers.MakeSprite(faceSprite)
                ?? neutralEmotion.Face;

            FaceAnim eyes = eyeSprites != null
                ? eyeSprites.GetSprites()
                : neutralEmotion.Eyes;

            FaceAnim mouth = mouthSprites != null
                ? mouthSprites.GetSprites()
                : neutralEmotion.Mouth;

            Sprite emission = AssetHelpers.MakeSprite(emissionSprite)
                ?? neutralEmotion.Emission;

            return new EmotionData(emotionValue, face, eyes, mouth, emission);
        }
    }
}