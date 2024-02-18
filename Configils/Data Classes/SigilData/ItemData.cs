using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Items;
using TinyJson;

namespace JLPlugin.Data
{
    [System.Serializable]
    public partial class ItemData : AConfigilData
    {
        public override string Name => rulebookName.EnglishValue;
        
        public string GUID;
        public LocalizableField rulebookName;
        public LocalizableField rulebookDescription;
        public LocalizableField description;
        public string icon;
        public string bottledCardName = "";
        public bool regionSpecific = false;
        public bool notRandomlyGiven = true;
        public string rulebookCategory = AbilityMetaCategory.Part1Rulebook.ToString();
        public string modelType = ConsumableItemManager.ModelType.BasicRuneWithVeins.ToString();
        public string pickupSoundId = "stone_object_up";
        public string placedSoundId = "stone_object_hit";
        public string examineSoundId = "stone_object_hit";
        public int powerLevel = 1;

        public sealed override void Initialize()
        {
            rulebookName = new("rulebookName"); // name, name_es... etc
            rulebookDescription = new("rulebookDescription"); // description, description_ko... etc
            description = new("description"); // description, description_ko... etc
        }
    }
}
