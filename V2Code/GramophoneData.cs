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

        public static void LoadAllGramophone()
        {
            foreach (string file in Directory.EnumerateFiles(Paths.PluginPath, "*.jldr2", SearchOption.AllDirectories))
            {
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1f);

                if (filename.EndsWith("_gram.jldr2"))
                {
                    Plugin.Log.LogDebug($"Loading JLDR2 (gramophone) {filename}");
                    GramophoneInfo gramInfo = JSONParser.FromJson<GramophoneInfo>(File.ReadAllText(file));

                    string guidAndPrefix = $"{Plugin.PluginGuid}_{gramInfo.Prefix ?? string.Empty}";

                    foreach (TrackData track in gramInfo.Tracks)
                    {
                        if (track == null) continue;
                        GramophoneManager.AddTrack(guidAndPrefix, track.Track, track.Volume ?? 1);
                    }

                    Plugin.Log.LogDebug($"Loaded JSON gramophone tracks from {filename}!");
                }
            }
        }
    }
}
