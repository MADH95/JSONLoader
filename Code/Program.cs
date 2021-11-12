using System;
using System.IO;
using System.Collections.Generic;

using DiskCardGame;

using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;

using HarmonyLib;

using APIPlugin;

namespace JSONLoaderPlugin
{
    [BepInPlugin( PluginGuid, PluginName, PluginVersion )]
    [BepInDependency( "cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency )]
    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "MADH.inscryption.JSONCardLoader";
        private const string PluginName = "JSONCardLoader";
        private const string PluginVersion = "1.3.5.0";

        internal static ManualLogSource Log;

        static readonly string[] artPaths = { Paths.PluginPath, "JSONLoader", "Artwork" };
        static readonly string[] jsonPaths = { Paths.PluginPath, "JSONLoader", "Cards" };

        public static readonly string ArtPath = Path.Combine(artPaths);
        private static readonly string JSONPath = Path.Combine(jsonPaths);
        public bool GetTestDeck() => Config.Bind( "JSONCardLoader", "TestDeck", false, new ConfigDescription( "Load start deck with specified cards" ) ).Value;

        public List<string> GetCards()
        {
            List<string> list = new(4);
            list.Add( Config.Bind( "JSONCardLoader", "Card1", "Wolf" ).Value );
            list.Add( Config.Bind( "JSONCardLoader", "Card2", "Opossum" ).Value );
            list.Add( Config.Bind( "JSONCardLoader", "Card3", "Stoat_Talking" ).Value );
            list.Add( Config.Bind( "JSONCardLoader", "Card4", "Bullfrog" ).Value );
            return list;
        }

        private void Awake()
        {
            Logger.LogInfo( $"Loaded {PluginName}!" );
            Log = base.Logger;

            Harmony harmony = new(PluginGuid);
            harmony.PatchAll();

            foreach ( string file in Directory.EnumerateFiles( JSONPath, "*.json" ) )
            {
                string fileName = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                CardData card = CardData.CreateFromJSON(File.ReadAllText(file));

                if ( card is not null )
                {
                    if ( card.fieldsToEdit.Count > 0 )
                    {
                        Log.LogInfo( $"Editing from { fileName }..." );

                        CardData.EditExistingCard( card );

                        Log.LogInfo( $"Edited from { fileName }!" );

                        continue;
                    }
                    else
                    {
                        Log.LogInfo( $"Loading from { fileName }..." );

                        CardData.GenerateNewCard( card );

                        //Log.LogInfo( $"Loaded from { fileName }!" );

                        continue;
                    }
                }

                Log.LogWarning( $"Failed to load { fileName }" );
            }

            //Load Config
            var hasTestDeck = GetTestDeck();
            var cardList = GetCards();
            if ( hasTestDeck )
            {
                foreach ( string name in cardList )
                {
                    if ( CustomCard.cards.Find( elem => elem.name == name ) is null &&
                         NewCard.cards.Find( elem => elem.name == name ) is null &&
                         ScriptableObjectLoader<CardInfo>.AllData.Find( elem => elem.name == name ) is null )
                    {
                        Logger.LogError( $"Can't find card with name \"{name}\" to add to deck" );
                    }
                }

                Traverse.Create( typeof( ScriptableObjectLoader<CardInfo> ) ).Field( "allData" ).SetValue( null );
            }
        }

        
    }

    #region Patches

    [HarmonyPatch( typeof( DeckInfo ), "InitializeAsPlayerDeck" )]
    public class DeckInfo_InitializeAsPlayerDeck
    {
        [HarmonyPrefix]
        public static bool Prefix( ref DeckInfo __instance )
        {
            Plugin p = new();
            if ( !p.GetTestDeck() )
                return true;

            List<string> Cards = p.GetCards();
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
            JLUtils.ProcessAbilityData();
        }
    }

    [HarmonyPatch( typeof( ChapterSelectMenu ), "OnChapterConfirmed" )]
    public class ChapterSelectMenu_OnChapterConfirmed
    {
        [HarmonyAfter( new string[] { "cyantist.inscryption.api" } )]
        public static void Prefix()
        {
            JLUtils.ProcessAbilityData();
        }
    }
    #endregion
}
