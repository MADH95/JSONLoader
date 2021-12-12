using System.Collections.Generic;

using DiskCardGame;
using static DiskCardGame.EncounterBlueprintData;

using APIPlugin;

namespace JLPlugin.Data
{
    using System.Linq;
    using UnityEngine;
    using Utils;

    public partial class RegionData
    {
        public void GenerateNew()
        {
            ErrorUtil.Identifier = this.name;
            ErrorUtil.Message = "Region {0} - {2} is an invalid value for {1}";

            DiskCardGame.RegionData standard = ResourceBank.Get<RegionProgression>("Data/Map/RegionProgression").regions[this.tier][0];

            DiskCardGame.RegionData rd = ScriptableObject.CreateInstance<DiskCardGame.RegionData>();
            rd.ambientLoopId =              standard.ambientLoopId;
            rd.boardLightColor =            this.boardLightColor != null ? RegionUtils.GetColor( this.boardLightColor ) : standard.boardLightColor;
            rd.bosses =                     standard.bosses;
            rd.bossPrepCondition =          standard.bossPrepCondition;
            rd.bossPrepEncounter =          standard.bossPrepEncounter;
            rd.cardsLightColor =            this.cardsLightColor != null ? RegionUtils.GetColor( this.cardsLightColor ) : standard.cardsLightColor;
            rd.consumableItems =            standard.consumableItems;
            rd.dominantTribes =             this.dominantTribes != null ? RegionUtils.Assign(   this.dominantTribes,    nameof(this.dominantTribes),    Dicts.Tribes   ) : standard.dominantTribes;
            rd.dustParticlesDisabled =      this.dustParticlesDisabled;
            rd.encounters =                 new List<EncounterBlueprintData>();
            rd.fillerScenery =              standard.fillerScenery;
            rd.fogAlpha =                   standard.fogAlpha;
            rd.fogEnabled =                 this.fogEnabled;
            rd.fogProfile =                 standard.fogProfile;
            rd.likelyCards =                this.likelyCards != null ? EDUtils.GetCardInfos( this.likelyCards ) : standard.likelyCards;
            rd.mapAlbedo =                  standard.mapAlbedo;
            rd.mapEmission =                this.mapEmission != null ? RegionUtils.Assign( this.mapEmission, nameof( this.mapEmission ) ) : standard.mapEmission;
            rd.mapEmissionColor =           this.mapEmissionColor != null ? RegionUtils.GetColor( this.mapEmissionColor ) : standard.mapEmissionColor;
            rd.mapParticlesPrefabs =        standard.mapParticlesPrefabs;
            rd.name =                       this.name;
            rd.predefinedNodes =            standard.predefinedNodes;
            rd.predefinedScenery =          standard.predefinedScenery;
            rd.scarceScenery =              standard.scarceScenery;
            rd.silenceCabinAmbience =       this.silenceCabinAmbience;
            rd.terrainCards =               this.terrainCards != null ? EDUtils.GetCardInfos(this.terrainCards) : standard.terrainCards;
            new NewRegion(rd, this.tier);

            ErrorUtil.Clear();
        }
    }
}
