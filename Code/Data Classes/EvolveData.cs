using DiskCardGame;

namespace JLPlugin.Data
{
    public class EvolveData
    {
        public string evolutionName;
        public int turnsToEvolve;

        public static EvolveData Generate( CardData card )
        {
            return card.evolve_evolutionName is null ? null : new()
            {
                evolutionName = card.evolve_evolutionName,
                turnsToEvolve = card.evolve_turnsToEvolve == 0 ? 1 : card.evolve_turnsToEvolve
            };
        }

        public EvolveParams AsParams( string owner )
        {
            CardInfo evolution = ScriptableObjectLoader<CardInfo>.AllData.Find( elem => elem.name == this.evolutionName );

            if ( evolution is null )
            {
                Plugin.Log.LogError( $"EvolveParams - No card named \"{ this.evolutionName }\" exists to assign to \"{ owner }\"" );
                return null;
            }

            Plugin.Log.LogInfo( $"EvolveParams - \"{ this.evolutionName }\" assigned to \"{ owner }\"" );

            return new()
            {
                evolution = evolution,
                turnsToEvolve = this.turnsToEvolve
            };
        }
    }
}
