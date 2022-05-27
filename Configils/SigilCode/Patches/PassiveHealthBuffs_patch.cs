// Using Inscryption
using DiskCardGame;
using HarmonyLib;
using JLPlugin.Data;
using System.Collections.Generic;
using System.Linq;
// Modding Inscryption

namespace JLPlugin.SigilCode
{

    [HarmonyPatch(typeof(PlayableCard), "GetPassiveHealthBuffs")]
    public class PassiveHealthBuffs_patch
    {
        [HarmonyPostfix]
        public static void Postfix(ref int __result, ref PlayableCard __instance)
        {
            if (__instance.OnBoard)
            {
                foreach (CardSlot slot in Singleton<BoardManager>.Instance.AllSlotsCopy)
                {
                    foreach (Ability ability in slot.Card?.Info.abilities ?? new List<Ability>())
                    {
                        if (!SigilDicts.ArgumentList.ContainsKey(ability))
                        {
                            continue;
                        }

                        foreach (AbilityBehaviourData abilityBehaviour in SigilData.GetAbilityArguments(ability).abilityBehaviour.Where(x => x.trigger.triggerType == "Passive"))
                        {
                            foreach (buffCards buffCards in abilityBehaviour.buffCards ?? new List<buffCards>())
                            {
                                SigilData.UpdateVariables(abilityBehaviour, slot.Card);
                                if (slotData.GetSlot(buffCards.slot, abilityBehaviour) == __instance.slot)
                                {
                                    __result += int.Parse(buffCards.stats.Split('/')[1]);
                                    Plugin.Log.LogWarning((__instance.Info.Health + __result));
                                    if ((__instance.Info.Health + __result) <= 0)
                                    {
                                        BoardManager.Instance.StartCoroutine(__instance.Die(false));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}