using System.Linq;
using System.Collections.Generic;

using APIPlugin;

namespace JLPlugin.Utils
{
    using System;
    using Data;

    [Obsolete]
    public static class IDUtils
    {
        public static EvolveIdentifier GenerateEvolveIdentifier( CardData card )
        {
            if ( card.evolution is null )
                return null;

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
                return null;

            if ( string.IsNullOrEmpty( card.tail.name ) )
            {
                Plugin.Log.LogError( $"{ card.name } - { nameof( card.tail ) } must have a name" );
                return null;
            }

            return new(
                name: card.tail.name,
                tailLostTex: CDUtils.Assign( card.tail.tailLostPortrait, nameof( card.tail.tailLostPortrait ) )
            );
        }

        public static IceCubeIdentifier GenerateIceCubeIdentifier( CardData card )
        {
            if ( card.iceCube is null )
                return null;

            if ( string.IsNullOrEmpty( card.iceCube.creatureWithin ) )
            {
                Plugin.Log.LogError( $"{ card.name } - { nameof( card.iceCube ) } must have a { nameof( card.iceCube.creatureWithin ) }" );
                return null;
            }

            return new(
                name: card.iceCube.creatureWithin
            );
        }

        public static List<AbilityIdentifier> GenerateAbilityIdentifiers( List<AbilityData> list )
            => list is not null ? list.Select( elem => AbilityIdentifier.GetAbilityIdentifier( elem.GUID, elem.name ) ).ToList() : null;

        public static List<SpecialAbilityIdentifier> GenerateSpecialAbilityIdentifiers( List<SpecialAbilityData> list )
            => list is not null ? list.Select( elem => SpecialAbilityIdentifier.GetID( elem.GUID, elem.name ) ).ToList() : null;
    }
}
