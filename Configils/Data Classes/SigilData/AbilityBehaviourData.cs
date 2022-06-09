using DiskCardGame;
using System.Collections.Generic;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class AbilityBehaviourData
    {
        public trigger trigger;

        public List<placeCards> placeCards;
        public List<buffCards> buffCards;
        public List<transformCards> transformCards;
        public gainCurrency gainCurrency;
        public string dealScaleDamage;
        public List<drawCards> drawCards;
        public List<chooseSlot> chooseSlots;
        public List<moveCards> moveCards;
        public List<damageSlots> damageSlots;
        public List<attackSlots> attackSlots;
        public List<slotData> extraAttacks;
        public messageData showMessage;

        public Dictionary<string, string> variables;
        public Dictionary<string, object> generatedVariables;
        public PlayableCard self;
        public int? TurnsInPlay;
        public Ability ability;
    }
}
