using System.Collections.Generic;
using BepInEx;
using JLPlugin;
using System.IO;
using TinyJson;
using InscryptionAPI.Sound;
using HarmonyLib;

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

                if (!filename.EndsWith("_gram.jldr2")) 
                    continue;
                
                files.RemoveAt(index--);
                
                Plugin.Log.LogDebug($"Loading JLDR2 (gramophone) {filename}");
                GramophoneInfo gramInfo = JSONParser.FromJson<GramophoneInfo>(File.ReadAllText(file));

                string guidAndPrefix = $"{Plugin.PluginGuid}_{gramInfo.Prefix ?? string.Empty}";

                foreach (TrackData track in gramInfo.Tracks)
                {
                    if (track == null) continue;
                    GramophoneManager.AddTrack(guidAndPrefix, track.Track, track.Volume ?? 1f);
                }

                Plugin.Log.LogDebug($"Loaded JSON gramophone tracks from {filename}!");
            }
        }
    }
}
