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
        private void Awake()
        {
            Logger.LogInfo($"Loaded {PluginName}!");
            Log = base.Logger;
            Harmony harmony = new(PluginGuid);
            harmony.PatchAll();
            betaCompatibility = Config.Bind("JSONLoader", "JDLR Backwards Compatibility", true, "Set to true to enable old-style JSON files (JLDR) to be read and converted to new-style files (JLDR2)");
            Log.LogWarning("Note: JSONLoader now uses .jldr2 files, not .json files.");
            if (betaCompatibility.Value)
                Log.LogWarning("Note: Backwards compatibility has been enabled. Old *.jldr files will be converted to *.jldr2 automatically. This will slow down your game loading!");
            if (betaCompatibility.Value)
                Utils.JLUtils.LoadCardsFromFiles();

            Plugin.Log.LogDebug(string.Join(", ", RegionManager.AllRegionsCopy.Select(x => x.name)));

            TribeList.LoadAllTribes();
            SigilData.LoadAllSigils();
            CardSerializeInfo.LoadAllJLDR2();
            Data.EncounterData.LoadAllEncounters();
            StarterDeckList.LoadAllStarterDecks();
            GramophoneData.LoadAllGramophone();
            JSONLoader.Data.TalkingCards.LoadTalkingCards.InitAndLoad();
            // ^ Ambiguity between JSONLoader.Data and JLPlugin.Data is annoying. = u= -Kelly
        }

        public void Update()
        {
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.R))
            {
                TribeList.LoadAllTribes();
                SigilData.LoadAllSigils();
                CardSerializeInfo.LoadAllJLDR2();
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
