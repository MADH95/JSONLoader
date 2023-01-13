using System;
using System.IO;
using BepInEx;
using InscryptionAPI.TalkingCards;
using TinyJson;

#nullable enable
namespace JSONLoader.Data.TalkingCards
{
    internal static class LoadTalkingCards
    {
        private static string[] GetTalkingJSON()
            => Directory.GetFiles(Paths.PluginPath, "*_talk.jldr2", SearchOption.AllDirectories);

        public static void LoadJSONCards()
        {
            foreach(string file in GetTalkingJSON())
                LoadTalkJSON(file);
        }

        public static void InitAndLoad()
        {
            RenameFiles.RenameAll(); // For seamless compatibility with my TalkingCardAPI. 
            LoadJSONCards();
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
                TalkingCardManager.Create(talk.GetFaceData(), null);
                var dialogueEvents = talk.MakeDialogueEvents();
                dialogueEvents.ForEach(x => TalkingCardManager.AddToDialogueCache(x?.id));
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