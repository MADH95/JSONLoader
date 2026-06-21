using System.Collections.Generic;
using JLPlugin;
using System.IO;
using TinyJson;
using InscryptionAPI.Sound;

namespace JSONLoader.Data
{
    [System.Serializable]
    public class GramophoneData
    {
        public class GramophoneInfo
        {
            public string Prefix;
            public TrackData[] Tracks;
        }

        public class TrackData
        {
            public string Track;
            public float? Volume;
        }

        public static void LoadAllGramophone(List<string> files)
        {
            for (int index = 0; index < files.Count; index++)
            {
                string file = files[index];
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (!filename.ToLower().EndsWith("_gram.jldr2")) 
                    continue;
                
                files.RemoveAt(index--);
                ImportExportUtils.SetDebugPath(file);
                
                try
                {
                    Plugin.VerboseLog($"Loading JLDR2 (gramophone) {filename}");
                    GramophoneInfo gramInfo = JSONParser.FromFilePath<GramophoneInfo>(file);

                    string guidAndPrefix = $"{Plugin.PluginGuid}_{gramInfo.Prefix ?? string.Empty}";

                    foreach (TrackData track in gramInfo.Tracks)
                    {
                        if (track == null) continue;
                        GramophoneManager.AddTrack(guidAndPrefix, track.Track, track.Volume ?? 1f);
                    }

                    Plugin.VerboseLog($"Loaded JSON gramophone tracks from {filename}!");
                }   
                catch (System.Exception ex)
                {
                    Plugin.Log.LogError($"Error loading JLDR2 (graphaphone) {filename}");
                    Plugin.Log.LogError(ex);
                }
            }
        }
    }
}
