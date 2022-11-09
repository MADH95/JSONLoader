using BepInEx;
using InscryptionAPI.Ascension;
using JLPlugin.Data;
using JLPlugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TinyJson;

namespace JSONLoader.V2Code
{
    internal class GramophoneData
    {
        public class GramophoneInfo
        {
            public string Guid;
            public string[] Tracks;
        }

        public static void LoadAllGramophone()
        {
            foreach (string file in Directory.EnumerateFiles(Paths.PluginPath, "*.jldr2", SearchOption.AllDirectories))
            {
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (filename.EndsWith("_gram.jldr2"))
                {
                    Plugin.Log.LogDebug($"Loading JLDR2 (gramophone) {filename}");
                    GramophoneInfo gramInfo = JSONParser.FromJson<GramophoneInfo>(File.ReadAllText(file));

                    foreach (string track in gramInfo.Tracks)
                    {
                        // Have to comment it out as assembly isn't added yet.
                        // GramophoneManager.Add(gramInfo.Guid, track);
                    }

                    Plugin.Log.LogDebug($"Loaded JSON gramophone tracks for {gramInfo.Guid}!");
                }
            }
        }
    }
}
