using System;
using System.Collections.Generic;
using System.IO;
using TinyJson;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class AppearenceBehaviors
    {
        public class AppearenceBehaviorInfo
        {
            public string Name;
            public string GUID;
            public string Layer;
            public List<string> ImageList = new List<string>();
            public bool Randomize;
            public string RandomizeConditions;
        }

        public static AppearenceBehaviorInfo[] AppearenceBehavior;

        public static void LoadAllAppearences(List<string> files)
        {
            for (int index = 0; index < files.Count; index++)
            {
                string file = files[index];
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar));

                if (!filename.ToLower().EndsWith("_appearence.jldr2"))
                    continue;

                ImportExportUtils.SetDebugPath(file);
                files.RemoveAt(index--);

                try
                {
                    Plugin.VerboseLog($"Loading JLDR2 (appearence behavior) {filename}");
                    AppearenceBehaviorInfo AppearenceBehaviors = JSONParser.FromFilePath<AppearenceBehaviorInfo>(file);
                    foreach (AppearenceBehaviorInfo appearencedata in AppearenceBehavior)
                    {
                        // Grant Logic
                    }
                }
                catch (Exception e)
                {
                    Plugin.Log.LogError($"Error loading appearence from file {file}");
                    Plugin.Log.LogError(e);
                }
            }
        }
    }
}