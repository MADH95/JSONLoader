// Using Inscryption
using DiskCardGame;
using HarmonyLib;
using JLPlugin.Data;
// Modding Inscryption
using System;

namespace JLPlugin.SigilCode
{

    [HarmonyPatch(typeof(CardTriggerHandler), "AddAbility", new Type[]
    {
        typeof(SpecialTriggeredAbility)
    })]
    public class Add__Special_Ability_patch
    {
        [HarmonyPrefix]
        public static bool Prefix(SpecialTriggeredAbility ability, CardTriggerHandler __instance)
        {
            if (!SigilDicts.SpecialArgumentList.ContainsKey(ability))
            {
                return true;
            }

            Type SigilType = SigilDicts.SpecialArgumentList[ability].Item1;

            if (!__instance.specialAbilities.Exists((Tuple<SpecialTriggeredAbility, SpecialCardBehaviour> x) => x.Item1 == ability))
            {
                ConfigurableSpecialBase specialConfigil = __instance.gameObject.AddComponent(SigilType) as ConfigurableSpecialBase;
                specialConfigil.abilityData = SigilData.GetAbilityArguments(ability);
                __instance.specialAbilities.Add(new Tuple<SpecialTriggeredAbility, SpecialCardBehaviour>(ability, specialConfigil));
            }
            return false;
        }
    }
}