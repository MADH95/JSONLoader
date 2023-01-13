using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using InscryptionAPI.TalkingCards.Animation;
using JLPlugin;
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

        private static void LogInfo(string message)
            => Plugin.Log?.LogInfo(message);

        private static void LogError(string message)
            => Plugin.Log?.LogError(message);

        private static void DebugLog(string message)
            => Plugin.Log?.LogDebug(message);

        private static void LoadTalkJSON(string file)
        {
            LogInfo($"Loading file: {Path.GetFileName(file)}");

            try
            {
                TalkingJSONData? talk = JSONParser.FromJson<TalkingJSONData>(File.ReadAllText(file));
                if (talk == null) return;
                //FileLog.Log($"Loading card: {talk.cardName}");
                TalkingCardManager.Create(talk.GetFaceData(), null);
                var dialogueEvents = talk.MakeDialogueEvents();
                dialogueEvents.ForEach(x => TalkingCardManager.AddToDialogueCache(x?.id));
                DebugLog($"Loaded talking card data for card: {talk.cardName}");
            }
            catch (Exception ex)
            {
                LogError($"Error loading JSON data from file {Path.GetFileName(file)}!");
                LogError(ex.ToString());
                // throw;
            }
        }
    }
}