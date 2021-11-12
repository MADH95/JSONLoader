using UnityEngine;

using DiskCardGame;

namespace JLPlugin.Data
{
    using Utils;

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
}
