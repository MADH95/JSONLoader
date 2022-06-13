// Using Inscryption
using DiskCardGame;
using HarmonyLib;
using JLPlugin.Data;
// Modding Inscryption
using System.Collections.Generic;
using System.Linq;

namespace JLPlugin.SigilCode
{
    // Token: 0x02000006 RID: 6
    [HarmonyPatch(typeof(PlayableCard), "GetOpposingSlots", 0)]
    public class GetOpposingSlots_patch
    {
        // Token: 0x06000038 RID: 56 RVA: 0x00003178 File Offset: 0x00001378
        [HarmonyPostfix]
        public static void Postfix(PlayableCard __instance, ref List<CardSlot> __result)
        {
            if (!__instance.OnBoard)
            {
                return;
            }

            foreach (Ability ability in __instance.Info.abilities)
            {
                if (!SigilDicts.ArgumentList.ContainsKey(ability) || !__instance.HasAbility(ability))
                {
                    continue;
                }

                foreach (AbilityBehaviourData abilityBehaviour in SigilData.GetAbilityArguments(ability).abilityBehaviour.Where(x => x.extraAttacks != null))
                {
                    __result.Remove(__instance.Slot.opposingSlot);
                    foreach (slotData slotData in abilityBehaviour.extraAttacks)
                    {
                        CardSlot attackslot = slotData.GetSlot(slotData, abilityBehaviour);
                        if (attackslot != null)
                        {
                            __result.Insert(__result.Count, attackslot);
                        }
                    }
                }
            }
        }
    }
}
