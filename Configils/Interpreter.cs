
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JLPlugin
{
	using NCalc;
	using Data;

	static class Interpreter
	{
		public static class RegexStrings
		{
			// Detects functions in the format name(params)
			public static string Function = @"([a-zA-Z]+)\((.*?)\)$";

			// Detects a variable in the format [variableName] or [variableName.memberName]
			public static string Variable = @"\[.*?\]";

			//Detects an Expression in the format (1 + 4)
			public static string Expression = @"\(.*?\)";

			// Detects all whitespace in a string
			public static string Whitespace = @"\s+";
		}

		private static readonly List<string> NCalcFunctions = new()
		{
			"if",
			"in"
		};

		public static AbilityBehaviourData abilityData;


		public static string Process( in string input )
		{
			string output = input;

			if ( Regex.Matches( output, RegexStrings.Variable ) is var variables
				&& variables.Cast<Match>().Any( variables => variables.Success ) )
			{
				foreach ( Match variable in variables )
				{
					output = input.Replace( variable.Value, ProcessVariable( variable ) );
				}
			}

			if ( Regex.Match( output, RegexStrings.Function ) is var function && function.Success )
			{
				output = input.Replace( function.Value, ProcessFunction( function ) );
			}

			if ( Regex.Match( output, RegexStrings.Expression ) is var expression && expression.Success )
			{
				Expression e = new( output );
				output = e.Evaluate().ToString();
			}

			if ( output == "True" || output == "False" )
			{
				output = output.ToLower();
			}

			return output;
		}

		private static string ProcessFunction ( Match function )
		{
			string fullFunction = function.Groups[0].Value;
			string functionName = function.Groups[1].Value;

			if ( NCalcFunctions.Contains( functionName ) )
			{
				return fullFunction;
			}

			string functionContents = Regex.Replace( function.Groups[ 2 ].Value, RegexStrings.Whitespace, string.Empty );

			switch ( functionName )
			{
				case "StringContains":
				{
					return ConfigilFunctions.StringContains( functionContents );
				}
				case "Random":
				{
					return ConfigilFunctions.Random( functionContents );
				}
				case "CardInSlot":
				{
					return ConfigilFunctions.CardInSlot( functionContents );
				}
				default:
				{
					throw new Exception( $"{functionName} - is an invalid function name" );
				}

				//Suggested functions
				
				//GetSlot( Player/Opponent, Index) might be easier to get slot in real time rather than updating a list every frame, would only get the required slots


			}
		}

		public static string ProcessVariable( Match variable )
		{
			if ( !variable.Value.Contains('.') )
			{
				bool validVariable = abilityData.variables.TryGetValue( variable.Value, out string value );

				if ( !validVariable )
				{
					throw new Exception( $"{ variable.Value } is an invalid variable" );
				}

				return value;
			}

			var fieldList = variable.Value.Trim( '[', ']' ).Split('.').ToList();


			object obj = null;

			for( int i = 0; i < fieldList.Count; ++i )
			{
				bool validVariable = abilityData.generatedVariables.TryGetValue( fieldList[ i ], out obj );

				if ( !validVariable )
				{
					throw new Exception( $"{ variable.Value } is an invalid generated variable" );
				}

				PropertyInfo property = obj.GetType().GetProperty( fieldList[i] );

				if ( property is null )
				{
					var field = obj.GetType().GetField( fieldList[i] );

					if ( field is null )
					{
						throw new Exception( $"{variable.Value} is an invalid generated variable" );
					}

					obj = field.GetValue( obj );
					continue;
				}

				if ( property.GetIndexParameters().Length < 1 )
				{
					obj = property.GetValue( obj );
					continue;
				}

				//If we decide to do index lookup it will be handled here.
				//Convert the fieldList[i+1] value to an integer and call GetValue(obj, new(){ convertedInteger } )
				//***I THINK***

				break;
			}

			string output = obj.ToString();

			if ( obj is IEnumerable collection )
			{
				output = $"'{ string.Join( "','", collection ) }'";
			}

			return output;
		}
	}
}
