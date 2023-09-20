using System.Collections.Generic;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Regions;
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
        public const string PluginGuid = "MADH.inscryption.JSONLoader";
        public const string PluginName = "JSONLoader";
        public const string PluginVersion = "2.4.0";
        
        internal static ConfigEntry<bool> betaCompatibility;

        internal static ManualLogSource Log;

        private static List<string> GetAllJLDRFiles()
        {
            return Directory.GetFiles(Paths.PluginPath, "*.jldr*", SearchOption.AllDirectories)
                .Where((a)=> a.EndsWith(".jldr") || a.EndsWith(".jldr2"))
                .ToList();
        }
        
        private void Awake()
        {
            Logger.LogInfo($"Loaded {PluginName}!");
            Log = Logger;
            Harmony harmony = new(PluginGuid);
            harmony.PatchAll();
            betaCompatibility = Config.Bind("JSONLoader", "JDLR Backwards Compatibility", true, "Set to true to enable old-style JSON files (JLDR) to be read and converted to new-style files (JLDR2)");
            Log.LogWarning("Note: JSONLoader now uses .jldr2 files, not .json files.");

            List<string> files = GetAllJLDRFiles();
            if (betaCompatibility.Value)
                Log.LogWarning("Note: Backwards compatibility has been enabled. Old *.jldr files will be converted to *.jldr2 automatically. This will slow down your game loading!");
            if (betaCompatibility.Value)
                Utils.JLUtils.LoadCardsFromFiles(files);

            Log.LogDebug(string.Join(", ", RegionManager.AllRegionsCopy.Select(x => x.name)));

            LoadAll(files);
        }

        public void LoadAll(List<string> files)
        {
            TribeList.LoadAllTribes(files);
            SigilData.LoadAllSigils(files);
            Data.EncounterData.LoadAllEncounters(files);
            StarterDeckList.LoadAllStarterDecks(files);
            GramophoneData.LoadAllGramophone(files);
            LanguageData.LoadAllLanguages(files);
            MaskData.LoadAllMasks(files);
            JSONLoader.Data.TalkingCards.LoadTalkingCards.InitAndLoad();
            // ^ Ambiguity between JSONLoader.Data and JLPlugin.Data is annoying. = u= -Kelly
            
            CardSerializeInfo.LoadAllJLDR2(files); // Expects the lsit to only have cards at this stage
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
    }
}
