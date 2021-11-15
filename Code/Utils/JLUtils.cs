using System;
using System.IO;
using System.Collections.Generic;

using UnityEngine;

using APIPlugin;

namespace JLPlugin.Utils
{
    using Data;

    public static class JLUtils
    {
        private static Texture2D LoadTexture2D( string image )
        {
            byte[] imgBytes = File.ReadAllBytes( Path.Combine( Plugin.ArtPath, image ) );

            Texture2D texture = new( 2, 2 );

            if ( texture.LoadImage( imgBytes ) )
            {
                return texture;
            }

            Plugin.Log.LogError( $"{ ErrorUtil.Card } - Couldn't find texture \"{ image }\" to load into { ErrorUtil.Field }" );

            return null;
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

            foreach(string value in list)
            {
                if ( !dict.ContainsKey( value ) )
                {
                    ErrorUtil.Log( value );
                    continue;
                }

                output.Add( dict[ value ] );
            }

            if ( output.Count == 0 )
            {
                return null;
            }

            return output;
        }

        public static Texture2D Assign( string image, string field )
        {
            ErrorUtil.Field = field;

            if ( string.IsNullOrEmpty( image ) )
            {
                return null;
            }

            if ( !image.EndsWith( ".png" ) )
            {
                ErrorUtil.Log( image, ", it must be a .png" );
                return null;
            }

            return LoadTexture2D( image );
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

                output.Add( LoadTexture2D( image ) );
            }

            if ( output.Count == 0 )
                return null;

            return output;
        }

        public static void CheckValidFields( List<string> fields )
        {
            foreach ( string field in fields )
            {
                if ( String.IsNullOrEmpty( field ) )
                {
                    Plugin.Log.LogError( $"{ ErrorUtil.Card } - fieldsToEdit cannot contain an empty string" );
                    continue;
                }

                if ( !Dicts.CardDataFields.Contains( field ) )
                    Plugin.Log.LogError( $"{ ErrorUtil.Card } - \"{ field }\" is an invalid field name" );
            }
        }

        public static EvolveIdentifier GenerateEvolveIdentifier( CardData card )
        {
            if ( card.evolution is null )
            {
                if ( string.IsNullOrEmpty( card.evolve_evolutionName ) )
                {
                    return null;
                }

                return new(
                    name: card.evolve_evolutionName,
                    turnsToEvolve: card.evolve_turnsToEvolve == 0 ? 1 : card.evolve_turnsToEvolve
                );
            }

            if ( string.IsNullOrEmpty( card.evolution.name ) )
            {
                Plugin.Log.LogError( $"{ card.name } - { nameof( card.evolution ) } must have a name" );
                return null;
            }

            return new(
                name: card.evolution.name,
                turnsToEvolve: card.evolution.turnsToEvolve == 0 ? 1 : card.evolution.turnsToEvolve
            );
        }

        public static TailIdentifier GenerateTailIdentifier( CardData card )
        {
            if ( card.tail is null )
            {
                if ( string.IsNullOrEmpty( card.tail_cardName ) )
                {
                    return null;
                }

                return new(
                    name: card.tail_cardName,
                    tailLostTex: Assign( card.tail_tailLostPortrait, nameof( card.tail_tailLostPortrait ) )
                );
            }

            if ( string.IsNullOrEmpty( card.tail.name ) )
            {
                Plugin.Log.LogError( $"{ card.name } - { nameof( card.tail ) } must have a name" );
                return null;
            }

            return new(
                name: card.tail.name,
                tailLostTex: Assign( card.tail.tailLostPortrait, nameof( card.tail.tailLostPortrait ) )
            );
        }

        public static IceCubeIdentifier GenerateIceCubeIdentifier( CardData card )
        {
            if ( card.iceCube is null )
            {
                if ( string.IsNullOrEmpty( card.iceCube_creatureWithin ) )
                {
                    return null;
                }

                return new(
                    name: card.iceCube_creatureWithin
                );
            }

            if ( string.IsNullOrEmpty( card.iceCube.creatureWithin ) )
            {
                Plugin.Log.LogError( $"{ card.name } - { nameof( card.iceCube ) } must have a { nameof( card.iceCube.creatureWithin ) }" );
                return null;
            }

            return new(
                name: card.iceCube.creatureWithin
            );
        }
    }
}
