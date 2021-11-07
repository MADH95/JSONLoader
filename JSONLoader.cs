using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

using DiskCardGame;

using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;

using HarmonyLib;

using APIPlugin;

namespace JSONCardParserPlugin
{
	[BepInPlugin( PluginGuid, PluginName, PluginVersion )]
	[BepInDependency( "cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency )]
	public class Plugin : BaseUnityPlugin
	{

		public bool getTestDeck()
		{
			return Config.Bind( "JSONCardLoader", "TestDeck", false, new ConfigDescription( "Load start deck with specified cards" ) ).Value;
		}

		public List<string> getCards()
		{
			List<string> list = new(4);
			list.Add( Config.Bind( "JSONCardLoader", "Card1", "Wolf" ).Value );
			list.Add( Config.Bind( "JSONCardLoader", "Card2", "Opossum" ).Value );
			list.Add( Config.Bind( "JSONCardLoader", "Card3", "Stoat_Talking" ).Value );
			list.Add( Config.Bind( "JSONCardLoader", "Card4", "Bullfrog" ).Value );
			return list;
		}

		private const string PluginGuid = "MADH.inscryption.JSONCardLoader";
		private const string PluginName = "JSONCardLoader";
		private const string PluginVersion = "1.2.0.0";

		internal static ManualLogSource Log;

		static readonly string[] artPaths = { Paths.PluginPath, "JSONLoader", "Artwork" };
		static readonly string[] jsonPaths = { Paths.PluginPath, "JSONLoader", "Cards" };

		public static readonly string ArtPath = Path.Combine(artPaths);
		private static readonly string JSONPath = Path.Combine(jsonPaths);

		private void Awake()
		{
			Logger.LogInfo( $"Loaded {PluginName}!" );
			Log = base.Logger;

			Harmony harmony = new( PluginGuid );
			harmony.PatchAll();

			foreach ( string file in Directory.EnumerateFiles( JSONPath, "*.json" ) )
			{
				string fileName = file.Substring( file.LastIndexOf( Path.DirectorySeparatorChar ) + 1 );

				Log.LogMessage( $"Loading { fileName }..." );

				CardData card = CardData.CreateFromJSON( File.ReadAllText( file ) );

				if ( card is not null )
				{
					CardData.GenerateNewCard( card );
					Log.LogMessage( $"Loaded { fileName }" );
					continue;
				}

				Log.LogWarning( $"Failed to load { fileName }" );
			}

			//Load Config
			var hasTestDeck = getTestDeck();
			var cardList = getCards();
			if ( hasTestDeck )
			{
				foreach ( string name in cardList )
				{
					if ( CustomCard.cards.Find( elem => elem.name == name ) is null &&
						 NewCard.cards.Find( elem => elem.name == name ) is null &&
						 ScriptableObjectLoader<CardInfo>.AllData.Find( elem => elem.name == name ) is null )
					{
						Logger.LogError( $"Can't find card with name \"{name}\"" );
					}
				}

				Traverse.Create( typeof( ScriptableObjectLoader<CardInfo> ) ).Field( "allData" ).SetValue( null );
			}
		}
	}

	public struct Dicts
	{
		public static readonly Dictionary<string, int> Priority = new()
		{
			{ "High",   0 },
			{ "Medium", 1 },
			{ "Low",    2 }
		};

		public static readonly Dictionary<string, CardMetaCategory> MetaCategory
			=   Enum.GetValues( typeof( CardMetaCategory ) )
					.Cast< CardMetaCategory >()
					.ToDictionary( t => t.ToString(), t => t );

		public static readonly Dictionary<string, CardComplexity> Complexity
			=   Enum.GetValues( typeof( CardComplexity ) )
					.Cast< CardComplexity >()
					.ToDictionary( t => t.ToString(), t => t );

		public static readonly Dictionary<string, CardTemple> Temple
			=   Enum.GetValues( typeof( CardTemple ) )
					.Cast< CardTemple >()
					.ToDictionary( t => t.ToString(), t => t );

		public static readonly Dictionary<string, GemType> GemColour
			=   Enum.GetValues( typeof( GemType ) )
					.Cast< GemType >()
					.ToDictionary( t => t.ToString(), t => t );

		public static readonly Dictionary<string, SpecialStatIcon> StatIcon
			=   Enum.GetValues( typeof( SpecialStatIcon ) )
					.Cast< SpecialStatIcon >()
					.ToDictionary( t => t.ToString(), t => t );

		public static readonly Dictionary<string, Tribe> Tribes
			=   Enum.GetValues( typeof( Tribe ) )
					.Cast< Tribe >()
					.ToDictionary( t => t.ToString(), t => t );

		public static readonly Dictionary<string, Trait> Traits
			=   Enum.GetValues( typeof( Trait ) )
					.Cast< Trait >()
					.ToDictionary( t => t.ToString(), t => t );

		public static readonly Dictionary<string, SpecialTriggeredAbility> SpecialAbilities
			=   Enum.GetValues( typeof( SpecialTriggeredAbility ) )
					.Cast< SpecialTriggeredAbility >()
					.ToDictionary( t => t.ToString(), t => t );

		public static readonly Dictionary<string, Ability> Abilities
			=   Enum.GetValues( typeof( Ability ) )
					.Cast< Ability >()
					.ToDictionary( t => t.ToString(), t => t );

		public static readonly Dictionary<string, CardAppearanceBehaviour.Appearance> AppearanceBehaviour
			=   Enum.GetValues( typeof( CardAppearanceBehaviour.Appearance ) )
					.Cast< CardAppearanceBehaviour.Appearance >()
					.ToDictionary( t => t.ToString(), t => t );
	}

	public class EvolveData
	{
		public int turnsToEvolve;
		public string evolution;
	}

	public class TailData
	{
		public string tail;
		public Texture2D tailLostPortrait;
	}

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
		public string evolve_evolutionName;
		public int evolve_turnsToEvolve;
		public string defaultEvolutionName;
		public string tail_cardName;
		public string tail_tailLostPortrait;
		public string iceCube_creatureWithin;
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
			return String.IsNullOrEmpty( image ) ? null : new Texture2D( size.x, size.y ).WithImage( image );
		}

		public static void GenerateNewCard( CardData card )
		{
			NewCard.Add(
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
				defaultEvolutionName: card.defaultEvolutionName,
				flipPortraitForStrafe: card.flipPortraitForStrafe,
				onePerDeck: card.onePerDeck,
				appearanceBehaviour: card.appearanceBehaviour?.AsEnum( Dicts.AppearanceBehaviour ),
				tex: CardData.LoadTexture2D( (2, 2), card.texture ),
				altTex: CardData.LoadTexture2D( (2, 2), card.altTexture ),
				titleGraphic: CardData.LoadTexture2D( (2, 2), card.titleGraphic ),
				pixelTex: CardData.LoadTexture2D( (2, 2), card.pixelTexture ),
				animatedPortrait: null, //TODO: implement
				decals: card.decals?.AsTextures() //TODO: fix sizes?
			);

			EvolveData evolveData = null;
			if ( card.evolve_evolutionName is not null )
			{
				evolveData = new()
				{
					evolution = card.evolve_evolutionName,
					turnsToEvolve = card.evolve_turnsToEvolve
				};

			}

			TailData tailData = null;
			if ( card.tail_cardName is not null )
			{ 

				tailData = new()
				{
					tail = card.tail_cardName,
					tailLostPortrait = LoadTexture2D( (2, 2), card.tail_tailLostPortrait )
				};
			}

			IceCubeData iceCubeData = null;
			if ( card.iceCube_creatureWithin is not null )
			{
				iceCubeData = new()
				{
					creatureWithin = card.iceCube_creatureWithin
				};

			}

			if ( evolveData is not null || tailData is not null || iceCubeData is not null )
				cardsWithParams.Add( card.name, ( evolveData, tailData, iceCubeData ) );
		}

		public static IceCubeParams CreateIceCubeParams( IceCubeData data )
		{
			CardInfo creatureWithin = ScriptableObjectLoader<CardInfo>.AllData.Find( elem => elem.name == data.creatureWithin );

			if ( creatureWithin is null )
				return null;

			IceCubeParams cubeParams = new();
			cubeParams.creatureWithin = creatureWithin;

			return cubeParams;
		}

		public static TailParams CreateTailParams( TailData data, string spriteName )
		{
			CardInfo tail = ScriptableObjectLoader<CardInfo>.AllData.Find( elem => elem.name == data.tail );

			if ( tail is null )
				return null;

			TailParams tailParams = new();
			tailParams.tail = tail;
			tailParams.tailLostPortrait = data.tailLostPortrait.AsSprite( spriteName + "_tailless" );

			return tailParams;
		}

		public static EvolveParams CreateEvolveParams( EvolveData data )
		{
			CardInfo evolution = ScriptableObjectLoader<CardInfo>.AllData.Find( elem => elem.name == data.evolution );

			if ( evolution is null )
				return null;

			EvolveParams evolveParams = new();
			evolveParams.evolution = evolution;
			evolveParams.turnsToEvolve = data.turnsToEvolve;

			return evolveParams;
		}

		public static Dictionary<string, (EvolveData evolveData, TailData tailData, IceCubeData iceCubeData)> cardsWithParams = new();
	}

	public static class UtilityExtensions
	{
		public static List<T> AsEnum<T>( this List<string> list, Dictionary<string, T> dict ) where T : struct, IConvertible
		{
			return list.Select( i => dict[ i ] ).ToList();
		}

		public static List<Texture> AsTextures( this List<string> list )
		{
			List<Texture> textures = new();
			foreach ( string image in list )
			{
				if ( image is not null )
				{
					Texture2D tex = new Texture2D( 2, 2 ).WithImage( image );
					textures.Add( tex );
				}
			}

			return textures.Count == 0 ? null : textures;
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
			Sprite sprite = Sprite.Create( tex, new( 0.0f, 0.0f, tex.width, tex.height ), new( 0.5f, 0.5f ) );
			sprite.name = "portrait_" + name;

			return sprite;
		}
	}

	[HarmonyPatch( typeof( DeckInfo ), "InitializeAsPlayerDeck" )]
	public class DeckInfo_InitializeAsPlayerDeck
	{
		[HarmonyPrefix]
		public static bool Prefix( ref DeckInfo __instance )
		{
			Plugin p = new();
			if ( !p.getTestDeck() )
				return true;

			List<string> Cards = p.getCards();
			__instance.AddCard( CardLoader.GetCardByName( Cards[ 0 ] ) );
			__instance.AddCard( CardLoader.GetCardByName( Cards[ 1 ] ) );
			__instance.AddCard( CardLoader.GetCardByName( Cards[ 2 ] ) );
			__instance.AddCard( CardLoader.GetCardByName( Cards[ 3 ] ) );

			return false;
		}
	}

	[HarmonyPatch( typeof( LoadingScreenManager ), "LoadGameData" )]
	public class LoadingScreenManager_LoadGameData
	{
		[HarmonyAfter( new string[] { "cyantist.inscryption.api" } )]
		public static void Prefix()
		{
			var allCards = ScriptableObjectLoader<CardInfo>.AllData;

			foreach ( var cardToMod in CardData.cardsWithParams.ToList() )
			{
				CardInfo card = allCards.Find( elem => elem.name == cardToMod.Key );

				if ( cardToMod.Value.evolveData is not null  )
				{
					if ( allCards.Find( elem => elem.name == cardToMod.Value.evolveData.evolution ) is null )
					{
						Plugin.Log.LogError( $"EvolveParams - No card named \"{cardToMod.Value.evolveData.evolution}\" exists" );
						goto SkipEvolve;
					}

					card.evolveParams = CardData.CreateEvolveParams( cardToMod.Value.evolveData );
					Plugin.Log.LogMessage( $"EvolveParams - \"{cardToMod.Value.evolveData.evolution}\" assigned to \"{card.name}\"" );
				}

			SkipEvolve:

				if ( cardToMod.Value.tailData is not null )
				{
					if ( allCards.Find( elem => elem.name == cardToMod.Value.tailData.tail ) is null )
					{
						Plugin.Log.LogError( $"TailParams - No card named \"{cardToMod.Value.tailData.tail}\" exists" );
						goto SkipTail;
					}

					card.tailParams = CardData.CreateTailParams( cardToMod.Value.tailData, cardToMod.Key );
					Plugin.Log.LogMessage( $"TailParams - \"{cardToMod.Value.tailData.tail}\" assigned to \"{card.name}\"" );
				}

			SkipTail:

				if ( cardToMod.Value.iceCubeData is not null )
				{
					if ( allCards.Find( elem => elem.name == cardToMod.Value.iceCubeData.creatureWithin ) is null )
					{
						Plugin.Log.LogError( $"IceCubeParams - No card named \"{cardToMod.Value.iceCubeData.creatureWithin}\" exists" );
						goto SkipIceCube;
					}

					card.iceCubeParams = CardData.CreateIceCubeParams( cardToMod.Value.iceCubeData );
					Plugin.Log.LogMessage( $"IceCubeParams - \"{cardToMod.Value.iceCubeData.creatureWithin}\" assigned to \"{card.name}\"" );
				}

			SkipIceCube:
				continue;
			}

			CardData.cardsWithParams.Clear();
		}
	}

	[HarmonyPatch( typeof( ChapterSelectMenu ), "OnChapterConfirmed" )]
	public class ChapterSelectMenu_OnChapterConfirmed
	{
		[HarmonyAfter( new string[] { "cyantist.inscryption.api" } )]
		public static void Prefix()
		{
			var allCards = ScriptableObjectLoader<CardInfo>.AllData;

			foreach ( var cardToMod in CardData.cardsWithParams.ToList() )
			{
				CardInfo card = allCards.Find( elem => elem.name == cardToMod.Key );

				if ( cardToMod.Value.evolveData != null )
				{
					if ( allCards.Find( elem => elem.name == cardToMod.Value.evolveData.evolution ) is null )
					{
						Plugin.Log.LogError( $"EvolveParams - No card named \"{cardToMod.Value.evolveData.evolution}\" exists" );
						goto SkipEvolve;
					}

					card.evolveParams = CardData.CreateEvolveParams( cardToMod.Value.evolveData );
					Plugin.Log.LogMessage( $"EvolveParams - \"{cardToMod.Value.evolveData.evolution}\" assigned to \"{card.name}\"" );
				}

			SkipEvolve:

				if ( cardToMod.Value.tailData is not null )
				{
					if ( allCards.Find( elem => elem.name == cardToMod.Value.tailData.tail ) is null )
					{
						Plugin.Log.LogError( $"TailParams - No card named \"{cardToMod.Value.tailData.tail}\" exists" );
						goto SkipTail;
					}

					card.tailParams = CardData.CreateTailParams( cardToMod.Value.tailData, cardToMod.Key );
					Plugin.Log.LogMessage( $"TailParams - \"{cardToMod.Value.tailData.tail}\" assigned to \"{card.name}\"" );
				}

			SkipTail:

				if ( cardToMod.Value.iceCubeData is not null )
				{
					if ( allCards.Find( elem => elem.name == cardToMod.Value.iceCubeData.creatureWithin ) is null )
					{
						Plugin.Log.LogError( $"IceCubeParams - No card named \"{cardToMod.Value.iceCubeData.creatureWithin}\" exists" );
						goto SkipIceCube;
					}

					card.iceCubeParams = CardData.CreateIceCubeParams( cardToMod.Value.iceCubeData );
					Plugin.Log.LogMessage( $"IceCubeParams - \"{cardToMod.Value.iceCubeData.creatureWithin}\" assigned to \"{card.name}\"" );
				}

			SkipIceCube:
				continue;
			}

			CardData.cardsWithParams.Clear();
		}
	}
}
