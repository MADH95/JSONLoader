using InscryptionAPI.TalkingCards;
using InscryptionAPI.TalkingCards.Animation;
using InscryptionAPI.TalkingCards.Create;
using JLPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using TinyJson;

#nullable enable
namespace JSONLoader.Data.TalkingCards
{
    internal static class LoadTalkingCards
    {
        public static void InitAndLoad(List<string> files)
        {
            if (Configs.BetaCompatibility)
            {
                string[] convertedFiles = RenameFiles.RenameAll();
                if (convertedFiles != null)
                {
                    files.AddRange(convertedFiles);
                }
            }

            for (var index = 0; index < files.Count; index++)
            {
                string file = files[index];
                if (!file.ToLower().EndsWith("_talk.jldr2"))
                    continue;

                files.RemoveAt(index--);

                LoadTalkJSON(file);
            }
        }

        private static void LoadTalkJSON(string file)
        {
            // I'm keeping all these as LogInfo instead of LogDebug *for now*.
            // This will be really useful information in console logs for me!
            // I'm going to change all of these to LogDebug soon when everything is stable.
            // c: <3

            LogHelpers.LogInfo($"Loading file: {Path.GetFileName(file)}");

            try
            {
                TalkingJSONData? talk = JSONParser.FromFilePath<TalkingJSONData>(file);
                if (talk == null) return;
                //FileLog.Log($"Loading card: {talk.cardName}");
                TalkingCardManager.Create(talk.GetFaceData(), GeneratePortrait.DialogueDummy);
                var dialogueEvents = talk.MakeDialogueEvents();
                dialogueEvents.ForEach(x => TalkingCardCreator.AddToDialogueCache(x?.id));
                LogHelpers.LogInfo($"Loaded talking card data for card: {talk.cardName}!");

                Plugin.VerboseLog($"Outputting Verbose Talking Card:");

                Plugin.VerboseLog($"Card Name: {talk.cardName}");
                Plugin.VerboseLog($"Face Sprite: {talk.faceSprite}");
                Plugin.VerboseLog($"Emission Sprite: {talk.emissionSprite}");

                if (talk.eyeSprites == null)
                {
                    Plugin.VerboseLog($"Eye Sprites: null");
                }
                else
                {
                    Plugin.VerboseLog($"Eye Sprites:");
                    Plugin.VerboseLog($"Eye Sprites - Open: {talk.eyeSprites.open}");
                    Plugin.VerboseLog($"Eye Sprites - Closed: {talk.eyeSprites.closed}");
                }

                if (talk.mouthSprites == null)
                {
                    Plugin.VerboseLog($"Mouth Sprites: null");
                }
                else
                {
                    Plugin.VerboseLog($"Mouth Sprites:");
                    Plugin.VerboseLog($"Mouth Sprites - Open: {talk.mouthSprites.open}");
                    Plugin.VerboseLog($"Mouth Sprites - Closed: {talk.mouthSprites.closed}");
                }

                if (talk.emissionSprites == null)
                {
                    Plugin.VerboseLog($"Emission Sprites: null");
                }
                else
                {
                    Plugin.VerboseLog($"Emission Sprites:");
                    Plugin.VerboseLog($"Emission Sprites - Open: {talk.emissionSprites.open}");
                    Plugin.VerboseLog($"Emission Sprites - Closed: {talk.emissionSprites.closed}");
                }

                if (talk.emotions == null)
                {
                    Plugin.VerboseLog($"Emotions: null");
                }
                else
                {
                    Plugin.VerboseLog($"Emotions: {talk.emotions.Length}");

                    for (int i = 0; i < talk.emotions.Length; i++)
                    {
                        EmotionImages emotion = talk.emotions[i];

                        if (emotion == null)
                        {
                            Plugin.VerboseLog($"Emotion {i + 1}: null");
                            continue;
                        }

                        Plugin.VerboseLog($"Emotion {i + 1}:");
                        Plugin.VerboseLog($"Emotion {i + 1} - Emotion: {emotion.emotion}");
                        Plugin.VerboseLog($"Emotion {i + 1} - Face Sprite: {emotion.faceSprite}");
                        Plugin.VerboseLog($"Emotion {i + 1} - Emission Sprite: {emotion.emissionSprite}");

                        if (emotion.eyeSprites == null)
                        {
                            Plugin.VerboseLog($"Emotion {i + 1} - Eye Sprites: null");
                        }
                        else
                        {
                            Plugin.VerboseLog($"Emotion {i + 1} - Eye Sprites:");
                            Plugin.VerboseLog($"Emotion {i + 1} - Eye Sprites - Open: {emotion.eyeSprites.open}");
                            Plugin.VerboseLog($"Emotion {i + 1} - Eye Sprites - Closed: {emotion.eyeSprites.closed}");
                        }

                        if (emotion.mouthSprites == null)
                        {
                            Plugin.VerboseLog($"Emotion {i + 1} - Mouth Sprites: null");
                        }
                        else
                        {
                            Plugin.VerboseLog($"Emotion {i + 1} - Mouth Sprites:");
                            Plugin.VerboseLog($"Emotion {i + 1} - Mouth Sprites - Open: {emotion.mouthSprites.open}");
                            LogHelpers.LogInfo(
                                $"Emotion {i + 1} - Mouth Sprites - Closed: {emotion.mouthSprites.closed}");
                        }

                        if (emotion.emissionSprites == null)
                        {
                            Plugin.VerboseLog($"Emotion {i + 1} - Emission Sprites: null");
                        }
                        else
                        {
                            Plugin.VerboseLog($"Emotion {i + 1} - Emission Sprites:");
                            LogHelpers.LogInfo(
                                $"Emotion {i + 1} - Emission Sprites - Open: {emotion.emissionSprites.open}");
                            LogHelpers.LogInfo(
                                $"Emotion {i + 1} - Emission Sprites - Closed: {emotion.emissionSprites.closed}");
                        }
                    }
                }

                Plugin.VerboseLog($"Face Info: {(talk.faceInfo == null ? "null" : talk.faceInfo.ToString())}");
            }
            catch (Exception ex)
            {
                LogHelpers.LogError($"Error loading JSON data from file {Path.GetFileName(file)}!");
                LogHelpers.LogError(ex.ToString());
                // throw;
            }
        }
    }
}