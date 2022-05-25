// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using JLPlugin.Data;

namespace JLPlugin.SigilCode
{
    public abstract class ConfigurableBase : ActivatedAbilityBehaviour
    {
        public SigilData abilityData = new SigilData();
        public Ability ability = new Ability();
    }
}


