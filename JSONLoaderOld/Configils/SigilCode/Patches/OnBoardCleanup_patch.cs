using DiskCardGame;
using HarmonyLib;
using JLPlugin.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace JLPlugin.SigilCode
{
    [HarmonyPatch]
    public class OnBoardCleanup_patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TurnManager), nameof(TurnManager.CleanupPhase))]
        public static void CleanupPhase()
        {
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

                    slot.Card.Info.temporaryDecals.Clear();
                    slot.Card.RenderCard();
                }

                foreach (SpecialTriggeredAbility specialAbility in slot.Card.Info.SpecialAbilities)
                {
                    if (!SigilDicts.SpecialArgumentList.ContainsKey(specialAbility))
                    {
                        continue;
                    }

                    slot.Card.Info.temporaryDecals.Clear();
                    slot.Card.RenderCard();
                }
            }
        }
    }
}
