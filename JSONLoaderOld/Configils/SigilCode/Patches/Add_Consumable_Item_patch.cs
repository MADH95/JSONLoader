using System;
using System.Collections;
using System.Reflection;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Items;
using InscryptionAPI.Items.Extensions;
using JLPlugin.Data;
using ItemData = DiskCardGame.ItemData;

namespace JLPlugin.SigilCode
{
    [HarmonyPatch(typeof(ItemSlot), "CreateItem", new Type[]
    {
        typeof(ItemData),
        typeof(bool)
    })]
    // [HarmonyPatch]
    internal class Add_Consumable_Item_patch
    {
        // private static MethodBase TargetMethod()
        // {
        //     Type type = Type.GetType("InscryptionAPI.Items.ConsumableItemManager.ItemSlot_CreateItem, InscryptionAPI, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
        //     Plugin.Log.LogInfo($"GetTargetMethods type = {type}");
        //     MethodInfo methodInfo = type.GetMethod("Prefix", new Type[]{typeof(ItemSlot), typeof(ItemData), typeof(bool)});
        //     Plugin.Log.LogInfo($"GetTargetMethods methodInfo = {methodInfo}");
        //     return methodInfo;
        // }
        
        
        
        // [HarmonyTargetMethods]
        // internal static IEnumerator GetTargetMethods()
        // {
        //     Type type = Type.GetType("InscryptionAPI.Items.ConsumableItemManager.ItemSlot_CreateItem");
        //     Plugin.Log.LogInfo($"GetTargetMethods type = {type}");
        //     MethodInfo methodInfo = type.GetMethod("Prefix", new Type[]{typeof(ItemSlot), typeof(ItemData), typeof(bool)});
        //     Plugin.Log.LogInfo($"GetTargetMethods methodInfo = {methodInfo}");
        //     yield return methodInfo;
        // }

        [HarmonyPostfix]
        internal static void Initialize_Configil(ItemSlot __instance, ItemData data, bool skipDropAnimation)
        {
            Plugin.Log.LogInfo($"Initialize_Configil {data.name}");
            if (SigilDicts.ConsumableItemList.TryGetValue(data.name, out JLPlugin.Data.ItemData itemData))
            {
                Plugin.Log.LogInfo($"\t{data.name} is a JLDR2 consumable item!");
                ConfigurableConsumableItem item = __instance.Item.GetComponent<ConfigurableConsumableItem>();
                item.Initialize(itemData);
            }
        }
    }
}