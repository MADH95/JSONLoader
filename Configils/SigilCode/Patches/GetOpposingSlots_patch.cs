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

            foreach (CardSlot slot in Singleton<BoardManager>.Instance.AllSlotsCopy)
            {
                if (slot.Card == null)
                {
                    continue;
                }

                foreach (Ability ability in slot.Card.GetTriggeredAbilities())
                {
                    if (!SigilDicts.ArgumentList.ContainsKey(ability) || !slot.Card.HasAbility(ability))
                    {
                        continue;
                    }

                    foreach (AbilityBehaviourData abilityBehaviour in SigilData.GetAbilityArguments(ability).abilityBehaviour.Where(x => x?.extraAttacks != null))
                    {
                        foreach (extraAttacks extraAttackData in abilityBehaviour.extraAttacks)
                        {
                            SigilData.UpdateVariables(abilityBehaviour, slot.Card);
                            abilityBehaviour.generatedVariables["TriggerCard"] = __instance;

                            if (SigilData.ConvertArgument(extraAttackData.runOnCondition, abilityBehaviour) == "false")
                            {
                                continue;
                            }

                            CardSlot chosenSlot = slotData.GetSlot(extraAttackData.attackingSlot, abilityBehaviour);
                            if (extraAttackData.attackingSlot == null)
                            {
                                chosenSlot = slot;
                            }
                            if (chosenSlot == __instance.slot)
                            {
                                __result.Remove(__instance.Slot.opposingSlot);
                                foreach (slotData slotToAttack in extraAttackData.slotsToAttack)
                                {
                                    CardSlot attackslot = slotData.GetSlot(slotToAttack, abilityBehaviour);
                                    if (attackslot != null)
                                    {
                                        __result.Add(attackslot);
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
