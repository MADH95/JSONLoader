using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using JLPlugin.Data;
using JLPlugin.Hotkeys;
using JLPlugin.V2.Data;
using JSONLoader.Data;
using JSONLoader.V2Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ItemData = JLPlugin.Data.ItemData;

namespace JLPlugin
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;

        public const string PluginGuid = "MADH.inscryption.JSONLoader";
        public const string PluginName = "JSONLoader";
        public const string PluginVersion = "2.5.3";

        public static string JSONLoaderDirectory = "";
        public static string BepInExDirectory = "";
        public static string ExportDirectory => Path.Combine(JSONLoaderDirectory, "Examples", "Exported");

        internal static ManualLogSource Log;
        private HotkeyController hotkeyController;

        private static List<string> GetAllJLDRFiles()
        {
            return System.IO.Directory.GetFiles(Paths.PluginPath, "*.jldr*", SearchOption.AllDirectories)
                .Where((a) => (a.EndsWith(".jldr") || a.EndsWith(".jldr2")) && !a.Contains(Path.Combine(JSONLoaderDirectory, "Examples")))
                .ToList();
        }

        private void Awake()
        {
            Logger.LogInfo($"Loading {PluginName}!");
            Instance = this;
            Log = Logger;
            JSONLoaderDirectory = Path.GetDirectoryName(Info.Location);

            //LogFields();

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

            Configs.InitializeConfigs(Config);

            hotkeyController = new HotkeyController();
            hotkeyController.AddHotkey(Configs.ReloadHotkey, ReloadGame);
            hotkeyController.AddHotkey(Configs.ExportHotkey, ExportAllToJLDR2);
            
            Log.LogWarning("Note: JSONLoader now uses .jldr2 files, not .json files.");
            List<string> files = GetAllJLDRFiles();
            if (Configs.BetaCompatibility)
            {
                Log.LogWarning("Note: Backwards compatibility has been enabled. Old *.jldr files will be converted to *.jldr2 automatically. This will slow down your game loading!");
                Utils.JLUtils.LoadCardsFromFiles(files);
            }

            try
            {
                LoadAll(files);
            }
            catch (Exception)
            {
                // ignored
            }

            Logger.LogInfo($"Loaded {PluginName}!");
        }

        public static void LogFields()
        {
            string output = "\n";
            List<Type> types = new List<Type>() { typeof(PlayableCard), typeof(CardInfo), typeof(CardSlot) };

            foreach (Type obj in types)
            {
                FieldInfo[] fields = obj.GetFields();
                if (fields.Length > 0)
                {
                    string fieldheader = $"{obj.Name} fields:\n";
                    output += fieldheader;
                    output += new String('-', fieldheader.Length - 1) + "\n";

                    foreach (FieldInfo fieldinfo in fields)
                    {
                        output += $"{fieldinfo.Name} ({fieldinfo.FieldType.Name})\n";
                    }
                    output += new String('-', fieldheader.Length - 1) + "\n\n";
                }

                PropertyInfo[] properties = obj.GetProperties();
                if (properties.Length > 0)
                {
                    string propertyheader = $"{obj.Name} properties:\n";
                    output += propertyheader;
                    output += new String('-', propertyheader.Length - 1) + "\n";

                    foreach (PropertyInfo propertyinfo in properties)
                    {
                        output += $"{propertyinfo.Name} ({propertyinfo.PropertyType.Name})\n";
                    }
                    output += new String('-', propertyheader.Length - 1) + "\n\n";
                }
            }

            Plugin.Log.LogInfo(output);
        }

        public void LoadAll(List<string> files)
        {
            TribeList.LoadAllTribes(files);
            TraitList.LoadAllTraits(files);
            SigilData.LoadAllSigils(files);
            ItemData.LoadAllConsumableItems(files);

            // NOTE: I really don't want to do this, but I can't figure out how to get the game to load the cards from
            // the JSON files without listing all the damn extension files....
            CardSerializeInfo.LoadAllJLDR2(files);

            Data.EncounterData.LoadAllEncounters(files);
            StarterDeckList.LoadAllStarterDecks(files);
            GramophoneData.LoadAllGramophone(files);
            LanguageData.LoadAllLanguages(files);
            MaskData.LoadAllMasks(files);
            RegionSerializeInfo.LoadAllRegions(files);
            JSONLoader.Data.TalkingCards.LoadTalkingCards.InitAndLoad(files);
            // ^ Ambiguity between JSONLoader.Data and JLPlugin.Data is annoying. = u= -Kelly
        }

        public void Update()
        {
            hotkeyController.Update();
        }

        private void ReloadGame()
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

        public void ExportAllToJLDR2()
        {
            TribeList.ExportAllTribes();
            // SigilData.LoadAllSigils(files);
            ItemData.ExportAllItems();
            Data.EncounterData.ExportAllEncounters();
            StarterDeckList.ExportAllStarterDecks();
            // GramophoneData.LoadAllGramophone(files);
            LanguageData.ExportAllLanguages();
            RegionSerializeInfo.ExportAllRegions();
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
            if (Configs.VerboseLogging)
                Log.LogInfo(s);
        }

        internal static void VerboseWarning(string s)
        {
            if (Configs.VerboseLogging)
                Log.LogWarning(s);
        }

        internal static void VerboseError(string s)
        {
            if (Configs.VerboseLogging)
                Log.LogError(s);
        }
    }
}
