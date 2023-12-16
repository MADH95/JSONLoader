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
        public string damageSource;
        public string damage;

        public static IEnumerator DamageSlots(AbilityBehaviourData abilitydata)
        {
            foreach (damageSlots damageslotinfo in abilitydata.damageSlots)
            {
                if (SigilData.ConvertArgument(damageslotinfo.runOnCondition, abilitydata) == "false")
                {
                    continue;
                }

                // yield return new WaitForSeconds(0.3f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);

                CardSlot slot = slotData.GetSlot(damageslotinfo.slot, abilitydata);
                if (damageslotinfo.slot == null)
                {
                    slot = abilitydata.self.Slot;
                }
                if (slot != null && !string.IsNullOrWhiteSpace(damageslotinfo.damage))
                {
                    int damage = int.Parse(SigilData.ConvertArgument(damageslotinfo.damage, abilitydata));
                    if (slot.Card != null)
                    {
                        PlayableCard damageSourceCard = abilitydata.self;
                        if (!string.IsNullOrWhiteSpace(damageslotinfo.damageSource))
                        {
                            if (SigilData.ConvertArgument(damageslotinfo.damageSource, abilitydata) == "null")
                            {
                                damageSourceCard = null;
                            }
                            else
                            {
                                damageSourceCard = ((PlayableCard)SigilData.ConvertArgumentToType(damageslotinfo.damageSource, abilitydata, typeof(PlayableCard)));
                            }
                        }
                        yield return slot.Card.TakeDamage(damage, damageSourceCard);
                    }
                    else
                    {
                        yield return Singleton<LifeManager>.Instance.ShowDamageSequence(damage, damage, slot.IsPlayerSlot, 0.125f, null, 0f, true);
                    }
                }
            }
            // yield return new WaitForSeconds(0.3f);
            yield break;
        }
    }
}
