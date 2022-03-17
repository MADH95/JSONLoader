using System.IO;
using UnityEngine;
using TinyJson;

namespace JLPlugin.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BepInEx;
    using Data;
    using JLPlugin.V2.Data;

    [Obsolete]
    public static class JLUtils
    {
        public static void LoadCardsFromFiles()
        {
            Dictionary<string, Data.CardData> loadedJldrs = new();
            foreach ( string file in Directory.EnumerateFiles( Paths.PluginPath, "*.jldr", SearchOption.AllDirectories ) )
            {
                string filename = file.Substring( file.LastIndexOf( Path.DirectorySeparatorChar ) + 1 );

                Data.CardData card = CreateFromJSON( File.ReadAllText( file ) );

                if ( card is null )
                {
                    Plugin.Log.LogWarning( $"Failed to load { filename }" );

                    continue;
                }

                loadedJldrs.Add(file, card);
            }

            List<Data.CardData> allCards = loadedJldrs.Values.ToList();

            foreach (var item in loadedJldrs)
            {
                string file = item.Key;
                Data.CardData card = item.Value;

                // Convert to JLDR2 and write back for now
                CardSerializeInfo info = card.ConvertToV2(allCards);
                if (info != null)
                    info.WriteToFile(file, false);
                else   
                    Plugin.Log.LogError($"{file} is a JLDR without a valid name");
            }
        }

        public static CardData CreateFromJSON( string jsonString )
            => JSONParser.FromJson<CardData>( jsonString );

        public static Texture2D LoadTexture2D( string image )
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

            byte[] imgBytes = File.ReadAllBytes( imagePaths[0] );

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
