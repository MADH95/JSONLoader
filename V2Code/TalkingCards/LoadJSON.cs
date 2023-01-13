using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using Newtonsoft.Json;
using InscryptionAPI.TalkingCards.Animation;
using JLPlugin;
using InscryptionAPI.TalkingCards;
using TinyJson;

#nullable enable
namespace JSONLoader.Data.TalkingCards
{
    internal static class LoadJSON
    {
        private static List<string> GetTalkingJSON()
            => Directory.GetFiles(Paths.PluginPath, "*_talk.jldr2", SearchOption.AllDirectories).ToList();

        public static void LoadJSONCards()
            => GetTalkingJSON().ForEach(x => LoadTalkJSON(x));

        public static void InitAndLoad()
        {
            RenameFiles.RenameAll();
            LoadJSONCards();
        }

        private static void LogInfo(string message)
            => Plugin.Log.LogInfo(message);

        private static void LoadTalkJSON(string file)
        {
            LogInfo($"Loading file: {Path.GetFileName(file)}");

            try
            {
                //TalkingJSONData? talk = JsonConvert.DeserializeObject<TalkingJSONData>(File.ReadAllText(file));
                TalkingJSONData? talk = JSONParser.FromJson<TalkingJSONData>(File.ReadAllText(file));
                if (talk == null) return;
                //FileLog.Log($"Loading card: {talk.cardName}");
                TalkingCardManager.Create(talk.GetFaceData(), GeneratePortrait.DialogueDummy);
                var dialogueEvents = talk.MakeDialogueEvents();
                dialogueEvents.ForEach(x => TalkingCardManager.AddToDialogueCache(x?.id));
            }
            catch (Exception)
            {
                Plugin.Log.LogError($"Error loading JSON data from file {Path.GetFileName(file)}!");
                // Plugin.LogError(ex.ToString());
                // throw;
            }
        }
    }
}