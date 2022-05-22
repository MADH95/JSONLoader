using DiskCardGame;
using System.Collections;
using UnityEngine;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class damageSlots
    {
        public string runOnCondition;
        public slotData slot;
        public string damage;

        public static IEnumerator DamageSlots(AbilityBehaviourData abilitydata)
        {
            yield return new WaitForSeconds(0.3f);
            if (Singleton<ViewManager>.Instance.CurrentView != View.Board)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
                yield return new WaitForSeconds(0.3f);
            }

            foreach (damageSlots damageslotinfo in abilitydata.damageSlots)
            {
                if (SigilData.ConvertArgument(damageslotinfo.runOnCondition, abilitydata) == "false")
                {
                    continue;
                }

                CardSlot slot = slotData.GetSlot(damageslotinfo.slot, abilitydata);
                if (slot != null)
                {
                    int damage = int.Parse(SigilData.ConvertArgument(damageslotinfo.damage, abilitydata));
                    if (slot.Card != null)
                    {
                        yield return slot.Card.TakeDamage(damage, slot.Card);
                    }
                    else
                    {
                        yield return Singleton<LifeManager>.Instance.ShowDamageSequence(damage, damage, slot.IsPlayerSlot, 0.125f, null, 0f, true);
                    }
                }
            }
            yield return new WaitForSeconds(0.3f);
            if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
            {
                Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            }
            yield break;
        }
    }
}