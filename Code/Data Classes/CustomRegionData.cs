using APIPlugin;
using System;
using System.Collections.Generic;

namespace JLPlugin.Data
{
    [System.Serializable]
    public partial class CustomRegionData
    {
        public string name;
        public int tier;

        public List<string> terrainCards;
        public List<string> likelyCards;
        public List<string> dominantTribes;

        public string boardLightColor;
        public string cardsLightColor;
        public bool dustParticlesDisabled;
        public bool fogEnabled;
        public float fogAlpha;
        public string mapAlbedo;
        public string mapEmission;
        public string mapEmissionColor;
        public bool silenceCabinAmbience;
    }
}
