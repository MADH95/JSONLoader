using System.Collections.Generic;

using DiskCardGame;

using HarmonyLib;

namespace JLPlugin
{

    [HarmonyPatch( typeof( DeckInfo ), "InitializeAsPlayerDeck" )]
    public class TestDeck
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
}