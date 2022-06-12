// Using Inscryption
using DiskCardGame;
using HarmonyLib;
using JLPlugin.Data;
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

                foreach (Ability ability in slot.Card.Info.abilities)
                {
                    if (!SigilDicts.ArgumentList.ContainsKey(ability))
                    {
                        continue;
                    }

                    foreach (AbilityBehaviourData abilityBehaviour in SigilData.GetAbilityArguments(ability).abilityBehaviour.Where(x => x.trigger?.triggerType == "Passive"))
                    {
                        if (abilityBehaviour.buffCards == null)
                        {
                            continue;
                        }

                        foreach (buffCards buffCards in abilityBehaviour.buffCards)
                        {
                            SigilData.UpdateVariables(abilityBehaviour, slot.Card);

                            CardSlot chosenSlot = slotData.GetSlot(buffCards.slot, abilityBehaviour);
                            if (buffCards.self == "true")
                            {
                                chosenSlot = abilityBehaviour.self.slot;
                            }
                            if (chosenSlot == __instance.slot)
                            {
                                if (buffCards.addStats != null)
                                {
                                    __result += int.Parse(SigilData.ConvertArgument(buffCards.addStats.Split('/')[1], abilityBehaviour));
                                }
                                if (buffCards.setStats != null)
                                {
                                    __result = int.Parse(SigilData.ConvertArgument(buffCards.setStats.Split('/')[1], abilityBehaviour));
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
    }
}