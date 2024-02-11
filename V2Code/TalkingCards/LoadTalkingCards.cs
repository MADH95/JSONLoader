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
                if (!file.EndsWith("_talk.jldr2"))
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
                TalkingJSONData? talk = JSONParser.FromJson<TalkingJSONData>(File.ReadAllText(file));
                if (talk == null) return;
                //FileLog.Log($"Loading card: {talk.cardName}");
                TalkingCardManager.Create(talk.GetFaceData(), GeneratePortrait.DialogueDummy);
                var dialogueEvents = talk.MakeDialogueEvents();
                dialogueEvents.ForEach(x => TalkingCardCreator.AddToDialogueCache(x?.id));
                LogHelpers.LogInfo($"Loaded talking card data for card: {talk.cardName}!");
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