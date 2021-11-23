using System.Collections.Generic;

using UnityEngine;

namespace JLPlugin.Utils
{
    public static class CDUtils
    {
        public static void CheckValidFields( List<string> fields )
        {
            foreach ( string field in fields )
            {
                if ( string.IsNullOrEmpty( field ) )
                {
                    Plugin.Log.LogError( $"{ ErrorUtil.Card } - fieldsToEdit cannot contain an empty string" );
                    continue;
                }

                if ( !Dicts.CardDataFields.Contains( field ) )
                    Plugin.Log.LogError( $"{ ErrorUtil.Card } - \"{ field }\" is an invalid field name" );
            }
        }

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

            return dict[ data ];
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

                output.Add( dict[ value ] );
            }

            if ( output.Count == 0 )
                return null;

            return output;
        }

        public static Texture2D Assign( string image, string field )
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

        public static List<Texture> Assign( List<string> list, string field )
        {
            ErrorUtil.Field = field;


            if ( list is null || list.Count == 0 )
                return null;

            List<Texture> output = new();

            foreach ( string image in list )
            {
                if ( string.IsNullOrEmpty( image ) )
                    continue;

                if ( !image.EndsWith( ".png" ) )
                {
                    ErrorUtil.Log( image, ", it must be a .png" );
                    continue;
                }

                output.Add( JLUtils.LoadTexture2D( image ) );
            }

            if ( output.Count == 0 )
                return null;

            return output;
        }
    }
}
