using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DiskCardGame;
using InscryptionAPI.Dialogue;
using InscryptionAPI.Encounters;
using InscryptionAPI.Regions;
using InscryptionAPI.TalkingCards.Create;
using JLPlugin;
using TinyJson;
using UnityEngine;

[Serializable]
public class RegionSerializeInfo
{
	public string name;
	public int tier = 0;
	public bool addToPool = true;
	public List<string> terrainCards;
	public List<string> encounters;
	public List<string> likelyCards;
	public List<string> dominantTribes;
	public string bossPrepEncounter;
	public string boardLightColor;
	public string cardsLightColor;
	public string mapAlbedo;
	public List<string> bosses;
	public List<SceneryEntrySerializedInfo> fillerScenery;
	public List<ScarceSceneryEntrySerializedInfo> scarceScenery;
	public List<PredefinedEntrySerializedInfo> predefinedScenery;
	public DialogueEventStrings dialogueEvent;
	private string ambientLoopId;

	public static void LoadAllRegions(List<string> files)
	{
		for (int i = files.Count - 1; i >= 0; i--)
		{
			string path = files[i];
			string fileName = path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar) + 1);
			if (!fileName.EndsWith("_region.jldr2"))
			{
				continue;
			}

			files.RemoveAt(i--);
			Plugin.VerboseLog("Loading JLDR2 (region) " + fileName);
			
			try
			{
				ImportExportUtils.SetDebugPath(path);
				RegionSerializeInfo data = path.FromFilePath<RegionSerializeInfo>();
				
				RegionData region = RegionManager.AllRegionsCopy.Find(a=>a.name == data.name);
				if (region == null)
				{
					region = RegionManager.New(data.name, data.tier, data.addToPool);
				}
				Process(region, data, true);

				Plugin.VerboseLog("Loaded JSON region from " + fileName + "!");
			}
			catch (Exception e)
			{
				Plugin.Log.LogError("Failed to load JSON region from " + fileName + "!");
				Plugin.Log.LogError(e);
			}
		}
	}

	private static void Process(RegionData region, RegionSerializeInfo data, bool toRegion)
	{
		string regionName = toRegion ? data.name : region.name;
		ImportExportUtils.SetID(regionName);
		
		GetFillerScenery(ref region.fillerScenery, ref data.fillerScenery, toRegion);
		GetScarceScenery(ref region.scarceScenery, ref data.scarceScenery, toRegion);
		
		List<SceneryElementData> predefinedSceneryScenery = region.predefinedScenery?.scenery;
		GetPredefinedScenery(ref predefinedSceneryScenery, ref data.predefinedScenery, toRegion);
		
		ImportExportUtils.ApplyValue(ref region.ambientLoopId, ref data.ambientLoopId, toRegion, "Regions", "ambientLoopId");
		ImportExportUtils.ApplyValue(ref region.terrainCards, ref data.terrainCards, toRegion, "Regions", "terrainCards");
		ImportExportUtils.ApplyValue(ref region.dominantTribes, ref data.dominantTribes, toRegion, "Regions", "dominantTribes");
		ImportExportUtils.ApplyValue(ref region.boardLightColor, ref data.boardLightColor, toRegion, "Regions", "boardLightColor");
		ImportExportUtils.ApplyValue(ref region.cardsLightColor, ref data.cardsLightColor, toRegion, "Regions", "cardsLightColor");
		ImportExportUtils.ApplyValue(ref region.mapAlbedo, ref data.mapAlbedo, toRegion, "Regions", "mapAlbedo");
		ImportExportUtils.ApplyValue(ref region.likelyCards, ref data.likelyCards, toRegion, "Regions", "likelyCards");
		ImportExportUtils.ApplyValue(ref region.bosses, ref data.bosses, toRegion, "Regions", "bosses");

		if (toRegion)
		{
			region.mapParticlesPrefabs = new List<GameObject>();
			region.name = data.name;
			
			if (region.predefinedScenery == null)
			{
				region.predefinedScenery = ScriptableObject.CreateInstance<PredefinedScenery>();
			}
			region.predefinedScenery.scenery = predefinedSceneryScenery;
			
			if (data.bossPrepEncounter != null)
			{
				EncounterBlueprintData val2 = EncounterManager.AllEncountersCopy.Find(a => a.name == data.bossPrepEncounter);
				if (val2 != null)
				{
					region.bossPrepEncounter = val2;
				}
				else
				{
					Plugin.Log.LogError($"Could not find boss prep encounter {data.bossPrepEncounter} for region {data.name}!");
				}
			}

			if (data.dialogueEvent != null)
			{
				DialogueManager.Add(Plugin.PluginGuid, CreateEvent(data.dialogueEvent));
			}
			else
			{
				Plugin.Log.LogError($"No dialogue specified for region {data.name}!");
			}

			region.encounters = new List<EncounterBlueprintData>();
			if (data.encounters != null)
			{
				foreach (string encounter in data.encounters)
				{
					EncounterBlueprintData val = EncounterManager.AllEncountersCopy.Find(a => a.name == encounter);
					if (val != null)
					{
						region.encounters.Add(val);
					}
					else
					{
						Plugin.Log.LogError($"Could not find encounter {encounter} for region {data.name}!");
					}
				}
			}
		}
		else
		{
			data.name = region.name;
			if (region.bossPrepEncounter != null)
			{
				data.bossPrepEncounter = region.bossPrepEncounter.name;
			}
			
			DialogueEvent dialogueEvent = DialogueDataUtil.Data.GetEvent("Region" + region.name);
			if (dialogueEvent != null)
			{
				string[] mainLines = dialogueEvent.mainLines.lines.Select((a)=>a.text).ToArray();
				string[][] repeatLines = dialogueEvent.repeatLines.Select((a) => a.lines.Select((b) => b.text).ToArray()).ToArray();
				data.dialogueEvent = new DialogueEventStrings(region.name, mainLines, repeatLines);
			}

			if (region.encounters != null)
			{
				data.encounters = new List<string>();
				foreach (EncounterBlueprintData encounter in region.encounters)
				{
					data.encounters.Add(encounter.name);
				}
			}
		}
	}
	
	private static DialogueEvent CreateEvent(DialogueEventStrings dialogueEvent)
	{
		List<CustomLine> mainLines = dialogueEvent.mainLines.Select(x => (CustomLine) x).ToList();
		List<List<CustomLine>> repeatLines = dialogueEvent.repeatLines.Select(x => x.Select(y => (CustomLine) y).ToList()).ToList();
		return DialogueManager.GenerateEvent(Plugin.PluginGuid, "Region" + dialogueEvent.eventName, mainLines, repeatLines);
	}

	private static void GetFillerScenery(ref List<FillerSceneryEntry> data, ref List<SceneryEntrySerializedInfo> info, bool toData)
	{
		if (toData)
		{
			data = new List<FillerSceneryEntry>();
			if (info == null || info.Count == 0)
			{
				return;
			}

			foreach (SceneryEntrySerializedInfo item in info)
			{
				FillerSceneryEntry entry = new FillerSceneryEntry();
				entry.data = ScriptableObject.CreateInstance<SceneryData>();
				entry.data.minScale = item.minScale;
				entry.data.maxScale = item.maxScale;
				entry.data.prefabNames = item.prefabNames;
				entry.data.radius = item.radius;
				entry.data.perlinNoiseHeight = item.perlinNoiseHeight;
				data.Add(entry);
			}
		}
		else
		{
			info = new List<SceneryEntrySerializedInfo>();
			if (data == null || data.Count == 0)
			{
				return;
			}

			foreach (FillerSceneryEntry item in data)
			{
				SceneryEntrySerializedInfo entry = new SceneryEntrySerializedInfo();
				entry.minScale = item.data.minScale;
				entry.maxScale = item.data.maxScale;
				entry.prefabNames = item.data.prefabNames;
				entry.radius = item.data.radius;
				entry.perlinNoiseHeight = item.data.perlinNoiseHeight;
				info.Add(entry);
			}
		}
	}

	private static void GetScarceScenery(ref List<ScarceSceneryEntry> data, ref List<ScarceSceneryEntrySerializedInfo> info, bool toData)
	{
		if (toData)
		{
			data = new List<ScarceSceneryEntry>();
			if (info == null || info.Count == 0)
			{
				return;
			}

			foreach (ScarceSceneryEntrySerializedInfo item in info)
			{
				ScarceSceneryEntry entry = new ScarceSceneryEntry();
				entry.data = ScriptableObject.CreateInstance<SceneryData>();
				entry.data.minScale = item.minScale;
				entry.data.maxScale = item.maxScale;
				entry.data.prefabNames = item.prefabNames;
				entry.data.radius = item.radius;
				entry.data.perlinNoiseHeight = item.perlinNoiseHeight;
				entry.minDensity = item.minDensity;
				entry.minInstances = item.minInstances;
				entry.maxInstances = item.maxInstances;
				data.Add(entry);
			}
		}
		else
		{
			info = new List<ScarceSceneryEntrySerializedInfo>();
			if (data == null || data.Count == 0)
			{
				return;
			}

			foreach (ScarceSceneryEntry item in data)
			{
				ScarceSceneryEntrySerializedInfo entry = new ScarceSceneryEntrySerializedInfo();
				entry.minScale = item.data.minScale;
				entry.maxScale = item.data.maxScale;
				entry.prefabNames = item.data.prefabNames;
				entry.radius = item.data.radius;
				entry.perlinNoiseHeight = item.data.perlinNoiseHeight;
				entry.minDensity = item.minDensity;
				entry.minInstances = item.minInstances;
				entry.maxInstances = item.maxInstances;
				info.Add(entry);
			}
		}
	}

	private static void GetPredefinedScenery(ref List<SceneryElementData> data, ref List<PredefinedEntrySerializedInfo> info, bool toData)
	{
		if (toData)
		{
			data = new List<SceneryElementData>();
			if (info == null || info.Count == 0)
			{
				return;
			}

			foreach (PredefinedEntrySerializedInfo item in info)
			{
				SceneryElementData entry = new SceneryElementData();
				entry.data = ScriptableObject.CreateInstance<SceneryData>();
				entry.data.minScale = item.minScale;
				entry.data.maxScale = item.maxScale;
				entry.data.prefabNames = item.prefabNames;
				entry.data.radius = item.radius;
				entry.data.perlinNoiseHeight = item.perlinNoiseHeight;
				entry.rotation = item.rotation;
				entry.scale = item.scale;
				data.Add(entry);
			}
		}
		else
		{
			info = new List<PredefinedEntrySerializedInfo>();
			if (data == null || data.Count == 0)
			{
				return;
			}

			foreach (SceneryElementData item in data)
			{
				PredefinedEntrySerializedInfo entry = new PredefinedEntrySerializedInfo();
				entry.minScale = item.data.minScale;
				entry.maxScale = item.data.maxScale;
				entry.prefabNames = item.data.prefabNames;
				entry.radius = item.data.radius;
				entry.perlinNoiseHeight = item.data.perlinNoiseHeight;
				entry.rotation = item.rotation;
				entry.scale = item.scale;
				info.Add(entry);
			}
		}
	}

	public static void ExportAllRegions()
	{
		Plugin.Log.LogInfo($"Exporting {RegionManager.AllRegionsCopy.Count} Regions");
		foreach (RegionData region in RegionManager.AllRegionsCopy)
		{
			RegionSerializeInfo serializeDeck = new RegionSerializeInfo();
			string path = Path.Combine(Plugin.ExportDirectory, "Regions", region.name + "_region.jldr2");
			ImportExportUtils.SetDebugPath(path);
			
			Process(region, serializeDeck, false);
                
			string directory = Path.GetDirectoryName(path);
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
                
			File.WriteAllText(path, JSONParser.ToJSON(serializeDeck));
		}
	}
}

[Serializable]
public class SceneryEntrySerializedInfo
{
	public Vector2SerializeInfo minScale = new Vector2SerializeInfo(0.05f, 0.05f);
	public Vector2SerializeInfo maxScale = new Vector2SerializeInfo(0.09f, 0.22f);
	public List<string> prefabNames = new List<string> { "Tree_3_Mossy" };
	public float radius = 0.06f;
	public bool perlinNoiseHeight = true;
}

[Serializable]
public class ScarceSceneryEntrySerializedInfo : SceneryEntrySerializedInfo
{
	public float minDensity;
	public int minInstances;
	public int maxInstances;
}

[Serializable]
public class PredefinedEntrySerializedInfo : SceneryEntrySerializedInfo
{
	public Vector3SerializeInfo rotation;
	public Vector3SerializeInfo scale;
}

[Serializable]
public class Vector2SerializeInfo
{
	public float x;
	public float y;

	public Vector2SerializeInfo(float x, float y)
	{
		this.x = x;
		this.y = y;
	}
	
	public static implicit operator Vector2(Vector2SerializeInfo info)
	{
		return new Vector2(info.x, info.y);
	}
	
	public static implicit operator Vector2SerializeInfo(Vector2 info)
	{
		return new Vector2SerializeInfo(info.x, info.y);
	}
}

[Serializable]
public class Vector3SerializeInfo : Vector2SerializeInfo
{
	public float z;


	public Vector3SerializeInfo(float x, float y, float z) : base(x, y)
	{
		this.z = z;
	}
	
	public static implicit operator Vector3(Vector3SerializeInfo info)
	{
		return new Vector3(info.x, info.y, info.z);
	}
	
	public static implicit operator Vector3SerializeInfo(Vector3 info)
	{
		return new Vector3SerializeInfo(info.x, info.y, info.z);
	}
}