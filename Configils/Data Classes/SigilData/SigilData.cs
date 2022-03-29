
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

        public trigger trigger;

        public List<placeCards> placeCards;
        public List<buffCards> buffCards;
        public List<transformCards> transformCards;
        public List<attackSlots> attackSlots;
        public gainCurrency gainCurrency;
        public string dealScaleDamage;
        public List<string> drawCards;
        public chooseSlots chooseSlots;
        public uses uses;
    }
}
