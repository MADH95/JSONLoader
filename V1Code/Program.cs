using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using JLPlugin.Data;
using JLPlugin.V2.Data;
using JSONLoader.Data;
using System.Linq;
using JSONLoader.V2Code;
using UnityEngine;

namespace JLPlugin
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;
        
        public const string PluginGuid = "MADH.inscryption.JSONLoader";
        public const string PluginName = "JSONLoader";
        public const string PluginVersion = "2.5.0";
        
        public static string JSONLoaderDirectory = "";
        public static string BepInExDirectory = "";
        public static string ExportDirectory => Path.Combine(JSONLoaderDirectory, "Exported");
        
        internal static ConfigEntry<bool> betaCompatibility;
        internal static ConfigEntry<bool> verboseLogging;

        internal static ManualLogSource Log;

        private static List<string> GetAllJLDRFiles()
        {
            return System.IO.Directory.GetFiles(Paths.PluginPath, "*.jldr*", SearchOption.AllDirectories)
                .Where((a)=> (a.EndsWith(".jldr") || a.EndsWith(".jldr2")) && !a.Contains(JSONLoaderDirectory))
                .Select((a)=>a.ToLower())
                .ToList();
        }
        
        private void Awake()
        {
            Logger.LogInfo($"Loading {PluginName}!");
            Instance = this;
            Log = Logger;
            JSONLoaderDirectory = Path.GetDirectoryName(Info.Location);

            int bepInExIndex = Info.Location.LastIndexOf("BepInEx");
            if (bepInExIndex > 0)
            {
                BepInExDirectory = Info.Location.Substring(0, bepInExIndex);
            }
            else
            {
                BepInExDirectory = Directory.GetParent(JSONLoaderDirectory)?.FullName ?? "";
            }

            Harmony harmony = new(PluginGuid);
            harmony.PatchAll();
            
            betaCompatibility = Config.Bind("JSONLoader", "JDLR Backwards Compatibility", true, "Set to true to enable old-style JSON files (JLDR) to be read and converted to new-style files (JLDR2)");
            verboseLogging = Config.Bind("JSONLoader", "Verbose Logging", false, "Set to true to see more logs on what JSONLoader is doing and what isn't working.");
            
            Log.LogWarning("Note: JSONLoader now uses .jldr2 files, not .json files.");
            List<string> files = GetAllJLDRFiles();
            if (betaCompatibility.Value)
                Log.LogWarning("Note: Backwards compatibility has been enabled. Old *.jldr files will be converted to *.jldr2 automatically. This will slow down your game loading!");
            if (betaCompatibility.Value)
                Utils.JLUtils.LoadCardsFromFiles(files);

            LoadAll(files);
            
            Logger.LogInfo($"Loaded {PluginName}!");
        }

        public void LoadAll(List<string> files)
        {
            TribeList.LoadAllTribes(files);
            SigilData.LoadAllSigils(files);
            
            // NOTE: I really don't want to do this, but I can't figure out how to get the game to load the cards from
            // the JSON files without listing all the damn extension files....
            CardSerializeInfo.LoadAllJLDR2(files);
            
            Data.EncounterData.LoadAllEncounters(files);
            StarterDeckList.LoadAllStarterDecks(files);
            GramophoneData.LoadAllGramophone(files);
            LanguageData.LoadAllLanguages(files);
            MaskData.LoadAllMasks(files);
            JSONLoader.Data.TalkingCards.LoadTalkingCards.InitAndLoad(files);
            // ^ Ambiguity between JSONLoader.Data and JLPlugin.Data is annoying. = u= -Kelly
        }

        public void Update()
        {
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.R))
            {
                List<string> files = GetAllJLDRFiles();
                LoadAll(files);
                SigilCode.CachedCardData.Flush();
                if (SaveFile.IsAscension)
                {
                    ReloadKaycees();
                }
                if (SaveManager.SaveFile.IsPart1)
                {
                    ReloadVanilla();
                }
            }

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.X))
            {
                ExportAllToJLDR2();
            }
        }

        public void ExportAllToJLDR2()
        {
            TribeList.ExportAllTribes();
            // SigilData.LoadAllSigils(files);
            Data.EncounterData.ExportAllEncounters();
            StarterDeckList.ExportAllStarterDecks();
            // GramophoneData.LoadAllGramophone(files);
            LanguageData.ExportAllLanguages();
            // MaskData.LoadAllMasks(files);
            // JSONLoader.Data.TalkingCards.LoadTalkingCards.InitAndLoad(files);
            // ^ Ambiguity between JSONLoader.Data and JLPlugin.Data is annoying. = u= -Kelly
            
            CardSerializeInfo.ExportAllCards();
        }

        public static void ReloadVanilla()
        {
            FrameLoopManager.Instance.SetIterationDisabled(false);
            MenuController.ReturnToStartScreen();
            MenuController.LoadGameFromMenu(false);
        }

        public static void ReloadKaycees()
        {
            FrameLoopManager.Instance.SetIterationDisabled(false);
            SceneLoader.Load("Ascension_Configure");
            FrameLoopManager.Instance.SetIterationDisabled(false);
            SaveManager.savingDisabled = false;
            MenuController.LoadGameFromMenu(false);
        }

        internal static void VerboseLog(string s)
        {
            if (verboseLogging.Value)
                Log.LogInfo(s);
        }
        
        internal static void VerboseWarning(string s)
        {
            if (verboseLogging.Value)
                Log.LogWarning(s);
        }
        
        internal static void VerboseError(string s)
        {
            if (verboseLogging.Value)
                Log.LogError(s);
        }
    }
}
