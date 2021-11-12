using DiskCardGame;

namespace JLPlugin.Data
{
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
