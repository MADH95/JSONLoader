using BepInEx;
using InscryptionAPI.Card;
using System.IO;
using System.Linq;
using TinyJson;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class TribeList
    {
        public class TribeInfo
        {
            public string name;
            public string guid;
            public string tribeIcon;
            public bool appearInTribeChoices;
            public string choiceCardBackTexture;
        }

        public TribeInfo[] tribes;

        public static void LoadAllTribes()
        {
            foreach (string file in Directory.EnumerateFiles(Paths.PluginPath, "*.jldr2", SearchOption.AllDirectories))
            {
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (filename.EndsWith("_tribe.jldr2"))
                {
                    Plugin.Log.LogDebug($"Loading JLDR2 (tribes) {filename}");
                    TribeList tribeInfo = JSONParser.FromJson<TribeList>(File.ReadAllText(file));
                    
                    foreach (var tribedata in tribeInfo.tribes)
                        TribeManager.Add(tribedata.guid, tribedata.name, tribedata.tribeIcon, tribedata.appearInTribeChoices, tribedata.choiceCardBackTexture);

                    Plugin.Log.LogDebug($"Loaded JSON tribes {string.Join(",", tribeInfo.tribes.Select(s => s.name).ToList())}");
                }
            }
        }
    }
}
