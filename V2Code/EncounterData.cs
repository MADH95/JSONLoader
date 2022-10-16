using BepInEx;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Encounters;
using InscryptionAPI.Regions;
using JLPlugin.V2.Data;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TinyJson;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class EncounterData
    {

        public class EncounterInfo
        {
            public string name;
            public int? minDifficulty;
            public int? maxDifficulty;
            public List<string> regions;
            public List<string> dominantTribes;
            public List<string> randomReplacementCards;
            public List<string> redundantAbilities;
            public List<TurnInfo> turns;
        }

        public class TurnInfo
        {
            public List<TurnCardInfo> cardInfo;
        }

        public class TurnCardInfo
        {
            public string card;
            public int? randomReplaceChance;
            public int? difficultyReq;
            public string difficultyReplacement;
        }

        public static void LoadAllEncounters()
        {
            foreach (string file in Directory.EnumerateFiles(Paths.PluginPath, "*.jldr2", SearchOption.AllDirectories))
            {
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (filename.EndsWith("_encounter.jldr2"))
                {
                    Plugin.Log.LogDebug($"Loading JLDR2 (encounters) {filename}");
                    EncounterInfo encounterInfo = JSONParser.FromJson<EncounterInfo>(File.ReadAllText(file));

                    EncounterBlueprintData encounter = EncounterManager.New(encounterInfo.name);

                    if (encounterInfo.minDifficulty != null && encounterInfo.maxDifficulty != null)
                    {
                        encounter.SetDifficulty((int)encounterInfo.minDifficulty, (int)encounterInfo.maxDifficulty);
                    }
                    if (encounterInfo.dominantTribes != null)
                    {
                        encounter.dominantTribes = encounterInfo.dominantTribes.Select(x => CardSerializeInfo.ParseEnum<Tribe>(x)).ToList();
                    }
                    if (encounterInfo.randomReplacementCards != null)
                    {
                        encounter.randomReplacementCards = encounterInfo.randomReplacementCards.Select(x => CardManager.AllCardsCopy.CardByName(x)).ToList();
                    }
                    if (encounterInfo.redundantAbilities != null)
                    {
                        encounter.SetRedundantAbilities(encounterInfo.redundantAbilities.Select(s => CardSerializeInfo.ParseEnum<Ability>(s)).ToArray());
                    }
                    foreach (TurnInfo turndata in encounterInfo.turns)
                    {
                        List<EncounterBlueprintData.CardBlueprint> TurnCardList = new List<EncounterBlueprintData.CardBlueprint>();
                        foreach (TurnCardInfo turncardinfodata in turndata.cardInfo)
                        {
                            EncounterBlueprintData.CardBlueprint TurnCardInfo = new EncounterBlueprintData.CardBlueprint();
                            TurnCardInfo.card = CardManager.AllCardsCopy.CardByName(turncardinfodata.card);
                            if (turncardinfodata.randomReplaceChance != null)
                            {
                                TurnCardInfo.randomReplaceChance = (int)turncardinfodata.randomReplaceChance;
                            }
                            if (turncardinfodata.difficultyReplacement != null)
                            {
                                TurnCardInfo.difficultyReplace = true;
                                TurnCardInfo.replacement = CardManager.AllCardsCopy.CardByName(turncardinfodata.difficultyReplacement);
                            }
                            if (turncardinfodata.difficultyReq != null)
                            {
                                TurnCardInfo.difficultyReq = (int)turncardinfodata.difficultyReq;
                            }
                            TurnCardList.Add(TurnCardInfo);
                        }
                        encounter.AddTurn(TurnCardList.ToArray());
                    }

                    if (encounterInfo.regions != null)
                    {
                        foreach (RegionData Region in RegionManager.AllRegionsCopy.Where(x => encounterInfo.regions.Contains(x.name)).ToList())
                        {
                            RegionExtensions.AddEncounters(Region, encounter);
                        }
                    }
                }
            }
        }
    }
}
