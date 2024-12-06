using System.IO;
using BepInEx;

namespace JSONLoader.Data.TalkingCards
{
    internal static class RenameFiles
    {
        /* Rename any '_talk.json' files to '_talk.jldr2' (for backwards compatibility with my TalkingCardAPI). c:
         * It's a simple little change. If none are found, this does nothing. */

        private static string[] FindJSON()
            => Directory.GetFiles(Paths.PluginPath, "*_talk.json", SearchOption.AllDirectories);

        internal static string[] RenameAll()
        {
            string[] files = FindJSON();
            if (files.Length == 0) 
                return null;

            LogHelpers.LogInfo($"TalkingCards: Found {files.Length} \'_talk.json\' files.");
            LogHelpers.LogInfo($"Renaming each to end in \'_talk.jldr2\' instead for compatibility!");

            for (int index = 0; index < files.Length; index++)
            {
                var file = files[index];
                string newFilePath = Rename(file);
                files[index] = newFilePath;
            }

            return files;
        }

        internal static string Rename(string filePath)
        {
            if (!filePath.ToLower().EndsWith("_talk.json")) 
                return null;
            
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

            return newFilePath;
        }
    }
}
