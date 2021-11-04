using APIPlugin;

using BepInEx;
using BepInEx.Logging;

using DiskCardGame;

using HarmonyLib;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

namespace JSONCardParserPlugin
{
	[BepInPlugin( PluginGuid, PluginName, PluginVersion )]
	[BepInDependency( "cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency )]
	public class Plugin : BaseUnityPlugin
	{
		private const string PluginGuid = "MADH.inscryption.JSONCardParser";
		private const string PluginName = "JSONCardParser";
		private const string PluginVersion = "1.0.0.0";

		internal static ManualLogSource Log;

		static readonly string[] artPaths = { Paths.PluginPath, "CardLoader", "Artwork" };
		static readonly string[] jsonPaths = { Paths.PluginPath, "CardLoader", "Cards" };

		public static readonly string ArtPath = Path.Combine(artPaths);
		private static readonly string JSONPath = Path.Combine(jsonPaths);

		private void Awake()
		{
			Logger.LogInfo( $"Loaded {PluginName}!" );
			Log = base.Logger;

			Harmony harmony = new Harmony( PluginGuid );
			harmony.PatchAll();


			Logger.LogInfo( "Loading cards from JSON..." );

			foreach ( string file in Directory.EnumerateFiles( JSONPath, "*.json" ) )
			{
				CardData card = CardData.CreateFromJSON( File.ReadAllText( file ) );

				CardData.GenerateNewCard( card );

				Log.LogInfo( $"Loaded { file.Substring( file.LastIndexOf( '/' ) + 1 ) }" );
			}
		}
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
		public int priority;
		public string name, displayedName, description;
		public List<int> metaCategories;
		public int cardComplexity;
		public int temple;
		public int baseAttack;
		public int baseHealth;
		public bool hideAttackAndHealth;
		public int cost, bonesCost, energyCost;
		public List<int> gemsCost;
		public int specialStatIcon;
		public List<int> tribes;
		public List<int> traits;
		public List<int> specialAbilities;
		public List<int> abilities;
		public EvolveData evolveParams;
		public string defaultEvolutionName;
		public TailData tailParams;
		public IceCubeData iceCubeParams;
		public bool flipPortraitForStrafe;
		public bool onePerDeck;
		public List<int> appearanceBehaviour;
		public string texture, altTexture;
		public string titleGraphic;
		public string pixelTexture;
		public string animatedPortrait;
		public List<string> decals;

		public static CardData CreateFromJSON( string jsonString )
		{
			return JsonUtility.FromJson<CardData>( jsonString );
		}

		public static Texture2D LoadTexture2D( string image )
		{
			return image == null ? null : new Texture2D( 2, 2 ).WithImage( image );
		}

		public static void GenerateNewCard( CardData card )
		{
			new NewCard(
				card.name,
				card.metaCategories.AsEnum<CardMetaCategory>(),
				(CardComplexity)card.cardComplexity,
				(CardTemple)card.temple,
				card.displayedName ?? "",
				card.baseAttack, card.baseHealth == 0 ? 1 : card.baseHealth,
				card.description ?? "",
				card.hideAttackAndHealth,
				card.cost, card.bonesCost,
				card.energyCost,
				card.gemsCost.AsEnum<GemType>(),
				(SpecialStatIcon)card.specialStatIcon,
				card.tribes.AsEnum<Tribe>(),
				card.traits.AsEnum<Trait>(),
				card.specialAbilities.AsEnum<SpecialTriggeredAbility>(),
				card.abilities.AsEnum<Ability>(),
				null, //TODO
				card.defaultEvolutionName,
				null, //TODO
				null, //TODO
				card.flipPortraitForStrafe,
				card.onePerDeck,
				card.appearanceBehaviour.AsEnum<CardAppearanceBehaviour.Appearance>(),
				CardData.LoadTexture2D( card.texture ),
				CardData.LoadTexture2D( card.altTexture ),
				CardData.LoadTexture2D( card.titleGraphic ),
				CardData.LoadTexture2D( card.pixelTexture ),
				null, //TODO
				card.decals.AsTextures()
			);
		}
	}

	public static class UtilityExtensions
	{
		public static List<T> AsEnum<T>( this List<int> list ) where T : struct, IConvertible
		{
			return list.Select( i => (T)(object)i ).ToList();
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
}
