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
            if (!__instance.OnBoard)
            {
                return;
            }

            foreach (CardSlot slot in Singleton<BoardManager>.Instance.AllSlotsCopy)
            {
                if (slot.Card == null)
                {
                    continue;
                }

                foreach (Ability ability in slot.Card.GetTriggeredAbilities())
                {
                    if (!SigilDicts.ArgumentList.ContainsKey(ability))
                    {
                        continue;
                    }

                    ApplyBuffs(SigilData.GetAbilityArguments(ability).abilityBehaviour, slot, ref __result, ref __instance);
                }

                foreach (SpecialTriggeredAbility specialAbility in slot.Card.Info.SpecialAbilities)
                {
                    if (!SigilDicts.SpecialArgumentList.ContainsKey(specialAbility))
                    {
                        continue;
                    }

                    ApplyBuffs(SigilData.GetAbilityArguments(specialAbility).abilityBehaviour, slot, ref __result, ref __instance);
                }
            }
        }
        public static void ApplyBuffs(List<AbilityBehaviourData> AbilityBehaviourList, CardSlot slot, ref int __result, ref PlayableCard __instance)
        {
            foreach (AbilityBehaviourData abilityBehaviour in AbilityBehaviourList.Where(x => x.trigger?.triggerType == "Passive"))
            {
                if (abilityBehaviour.buffCards == null)
                {
                    continue;
                }

                foreach (buffCards buffCards in abilityBehaviour.buffCards)
                {
                    SigilData.UpdateVariables(abilityBehaviour, slot.Card);

                    if (SigilData.ConvertArgument(buffCards.runOnCondition, abilityBehaviour, false) == "false")
                    {
                        continue;
                    }

                    CardSlot chosenSlot = slotData.GetSlot(buffCards.slot, abilityBehaviour, false);
                    if (buffCards.slot == null)
                    {
                        chosenSlot = slot;
                    }
                    if (chosenSlot == __instance.slot)
                    {
                        if (buffCards.addStats != null)
                        {
                            __result += int.Parse(SigilData.ConvertArgument(buffCards.addStats.Split('/')[1], abilityBehaviour, false));
                        }
                        if (buffCards.setStats != null)
                        {
                            __result = int.Parse(SigilData.ConvertArgument(buffCards.setStats.Split('/')[1], abilityBehaviour, false)) - slot.Card.Info.Health;
                        }
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