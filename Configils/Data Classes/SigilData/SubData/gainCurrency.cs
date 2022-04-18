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

        public static IEnumerator GainCurrency(SigilData abilitydata)
        {
            if (SigilData.ConvertArgument(abilitydata.gainCurrency.runOnCondition, abilitydata) == "false")
            {
                yield break;
            }

            if (abilitydata.gainCurrency.bones != null)
            {
                yield return Singleton<ResourcesManager>.Instance.AddBones(int.Parse(SigilData.ConvertArgument(abilitydata.gainCurrency.bones, abilitydata)));
            }
            if (abilitydata.gainCurrency.energy != null)
            {
                yield return Singleton<ResourcesManager>.Instance.AddEnergy(int.Parse(SigilData.ConvertArgument(abilitydata.gainCurrency.energy, abilitydata)));
            }
            yield break;
        }
    }
}