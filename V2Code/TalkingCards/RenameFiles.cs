using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;

namespace JSONLoader.Data.TalkingCards
{
    internal static class RenameFiles
    {
        internal static string[] FindJSON()
            => Directory.GetFiles(Paths.PluginPath, "*_talk.json", SearchOption.AllDirectories);

        private static void LogInfo(string message)
            => JLPlugin.Plugin.Log?.LogInfo(message);

        private static void LogError(string message)
            => JLPlugin.Plugin.Log?.LogError(message);

        internal static void RenameAll()
        {
            string[] files = FindJSON();
            if(files.Length > 0)
            {
                LogInfo($"TalkingCards: Found {files.Length} \'_talk.json\' files.");
                LogInfo($"Renaming them to \'_talk.jldr2\' files for backwards compatibility!");
            }

            foreach (string file in files) Rename(file);
        }

        internal static void Rename(string filePath)
        {
            if (!filePath.EndsWith("_talk.json")) return;
            string noExtension = filePath.Substring(0, filePath.Length - ".json".Length);
            string newFilePath = $"{noExtension}.jldr2";

            string oldName = Path.GetFileName(filePath);
            string newName = Path.GetFileName(newFilePath);

            if (!File.Exists(newFilePath))
            {
                File.Move(filePath, newFilePath);
                LogInfo($"Renamed file \'{oldName}\' to \'{newName}\' for backwards compatibility.");
            }
            else
            {
                string fileDir = new DirectoryInfo(filePath).Parent.Name;
                LogError($"Couldn't rename file {oldName} to {newName} as there's already a file named {newName} in the {fileDir} directory.");
            }
        }
    }
}
