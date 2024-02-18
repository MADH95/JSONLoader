using System.Collections.Generic;
using InscryptionAPI.Items;
using TinyJson;

namespace JLPlugin.Data
{
    [System.Serializable]
    public partial class ItemData : AConfigilData
    {
        public override string Name => rulebookName.EnglishValue;
        
        public string GUID;
        public JSONParser.LocalizableField rulebookName;
        public JSONParser.LocalizableField rulebookDescription;
        public JSONParser.LocalizableField description;
        public string icon;
        public bool? isBottledCard = false;
        public string bottledCardName = "";
        private bool regionSpecific = false;
        private bool notRandomlyGiven = true;
        private string modelType = ConsumableItemManager.ModelType.BasicRuneWithVeins.ToString();
        private string pickupSoundId = "stone_object_up";
        private string placedSoundId = "stone_object_hit";
        private string examineSoundId = "stone_object_hit";
        private int powerLevel = 1;

        public sealed override void Initialize()
        {
            rulebookName = new("rulebookName"); // name, name_es... etc
            rulebookDescription = new("rulebookDescription"); // description, description_ko... etc
            description = new("description"); // description, description_ko... etc
        }
    }
}
