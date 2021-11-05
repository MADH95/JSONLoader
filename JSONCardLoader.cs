using APIPlugin;

using BepInEx;
using BepInEx.Logging;

using DiskCardGame;

using HarmonyLib;

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace JSONCardParserPlugin
{
	[BepInPlugin( PluginGuid, PluginName, PluginVersion )]
	[BepInDependency( "cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency )]
	public class Plugin : BaseUnityPlugin
	{

		public bool getTestDeck()
		{
			return Config.Bind( "JSONCardLoader", "TestDeck", false, new BepInEx.Configuration.ConfigDescription( "Load start deck with specified cards" ) ).Value;
		}

		public List<string> getCards()
		{
			List<string> list = new List<string>(4);
			list.Add( Config.Bind( "JSONCardLoader", "Card1", "Wolf" ).Value );
			list.Add( Config.Bind( "JSONCardLoader", "Card2", "Opossum" ).Value );
			list.Add( Config.Bind( "JSONCardLoader", "Card3", "Stoat_Talking" ).Value );
			list.Add( Config.Bind( "JSONCardLoader", "Card4", "Bullfrog" ).Value );
			return list;
		}

		private const string PluginGuid = "MADH.inscryption.JSONCardLoader";
		private const string PluginName = "JSONCardLoader";
		private const string PluginVersion = "1.1.0.0";

		internal static ManualLogSource Log;

		static readonly string[] artPaths = { Paths.PluginPath, "JSONLoader", "Artwork" };
		static readonly string[] jsonPaths = { Paths.PluginPath, "JSONLoader", "Cards" };

		public static readonly string ArtPath = Path.Combine(artPaths);
		private static readonly string JSONPath = Path.Combine(jsonPaths);

		private void Awake()
		{
			Logger.LogInfo( $"Loaded {PluginName}!" );
			Log = base.Logger;

			Harmony harmony = new Harmony( PluginGuid );
			harmony.PatchAll();

			//Load Config
			getTestDeck();
			getCards();

			Logger.LogInfo( "Loading cards from JSON..." );

			foreach ( string file in Directory.EnumerateFiles( JSONPath, "*.json" ) )
			{
				CardData card = CardData.CreateFromJSON( File.ReadAllText( file ) );

				CardData.GenerateNewCard( card );

				Log.LogInfo( $"Loaded { file.Substring( file.LastIndexOf( '/' ) + 1 ) }" );
			}
		}
	}

	public struct Dicts
	{
		public static Dictionary<string, int> Priority = new Dictionary<string, int>()
		{
			{ "High",	0 },
			{ "Medium", 1 },
			{ "Low",	2 }
		};

		public static Dictionary<string, CardMetaCategory> MetaCategory
			=	Enum.GetValues( typeof( CardMetaCategory ) )
					.Cast< CardMetaCategory >()
					.ToDictionary( t => t.ToString(), t => t );

		public static Dictionary<string, CardComplexity> Complexity 
			=	Enum.GetValues( typeof( CardComplexity ) )
					.Cast< CardComplexity >()
					.ToDictionary( t => t.ToString(), t => t );

		public static Dictionary<string, CardTemple> Temple 
			=	Enum.GetValues( typeof( CardTemple ) )
					.Cast< CardTemple >()
					.ToDictionary( t => t.ToString(), t => t );

		public static Dictionary<string, GemType> GemColour 
			=   Enum.GetValues( typeof( GemType ) )
					.Cast< GemType >()
					.ToDictionary( t => t.ToString(), t => t );

		public static Dictionary<string, SpecialStatIcon> StatIcon 
			=   Enum.GetValues( typeof( SpecialStatIcon ) )
					.Cast< SpecialStatIcon >()
					.ToDictionary( t => t.ToString(), t => t );

		public static Dictionary<string, Tribe> Tribes 
			=   Enum.GetValues( typeof( Tribe ) )
					.Cast< Tribe >()
					.ToDictionary( t => t.ToString(), t => t );

		public static Dictionary<string, Trait> Traits 
			=   Enum.GetValues( typeof( Trait ) )
					.Cast< Trait >()
					.ToDictionary( t => t.ToString(), t => t );

		public static Dictionary<string, SpecialTriggeredAbility> SpecialAbilities 
			=   Enum.GetValues( typeof( SpecialTriggeredAbility ) )
					.Cast< SpecialTriggeredAbility >()
					.ToDictionary( t => t.ToString(), t => t );

		public static Dictionary<string, Ability> Abilities 
			=   Enum.GetValues( typeof( Ability ) )
					.Cast< Ability >()
					.ToDictionary( t => t.ToString(), t => t );

		public static Dictionary<string, CardAppearanceBehaviour.Appearance> AppearanceBehaviour 
			=   Enum.GetValues( typeof( CardAppearanceBehaviour.Appearance ) )
					.Cast< CardAppearanceBehaviour.Appearance >()
					.ToDictionary( t => t.ToString(), t => t );
	}



	[System.Serializable]
	public class EvolveData
	{
		public int turnsToEvolve;
		public string evolution;
	}

	[System.Serializable]
	public class TailData
	{
		public string tail;
		public string tailLostPortrait;
	}

	[System.Serializable]
	public class IceCubeData
	{
		public string creatureWithin;
	}

	[System.Serializable]
	public class CardData
	{
		public string priority;
		public string name, displayedName, description;
		public List<string> metaCategories;
		public string cardComplexity;
		public string temple;
		public int baseAttack;
		public int baseHealth;
		public bool hideAttackAndHealth;
		public int cost, bonesCost, energyCost;
		public List<string> gemsColour;
		public string specialStatIcon;
		public List<string> tribes;
		public List<string> traits;
		public List<string> specialAbilities;
		public List<string> abilities;
		public EvolveData evolveParams;
		public string defaultEvolutionName;
		public TailData tailParams;
		public IceCubeData iceCubeParams;
		public bool flipPortraitForStrafe;
		public bool onePerDeck;
		public List<string> appearanceBehaviour;
		public string texture, altTexture;
		public string titleGraphic;
		public string pixelTexture;
		public string animatedPortrait;
		public List<string> decals;

		public static CardData CreateFromJSON( string jsonString )
		{
			return JsonUtility.FromJson<CardData>( jsonString );
		}

		public static Texture2D LoadTexture2D( (int x, int y) size, string image )
		{
			return image == null ? null : new Texture2D( size.x, size.y ).WithImage( image );
		}

		public static void GenerateNewCard( CardData card )
		{
			new NewCard(
				card.name,
				card.metaCategories.AsEnum( Dicts.MetaCategory ),
				card.cardComplexity == null ? CardComplexity.Vanilla : Dicts.Complexity[ card.cardComplexity ],
				card.temple == null ? CardTemple.Nature : Dicts.Temple[ card.temple ],
				card.displayedName ?? "",
				card.baseAttack, card.baseHealth == 0 ? 1 : card.baseHealth,
				card.description ?? "",
				card.hideAttackAndHealth,
				card.cost, card.bonesCost, card.energyCost,
				card.gemsColour?.AsEnum( Dicts.GemColour ),
				card.specialStatIcon == null ? SpecialStatIcon.None : Dicts.StatIcon[ card.specialStatIcon ],
				card.tribes?.AsEnum( Dicts.Tribes ),
				card.traits?.AsEnum( Dicts.Traits ),
				card.specialAbilities?.AsEnum( Dicts.SpecialAbilities ),
				card.abilities?.AsEnum( Dicts.Abilities ),
				null, //TODO: implement
				card.defaultEvolutionName,
				null, //TODO: implement
				null, //TODO: implement
				card.flipPortraitForStrafe,
				card.onePerDeck,
				card.appearanceBehaviour?.AsEnum( Dicts.AppearanceBehaviour ),
				CardData.LoadTexture2D( (2, 2), card.texture ),
				CardData.LoadTexture2D( (2, 2), card.altTexture ), //TODO: fix sizes
				CardData.LoadTexture2D( (2, 2), card.titleGraphic ),
				CardData.LoadTexture2D( (2, 2), card.pixelTexture ),
				null, //TODO: implement
				card.decals?.AsTextures()
			);
		}
	}

	public static class UtilityExtensions
	{
		public static List<T> AsEnum<T>( this List<string> list, Dictionary<string, T> dict ) where T : struct, IConvertible
		{
			return list.Select( i => dict[i] ).ToList();
		}

		public static List<Texture> AsTextures( this List<string> list )
		{
			List<Texture> textures = new List<Texture>();
			foreach ( string image in list )
			{
				Texture2D tex = new Texture2D( 2, 2 ).WithImage( image );
				textures.Add( tex );
			}

			return textures.Count() == 0 ? null : textures;
		}

		public static Texture2D WithImage( this Texture2D texture, string image )
		{
			byte[] imgBytes = File.ReadAllBytes( Path.Combine( Plugin.ArtPath, image ) );
			texture.LoadImage( imgBytes );
			return texture;
		}
	}

	[HarmonyPatch( typeof( DeckInfo ), "InitializeAsPlayerDeck" )]
	public class DeckInfo_InitializeAsPlayerDeck : DeckInfo
	{
		[HarmonyPrefix]
		public static bool Prefix( ref DeckInfo __instance )
		{
			Plugin p = new Plugin();
			if ( !p.getTestDeck() ) return true;

			List<string> Cards = p.getCards();
			__instance.AddCard( CardLoader.GetCardByName( Cards[0] ) );
			__instance.AddCard( CardLoader.GetCardByName( Cards[1] ) );
			__instance.AddCard( CardLoader.GetCardByName( Cards[2] ) );
			__instance.AddCard( CardLoader.GetCardByName( Cards[3] ) );

			return false;
		}
	}
}
