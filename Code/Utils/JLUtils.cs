using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using DiskCardGame;

namespace JLPlugin.Utils
{
    using Data;

    public static class JLUtils
    {
        private record struct AbilityData( EvolveData evolveData, TailData tailData, IceCubeData iceCubeData );

        private static readonly Dictionary< string, AbilityData > cardsWithParams = new();

        private static bool ValidateTexture( string texture )
            => TryLog( () => texture.Substring( texture.Length - 4 ) is not ".png" ? throw new Exception() : true, $"\"{ texture }\" is not a .png file" );


        private static Texture2D LoadTexture2D( string image )
            => !string.IsNullOrEmpty( image ) && ValidateTexture( image ) ? new Texture2D( 2, 2 ).WithImage( image ) : null;

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
            => TryLog( () => LoadTexture2D( fieldData ), Message( card.name, fieldData, fieldName ) );

        public static List<Texture> Assign( CardData card, List<string> fieldData, string fieldName )
            => fieldData?.Select( elem => TryLog( () => ( Texture ) LoadTexture2D( elem ), Message( card.name, elem, fieldName ) ) ).ToList();


        //CustomCards need to be checked before assignment
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
}
