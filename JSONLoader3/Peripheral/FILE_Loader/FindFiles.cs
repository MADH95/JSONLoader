using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JSONLoader3.Subperipheral.JSONLoader_Configuration;

namespace JSONLoader3.Peripheral.FILE_Loader;

/// <summary>
/// This class handles the finding of files for the JSONLoaders and CSVLoader.
/// </summary>
public class FindFiles
{
    /// <summary>
    /// The Path to the Executing Assembly.
    /// </summary>
    private static Assembly assembly = Assembly.GetExecutingAssembly();
    /// <summary>
    /// The Path to the JSONLoader DLL.
    /// </summary>
    internal static string DLLPath = Path.GetDirectoryName(assembly.Location);

    /// <summary>
    /// A function which will find Files in which need to be loaded from the configured Paths.
    /// </summary>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static void FindFilesToLoad()
    {
        string BepInExPath = FindElementInPath(DLLPath, "BepInEx");
        if (BepInExPath != null)
        {
            string PathBase = BepInExPath + Path.DirectorySeparatorChar + "plugins";
            if (Directory.Exists(PathBase))
            {
                JSONLoader3.FormatLogger("Information", "FindFiles",
                    "We are now attempting to scan for files in which JSONLoader3 or CSVLoader will need to load. If you have verbose lFogging on we'll show all the paths as it happens.");

                List<string> plugins = Directory.GetDirectories(PathBase).ToList();
                foreach (string plugin in plugins)
                {
                    JSONLoader3.FormatLogger("Debug", "FindFiles", $"Checking Plugin: {plugin} for loadable JSON files.");
                    foreach (string JSONPath in DefineConfiguration.JSONLoadingPaths.Value.Split(','))
                    {
                        if (Directory.Exists(plugin + Path.DirectorySeparatorChar + JSONPath.Trim().Replace('/', Path.DirectorySeparatorChar)))
                        {
                            string newPath = plugin +
                                             Path.DirectorySeparatorChar + JSONPath.Trim().Replace('/', Path.DirectorySeparatorChar);
                            List<string> JSONFiles = Directory.GetFiles(newPath, "*", SearchOption.AllDirectories).ToList();
                            foreach (string JSONFile in JSONFiles)
                            {
                                HandleJSONFile(JSONFile, plugin);
                            }
                        }
                    }
                    
                    JSONLoader3.FormatLogger("Debug", "FindFiles", $"Checking Plugin: {plugin} for loadable CSV files.");
                    foreach (string CSVPath in DefineConfiguration.CSVLoadingPaths.Value.Split(','))
                    {
                        if (Directory.Exists(plugin + Path.DirectorySeparatorChar + CSVPath.Trim().Replace('/', Path.DirectorySeparatorChar)))
                        {
                            string newPath = plugin +
                                             Path.DirectorySeparatorChar + CSVPath.Trim().Replace('/', Path.DirectorySeparatorChar);
                            List<string> CSVFiles = Directory.GetFiles(newPath, "*", SearchOption.AllDirectories).ToList();
                            foreach (string CSVFile in CSVFiles)
                            {
                                HandleCSVFile(CSVFile, plugin);
                            }
                        }
                    }
                }
            }
            else
            {
                JSONLoader3.FormatLogger("Error", "FindFiles",
                    "Your install is missing a 'plugins' Folder, please ensure there is one if you are seeing this error arise and that it is lowercase.");
                JSONLoader3.FormatLogger("ExtendedInformation", "FindFiles",
                    "This error is mainly because for whatever reason you either don't have a 'plugins' folder, or your 'plugins' folder was capitalized wrong.");
            }
        }
        else
        {
            JSONLoader3.FormatLogger("Error", "FindFiles",
                "Not sure how this is even running right now, but this error is because JSONLoader was not found within the BepInEx path.");
            JSONLoader3.FormatLogger("ExtendedInformation", "FindFiles",
                "This error occurs primarily when this Plugin was not placed under the Plugins Folder, or if the Plugins folder could not be found. We expect Lowercase for the Path.");
        }
    }

    /// <summary>
    /// A function used to handle Files for JSONLoader.
    /// </summary>
    /// <param name="JSONFile">The exact path to a JSONLoader file.</param>
    /// <param name="plugin">The Full Path to the Plugin.</param>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    internal static void HandleJSONFile(string JSONFile, string plugin)
    {
        if (!JSONFile.EndsWith(".jldr", StringComparison.OrdinalIgnoreCase) && !JSONFile.EndsWith(".jldr2", StringComparison.OrdinalIgnoreCase) && !JSONFile.EndsWith(".jldr3", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }
        
        if (JSONFile.EndsWith("_example.jldr", StringComparison.OrdinalIgnoreCase) || JSONFile.EndsWith("_example.jldr2", StringComparison.OrdinalIgnoreCase) || JSONFile.EndsWith("_example.jldr3", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (Directory.Exists(JSONFile))
        {
            List<string> JSONFilesNew = Directory.GetFiles(JSONFile).ToList();
            foreach (string JSONFileNew in JSONFilesNew)
            {
                HandleJSONFile(JSONFileNew, plugin);
            }
        }
        
        if (JSONFile.EndsWith(".jldr", StringComparison.OrdinalIgnoreCase))
        {
            LoadFiles.JLDRFiles.Add((plugin, JSONFile));
        }
        else if (JSONFile.EndsWith(".jldr2", StringComparison.OrdinalIgnoreCase))
        {
            LoadFiles.JLDR2Files.Add((plugin, JSONFile));
        }
        else if (JSONFile.EndsWith(".jldr3", StringComparison.OrdinalIgnoreCase))
        {
            LoadFiles.JLDR3Files.Add((plugin, JSONFile));
        }
                                
        JSONLoader3.FormatLogger("Debug", "FindFiles", $"Found File: {JSONFile} to load.");
    }
    
    /// <summary>
    /// A function used to handle Files for CSVLoader.
    /// </summary>
    /// <param name="CSVFile">The exact path to a CSVLoader path.</param>
    /// <param name="plugin">The Full Path to the Plugin.</param>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    internal static void HandleCSVFile(string CSVFile, string plugin)
    {
        if (!CSVFile.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (CSVFile.EndsWith("_example.csv", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (Directory.Exists(CSVFile))
        {
            List<string> CSVFilesNew = Directory.GetFiles(CSVFile).ToList();
            foreach (string CSVFileNew in CSVFilesNew)
            {
                HandleCSVFile(CSVFileNew, plugin);
            }
        }
        if (CSVFile.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
        {
            LoadFiles.CSVFiles.Add((plugin, CSVFile));
        }
                                
        JSONLoader3.FormatLogger("Debug", "FindFiles", $"Found File: {CSVFile} to load.");
    }

    /// <summary>
    /// A function to find a given element within the Path passed in.
    /// </summary>
    /// <param name="fullPath">The entire Path.</param>
    /// <param name="toFind">The folder in that path your trying to get a path to.</param>
    /// <returns>The Full Path up to the folder you were after, if not found we return a 'null' value.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    internal static string FindElementInPath(string fullPath, string toFind)
    {
        string newPath = fullPath;

        while (!string.IsNullOrEmpty(newPath))
        {
            string part = Path.GetFileName(newPath);

            if (string.Equals(part, toFind, StringComparison.OrdinalIgnoreCase))
            {
                return newPath;
            }
            
            newPath = Path.GetDirectoryName(newPath);
        }
        
        return null;
    }
}