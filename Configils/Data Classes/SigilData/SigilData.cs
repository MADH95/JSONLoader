using System.Collections.Generic;
using TinyJson;

namespace JLPlugin.Data
{
    [System.Serializable]
    public partial class SigilData : AConfigilData
    {
        public override string Name => name.EnglishValue;
        
        public string GUID;
        public JSONParser.LocalizableField name;
        public JSONParser.LocalizableField description;
        public List<string> metaCategories;
        public string texture;
        public string pixelTexture;
        public int? powerLevel;
        public string abilityLearnedDialogue;

        public int? priority;
        public bool? opponentUsable;
        public bool? canStack;
        public bool? isSpecialAbility;
        
        public bool? isPowerStat;
        public bool? appliesToAttack;
        public bool? appliesToHealth;
        private AConfigilData _aConfigilDataImplementation;


        public sealed override void Initialize()
        {
            name = new("name"); // name, name_es... etc
            description = new("description"); // description, description_ko... etc
        }
    }
}
