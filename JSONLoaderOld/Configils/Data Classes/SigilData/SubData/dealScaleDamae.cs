using DiskCardGame;
using System.Collections;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class dealScaleDamage
    {
        public string runOnCondition;
        public string damage;

        public static IEnumerator DealScaleDamage(AbilityBehaviourData abilitydata)
        {
            if (AConfigilData.ConvertArgument(abilitydata.dealScaleDamage.runOnCondition, abilitydata) == "false")
            {
                yield break;
            }

            if (string.IsNullOrWhiteSpace(abilitydata.dealScaleDamage.damage))
                yield break;

            int damage = int.Parse(AConfigilData.ConvertArgument(abilitydata.dealScaleDamage.damage, abilitydata));
            if (damage > 0)
            {
                yield return Singleton<LifeManager>.Instance.ShowDamageSequence(damage, damage, false, 0.125f, null, 0f, true);
            }
            else if (damage < 0)
            {
                yield return Singleton<LifeManager>.Instance.ShowDamageSequence(-damage, -damage, true, 0.125f, null, 0f, true);
            }
            yield break;
        }
    }
}