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

        public static void Process(EncounterBlueprintData encounter, EncounterInfo encounterInfo, bool toEncounter, string path)
        {
            ImportExportUtils.SetDebugPath(path);
            ImportExportUtils.SetID(toEncounter ? encounterInfo.name : encounter.name);
            
            ImportExportUtils.ApplyProperty(()=>encounter.name, (a)=>encounter.name = a, ref encounterInfo.name, toEncounter, "Encounters", "name");
            ImportExportUtils.ApplyValue(ref encounter.minDifficulty, ref encounterInfo.minDifficulty, toEncounter, "Encounters", "minDifficulty");
            ImportExportUtils.ApplyValue(ref encounter.maxDifficulty, ref encounterInfo.maxDifficulty, toEncounter, "Encounters", "maxDifficulty");
            ImportExportUtils.ApplyValue(ref encounter.dominantTribes, ref encounterInfo.dominantTribes, toEncounter, "Encounters", "dominantTribes");
            ImportExportUtils.ApplyValue(ref encounter.randomReplacementCards, ref encounterInfo.randomReplacementCards, toEncounter, "Encounters", "randomReplacementCards");
            ImportExportUtils.ApplyValue(ref encounter.redundantAbilities, ref encounterInfo.redundantAbilities, toEncounter, "Encounters", "redundantAbilities");

            if (toEncounter)
            {
                encounter.turns.Clear();
                foreach (TurnInfo turnData in encounterInfo.turns)
                {
                    List<EncounterBlueprintData.CardBlueprint> TurnCardList = new List<EncounterBlueprintData.CardBlueprint>();
                    foreach (TurnCardInfo turnCardInfo in turnData.cardInfo)
                    {
                        EncounterBlueprintData.CardBlueprint TurnCardInfo = new EncounterBlueprintData.CardBlueprint();
                        TurnCardInfo.card = CardManager.AllCardsCopy.CardByName(turnCardInfo.card);
                        if (turnCardInfo.randomReplaceChance != null)
                        {
                            TurnCardInfo.randomReplaceChance = (int)turnCardInfo.randomReplaceChance;
                        }

                        if (turnCardInfo.difficultyReplacement != null)
                        {
                            TurnCardInfo.difficultyReplace = true;
                            TurnCardInfo.replacement =
                                CardManager.AllCardsCopy.CardByName(turnCardInfo.difficultyReplacement);
                        }

                        if (turnCardInfo.difficultyReq != null)
                        {
                            TurnCardInfo.difficultyReq = (int)turnCardInfo.difficultyReq;
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
                
                EncounterInfo encounterInfo = JSONParser.FromJson<EncounterInfo>(File.ReadAllText(file));
                EncounterBlueprintData encounter = GetBluePrint(encounterInfo.name);
                if (encounter == null)
                {
                    encounter = EncounterManager.New(encounterInfo.name);
                    Plugin.VerboseLog($"Loading new JLDR2 (encounters) {filename}");
                }
                else
                {
                    Plugin.VerboseLog($"Loading replacement JLDR2 (encounters) {filename}");
                }

                Process(encounter, encounterInfo, true, file);
            }
            EncounterManager.SyncEncounterList();
        }

        private static EncounterBlueprintData GetBluePrint(string name)
        {
            foreach (EncounterBlueprintData data in EncounterManager.BaseGameEncounters)
            {
                if (data.name == name)
                {
                    return data;
                }
            }
            foreach (EncounterBlueprintData data in EncounterManager.NewEncounters)
            {
                if (data.name == name)
                {
                    return data;
                }
            }

            return null;
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
            Process(info, serializedTribe, false, path);
            
            string json = JSONParser.ToJSON(serializedTribe);
            File.WriteAllText(Path.Combine(path, serializedTribe.name + "_encounter.jldr2"), json);
        }
    }
}
