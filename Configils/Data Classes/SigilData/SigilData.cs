using DiskCardGame;
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

        public trigger trigger;

        public List<placeCards> placeCards;
        public List<buffCards> buffCards;
        public List<transformCards> transformCards;
        public List<attackSlots> attackSlots;
        public gainCurrency gainCurrency;
        public string dealScaleDamage;
        public List<drawCards> drawCards;
        public List<chooseSlot> chooseSlots;
        public uses uses;

        public Dictionary<string, string> variables;
        public Dictionary<string, object> generatedVariables;
        public PlayableCard self;
        public Ability ability;
    }
}
