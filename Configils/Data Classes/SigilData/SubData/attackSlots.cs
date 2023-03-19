using DiskCardGame;
using System.Collections;
using UnityEngine;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class attackSlots
    {
        public string runOnCondition;
        public slotData attackerSlot;
        public slotData victimSlot;

        public static IEnumerator AttackSlots(AbilityBehaviourData abilitydata)
        {
            foreach (attackSlots attackslotinfo in abilitydata.attackSlots)
            {
                if (SigilData.ConvertArgument(attackslotinfo.runOnCondition, abilitydata) == "false")
                {
                    continue;
                }

                yield return new WaitForSeconds(0.3f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);

                CardSlot attackerSlot = slotData.GetSlot(attackslotinfo.attackerSlot, abilitydata);
                if (attackerSlot == null)
                {
                    attackerSlot = abilitydata.self.slot;
                }
                CardSlot victimSlot = slotData.GetSlot(attackslotinfo.victimSlot, abilitydata);
                if (attackerSlot != null && victimSlot != null)
                {
                    yield return Singleton<CombatPhaseManager>.Instance.SlotAttackSlot(attackerSlot, victimSlot);
                }
            }

            // yield return new WaitForSeconds(0.3f);
            yield break;
        }
    }
}
