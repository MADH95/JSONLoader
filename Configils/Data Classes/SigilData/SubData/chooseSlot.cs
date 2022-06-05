using DiskCardGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class chooseSlot
    {
        public string slotChooseableOnCondition;

        public static IEnumerator ChooseSlot(AbilityBehaviourData abilitydata, chooseSlot chooseslot, CardSlot baseSlot)
        {
            List<CardSlot> allTargets = Singleton<BoardManager>.Instance.AllSlotsCopy;
            List<CardSlot> validtargets = Singleton<BoardManager>.Instance.AllSlotsCopy;
            foreach (CardSlot slot in Singleton<BoardManager>.Instance.AllSlotsCopy)
            {
                abilitydata.generatedVariables["ChooseableSlot"] = slot;
                Plugin.Log.LogInfo("SHOULD BE FALSE: " + SigilData.ConvertArgument(chooseslot.slotChooseableOnCondition, abilitydata));
                if (SigilData.ConvertArgument(chooseslot.slotChooseableOnCondition, abilitydata) == "false")
                {
                    validtargets.Remove(slot);
                }
            }
            CardSlot target = null;
            if (validtargets.Count > 0)
            {
                CardSlot slot = baseSlot;
                List<CardSlot> opposingSlots = new List<CardSlot>();
                Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.CombatView, false, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
                Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.ChoosingSlotViewMode, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
                CardSlot cardSlot = Singleton<InteractionCursor>.Instance.CurrentInteractable as CardSlot;
                BoardManager instance = Singleton<BoardManager>.Instance;
                yield return instance.ChooseTarget(allTargets, validtargets, delegate (CardSlot slot) { target = slot; }, null, null, () => false, CursorType.Target);
                Singleton<ViewManager>.Instance.Controller.SwitchToControlMode(Singleton<BoardManager>.Instance.DefaultViewMode, false);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
                Singleton<ViewManager>.Instance.SwitchToView(Singleton<BoardManager>.Instance.CombatView, false, false);

                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            }
            yield return target;
        }
    }
}