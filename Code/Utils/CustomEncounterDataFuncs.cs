using System.Collections.Generic;

using DiskCardGame;
using APIPlugin;

namespace JLPlugin.Data
{
    using System.Linq;
    using Utils;

    public partial class CustomEncounterData
    {
        public void GenerateNew()
        {
            ErrorUtil.Encounter = this.name;
            ErrorUtil.Message = "Encounter {0} - {2} is an invalid value for {1}";

            bool regionSpecific = ( this.regions.Count <= 1 );
            foreach ( string regionName in this.regions )
            {
                NewEncounter.Add(
                    name:               this.name,
                    regionName:         regionName,
                    regionSpecific:     regionSpecific,
                    minDifficulty:      this.minDifficulty,
                    maxDifficulty:      maxDifficulty == 0 ? this.maxDifficulty : 30,

                    dominantTribes:     CustomEncounterUtils.Assign(    this.dominantTribes,        nameof( this.dominantTribes     ),      Dicts.Tribes        ),
                    redundantAbilities: CustomEncounterUtils.Assign(    this.redundantAbilities,    nameof( this.redundantAbilities ),      Dicts.Abilities     ),

                    unlockedCardPrerequisites:  CustomEncounterUtils.GetCardInfos( this.unlockedCardPrerequisites ),
                    randomReplacementCards:     CustomEncounterUtils.GetCardInfos( this.randomReplacementCards ),
                    turns:                      CustomEncounterUtils.GetCardBlueprints( this.turns ),

                    turnMods:           CustomEncounterUtils.GetTurnMods( this.turnMods ),

                    regular:            this.bossPrep ? this.regular : true,
                    bossPrep:           this.bossPrep,

                    oldPreviewDifficulty: 0 // Unused
                );
            }
            if ( this.regions.Count == 0 )
            {
                Plugin.Log.LogInfo( $"Encounter { this.name } does not have any regions defined." );
            }

            ErrorUtil.Clear();
        }
    }
}
