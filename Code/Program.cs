using BepInEx;
using BepInEx.Logging;

using HarmonyLib;

namespace JLPlugin
{
    [BepInPlugin( PluginGuid, PluginName, PluginVersion )]
    [BepInDependency( "cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency )]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "MADH.inscryption.JSONLoader";
        private const string PluginName = "JSONLoader";
        private const string PluginVersion = "1.8.1.0";

        internal static ManualLogSource Log;

        private void Awake()
        {
            Logger.LogInfo( $"Loaded {PluginName}!" );
            Log = base.Logger;

            Harmony harmony = new(PluginGuid);
            harmony.PatchAll();

            Log.LogWarning( "Note: JSONLoader now uses .jldr files, not .json files" );

            Utils.JLUtils.LoadCardsFromFiles();
            Utils.JLUtils.LoadEncountersFromFiles();
        }
    }
}
