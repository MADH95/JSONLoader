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
        typeof(Ability)
    })]
    public class Add_Ability_patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Ability ability, CardTriggerHandler __instance)
        {
            if (!SigilDicts.ArgumentList.ContainsKey(ability))
            {
                return true;
            }

            Type SigilType = SigilDicts.ArgumentList[ability].Item1;

            if ((!__instance.triggeredAbilities.Exists((Tuple<Ability, AbilityBehaviour> x) => x.Item1 == ability) || AbilitiesUtil.GetInfo(ability).canStack) && !AbilitiesUtil.GetInfo(ability).passive)
            {
                ConfigurableBase configil = __instance.gameObject.AddComponent(SigilType) as ConfigurableBase;
                configil.abilityData = SigilData.GetAbilityArguments(ability);
                configil.ability = ability;
                __instance.triggeredAbilities.Add(new Tuple<Ability, AbilityBehaviour>(ability, (configil as ActivatedAbilityBehaviour)));
            }
            return false;
        }
    }
}