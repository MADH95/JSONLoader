using System.Collections.Generic;
using System;
using JLPlugin.V2.Data;
using DiskCardGame;

namespace JLPlugin.Data
{
    using System.Linq;
    using Utils;

    public partial class CardData
    {
        private static string[] Combine(List<string> normal, List<AbilityData> custom)
        {
            List<string> retval = new();
            if (normal != null)
                retval.AddRange(normal);
            if (custom != null)
                retval.AddRange(custom.Select(x => $"{x.GUID}.{x.name}"));
            return retval.ToArray();
        }

        private static string[] Combine(List<string> normal, List<SpecialAbilityData> custom)
        {
            List<string> retval = new();
            if (normal != null)
                retval.AddRange(normal);
            if (custom != null)
                retval.AddRange(custom.Select(x => $"{x.GUID}.{x.name}"));
            return retval.ToArray();
        }

        public CardSerializeInfo ConvertToV2()
        {
            if (string.IsNullOrEmpty(this.name))
            {
                Plugin.Log.LogError($"I found a JLDR without a name!!");
                return null;
            }

            Plugin.Log.LogDebug($"Converting {this.name} to JLDR2");

            CardSerializeInfo info = new();

            string[] nameSplit = this.name.Split('_');

            if (name.Length == 1)
            {
                info.name = this.name;
            }
            else
            {
                info.modPrefix = nameSplit[0];
                info.name = this.name.Replace($"{nameSplit[0]}_", "");
            }

            info.name = this.name;
            info.displayedName = this.displayedName;
            info.description = this.description;
            info.metaCategories = this.metaCategories?.ToArray();
            info.cardComplexity = this.cardComplexity;
            info.temple = this.temple;
            info.tribes = this.tribes?.ToArray();

            var testFieldsToEdit = this.fieldsToEdit ?? new List<string>();

            if (this.baseAttack > 0 || testFieldsToEdit.Exists(f => f.Equals("baseAttack", StringComparison.OrdinalIgnoreCase)))
                info.baseAttack = this.baseAttack;

            if (this.baseHealth > 0 || testFieldsToEdit.Exists(f => f.Equals("baseHealth", StringComparison.OrdinalIgnoreCase)))
                info.baseHealth = this.baseHealth;

            if (this.hideAttackAndHealth || testFieldsToEdit.Exists(f => f.Equals("hideAttackAndHealth", StringComparison.OrdinalIgnoreCase)))
                info.hideAttackAndHealth = this.hideAttackAndHealth;

            if (this.bloodCost > 0 || testFieldsToEdit.Exists(f => f.Equals("bloodcost", StringComparison.OrdinalIgnoreCase)))
                info.bloodCost = this.bloodCost;

            if (this.bonesCost > 0 || testFieldsToEdit.Exists(f => f.Equals("bonesCost", StringComparison.OrdinalIgnoreCase)))
                info.bonesCost = this.bonesCost;

            if (this.bonesCost > 0 || testFieldsToEdit.Exists(f => f.Equals("energyCost", StringComparison.OrdinalIgnoreCase)))
                info.energyCost = this.energyCost;
            
            info.gemsColour = this.gemsColour?.ToArray();
            info.abilities = Combine(this.abilities, this.customAbilities);
            info.traits = this.traits?.ToArray();
            info.specialAbilities = Combine(this.specialAbilities, this.customSpecialAbilities);
            info.specialStatIcon = this.specialStatIcon;

            if (this.evolution != null)
            {
                info.evolveIntoName = this.evolution.name;
                info.evolveTurns = this.evolution.turnsToEvolve;
            }

            if (this.tail != null)
            {
                info.tailName = this.tail.name;
                info.tailLostPortrait = this.tail.tailLostPortrait;
            }

            if (this.iceCube != null)
                info.iceCubeName = this.iceCube.creatureWithin;
            
            if (this.flipPortraitForStrafe || testFieldsToEdit.Exists(f => f.Equals("flipPortraitForStrafe", StringComparison.OrdinalIgnoreCase)))
                info.flipPortraitForStrafe = this.flipPortraitForStrafe;
            
            if (this.onePerDeck || testFieldsToEdit.Exists(f => f.Equals("onePerDeck", StringComparison.OrdinalIgnoreCase)))
                info.onePerDeck = this.onePerDeck;
            
            info.appearanceBehaviour = this.appearanceBehaviour?.ToArray();
            info.texture = this.texture;
            info.altTexture = this.altTexture;
            info.titleGraphic = this.titleGraphic;
            info.pixelTexture = this.pixelTexture;
            info.animatedPortrait = this.animatedPortrait;
            info.emissionTexture = this.emissionTexture;
            info.decals = this.decals?.ToArray();

            return info;
        }
    }
}
