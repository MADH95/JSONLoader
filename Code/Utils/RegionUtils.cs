using APIPlugin;
using DiskCardGame;
using JLPlugin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DiskCardGame.EncounterBlueprintData;

namespace JLPlugin.Utils
{
    public static class RegionUtils
    {
        public static T Assign<T>( string data, string field, Dictionary<string, T> dict )
        {
            ErrorUtil.Field = field;

            if ( string.IsNullOrEmpty( data ) )
                return default;

            if ( !dict.ContainsKey( data ) )
            {
                ErrorUtil.Log( data );
                return default;
            }

            return dict[data];
        }

        public static List<T> Assign<T>( List<string> list, string field, Dictionary<string, T> dict )
        {
            ErrorUtil.Field = field;

            if ( list is null || list.Count == 0 )
                return null;

            List<T> output = new();

            foreach ( string value in list )
            {
                if ( !dict.ContainsKey( value ) )
                {
                    ErrorUtil.Log( value );
                    continue;
                }

                output.Add( dict[value] );
            }

            if ( output.Count == 0 )
                return null;

            return output;
        }

        public static Texture Assign( string image, string field )
        {
            ErrorUtil.Field = field;

            if ( string.IsNullOrEmpty( image ) )
                return null;

            if ( !image.EndsWith( ".png" ) )
            {
                ErrorUtil.Log( image, ", it must be a .png" );
                return null;
            }

            return JLUtils.LoadTexture2D( image );
        }

        public static Color GetColor( string color )
        {
            Color c;
            ColorUtility.TryParseHtmlString( color, out c );
            return c;
        }
    }
}
