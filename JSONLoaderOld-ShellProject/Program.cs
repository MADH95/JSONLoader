using BepInEx;

namespace JLPlugin
{
    /// <summary>
    /// The Stubbed Plugin.
    /// </summary>
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        /// <summary>
        /// The Plugins GUID
        /// </summary>
        public const string PluginGuid = "MADH.inscryption.JSONLoader";
        /// <summary>
        /// The Plugins Name
        /// </summary>
        public const string PluginName = "JSONLoader";
        /// <summary>
        /// The Plugins Version.
        /// </summary>
        public const string PluginVersion = "3.0.0";

        /// <summary>
        /// A stubbed Awake function thats called during load.
        /// </summary>
        public void Awake()
        {
            Logger.LogInfo($"Hey we are writing this to inform you that the OLD JSONLoader will not load while you have Nightly Installed.");
        }
    }
}
