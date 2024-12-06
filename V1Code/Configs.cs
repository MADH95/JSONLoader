
using BepInEx.Configuration;
using InscryptionAPI.Saves;
using System;

namespace JLPlugin
{
    internal static class Configs
    {
        internal static bool BetaCompatibility => betaCompatibility.Value;
        internal static bool VerboseLogging => verboseLogging.Value;
        internal static string ReloadHotkey => reloadHotkey.Value;
        internal static string ExportHotkey => exportHotkey.Value;
        internal static bool ExportAllLanguages => exportAllLanguages.Value;

        private static ConfigEntry<bool> betaCompatibility;
        private static ConfigEntry<bool> verboseLogging;
        private static ConfigEntry<bool> exportAllLanguages;
        private static ConfigEntry<string> reloadHotkey;
        private static ConfigEntry<string> exportHotkey;
        private static ConfigFile configFile;

        private static Version oldConfigVersion;
        private static Version currentVersion;

        public static void InitializeConfigs(ConfigFile config)
        {
            configFile = config;
            currentVersion = new Version(Plugin.PluginVersion);
            oldConfigVersion = GetOldConfigVersion();

            betaCompatibility = config.Bind("JSONLoader", "JDLR Backwards Compatibility", true, "Set this to true if your using a mod that utilizes the old `.jldr` system. If the mod your using uses `.json` use JSON Rename Utility by MadH95Mods on Thunderstore to convert them to `.jldr`.");
            verboseLogging = config.Bind("JSONLoader", "Verbose Logging", false, "Set this to true if you wish to enable debug logging that tells you exactly what JSONLoader is reading and a bit more in depth info on when its erroring, or just to fill your log while you wait.");
            exportAllLanguages = config.Bind("JSONLoader Exporting", "Export All Languages", false, "Set this to true if you wish to export all of the base games languages for everything you can do within JSONLoader.");
            reloadHotkey = config.Bind("Hotkeys", "Reload JLDR2 and game", "LeftShift+R", "Reloads the game whenever the keybind this is set to is pressed to re register all `.jldr2` files.");
            exportHotkey = config.Bind("Hotkeys", "Export all to JLDR2", "LeftControl+RightControl+X", "Exports everything from the base game that you can do with JSONLoader when the keybind is pressed.");

            MigrateConfigs();
            ModdedSaveManager.SaveData.SetValue(Plugin.PluginGuid, "LastLoadedVersion", currentVersion.ToString());
        }

        private static Version GetOldConfigVersion()
        {
            string oldVersion = ModdedSaveManager.SaveData.GetValue(Plugin.PluginGuid, "LastLoadedVersion");
            if (string.IsNullOrEmpty(oldVersion))
            {
                // Configs exist but they deleted their save data!
                // This means their configs need verifying
                return new Version("2.5.2"); // 2.5.3 we added the save data so this is our earliest migration version
            }

            // Configs and save up to date. Migrate if the versions do not match!
            return new Version(oldVersion); // Everything up to date!
        }

        private static void MigrateConfigs()
        {
            if (oldConfigVersion == currentVersion)
            {
                // Nothing to migrate!
                return;
            }

            if (oldConfigVersion <= new Version("2.5.3"))
            {
                Plugin.Log.LogInfo($"Migrating from {oldConfigVersion} to {currentVersion}!");
                if (ReloadHotkey == (string)exportHotkey.DefaultValue &&
                    ExportHotkey == (string)reloadHotkey.DefaultValue)
                {
                    Plugin.Log.LogInfo($"\tMigrating hotkeys to new defaults!");
                    exportHotkey.Value = (string)exportHotkey.DefaultValue;
                    reloadHotkey.Value = (string)reloadHotkey.DefaultValue;
                    configFile.Save();
                }
            }
        }
    }
}