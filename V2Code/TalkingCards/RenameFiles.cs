using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;

namespace JSONLoader.Data.TalkingCards
{
    internal static class RenameFiles
    {
        /* Rename any '_talk.json' files to '_talk.jldr2' (for backwards compatibility with my TalkingCardAPI). c:
         * It's a simple little change. If none are found, this does nothing. */

        private static string[] FindJSON()
            => Directory.GetFiles(Paths.PluginPath, "*_talk.json", SearchOption.AllDirectories);

        internal static void RenameAll()
        {
            string[] files = FindJSON();
            if (files.Length == 0) return;

            LogHelpers.LogInfo($"TalkingCards: Found {files.Length} \'_talk.json\' files.");
            LogHelpers.LogInfo($"Renaming each to end in \'_talk.jldr2\' instead for compatibility!");

            foreach (string file in files) Rename(file);
        }

        internal static void Rename(string filePath)
        {
            if (!filePath.EndsWith("_talk.json")) 
                return;
            
            string noExtension = filePath.Substring(0, filePath.Length - ".json".Length);
            string newFilePath = $"{noExtension}.jldr2";

            string oldName = Path.GetFileName(filePath);
            string newName = Path.GetFileName(newFilePath);

            if (!File.Exists(newFilePath))
            {
                File.Move(filePath, newFilePath);
                LogHelpers.DebugLog($"Renamed file \'{oldName}\' to \'{newName}\' for compatibility.");
            }
            else
            {
                string fileDir = Path.GetDirectoryName(filePath);
                LogHelpers.LogError($"Couldn't rename file \'{oldName}\' to \'{newName}\' as there's already a file named \'{newName}\' in the \'{fileDir}\' directory.");
            }
        }
    }
}
