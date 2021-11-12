using DiskCardGame;

using HarmonyLib;

namespace JLPlugin
{
    using Utils;

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
}