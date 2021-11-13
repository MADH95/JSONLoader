using System.Collections.Generic;

namespace JLPlugin.Data
{
    [System.Serializable]
    public partial class CardData
    {
        public List<string> fieldsToEdit;
        public string name, displayedName, description;

        public List<string> metaCategories;
        public string cardComplexity;
        public string temple;
        public List<string> tribes;


        public int baseAttack;
        public int baseHealth;
        public bool hideAttackAndHealth;

        public int cost, bonesCost, energyCost;
        public List<string> gemsColour;

        public List<string> abilities;
        public List<string> customAbilities;
        public List<string> traits;
        public List<string> specialAbilities;
        public string specialStatIcon;

        public EvolveData evolution;
        public string evolve_evolutionName;
        public int evolve_turnsToEvolve;
        public string defaultEvolutionName;

        public TailData tail;
        public string tail_cardName;
        public string tail_tailLostPortrait;

        public IceCubeData iceCube;
        public string iceCube_creatureWithin;
        
        public bool flipPortraitForStrafe;
        public bool onePerDeck;

        public List<string> appearanceBehaviour;
        public string texture, altTexture;
        public string titleGraphic;
        public string pixelTexture;
        public string animatedPortrait;
        public List<string> decals;
    }
}
