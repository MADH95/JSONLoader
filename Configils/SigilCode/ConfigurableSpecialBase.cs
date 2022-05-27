// Using Inscryption
using DiskCardGame;
// Modding Inscryption
using JLPlugin.Data;

namespace JLPlugin.SigilCode
{
    public abstract class ConfigurableSpecialBase : SpecialCardBehaviour
    {
        public SigilData abilityData = new SigilData();
        public SpecialTriggeredAbility specialAbility = new SpecialTriggeredAbility();
    }
}


