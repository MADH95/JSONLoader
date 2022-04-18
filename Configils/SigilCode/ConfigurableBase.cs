// Using Inscryption
using DiskCardGame;
using JLPlugin.Data;

// Modding Inscryption


namespace JLPlugin.SigilCode
{
    public abstract class ConfigurableBase : AbilityBehaviour
    {
        public SigilData abilityData = new SigilData();
        public Ability ability = new Ability();
    }
}


