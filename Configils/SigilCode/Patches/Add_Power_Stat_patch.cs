// Using Inscryption
using DiskCardGame;
using HarmonyLib;
using JLPlugin.Data;
// Modding Inscryption
using System;

namespace JLPlugin.SigilCode
{

    [HarmonyPatch(typeof(CardTriggerHandler), "AddAbility", new Type[]
    {
        typeof(SpecialTriggeredAbility)
    })]
    public class Add__Power_Stat_patch
    {
        [HarmonyPrefix]
        public static bool Prefix(SpecialTriggeredAbility ability, CardTriggerHandler __instance)
        {
            if (!SigilDicts.PowerStatArgumentList.TryGetValue(ability, out var data)) 
                return true;
            
            Type SigilType = data.Item1;

            if (!__instance.specialAbilities.Exists((Tuple<SpecialTriggeredAbility, SpecialCardBehaviour> x) =>
                    x.Item1 == ability))
            {
                ConfigurablePowerStat specialConfigil =
                    __instance.gameObject.AddComponent(SigilType) as ConfigurablePowerStat;
                specialConfigil.Initialize(data.Item2, data.Item3);
                __instance.specialAbilities.Add(new Tuple<SpecialTriggeredAbility, SpecialCardBehaviour>(ability, specialConfigil));
            }

            return false;

        }
    }
    
    // [HarmonyPatch(typeof(Card), "AttachAbilities", new Type[]
    // {
    //     typeof(CardInfo)
    // })]
    // public class Add__Power_Stat_patch_AttachAbilities
    // {
    //     [HarmonyPrefix]
    //     public static bool Postfix(Card __instance, CardInfo info)
    //     {
    //         foreach (SpecialTriggeredAbility specialAbility in info.specialAbilities)
    //         {
    //             if (SigilDicts.PowerStatArgumentList.TryGetValue(specialAbility, out var data))
    //             {
    //                 ConfigurablePowerStat[] stat = __instance.GetComponents<ConfigurablePowerStat>();
    //                 foreach (ConfigurablePowerStat powerStat in stat)
    //                 {
    //                     if(powerStat.IconType == )
    //                 }
    //             }
    //         }
    //
    //         return false;
    //
    //     }
    // }
}