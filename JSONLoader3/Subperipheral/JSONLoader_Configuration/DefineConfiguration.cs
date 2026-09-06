using BepInEx.Configuration;

namespace JSONLoader3.Subperipheral.JSONLoader_Configuration;

/// <summary>
/// This class handles the defining of Configuration relevant to the JSONLoaders and CSVLoaders.
/// </summary>
public class DefineConfiguration
{
    /// <summary>
    /// This config determines the paths for JSON Based Loading in JSONLoader3+.
    /// </summary>
    public static ConfigEntry<string> JSONLoadingPaths;
    /// <summary>
    /// This config determines the paths for CSV Based Loading in CSVLoader.
    /// </summary>
    public static ConfigEntry<string> CSVLoadingPaths;
    /// <summary>
    /// This config determines where JSON Schemas will be saved to on boot.
    /// </summary>
    public static ConfigEntry<string> SchemaSavePath;
    /// <summary>
    /// This config determines whether VerboseLogging is enabled by the user or not.
    /// </summary>
    public static ConfigEntry<bool> ShowVerboseLogging;
    /// <summary>
    /// This config determines whether AdditionalInformation is enabled by the user or not.
    /// </summary>
    public static ConfigEntry<bool> ShowAdditionalInformation;
    /// <summary>
    /// The ConfigFile Variable referenced throughout this class.
    /// </summary>
    private static ConfigFile configFile;
    
    /// <summary>
    /// This function defines all of our Configurations into the Config associated with the API.
    /// </summary>
    /// <param name="config">(Arbitrary but represents this mods ConfigFile.)</param>
    public static void DefineConfigs(ConfigFile config)
    {
        configFile = config;
        
        JSONLoadingPaths = configFile.Bind("Configuration", "JSON Loading Origination Path", "Scripts, Plugins/Scripts",
            "These paths are case insensitive, and determine where JSON Scripts may be sourced from in order to load. If your a mod maker shipping mods, make a 'plugins' folder in your mods folder, and put a folder in there called 'scripts', this is where your JSON scripts should reside. If you need another path, you can override this value with your mod, we'll provide a system for you to do so.");
        CSVLoadingPaths = configFile.Bind("Configuration", "CSV Loading Origination Path", "Sheets, Plugins/Sheets",
            "These paths are case insensitive, and determine where CSV Sheets may be sourced from in order to load. If your a mod maker shipping mods, make a 'plugins' folder in your mods folder, and put a folder in there called 'sheets', this is where your CSV Sheets should reside. If you need another path, you can override this value with your mod, we'll provide a system for you to do so.");
        SchemaSavePath = configFile.Bind("Configuration", "Schema Save Path", "/Schemas", "This determines where JSON Schemas will be saved to as we create them, this path will be localized to the DLL's folder. You can use '../' to mean go up a folder.");
        JSONLoader3.FormatLogger("info", "Configuration","Finished adding configuration for the Loading Paths associated with this mod.");
        ShowVerboseLogging = configFile.Bind("Logging", "Show Verbose Logging", true,
            "While this value is set to true, this API will show what is happening when its happening.");
        ShowAdditionalInformation = configFile.Bind("Logging", "Show Additional Information", false,
            "While this value is set to true, this API will show a more in depth lense as to what went wrong, and try to help explain why its wrong with links to references.");
        JSONLoader3.FormatLogger("info", "Configuration","Finished adding configuration for the Logging associated with this mod.");
    }
}