using System.Collections.Generic;
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
                
                Plugin.VerboseLog($"Loading JLDR2 (starter decks) {filename}");
                StarterDeckList starterDeckInfo = JSONParser.FromFilePath<StarterDeckList>(file);

                foreach (var deckdata in starterDeckInfo.decks)
                    StarterDeckManager.New(Plugin.PluginGuid, deckdata.name, deckdata.iconTexture, deckdata.cards,
                        deckdata.unlockLevel);

                Plugin.VerboseLog(
                    $"Loaded JSON starter decks {string.Join(",", starterDeckInfo.decks.Select(s => s.name).ToList())}");
            }
        }
        
        public static void Process(StarterDeckManager.FullStarterDeck deckInfo, StarterDeckInfo serializeInfo, bool toDeckInfo, string path)
        {
            ImportExportUtils.SetDebugPath(path);
            ImportExportUtils.SetID(toDeckInfo ? serializeInfo.name : deckInfo.Info.name);
            ImportExportUtils.ApplyProperty(()=>deckInfo.Info.name, (a)=>deckInfo.Info.name=a, ref serializeInfo.name, toDeckInfo, "StarterDecks", "name");
            ImportExportUtils.ApplyProperty(()=>deckInfo.CardNames, (a)=>deckInfo.CardNames=a, ref serializeInfo.cards, toDeckInfo, "StarterDecks", "cards");
            ImportExportUtils.ApplyValue(ref deckInfo.Info.iconSprite, ref serializeInfo.iconTexture, toDeckInfo, "StarterDecks", "iconTexture");
            ImportExportUtils.ApplyProperty(()=>deckInfo.UnlockLevel, (a)=>deckInfo.UnlockLevel=a, ref serializeInfo.unlockLevel, toDeckInfo, "StarterDecks", "unlockLevel");
        }

        public static void ExportAllStarterDecks()
        {
            Plugin.Log.LogInfo($"Exporting {StarterDeckManager.AllDecks.Count} Starter decks");
            foreach (StarterDeckManager.FullStarterDeck deck in StarterDeckManager.AllDecks)
            {
                StarterDeckInfo serializeDeck = new StarterDeckInfo();
                string path = Path.Combine(Plugin.ExportDirectory, "StarterDecks", deck.Info.name + "_deck.jldr2");
                Process(deck, serializeDeck, false, path);
                
                string directory = Path.GetDirectoryName(path);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                
                File.WriteAllText(path, JSONParser.ToJSON(serializeDeck));
            }
        }
    }
}
