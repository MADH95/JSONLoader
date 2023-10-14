using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Encounters;
using InscryptionAPI.Regions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TinyJson;
using UnityEngine;

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

        public static void Process(EncounterBlueprintData encounter, EncounterInfo encounterInfo, bool toEncounter)
        {
            ImportExportUtils.SetID(toEncounter ? encounterInfo.name : encounter.name);
            
            ImportExportUtils.ApplyProperty(()=>encounter.name, (a)=>encounter.name = a, ref encounterInfo.name, toEncounter, "Encounters", "name");
            ImportExportUtils.ApplyValue(ref encounter.minDifficulty, ref encounterInfo.minDifficulty, toEncounter, "Encounters", "minDifficulty");
            ImportExportUtils.ApplyValue(ref encounter.maxDifficulty, ref encounterInfo.maxDifficulty, toEncounter, "Encounters", "maxDifficulty");
            ImportExportUtils.ApplyValue(ref encounter.dominantTribes, ref encounterInfo.dominantTribes, toEncounter, "Encounters", "dominantTribes");
            ImportExportUtils.ApplyValue(ref encounter.randomReplacementCards, ref encounterInfo.randomReplacementCards, toEncounter, "Encounters", "randomReplacementCards");
            ImportExportUtils.ApplyValue(ref encounter.redundantAbilities, ref encounterInfo.redundantAbilities, toEncounter, "Encounters", "redundantAbilities");

            if (toEncounter)
            {
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
                            TurnCardInfo.replacement =
                                CardManager.AllCardsCopy.CardByName(turncardinfodata.difficultyReplacement);
                        }

                        if (turncardinfodata.difficultyReq != null)
                        {
                            TurnCardInfo.difficultyReq = (int)turncardinfodata.difficultyReq;
                        }

                        TurnCardList.Add(TurnCardInfo);
                    }

                    encounter.AddTurn(TurnCardList.ToArray());
                }
            }
            else
            {
                if (encounter.turns != null)
                {
                    encounterInfo.turns = new List<TurnInfo>();
                    foreach (List<EncounterBlueprintData.CardBlueprint> turn in encounter.turns)
                    {
                        if(turn == null)
                            continue;
                        
                        TurnInfo turnInfo = new TurnInfo();
                        turnInfo.cardInfo = new List<TurnCardInfo>();
                        foreach (EncounterBlueprintData.CardBlueprint card in turn)
                        {
                            TurnCardInfo turnCardInfo = new TurnCardInfo();
                            turnCardInfo.randomReplaceChance = card.randomReplaceChance;
                            turnCardInfo.difficultyReq = card.difficultyReq;
                            
                            if(card.card != null)
                                turnCardInfo.card = card.card.name;
                            
                            if(card.replacement != null)
                                turnCardInfo.difficultyReplacement = card.replacement.name;

                            turnInfo.cardInfo.Add(turnCardInfo);
                        }

                        encounterInfo.turns.Add(turnInfo);
                    }
                }
            }
            
            if (toEncounter)
            {
                if (encounterInfo.regions != null)
                {
                    foreach (RegionData Region in RegionManager.AllRegionsCopy
                                 .Where(x => encounterInfo.regions.Contains(x.name)).ToList())
                    {
                        RegionExtensions.AddEncounters(Region, encounter);
                    }
                }
            }
            else
            {
                RegionData[] regionDatas = RegionManager.AllRegionsCopy.FindAll((a) =>
                    a.encounters.FirstOrDefault((a) => a.name == encounter.name) != null).ToArray();
                encounterInfo.regions = regionDatas.Select((a) => a.name).ToList();
            }
        }
        
        public static void LoadAllEncounters(List<string> files)
        {
            for (int index = 0; index < files.Count; index++)
            {
                string file = files[index];
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (!filename.EndsWith("_encounter.jldr2")) 
                    continue;
                
                files.RemoveAt(index--);
                
                Plugin.Log.LogDebug($"Loading JLDR2 (encounters) {filename}");
                EncounterInfo encounterInfo = JSONParser.FromJson<EncounterInfo>(File.ReadAllText(file));

                EncounterBlueprintData encounter = EncounterManager.New(encounterInfo.name);

                if (encounterInfo.minDifficulty != null && encounterInfo.maxDifficulty != null)
                {
                    encounter.SetDifficulty((int)encounterInfo.minDifficulty, (int)encounterInfo.maxDifficulty);
                }

                if (encounterInfo.dominantTribes != null)
                {
                    encounter.dominantTribes = encounterInfo.dominantTribes
                        .Select(ImportExportUtils.ParseEnum<Tribe>).ToList();
                }

                if (encounterInfo.randomReplacementCards != null)
                {
                    encounter.randomReplacementCards = encounterInfo.randomReplacementCards
                        .Select(x => CardManager.AllCardsCopy.CardByName(x)).ToList();
                }

                if (encounterInfo.redundantAbilities != null)
                {
                    encounter.SetRedundantAbilities(encounterInfo.redundantAbilities
                        .Select(ImportExportUtils.ParseEnum<Ability>).ToArray());
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
                            TurnCardInfo.replacement =
                                CardManager.AllCardsCopy.CardByName(turncardinfodata.difficultyReplacement);
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
                    foreach (RegionData Region in RegionManager.AllRegionsCopy
                                 .Where(x => encounterInfo.regions.Contains(x.name)).ToList())
                    {
                        RegionExtensions.AddEncounters(Region, encounter);
                    }
                }
            }
        }

        public static void ExportAllEncounters()
        {
            Plugin.Log.LogInfo($"Exporting {EncounterManager.AllEncountersCopy.Count} Encounters to JSON");
            foreach (EncounterBlueprintData tribe in EncounterManager.AllEncountersCopy)
            {
                ExportEncounter(tribe);
            }
        }
        
        public static void ExportEncounter(EncounterBlueprintData info)
        {
            string path = Path.Combine(Plugin.ExportDirectory, "Encounters");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            EncounterInfo serializedTribe = new EncounterInfo();
            Process(info, serializedTribe, false);
            
            string json = JSONParser.ToJSON(serializedTribe);
            File.WriteAllText(Path.Combine(path, serializedTribe.name + ".jldr2"), json);
        }
    }
}
