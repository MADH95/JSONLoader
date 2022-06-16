using System.Collections.Generic;

namespace JLPlugin.Data
{
    [System.Serializable]
    public partial class SigilData
    {
        public string name, GUID, description;
        public List<string> metaCategories;
        public string texture;
        public string pixelTexture;
        public int? powerLevel;
        public string abilityLearnedDialogue;

        public int? priority;
        public bool? opponentUsable;

        public bool? canStack;

        public bool? isSpecialAbility;

        public activationCost activationCost;

        public List<AbilityBehaviourData> abilityBehaviour;
    }
}
