using DiskCardGame;
using System.Collections;
using System.Linq;
using UnityEngine;
using ItemData = JLPlugin.Data.ItemData;
using Object = System.Object;

namespace JLPlugin.SigilCode
{
    public class ConfigurableConsumableItem : ConsumableItem
    {
        private ABaseConfigilLogic _logic = null;
        
        public void Initialize(ItemData abilityData)
        {
            _logic = new ConfigConsumableItemLogic(this, abilityData);
        }

        private IEnumerator Start()
        {
            yield return _logic.Start();
        }

        public override IEnumerator ActivateSequence()
        {
            PlayExitAnimation();
            yield return new WaitForSeconds(0.1f);
            yield return _logic.Activate();
            yield return new WaitForSeconds(0.5f);
        }

        public override bool ExtraActivationPrerequisitesMet()
        {
            return _logic.CanActivate();
        }
    }
}