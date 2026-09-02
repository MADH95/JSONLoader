using System;
using BepInEx;
using Cecil_Libraries.ANSI_Utils.Lists;
using Cecil_Libraries.ANSI_Utils.Objects;
using JSONLoader3.Subperipheral.JSONLoader_Configuration;

namespace JSONLoader3
{
    /// <summary>
    /// This class is the Origin Point of the JSONLoader3 and CSVLoader API's.
    /// </summary>
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    public class JSONLoader3 : BaseUnityPlugin
    {
        /// <summary>
        /// This is the PluginGuid for the API, it will be hardcoded to this, if anything changes it will be addressed in the Changelog so you can fix any C# Dependencies you have upon it.
        /// </summary>
        public const string PluginGuid = "Chaosyr.MADH95.Inscryption.JSON.CSVLoader";
        /// <summary>
        /// This is the PluginName for the API, note it may change over time.
        /// </summary>
        public const string PluginName = "JSON_and_CSV_Loader";
        /// <summary>
        /// This resembles the Version of the API, when this is updated make sure to update the value.
        /// </summary>
        public const string PluginVersion = "3.0.0";

        /// <summary>
        /// This color is associated with the Error Logging Level.
        /// </summary>
        private static Color Error = new Color("Underline", "Red", highIntensity: true);
        /// <summary>
        /// This color is associated with the Warning Logging Level.
        /// </summary>
        private static Color Warning = new Color("Italic", "Yellow", highIntensity: true);
        /// <summary>
        /// This color is associated with the Information Logging Level.
        /// </summary>
        private static Color256 Information = new Color256("Regular", 230);
        /// <summary>
        /// This color is associated with the Debug Logging Level.
        /// </summary>
        private static Color256 Debug = new Color256("Dulled", 248);
        /// <summary>
        /// This color is associated with the ExtendedInformation Logging Level.
        /// </summary>
        private static Color256 ExtendedInformation = new Color256("Bold", 228);
        
        /// <summary>
        /// This serves as the Starting Point for the entire API, whatever is put here will be done first and foremost in startup.
        /// </summary>
        public void Awake()
        {
            DefineConfiguration.DefineConfigs(Config);
            FormatLogger("info", "Initialization for JSONLoader3 and CSVLoader","Finished Creating JSONLoader3 and CSVLoader Configuration");
        }

        /// <summary>
        /// This is a nifty function which will help automate and keep tidy our Logging System across our API. Below defines all of the levels, and what Message and Source reference.
        /// </summary>
        /// <param name="level">The following are all of the levels in which apply to our Logger.
        ///     <list type="table">
        ///         <listheader>
        ///             <term>Level</term>
        ///             <description>What It Is Meant For</description>
        ///         </listheader>
        ///         <item>
        ///             <term>Error</term>
        ///             <description>This is meant for any Errors in which this API may spit out.</description>
        ///         </item>
        ///         <item>
        ///             <term>Warning</term>
        ///             <description>This is meant for any Warnings in which this API may spit out (something that isn't quite right, but may still work).</description>
        ///         </item>
        ///         <item>
        ///             <term>Information (Info)</term>
        ///             <description>This is meant for any General Details in which this API may spit out.</description>
        ///         </item>
        ///         <item>
        ///             <term>ExtendedInformation (ExtendedInfo)</term>
        ///             <description>This is meant for any Additional Details in which this API may spit out.</description>
        ///         </item>
        ///         <item>
        ///             <term>Debug</term>
        ///             <description>This is meant for any Details in which this API usually would keep BTS but may spit out.</description>
        ///         </item>
        ///     </list>
        /// </param>
        /// <param name="message">This is the message in which is to be spit out.</param>
        /// <param name="source">This is where the message came from, so we can indentify the stemming point of the error.</param>
        /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
        public static void FormatLogger(string level, string source, string message)
        {
            if (level.ToLower() == "error")
            {
                Console.WriteLine(Error.Format() + $"[({source}) Error]: " + message + ANSICodeLists.ResetColor);
            } else if (level.ToLower() == "warning")
            {
                Console.WriteLine(Warning.Format() + $"[({source}) Warning]: "+ message + ANSICodeLists.ResetColor);
            } else if (level.ToLower() == "info" || level.ToLower() == "information")
            {
                Console.WriteLine(Information.Format() + $"[({source}) Information]: "+ message + ANSICodeLists.ResetColor);
            } else if (level.ToLower() == "debug" && DefineConfiguration.ShowVerboseLogging.Value)
            {
                Console.WriteLine(Debug.Format() + $"[({source}) Debug]: "+ message + ANSICodeLists.ResetColor);
            } else if ((level.ToLower() == "extendedinfo" || level.ToLower() == "extendedinformation") && DefineConfiguration.ShowAdditionalInformation.Value)
            {
                Console.WriteLine(ExtendedInformation.Format() + $"[({source}) Extended Information]: "+ message + ANSICodeLists.ResetColor);
            }
        }
    }
}