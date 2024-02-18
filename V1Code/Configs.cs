
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

            betaCompatibility = config.Bind("JSONLoader", "JDLR Backwards Compatibility", true, "Set to true to enable old-style JSON files (JLDR) to be read and converted to new-style files (JLDR2)");
            verboseLogging = config.Bind("JSONLoader", "Verbose Logging", false, "Set to true to see more logs on what JSONLoader is doing and what isn't working.");
            exportAllLanguages = config.Bind("JSONLoader Exporting", "Export All Languages", false, "Set to true to export all languages for all cards, items, sigils... etc.");
            reloadHotkey = config.Bind("Hotkeys", "Reload JLDR2 and game", "LeftShift+R", "Restarts the game and reloads all JLDR2 files.");
            exportHotkey = config.Bind("Hotkeys", "Export all to JLDR2", "LeftControl+RightControl+X", "Exports all data in the game back to .JLDR2 files.");

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