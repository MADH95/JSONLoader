using BepInEx;
using DiskCardGame;
using InscryptionAPI.Ascension;
using InscryptionAPI.Helpers;
using JLPlugin.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TinyJson;
using UnityEngine;

namespace JLPlugin.Data
{
    public partial class StarterDecksDataMain
    {
        public static void LoadAllStarterDecks()
        {
            foreach (string file in Directory.EnumerateFiles(Paths.PluginPath, "*.jldr2", SearchOption.AllDirectories))
            {
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (filename.EndsWith("_deck.jldr2"))
                {
                    Plugin.Log.LogDebug($"Loading JLDR2 (starter decks) {filename}");
                    StarterDecksDataMain starterDeckInfo = JSONParser.FromJson<StarterDecksDataMain>(File.ReadAllText(file));
                    starterDeckInfo.GenerateNew();
                    Plugin.Log.LogDebug($"Loaded JSON starter decks {string.Join(",", starterDeckInfo.starterDecksData.Select(s => s.name).ToList())}");
                }
            }
        }

        public void GenerateNew()
        {
            foreach (StarterDecksData deckdata in this.starterDecksData)
            {
                Texture2D StarterDeckIcon = CDUtils.Assign(deckdata.iconTexture, nameof(deckdata.iconTexture));
                StarterDeckInfo StarterDeck = ScriptableObject.CreateInstance<StarterDeckInfo>();
                StarterDeck.title = deckdata.name;
                StarterDeck.iconSprite = TextureHelper.ConvertTexture(StarterDeckIcon, TextureHelper.SpriteType.StarterDeckIcon);
                StarterDeck.cards = new List<CardInfo>();
                foreach (string card in deckdata.cards)
                {
                    StarterDeck.cards.Add(CardLoader.GetCardByName(card));
                }

                StarterDeckManager.Add(Plugin.PluginGuid, StarterDeck);
            }
        }
    }
}