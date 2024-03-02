using DiskCardGame;
using System.Collections.Generic;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class AbilityBehaviourData
    {
        public trigger trigger;
        public List<string> actionOrder;

        public List<placeCards> placeCards;
        public List<buffCards> buffCards;
        public List<transformCards> transformCards;
        public List<changeAppearance> changeAppearance;
        public gainCurrency gainCurrency;
        public dealScaleDamage dealScaleDamage;
        public getStatValues getStatValues;
        public List<drawCards> drawCards;
        public List<chooseSlot> chooseSlots;
        public List<moveCards> moveCards;
        public List<damageSlots> damageSlots;
        public List<attackSlots> attackSlots;
        public List<extraAttacks> extraAttacks;
        public messageData showMessage;
        public Dictionary<string, List<Dictionary<string, string>>> customActions;

        public Dictionary<string, string> variables;
        public Dictionary<string, object> generatedVariables;
        public PlayableCard self;
        public int? TurnsInPlay;
        public Ability? ability;
        public SpecialTriggeredAbility? specialAbility;
        public SpecialStatIcon? specialStatIcon;
        public string consumableItem;
    }
}
