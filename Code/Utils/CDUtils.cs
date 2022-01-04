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

            if ( !image.EndsWith( ".png" ) || !image.StartsWith( "data:image/png;base64," ) )
            {
                ErrorUtil.Log( image, ", it must be a .png, or base64 encoded .png" );
                return null;
            }

            return JLUtils.LoadTexture2D( image );
        }

        public static List<Texture> Assign( List<string> list, string field )
        {
            // vladde: is this necessary?
            ErrorUtil.Field = field;

            if ( list is null || list.Count == 0 )
                return null;

            List<Texture> output = new();

            foreach ( string image in list )
            {
                Texture2D texture = CDUtils.Assign( image, field );

                if ( texture is null ) 
                    continue;

                output.Add( texture );
            }

            if ( output.Count == 0 )
                return null;

            return output;
        }
    }
}
