using DiskCardGame;
using InscryptionAPI;
using System.Collections;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class gainCurrency
    {
        public string runOnCondition;
        public string bones;
        public string energy;
        public string maxEnergy;
        public string foils;

        public static IEnumerator GainCurrency(AbilityBehaviourData abilitydata)
        {
            if (SigilData.ConvertArgument(abilitydata.gainCurrency.runOnCondition, abilitydata) == "false")
            {
                yield break;
            }

            if (!string.IsNullOrWhiteSpace(abilitydata.gainCurrency.bones))
            {
                int boneamount = int.Parse(SigilData.ConvertArgument(abilitydata.gainCurrency.bones, abilitydata));
                if (boneamount > 0)
                {
                    yield return Singleton<ResourcesManager>.Instance.AddBones(boneamount);
                }
                else if (boneamount < 0)
                {
                    yield return Singleton<ResourcesManager>.Instance.SpendBones(boneamount * -1);
                }
            }

            if (!string.IsNullOrWhiteSpace(abilitydata.gainCurrency.energy))
            {
                int energyamount = int.Parse(SigilData.ConvertArgument(abilitydata.gainCurrency.energy, abilitydata));
                if (energyamount > 0)
                {
                    yield return Singleton<ResourcesManager>.Instance.AddEnergy(energyamount);
                }
                else if (energyamount < 0)
                {
                    yield return Singleton<ResourcesManager>.Instance.SpendEnergy(energyamount * -1);
                }
            }

            if (!string.IsNullOrWhiteSpace(abilitydata.gainCurrency.maxEnergy))
            {
                int maxEnergyamount = int.Parse(SigilData.ConvertArgument(abilitydata.gainCurrency.maxEnergy, abilitydata));

                if (maxEnergyamount > 0)
                {
                    yield return Singleton<ResourcesManager>.Instance.AddMaxEnergy(maxEnergyamount);
                }
                else if (maxEnergyamount < 0) // sketchy, may bug
                {
                    Singleton<ResourcesManager>.Instance.PlayerMaxEnergy -= maxEnergyamount;
                }
            }

            if (!string.IsNullOrWhiteSpace(abilitydata.gainCurrency.foils))
            {
                int foilamount = int.Parse(SigilData.ConvertArgument(abilitydata.gainCurrency.foils, abilitydata));
                if (foilamount > 0)
                {
                    RunState.Run.currency += foilamount;
                    yield return Singleton<CurrencyBowl>.Instance.DropWeightsIn(foilamount);
                }
                else if (foilamount < 0)
                {
                    RunState.Run.currency -= foilamount;
                    yield return Singleton<CurrencyBowl>.Instance.TakeWeights(foilamount * -1);
                }
            }
            yield break;
        }
    }
}