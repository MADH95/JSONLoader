using System.IO;

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
            foreach ( string file in Directory.EnumerateFiles( Paths.PluginPath, "*_card.jldr", SearchOption.AllDirectories ) )
            {
                string filename = file.Substring( file.LastIndexOf( Path.DirectorySeparatorChar ) + 1 );

                CardData card = CreateCardFromJSON( File.ReadAllText( file ) );

                if ( card is null )
                {
                    Plugin.Log.LogWarning( $"Failed to load card { filename }" );

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

        public static void LoadRegionsFromFiles()
        {
            foreach ( string file in Directory.EnumerateFiles( Paths.PluginPath, "*_region.jldr", SearchOption.AllDirectories ) )
            {
                string filename = file.Substring( file.LastIndexOf( Path.DirectorySeparatorChar ) + 1 );

                string text = File.ReadAllText (file );

                if ( !text.ToLowerInvariant().StartsWith( "#ignore" ) && !text.ToLowerInvariant().StartsWith( "# ignore" ) )
                {
                    Data.CustomRegionData region = CreateRegionFromJSON( text );

                    if (region is null)
                    {
                        Plugin.Log.LogWarning( $"Failed to load region { filename }" );

                        continue;
                    }

                    region.GenerateNew();
                }
            }
        }

        public static void LoadEncountersFromFiles()
        {
            foreach ( string file in Directory.EnumerateFiles( Paths.PluginPath, "*_encounter.jldr", SearchOption.AllDirectories ) )
            {
                string filename = file.Substring( file.LastIndexOf( Path.DirectorySeparatorChar ) + 1 );

                EncounterData encounter = CreateEncounterFromJSON( File.ReadAllText( file ) );

                if ( encounter is null )
                {
                    Plugin.Log.LogWarning( $"Failed to load encounter { filename }" );

                    continue;
                }

                encounter.GenerateNew();
            }
        }

        public static CardData CreateCardFromJSON( string jsonString )
            => JSONParser.FromJson<CardData>( jsonString );

        public static EncounterData CreateEncounterFromJSON( string jsonString )
            => JSONParser.FromJson<EncounterData>( jsonString );

        public static CustomRegionData CreateRegionFromJSON( string jsonString )
            => JSONParser.FromJson<CustomRegionData>( jsonString );

        public static Texture2D LoadTexture2D( string image )
        {
            string[] imagePaths = Directory.GetFiles( Paths.PluginPath, image, SearchOption.AllDirectories );

            if ( imagePaths.Length == 0 )
            {
                Plugin.Log.LogError( $"{ ErrorUtil.Identifier } - Couldn't find texture \"{ image }\" to load into { ErrorUtil.Field }" );
                return null;
            }

            if ( imagePaths.Length > 1 )
            {
                Plugin.Log.LogError( $"{ ErrorUtil.Identifier } - Couldn't load \"{ image }\" into { ErrorUtil.Field }, more than one file with that name found in the plugins folder" );
                return null;
            }

            byte[] imgBytes = File.ReadAllBytes( imagePaths[0] );

            Texture2D texture = new( 2, 2 );

            if ( !texture.LoadImage( imgBytes ) )
            {
                Plugin.Log.LogError( $"{ ErrorUtil.Identifier } - Couldn't load \"{ image }\" into { ErrorUtil.Field }" );
                return null;
            }

            return texture;

        }
    }
}
