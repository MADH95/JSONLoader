using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using BepInEx;

using TinyJson;

namespace JLPlugin.Utils
{
    using Data;

    public static class JLUtils
    {
        public static void LoadCardsFromFiles()
        {
            foreach ( string file in Directory.EnumerateFiles( Paths.PluginPath, "*.jldr", SearchOption.AllDirectories ) )
            {
                string filename = file.Substring( file.LastIndexOf( Path.DirectorySeparatorChar ) + 1 );

                Data.CardData card = CreateFromJSON( File.ReadAllText( file ) );

                if ( card is null )
                {
                    Plugin.Log.LogWarning( $"Failed to load { filename }" );

                    continue;
                }

                if ( card.fieldsToEdit is null )
                {
                    card.GenerateNew();
                    continue;
                }

                card.Edit();
            }
        }

        public static CardData CreateFromJSON( string jsonString )
            => JSONParser.FromJson<CardData>( jsonString );

        public static Texture2D LoadTexture2D( string image )
        {
            byte[] imgBytes = [];

            if ( image.StartsWith( "data:image/png;base64," ) )
            {
                imgBytes = Convert.FromBase64String( image.Substring( 22 ) );
            }
            else
            {
                string[] imagePaths = Directory.GetFiles( Paths.PluginPath, image, SearchOption.AllDirectories );

                if ( imagePaths.Length == 0 )
                {
                    Plugin.Log.LogError( $"{ ErrorUtil.Card } - Couldn't find texture \"{ image }\" to load into { ErrorUtil.Field }" );
                    return null;
                }

                if ( imagePaths.Length > 1 )
                {
                    Plugin.Log.LogError( $"{ ErrorUtil.Card } - Couldn't load \"{ image }\" into { ErrorUtil.Field }, more than one file with that name found in the plugins folder" );
                    return null;
                }

                imgBytes = File.ReadAllBytes( imagePaths[0] );
            }

            Texture2D texture = new( 2, 2 );

            if ( !texture.LoadImage( imgBytes ) )
            {
                Plugin.Log.LogError( $"{ ErrorUtil.Card } - Couldn't load \"{ image }\" into { ErrorUtil.Field }" );
                return null;
            }

            return texture;

        }
    }
}
