using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using DiskCardGame;

namespace JSONLoaderPlugin
{
    public static class JLUtils
    {
        private static readonly Dictionary< string, AbilityData > cardsWithParams = new();

        private static Texture2D LoadTexture2D( string image )
            => String.IsNullOrEmpty( image ) ? null : new Texture2D( 2, 2 ).WithImage( image );

        private static T TryLog<T>( Func<T> func, string message )
        {
            try
            {
                return func();
            }
            catch
            {
                Plugin.Log.LogError( message );
            }

            return default;
        }

        #region Assignment Helpers

        private static string Message( string cardName, string value, string field )
            => $"{ cardName } - \"{ value }\" is an invalid value for { field }";

        public static List<T> Assign<T>( CardData card, List<string> fieldData, string fieldName, Dictionary<string, T> dict ) where T : struct, IConvertible
            => fieldData?.AsEnum( dict, v => Message( card.name, v, fieldName ) );

        public static T Assign<T>( CardData card, string fieldData, string fieldName, Dictionary<string, T> dict ) where T : struct, IConvertible
            => string.IsNullOrEmpty( fieldData ) ? default : TryLog( () => dict[ fieldData ], Message( card.name, fieldData, fieldName ) );

        public static Texture2D Assign( CardData card, string fieldData, string fieldName )
            => string.IsNullOrEmpty( fieldData ) ? null : TryLog( () => LoadTexture2D( card.texture ), Message( card.name, fieldData, fieldName ) );

        public static List<Texture> Assign( CardData card, List<string> fieldData, string fieldName )
            => fieldData?.Select( elem => TryLog( () => ( Texture ) LoadTexture2D( elem ), Message( card.name, elem, fieldName ) ) ).ToList();

        public static List<T> CheckThenAssign<T>( CardData card, List<string> fieldData, string fieldName, Dictionary<string, T> dict ) where T : struct, IConvertible
            => card.fieldsToEdit.Contains( fieldName ) ? Assign( card, fieldData, fieldName, dict ) : null;

        public static T? CheckThenAssign<T>( CardData card, string fieldData, string fieldName, Dictionary<string, T> dict ) where T : struct, IConvertible
            => card.fieldsToEdit.Contains( fieldName ) ? Assign( card, fieldData, fieldName, dict ) : null;

        public static Texture2D CheckThenAssign( CardData card, string fieldData, string fieldName )
            => card.fieldsToEdit.Contains( fieldName ) ? Assign( card, fieldData, fieldName ) : null;

        public static List<Texture> CheckThenAssign( CardData card, List<string> fieldData, string fieldName )
            => card.fieldsToEdit.Contains( fieldName ) ? Assign( card, fieldData, fieldName ) : null;

        #endregion

        public static void AssignAbilityData( CardData card )
            => cardsWithParams.Add( card.name, new( EvolveData.Generate( card ), TailData.Generate( card ), IceCubeData.Generate( card ) ) );
        
        public static void ProcessAbilityData()
        {
            foreach ( var cardToMod in cardsWithParams.ToList() )
            {
                CardInfo card = ScriptableObjectLoader<CardInfo>.AllData.Find( elem => elem.name == cardToMod.Key );

                if ( card is null )
                {
                    Plugin.Log.LogError( $"Couldn't find \"{ cardToMod.Key }\" to add Evolve/Tail/IceCube params to" );
                    continue;
                }

                card.evolveParams = cardToMod.Value.evolveData?.AsParams( card.name );

                card.tailParams = cardToMod.Value.tailData?.AsParams( card.name );

                card.iceCubeParams = cardToMod.Value.iceCubeData?.AsParams( card.name );
            }

            cardsWithParams.Clear();
        }

        public static void CheckValidFields( CardData card )
        {
            foreach ( string field in card.fieldsToEdit )
            {
                if ( String.IsNullOrEmpty( field ) )
                {
                    Plugin.Log.LogError( $"{ card.name } - fieldsToEdit cannot contain an empty string" );
                    continue;
                }

                if ( !Dicts.CardDataFields.Contains( field ) )
                    Plugin.Log.LogError( $"{ card.name } - \"{ field }\" is an invalid field name" );
            }
        }
    }

    public static class UtilityExtensions
    {
        public static List<T> AsEnum<T>( this List<string> list, Dictionary<string, T> dict, Func<string, string> message ) where T : struct, IConvertible
        {
            List<T> output = new();

            foreach ( string elem in list )
            {
                try
                {
                    output.Add( dict[ elem ] );
                }
                catch
                {
                    Plugin.Log.LogError( message( elem ) );
                }
            }

            return output;
        }

        public static Texture2D WithImage( this Texture2D texture, string image )
        {
            byte[] imgBytes = File.ReadAllBytes( Path.Combine( Plugin.ArtPath, image ) );
            texture.LoadImage( imgBytes );
            return texture;
        }

        public static Sprite AsSprite( this Texture2D tex, string name )
        {
            tex.name = "portrait_" + name;
            tex.filterMode = FilterMode.Point;
            Sprite sprite = Sprite.Create(tex, new(0.0f, 0.0f, tex.width, tex.height), new(0.5f, 0.5f));
            sprite.name = "portrait_" + name;

            return sprite;
        }
    }
}
