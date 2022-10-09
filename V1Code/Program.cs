using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using JLPlugin.Data;
using JLPlugin.V2.Data;
using UnityEngine;

namespace JLPlugin
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "MADH.inscryption.JSONLoader";
        public const string PluginName = "JSONLoader";
        public const string PluginVersion = "1.7.0.0";

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

            SigilData.LoadAllSigils();
            CardSerializeInfo.LoadAllJLDR2();
            StarterDeckList.LoadAllStarterDecks();
        }

        public void Update()
        {
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && Input.GetKeyDown(KeyCode.R))
            {
                SigilData.LoadAllSigils();
                CardSerializeInfo.LoadAllJLDR2();
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