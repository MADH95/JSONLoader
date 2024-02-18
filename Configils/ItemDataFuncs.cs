using DiskCardGame;
using System;
using System.Collections.Generic;
using System.IO;
using InscryptionAPI.Items;
using InscryptionAPI.Items.Extensions;
using TinyJson;
using UnityEngine;

namespace JLPlugin.Data
{
    using SigilCode;

    public partial class ItemData
    {
        public static void LoadAllConsumableItems(List<string> files)
        {
            ItemData test = new ItemData();
            test.Initialize();
            Plugin.Log.LogInfo(JSONParser.ToJSON(test));
            
            for (int index = 0; index < files.Count; index++)
            {
                string file = files[index];
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (!filename.EndsWith("_item.jldr2"))
                    continue;

                ImportExportUtils.SetDebugPath(file);
                files.RemoveAt(index--);
                
                try
                {
                    Plugin.VerboseLog($"Loading JLDR2 (consumableItem) {filename}");
                    ItemData item = JSONParser.FromFilePath<ItemData>(file);

                    ConsumableItemData data = null;
                    string guid = item.GUID ?? Plugin.PluginGuid;
                    Texture2D texture = null;
                    ImportExportUtils.ApplyValue(ref texture, ref item.icon, true, "Items", "icon");
                    if (item.isBottledCard == true)
                    {
                        data = ConsumableItemManager.NewCardInABottle(guid, item.bottledCardName, texture);
                    }
                    else
                    {
                        string rulebookName = null;
                        string rulebookDescription = null;
                        ImportExportUtils.ApplyLocaleField("rulebookName", ref item.rulebookName, ref rulebookName, true);
                        ImportExportUtils.ApplyLocaleField("rulebookDescription", ref item.rulebookDescription, ref rulebookDescription, true);
                        ConsumableItemManager.ModelType modelType = ConsumableItemManager.ModelType.BasicRuneWithVeins;
                        ImportExportUtils.ApplyValue(ref modelType, ref item.modelType, true, "Items", "modelType");
                        
                        data = ConsumableItemManager.New(guid, rulebookName, rulebookDescription, texture, typeof(ConfigurableConsumableItem), modelType);
                        SigilDicts.ConsumableItemList[data.name] = item;
                    }

                    Process(data, item, true);

                    Plugin.VerboseLog($"Loaded JLDR2 (consumableItem) {filename}");
                }
                catch (Exception e)
                {
                    Plugin.Log.LogError($"Error loading trait from {file}");
                    Plugin.Log.LogError(e);
                }
            }
        }

        private static void Process(ConsumableItemData info, ItemData data, bool toInfo)
        {
            ImportExportUtils.ApplyValue(ref info.regionSpecific, ref data.regionSpecific, toInfo, "Items", "regionSpecific");
            ImportExportUtils.ApplyValue(ref info.notRandomlyGiven, ref data.notRandomlyGiven, toInfo, "Items", "notRandomlyGiven");
            ImportExportUtils.ApplyLocaleField("description", ref data.description, ref info.description, toInfo);
            ImportExportUtils.ApplyValue(ref info.rulebookSprite, ref data.icon, toInfo, "Items", "icon");
            ImportExportUtils.ApplyValue(ref info.notRandomlyGiven, ref data.notRandomlyGiven, toInfo, "Items", "notRandomlyGiven");
            ImportExportUtils.ApplyValue(ref info.pickupSoundId, ref data.pickupSoundId, toInfo, "Items", "pickupSoundId");
            ImportExportUtils.ApplyValue(ref info.placedSoundId, ref data.placedSoundId, toInfo, "Items", "placedSoundId");
            ImportExportUtils.ApplyValue(ref info.examineSoundId, ref data.examineSoundId, toInfo, "Items", "examineSoundId");
            ImportExportUtils.ApplyValue(ref info.powerLevel, ref data.powerLevel, toInfo, "Items", "powerLevel");

            if (!toInfo)
            {
                ImportExportUtils.ApplyValue(ref info.rulebookSprite, ref data.icon, false, "Items", "icon");
                ImportExportUtils.ApplyProperty(info.GetPrefabModelType, (a)=>{}, ref data.modelType, false, "Items", "modelType");
                ImportExportUtils.ApplyProperty(info.GetModPrefix, (a)=>{}, ref data.GUID, false, "Items", "GUID");
                ImportExportUtils.ApplyLocaleField("rulebookName", ref data.rulebookName, ref info.rulebookName, false);
                ImportExportUtils.ApplyLocaleField("rulebookDescription", ref data.rulebookDescription, ref info.rulebookDescription, false);
                
                // TODO: Export model somehow maybe maybe?
            }
        }
        
        public static void ExportAllItems()
        {
            List<ConsumableItemData> allItems = new List<ConsumableItemData>(ConsumableItemManager.NewConsumableItemDatas);
            allItems.AddRange(Resources.LoadAll<ConsumableItemData>(""));
            
            Plugin.Log.LogInfo($"Exporting {allItems.Count} ConsumableItems to JSON");
            string path = Path.Combine(Plugin.ExportDirectory, "Items");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            for (var i = 0; i < allItems.Count; i++)
            {
                ConsumableItemData item = allItems[i];
                ImportExportUtils.SetID(item.name);

                ItemData data = new ItemData();

                Process(item, data, false);

                string json = JSONParser.ToJSON(data);
                File.WriteAllText(Path.Combine(path, data.rulebookName.EnglishValue + "_item.jldr2"), json);
            }
        }
    }
}
