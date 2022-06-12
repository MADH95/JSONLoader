// Using Inscryption
using DiskCardGame;
using JLPlugin.Data;
// Modding Inscryption

namespace JLPlugin.SigilCode
{
    public abstract class ConfigurableBase : ActivatedAbilityBehaviour
    {
        public override Ability Ability
        {
            get
            {
                return ability;
            }
        }

        public PlayableCard PlayableCard
        {
            get
            {
                return base.GetComponent<PlayableCard>();
            }
        }

        public SigilData abilityData = new SigilData();
        public Ability ability = new Ability();
    }
}


