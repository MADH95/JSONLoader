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

        private string GetUpdatedName(string name, List<CardData> allKnownCards)
        {
            // If the name looks like it has a prefix, just return it as is
            if (string.IsNullOrEmpty(name))
                return name;

            string[] nameSplit = name.Split('_');
            if (nameSplit.Length > 1)
                return name;

            // Okay, lets see if we can find this in the rest of the json cards
            // If we can, it means it's just a bad custom card without a prefix
            
            if (allKnownCards.Exists(c => c != null && !string.IsNullOrEmpty(c.name) && c.name.Equals(name)))
                return $"{CardSerializeInfo.DEFAULT_MOD_PREFIX}_{name}";

            // Well, I guess we'll just assume its one of the game's default cards?
            // Or its some other card we can't identify.
            // Either way, we don't know what to do with it
            return name;
        }

        public CardSerializeInfo ConvertToV2(List<CardData> allKnownCards)
        {
            if (string.IsNullOrEmpty(this.name))
            {
                Plugin.Log.LogError($"I found a JLDR without a name!!");
                return null;
            }

            Plugin.Log.LogDebug($"Converting {this.name} to JLDR2");

            CardSerializeInfo info = new();

            string[] nameSplit = this.name.Split('_');
            if (nameSplit.Length > 1)
            {
                info.modPrefix = nameSplit[0];
                info.name = this.name;
            }
            else
            {
                info.modPrefix = CardSerializeInfo.DEFAULT_MOD_PREFIX;
                info.name = $"{info.modPrefix}_{this.name}";
            }
            
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
            
            info.gemsCost = this.gemsColour?.ToArray();
            info.abilities = Combine(this.abilities, this.customAbilities);
            info.traits = this.traits?.ToArray();
            info.specialAbilities = Combine(this.specialAbilities, this.customSpecialAbilities);
            info.specialStatIcon = this.specialStatIcon;
            info.defaultEvolutionName = GetUpdatedName(this.defaultEvolutionName, allKnownCards);

            if (this.evolution != null)
            {
                info.evolveIntoName = GetUpdatedName(this.evolution.name, allKnownCards);
                info.evolveTurns = this.evolution.turnsToEvolve;
            }

            if (this.tail != null)
            {
                info.tailName = GetUpdatedName(this.tail.name, allKnownCards);
                info.tailLostPortrait = this.tail.tailLostPortrait;
            }

            if (this.iceCube != null)
                info.iceCubeName = GetUpdatedName(this.iceCube.creatureWithin, allKnownCards);
            
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
