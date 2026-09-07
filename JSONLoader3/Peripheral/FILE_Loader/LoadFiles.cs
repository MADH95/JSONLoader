using System.Collections.Generic;
using JSONLoader3.Cores.JSONLoaderV1Support.Schemas;
using JSONLoader3.Cores.JSONLoaderV1Support.Utilities;
using JSONLoader3.Peripheral.JSON_SCHEMA;
using JSONLoader3.Peripheral.XML_Parser;

namespace JSONLoader3.Peripheral.FILE_Loader;

/// <summary>
/// This class handles the Loading of Files into their Respective Lists.
/// </summary>
public class LoadFiles
{
    /// <summary>
    /// A list of ALL JLDR Files.
    /// </summary>
    internal static List<(string plugin, string file)> JLDRFiles = new List<(string plugin, string file)>();
    /// <summary>
    /// A list of ALL JLDR2 Files.
    /// </summary>
    internal static List<(string plugin, string file)> JLDR2Files = new List<(string plugin, string file)>();
    /// <summary>
    /// A list of all JLDR3 Files.
    /// </summary>
    internal static List<(string plugin, string file)> JLDR3Files = new List<(string plugin, string file)>();
    /// <summary>
    /// A list of all CSV Files.
    /// </summary>
    internal static List<(string plugin, string file)> CSVFiles = new List<(string plugin, string file)>();

    /// <summary>
    /// A function to Load All Found Files.
    /// </summary>
    public static void LoadFoundFiles()
    {
        JSONLoader3.FormatLogger("Info", "LoadFiles", "Loading XML Documentation File!!");
        ReadDocumentationFile.FetchDocFile();
        
        JSONLoader3.FormatLogger("Info", "LoadFiles", "Loading JLDR Files!!");
        foreach ((string pluginName, string file) in JLDRFiles)
        {
            CardUtils.CardsToLoad.Add((pluginName, file));
        }
        CardUtils.HandleCards();
        
        JSONLoader3.FormatLogger("Info", "LoadFiles", "Loading JLDR2 Files!!");
        foreach ((string pluginName, string file) in JLDR2Files)
        {
            
        }
        
        JSONLoader3.FormatLogger("Info", "LoadFiles", "Loading JLDR3 Files!!");
        foreach ((string pluginName, string file) in JLDR3Files)
        {
            
        }
        
        JSONLoader3.FormatLogger("Info", "LoadFiles", "Loading CSV Files!!");
        foreach ((string pluginName, string file) in CSVFiles)
        {
            
        }
    }
}