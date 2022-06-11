using DiskCardGame;
using System.Collections;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class gainCurrency
    {
        public string runOnCondition;
        public string bones;
        public string energy;
        public string foils;

        public static IEnumerator GainCurrency(AbilityBehaviourData abilitydata)
        {
            if (SigilData.ConvertArgument(abilitydata.gainCurrency.runOnCondition, abilitydata) == "false")
            {
                yield break;
            }

            if (abilitydata.gainCurrency.bones != null)
            {
                int boneamount = int.Parse(SigilData.ConvertArgument(abilitydata.gainCurrency.bones, abilitydata));
                if (boneamount > 0)
                {
                    yield return Singleton<ResourcesManager>.Instance.AddBones(boneamount);
                }
                if (boneamount < 0)
                {
                    yield return Singleton<ResourcesManager>.Instance.SpendBones(boneamount);
                }
            }
            if (abilitydata.gainCurrency.energy != null)
            {
                int energyamount = int.Parse(SigilData.ConvertArgument(abilitydata.gainCurrency.energy, abilitydata));
                if (energyamount > 0)
                {
                    yield return Singleton<ResourcesManager>.Instance.AddEnergy(energyamount);
                }
                if (energyamount < 0)
                {
                    yield return Singleton<ResourcesManager>.Instance.SpendEnergy(energyamount);
                }
            }
            if (abilitydata.gainCurrency.foils != null)
            {
                int foilamount = int.Parse(SigilData.ConvertArgument(abilitydata.gainCurrency.foils, abilitydata));
                if (foilamount > 0)
                {
                    yield return Singleton<CurrencyBowl>.Instance.DropWeightsIn(foilamount);
                }
                if (foilamount < 0)
                {
                    yield return Singleton<CurrencyBowl>.Instance.TakeWeights(foilamount);
                }
            }
            yield break;
        }
    }
}