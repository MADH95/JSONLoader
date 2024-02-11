/*
using DiskCardGame;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace JLPlugin.SigilCode
{
    [HarmonyPatch]
    public class Display_Name_patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardInfo), nameof(CardInfo.DisplayedNameEnglish), MethodType.Getter)]
        public static bool DisplayedNameEnglish(ref string __result, ref CardInfo __instance)
        {
            string nameReplacement = __instance.displayedName;
            foreach (CardModificationInfo cardModificationInfo in __instance.)
            {
                if (!string.IsNullOrEmpty(cardModificationInfo.nameReplacement))
                {
                    nameReplacement = cardModificationInfo.nameReplacement;
                }
            }
            return nameReplacement;
        }
    }
}
*/
