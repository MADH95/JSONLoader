using DiskCardGame;

using UnityEngine;

namespace JSONLoaderPlugin
{

    public record struct AbilityData( EvolveData evolveData, TailData tailData, IceCubeData iceCubeData );

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

    public class TailData
    {
        public string tailName;
        public Texture2D tailLostPortrait;

        public static TailData Generate( CardData card )
        {
            return card.tail_cardName is null ? null : new()
            {
                tailName = card.tail_cardName,
                tailLostPortrait = JLUtils.CheckThenAssign( card, card.tail_tailLostPortrait, nameof( card.tail_tailLostPortrait ) )
            };
        }

        public TailParams AsParams( string owner )
        {
            CardInfo tail = ScriptableObjectLoader<CardInfo>.AllData.Find( elem => elem.name == this.tailName );

            if ( tail is null )
            {
                Plugin.Log.LogError( $"TailParams - No card named \"{ this.tailName }\" exists to assign to \"{ owner }\"" );
                return null;
            }

            Plugin.Log.LogInfo( $"TailParams - \"{ this.tailName }\" assigned to \"{ owner }\"" );

            return new()
            {
                tail = tail,
                tailLostPortrait = this.tailLostPortrait?.AsSprite( owner + "_tailless" )
            };
        }
    }

    public class IceCubeData
    {
        public string creatureWithin;

        public static IceCubeData Generate( CardData card )
        {
            return card.iceCube_creatureWithin is null ? null : new()
            {
                creatureWithin = card.iceCube_creatureWithin
            };
        }

        public IceCubeParams AsParams( string owner )
        {
            CardInfo creatureWithin = ScriptableObjectLoader<CardInfo>.AllData.Find(elem => elem.name == this.creatureWithin);

            if ( creatureWithin is null )
            {
                Plugin.Log.LogError( $"IceCubeParams - No card named \"{ this.creatureWithin }\" exists to assign to \"{ owner }\"" );
                return null;
            }

            Plugin.Log.LogInfo( $"IceCubeParams - \"{ this.creatureWithin }\" assigned to \"{ owner }\"" );

            return new()
            {
                creatureWithin = creatureWithin
            };
        }
    }
}
