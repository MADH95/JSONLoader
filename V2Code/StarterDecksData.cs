using System.Collections.Generic;
using BepInEx;
using InscryptionAPI.Ascension;
using System.IO;
using System.Linq;
using TinyJson;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class StarterDeckList
    {
        public class StarterDeckInfo
        {
            public string name;
            public string[] cards;
            public string iconTexture;
            public int unlockLevel;
        }

        public StarterDeckInfo[] decks;

        public static void LoadAllStarterDecks(List<string> files)
        {
            for (int index = 0; index < files.Count; index++)
            {
                string file = files[index];
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (!filename.EndsWith("_deck.jldr2")) 
                    continue;
                
                files.RemoveAt(index--);
                
                Plugin.Log.LogDebug($"Loading JLDR2 (starter decks) {filename}");
                StarterDeckList starterDeckInfo = JSONParser.FromJson<StarterDeckList>(File.ReadAllText(file));

                foreach (var deckdata in starterDeckInfo.decks)
                    StarterDeckManager.New(Plugin.PluginGuid, deckdata.name, deckdata.iconTexture, deckdata.cards,
                        deckdata.unlockLevel);

                Plugin.Log.LogDebug(
                    $"Loaded JSON starter decks {string.Join(",", starterDeckInfo.decks.Select(s => s.name).ToList())}");
            }
        }
    }
}
