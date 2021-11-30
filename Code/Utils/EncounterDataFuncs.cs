
using APIPlugin;

namespace JLPlugin.Data
{
    using System.Linq;
    using Utils;

    public partial class EncounterData
    {
        public void GenerateNew()
        {
            ErrorUtil.Identifier = this.name;
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

                    dominantTribes:     EDUtils.Assign(    this.dominantTribes,        nameof( this.dominantTribes     ),      Dicts.Tribes        ),
                    redundantAbilities: EDUtils.Assign(    this.redundantAbilities,    nameof( this.redundantAbilities ),      Dicts.Abilities     ),

                    unlockedCardPrerequisites:  EDUtils.GetCardInfos( this.unlockedCardPrerequisites ),
                    randomReplacementCards:     EDUtils.GetCardInfos( this.randomReplacementCards ),
                    turns:                      EDUtils.GetCardBlueprints( this.turns ),

                    turnMods:           EDUtils.GetTurnMods( this.turnMods ),

                    regular:            !this.bossPrep || this.regular,
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
