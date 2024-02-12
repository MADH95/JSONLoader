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
		RegionSerializeInfo serializeInfo = new RegionSerializeInfo();
		Plugin.Log.LogInfo(JSONParser.ToJSON(serializeInfo));
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
			ImportExportUtils.SetDebugPath(path);
			RegionSerializeInfo data = path.FromFilePath<RegionSerializeInfo>();

			ImportExportUtils.SetID(data.name);
			RegionData region = RegionManager.New(data.name, data.tier, data.addToPool);
			Process(region, data, true);

			Plugin.VerboseLog("Loaded JSON region from " + fileName + "!");
		}
	}

	private static void Process(RegionData region, RegionSerializeInfo data, bool toRegion)
	{
		ImportExportUtils.SetID(data.name);
		if (toRegion)
		{
			region.mapParticlesPrefabs = new List<GameObject>();
		}
		else
		{
			//data.mapParticlesPrefabs = new List<GameObject>();
		}
		
		GetFillerScenery(ref region.fillerScenery, ref data.fillerScenery, toRegion);
		GetScarceScenery(ref region.scarceScenery, ref data.scarceScenery, toRegion);
		
		List<SceneryElementData> predefinedSceneryScenery = region.predefinedScenery?.scenery;
		GetPredefinedScenery(ref predefinedSceneryScenery, ref data.predefinedScenery, toRegion);
		if (toRegion)
		{
			if (region.predefinedScenery == null)
			{
				region.predefinedScenery = ScriptableObject.CreateInstance<PredefinedScenery>();
			}
			region.predefinedScenery.scenery = predefinedSceneryScenery;
		}
		
		ImportExportUtils.ApplyValue(ref region.ambientLoopId, ref data.ambientLoopId, toRegion, "Regions", "ambientLoopId");
		ImportExportUtils.ApplyValue(ref region.terrainCards, ref data.terrainCards, toRegion, "Regions", "terrainCards");
		ImportExportUtils.ApplyValue(ref region.encounters, ref data.encounters, toRegion, "Regions", "encounters");
		ImportExportUtils.ApplyValue(ref region.dominantTribes, ref data.dominantTribes, toRegion, "Regions", "dominantTribes");
		ImportExportUtils.ApplyValue(ref region.boardLightColor, ref data.boardLightColor, toRegion, "Regions", "boardLightColor");
		ImportExportUtils.ApplyValue(ref region.cardsLightColor, ref data.cardsLightColor, toRegion, "Regions", "cardsLightColor");
		ImportExportUtils.ApplyValue(ref region.mapAlbedo, ref data.mapAlbedo, toRegion, "Regions", "mapAlbedo");
		ImportExportUtils.ApplyValue(ref region.likelyCards, ref data.likelyCards, toRegion, "Regions", "likelyCards");
		ImportExportUtils.ApplyValue(ref region.bosses, ref data.bosses, toRegion, "Regions", "bosses");
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
			DialogueManager.Add(Plugin.PluginGuid, data.dialogueEvent.CreateEvent(data.name));
		}

	}

	private static List<FillerSceneryEntry> GetFillerScenery(RegionSerializeInfo data)
	{
		List<FillerSceneryEntry> list = new List<FillerSceneryEntry>();
		if (data.fillerScenery == null || data.fillerScenery.Count == 0)
		{
			return list;
		}
		
		foreach (SceneryEntrySerializedInfo item in data.fillerScenery)
		{
			FillerSceneryEntry entry = new FillerSceneryEntry();
			entry.data = ScriptableObject.CreateInstance<SceneryData>();
			entry.data.minScale = item.minScale;
			entry.data.maxScale = item.maxScale;
			entry.data.prefabNames = ParsePrefabNames(item.prefabNames);
			entry.data.radius = item.radius;
			entry.data.perlinNoiseHeight = item.perlinNoiseHeight;
			list.Add(entry);
		}
		return list;
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
				entry.data.prefabNames = ParsePrefabNames(item.prefabNames);
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
				entry.prefabNames = ParsePrefabNames(item.data.prefabNames);
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
				entry.data.prefabNames = ParsePrefabNames(item.prefabNames);
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
				entry.prefabNames = ParsePrefabNames(item.data.prefabNames);
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
				entry.data.prefabNames = ParsePrefabNames(item.prefabNames);
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
				entry.prefabNames = ParsePrefabNames(item.data.prefabNames);
				entry.radius = item.data.radius;
				entry.perlinNoiseHeight = item.data.perlinNoiseHeight;
				entry.rotation = item.rotation;
				entry.scale = item.scale;
				info.Add(entry);
			}
		}
	}

	private static List<string> ParsePrefabNames(List<string> infoPrefabNames)
	{
		List<string> list = new List<string>();
		foreach (string infoPrefabName in infoPrefabNames)
		{
			string path = "Prefabs/Map/MapScenery/" + infoPrefabName;
			if (ResourceBank.instance.resources.Any(a => a.path == path))
			{
				list.Add(infoPrefabName);
			}
			else
			{
				Plugin.Log.LogError("Unknown prefab name " + infoPrefabName + " in region JSON!");
			}
		}
		return list;
	}
}

[Serializable]
public class SceneryEntrySerializedInfo
{
	public Vector2 minScale = new Vector2(0.05f, 0.05f);
	public Vector2 maxScale = new Vector2(0.09f, 0.22f);
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
	public Vector3 rotation;
	public Vector3 scale;
}
