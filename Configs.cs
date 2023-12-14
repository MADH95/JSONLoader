
using BepInEx.Configuration;

namespace JLPlugin
{
    internal static class Configs
    {
        internal static bool BetaCompatibility => betaCompatibility.Value;
        internal static bool VerboseLogging => verboseLogging.Value;
        internal static string ReloadHotkey => reloadHotkey.Value;
        internal static string ExportHotkey => exportHotkey.Value;

        private static ConfigEntry<bool> betaCompatibility;
        private static ConfigEntry<bool> verboseLogging;
        private static ConfigEntry<string> reloadHotkey;
        private static ConfigEntry<string> exportHotkey;

        public static void InitializeConfigs(ConfigFile configFile)
        {
            betaCompatibility = configFile.Bind("JSONLoader", "JDLR Backwards Compatibility", true, "Set to true to enable old-style JSON files (JLDR) to be read and converted to new-style files (JLDR2)");
            verboseLogging = configFile.Bind("JSONLoader", "Verbose Logging", false, "Set to true to see more logs on what JSONLoader is doing and what isn't working.");
            reloadHotkey = configFile.Bind("Hotkeys", "Reload JLDR2 and game", "LeftShift+R", "Restarts the game and reloads all JLDR2 files.");
            exportHotkey = configFile.Bind("Hotkeys", "Export all to JLDR2", "LeftControl+RightControl+X", "Exports all data in the game back to .JLDR2 files.");
        }
    }
}