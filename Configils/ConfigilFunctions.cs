
using System;
using System.Linq;

namespace JLPlugin
{
	static class ConfigilFunctions
	{
		public static string StringContains( string functionContents )
		{
			if ( !functionContents.Contains( "," ) )
			{
				throw new Exception( "StringContains: Too few function parameters" );
			}

			var parameters = functionContents.Split( ',' );

			if ( parameters.Length > 2 )
			{
				throw new Exception( "StringContains: Too many function parameters" );
			}

			if ( parameters.Any( elem => string.IsNullOrEmpty( elem ) ) )
			{
				throw new Exception( "StringContains: Invalid parameters" );
			}

			return parameters[ 0 ].Contains( parameters[ 1 ] ).ToString();
		}

		public static string Random( string functionContents )
		{
			var parameters = functionContents.Split( ',' );

			Random random = new();

			return parameters[ random.Next( parameters.Length ) ];
		}

		public static string CardInSlot( string functionContents )
		{
			if ( int.TryParse( functionContents, out int slotIndex ) )
			{
				throw new Exception( $"CardInSlot: {functionContents} is an invalid slot index" );
			}

			//TODO: figure the rest of this out later

			throw new Exception( "CardInSlot: function is incomplete" );
		}

	}
}
