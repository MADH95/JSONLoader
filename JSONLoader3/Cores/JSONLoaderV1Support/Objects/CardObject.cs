using System;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using JSONLoader3.Cores.JSONLoaderV1Support.Utilities;
using JSONLoader3.Peripheral.FILE_Loader;
using JSONLoader3.Peripheral.ImageHandling;
using Sirenix.Utilities;
using UnityEngine;

namespace JSONLoader3.Cores.JSONLoaderV1Support.Objects;

/// <summary>
/// An Object representing an <see cref="JSONLoaderV1Support.Schemas.Card"/>.
/// </summary>
/// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
public class CardObject
{
    /// <summary>
    /// A List of fields to Overwrite in the case the Card's Name belongs to the base game.
    /// </summary>
    public List<string> fieldsToEdit { get; set; }

    /// <summary>
    /// The cards In-Code name.
    /// </summary>
    public string name { get; set; }

    /// <summary>
    /// The cards In-Game name.
    /// </summary>
    public string displayedName { get; set; }

    /// <summary>
    /// The Description of the card.
    /// </summary>
    public string description { get; set; }

    /// <summary>
    /// A List of all Meta Categories the card has.
    /// </summary>
    public List<string> metaCategories { get; set; }

    /// <summary>
    /// The Complexity of the Card.
    /// </summary>
    public string cardComplexity { get; set; }

    /// <summary>
    /// The Card's Temple.
    /// </summary>
    public string temple { get; set; }

    /// <summary>
    /// An Int determining the cards BaseAttack.
    /// </summary>
    public int baseAttack { get; set; }

    /// <summary>
    /// An Int determining the cards BaseHealth.
    /// </summary>
    public int baseHealth { get; set; }

    /// <summary>
    /// A Boolean determining whether the Attack and Health are hidden or not.
    /// </summary>
    public bool hideAttackAndHealth { get; set; }

    /// <summary>
    /// An Int representing the Blood Cost of the card.
    /// </summary>
    public int bloodCost { get; set; }

    /// <summary>
    /// An Int representing the Bone Cost of the card.
    /// </summary>
    public int bonesCost { get; set; }

    /// <summary>
    /// An Int representing the Energy Cost of the card.
    /// </summary>
    public int energyCost { get; set; }

    /// <summary>
    /// A List representing all the Gem Colors applied to the card.
    /// </summary>
    public List<string> gemColors { get; set; }

    /// <summary>
    /// A string representing what Stat Icon to apply to the card.
    /// </summary>
    public string specialStatIcon { get; set; }

    /// <summary>
    /// A List of all Tribes that the card has.
    /// </summary>
    public List<string> tribes { get; set; }

    /// <summary>
    /// A List of all Traits that the card has.
    /// </summary>
    public List<string> traits { get; set; }

    /// <summary>
    /// A List of all Special Abilities that the card has.
    /// </summary>
    public List<string> specialAbilities { get; set; }

    /// <summary>
    /// A List of all Abilities that the card has.
    /// </summary>
    public List<string> abilities { get; set; }

    /// <summary>
    /// A List of the Modded Abilities that the card has.
    /// </summary>
    public List<AbilityData> customAbilities { get; set; }

    /// <summary>
    /// A List of the Modded Special Abilities that the card has.
    /// </summary>
    public List<SpecialAbilityData> customSpecialAbilities { get; set; }

    /// <summary>
    /// The Evolve Ability Related Parameters.
    /// </summary>
    public EvolveData evolution { get; set; }

    /// <summary>
    /// The Default Evolution Name for the Card if it has no Evolve Ability Related Parameters.
    /// </summary>
    public string defaultEvolutionName { get; set; }

    /// <summary>
    /// The LooseTail Ability Related Parameters.
    /// </summary>
    public TailData tail { get; set; }

    /// <summary>
    /// The IceCube Ability related Parameters.
    /// </summary>
    public IceCubeData iceCube { get; set; }

    /// <summary>
    /// A boolean determining whether the Portrait should Flip on Strafe.
    /// </summary>
    public bool flipPortraitForStrafe { get; set; }

    /// <summary>
    /// A boolean determining if only one version of the card is allowed in the deck or not.
    /// </summary>
    public bool onePerDeck { get; set; }

    /// <summary>
    /// A List of AppearanceBehaviours to apply to the card.
    /// </summary>
    public List<string> appearanceBehaviour { get; set; }

    /// <summary>
    /// The Card's Portrait.
    /// </summary>
    public string texture { get; set; }

    /// <summary>
    /// The Alternate Version of the Card's Portrait.
    /// </summary>
    public string altTexture { get; set; }

    /// <summary>
    /// The Emission Version of the Card's Portrait.
    /// </summary>
    public string emissionTexture { get; set; }

    /// <summary>
    /// The Title Graphic of the Card, this appears overlayed on the card's name.
    /// </summary>
    public string titleGraphic { get; set; }

    /// <summary>
    /// The Act 2 Version of the Card's Portrait.
    /// </summary>
    public string pixelTexture { get; set; }

    /// <summary>
    /// A list of all Decal paths to be on the card.
    /// </summary>
    public List<string> decals { get; set; }

    /// <summary>
    /// The Internal full path to the File the card came from.
    /// </summary>
    internal string file { get; set; }

    /// <summary>
    /// The Internal full path to the Plugin the card came from.
    /// </summary>
    internal string pluginName { get; set; }

    /// <summary>
    /// The Constructor for making Card Objects.
    /// </summary>
    /// <param name="fieldsToEdit"><see cref="fieldsToEdit"/></param>
    /// <param name="name"><see cref="name"/></param>
    /// <param name="displayedName"><see cref="displayedName"/></param>
    /// <param name="description"><see cref="description"/></param>
    /// <param name="metaCategories"><see cref="metaCategories"/></param>
    /// <param name="cardComplexity"><see cref="cardComplexity"/></param>
    /// <param name="temple"><see cref="temple"/></param>
    /// <param name="baseAttack"><see cref="baseAttack"/></param>
    /// <param name="baseHealth"><see cref="baseHealth"/></param>
    /// <param name="hideAttackAndHealth"><see cref="hideAttackAndHealth"/></param>
    /// <param name="bloodCost"><see cref="bloodCost"/></param>
    /// <param name="bonesCost"><see cref="bonesCost"/></param>
    /// <param name="energyCost"><see cref="energyCost"/></param>
    /// <param name="gemColors"><see cref="gemColors"/></param>
    /// <param name="specialStatIcon"><see cref="specialStatIcon"/></param>
    /// <param name="tribes"><see cref="tribes"/></param>
    /// <param name="traits"><see cref="traits"/></param>
    /// <param name="specialAbilities"><see cref="specialAbilities"/></param>
    /// <param name="abilities"><see cref="abilities"/></param>
    /// <param name="customAbilities"><see cref="customAbilities"/></param>
    /// <param name="customSpecialAbilities"><see cref="customSpecialAbilities"/></param>
    /// <param name="evolution"><see cref="evolution"/></param>
    /// <param name="defaultEvolutionName"><see cref="defaultEvolutionName"/></param>
    /// <param name="tail"><see cref="tail"/></param>
    /// <param name="iceCube"><see cref="iceCube"/></param>
    /// <param name="flipPortraitForStrafe"><see cref="flipPortraitForStrafe"/></param>
    /// <param name="onePerDeck"><see cref="onePerDeck"/></param>
    /// <param name="appearanceBehaviour"><see cref="appearanceBehaviour"/></param>
    /// <param name="texture"><see cref="texture"/></param>
    /// <param name="altTexture"><see cref="altTexture"/></param>
    /// <param name="emissionTexture"><see cref="emissionTexture"/></param>
    /// <param name="titleGraphic"><see cref="titleGraphic"/></param>
    /// <param name="pixelTexture"><see cref="pixelTexture"/></param>
    /// <param name="decals"><see cref="decals"/></param>
    /// <param name="file"><see cref="file"/></param>
    /// <param name="pluginName"><see cref="pluginName"/></param>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public CardObject(
        List<string> fieldsToEdit,
        string name,
        string displayedName = "",
        string description = "",
        List<string> metaCategories = null,
        string cardComplexity = "Vanilla",
        string temple = "Nature",
        int baseAttack = 0,
        int baseHealth = 1,
        bool hideAttackAndHealth = false,
        int bloodCost = 0,
        int bonesCost = 0,
        int energyCost = 0,
        List<string> gemColors = null,
        string specialStatIcon = "",
        List<string> tribes = null,
        List<string> traits = null,
        List<string> specialAbilities = null,
        List<string> abilities = null,
        List<AbilityData> customAbilities = null,
        List<SpecialAbilityData> customSpecialAbilities = null,
        EvolveData evolution = null,
        string defaultEvolutionName = "",
        TailData tail = null,
        IceCubeData iceCube = null,
        bool flipPortraitForStrafe = false,
        bool onePerDeck = false,
        List<string> appearanceBehaviour = null,
        string texture = "",
        string altTexture = "",
        string emissionTexture = "",
        string titleGraphic = "",
        string pixelTexture = "",
        List<string> decals = null,
        string file = null,
        string pluginName = null)
    {
        this.fieldsToEdit = fieldsToEdit;
        this.name = JSONLoader3.PluginGuid + "_" + name;
        this.displayedName = displayedName;
        this.description = description;
        this.metaCategories = metaCategories;
        this.cardComplexity = cardComplexity;
        this.temple = temple;
        this.baseAttack = baseAttack;
        this.baseHealth = baseHealth;
        this.hideAttackAndHealth = hideAttackAndHealth;
        this.bloodCost = bloodCost;
        this.bonesCost = bonesCost;
        this.energyCost = energyCost;
        this.gemColors = gemColors;
        this.specialStatIcon = specialStatIcon;
        this.tribes = tribes;
        this.traits = traits;
        this.specialAbilities = specialAbilities;
        this.abilities = abilities;
        this.customAbilities = customAbilities;
        this.customSpecialAbilities = customSpecialAbilities;
        this.evolution = evolution;
        this.defaultEvolutionName = defaultEvolutionName;
        this.tail = tail;
        this.iceCube = iceCube;
        this.flipPortraitForStrafe = flipPortraitForStrafe;
        this.onePerDeck = onePerDeck;
        this.appearanceBehaviour = appearanceBehaviour;
        this.texture = texture;
        this.altTexture = altTexture;
        this.emissionTexture = emissionTexture;
        this.titleGraphic = titleGraphic;
        this.pixelTexture = pixelTexture;
        this.decals = decals;
        this.file = file;
        this.pluginName = pluginName;
    }

    /// <summary>
    /// Converts the given CardObject into a CardInfo and adds it automatically via the API.
    /// </summary>
    /// <returns>A CardInfo representing the card passed in.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public CardInfo ConvertCardObjectToCardInfo()
    {
        CardInfo info = null;
        if (!fieldsToEdit.IsNullOrEmpty())
        {
            CardInfo infoTemp = CardManager.AllCardsCopy.FirstOrDefault(x => x.name == name.Replace(JSONLoader3.PluginGuid + "_", ""));
            if (infoTemp != null)
            {
                info = infoTemp;
        
                if (fieldsToEdit.Contains("displayedName"))
                {
                    info.displayedName = displayedName;
                }
        
                if (fieldsToEdit.Contains("description"))
                {
                    info.description = description;
                }
        
                if (fieldsToEdit.Contains("metaCategories"))
                {
                    if (metaCategories != null) 
                        info.metaCategories = metaCategories.Where(x => !string.IsNullOrEmpty(x)).Select(x => (CardMetaCategory)Enum.Parse(typeof(CardMetaCategory), x)).ToList();
                }
        
                if (fieldsToEdit.Contains("cardComplexity"))
                {
                    if (!string.IsNullOrEmpty(cardComplexity))
                        info.cardComplexity = (CardComplexity)Enum.Parse(typeof(CardComplexity), cardComplexity);
                }
        
                if (fieldsToEdit.Contains("temple"))
                {
                    if (!string.IsNullOrEmpty(temple))
                        info.temple = (CardTemple)Enum.Parse(typeof(CardTemple), temple);
                }
        
                if (fieldsToEdit.Contains("baseAttack"))
                {
                    info.baseAttack = baseAttack;
                }
        
                if (fieldsToEdit.Contains("baseHealth"))
                {
                    info.baseHealth = baseHealth;
                }
        
                if (fieldsToEdit.Contains("hideAttackAndHealth"))
                {
                    info.hideAttackAndHealth = hideAttackAndHealth;
                }
        
                if (fieldsToEdit.Contains("bloodCost"))
                {
                    info.cost = bloodCost;
                }
        
                if (fieldsToEdit.Contains("bonesCost"))
                {
                    info.bonesCost = bonesCost;
                }
        
                if (fieldsToEdit.Contains("energyCost"))
                {
                    info.energyCost = energyCost;
                }
        
                if (fieldsToEdit.Contains("gemColors"))
                {
                    if (gemColors != null)
                        info.gemsCost = gemColors.Where(x => !string.IsNullOrEmpty(x)).Select(x => (GemType)Enum.Parse(typeof(GemType), x)).ToList();
                }
        
                if (fieldsToEdit.Contains("specialStatIcon"))
                {
                    if (!string.IsNullOrEmpty(specialStatIcon))
                        info.specialStatIcon = (SpecialStatIcon)Enum.Parse(typeof(SpecialStatIcon), specialStatIcon);
                }
        
                if (fieldsToEdit.Contains("tribes"))
                {
                    if (tribes != null)
                        info.tribes = tribes.Where(x => !string.IsNullOrEmpty(x)).Select(x => (Tribe)Enum.Parse(typeof(Tribe), x)).ToList();
                }
        
                if (fieldsToEdit.Contains("traits"))
                {
                    if (traits != null)
                        info.traits = traits.Where(x => !string.IsNullOrEmpty(x)).Select(x => (Trait)Enum.Parse(typeof(Trait), x)).ToList();
                }
        
                if (fieldsToEdit.Contains("specialAbilities"))
                {
                    if (specialAbilities != null)
                        info.specialAbilities = specialAbilities.Where(x => !string.IsNullOrEmpty(x)).Select(x => (SpecialTriggeredAbility)Enum.Parse(typeof(SpecialTriggeredAbility), x)).ToList();
                }
        
                if (fieldsToEdit.Contains("abilities"))
                {
                    if (abilities != null)
                        info.abilities = abilities.Where(x => !string.IsNullOrEmpty(x)).Select(x => (Ability)Enum.Parse(typeof(Ability), x)).ToList();
                }
        
                if (fieldsToEdit.Contains("customAbilities"))
                {
                    if (customAbilities != null)
                    {
                        foreach (AbilityData data in customAbilities)
                        {
                            info.AddAbilities(data.GetAbility());
                        }
                    }
                }
        
                if (fieldsToEdit.Contains("customSpecialAbilities"))
                {
                    if (customSpecialAbilities != null)
                    {
                        foreach (SpecialAbilityData data in customSpecialAbilities)
                        {
                            info.AddSpecialAbilities(data.GetSpecialAbility());
                        }
                    }
                }
        
                if (fieldsToEdit.Contains("evolution"))
                {
                    if (evolution != null)
                    {
                        if(CardUtils.allJLDRCardsPublic.FirstOrDefault(x => x.name.Replace(JSONLoader3.PluginGuid + "_", "") == evolution.name.Replace(JSONLoader3.PluginGuid + "_", "")) != null) 
                        {
                            info.SetEvolve(JSONLoader3.PluginGuid + "_" + evolution.name, evolution.turnsToEvolve);
                        }
                        else
                        {
                            info.SetEvolve(evolution.name, evolution.turnsToEvolve);
                        }
                    }
                }
        
                if (fieldsToEdit.Contains("defaultEvolutionName"))
                {
                    if (!string.IsNullOrEmpty(defaultEvolutionName))
                        info.defaultEvolutionName = defaultEvolutionName;
                }
        
                if (fieldsToEdit.Contains("tail"))
                {
                    if (tail != null)
                    {
                        if(CardUtils.allJLDRCardsPublic.FirstOrDefault(x => x.name.Replace(JSONLoader3.PluginGuid + "_", "") == tail.name.Replace(JSONLoader3.PluginGuid + "_", "")) != null) 
                        {
                            info.SetTail(JSONLoader3.PluginGuid + "_" + tail.name, tail.tailLostPortrait);
                        }
                        else
                        {
                            info.SetTail(tail.name, tail.tailLostPortrait);
                        }
                    }
                }
        
                if (fieldsToEdit.Contains("iceCube"))
                {
                    if (iceCube != null)
                    {
                        if(CardUtils.allJLDRCardsPublic.FirstOrDefault(x => x.name.Replace(JSONLoader3.PluginGuid + "_", "") == iceCube.creatureWithin.Replace(JSONLoader3.PluginGuid + "_", "")) != null) 
                        {
                            info.SetIceCube(JSONLoader3.PluginGuid + "_" + iceCube.creatureWithin);
                        }
                        else
                        {
                            info.SetIceCube(iceCube.creatureWithin);
                        }
                    }
                }
        
                if (fieldsToEdit.Contains("flipPortraitForStrafe"))
                {
                    info.flipPortraitForStrafe = flipPortraitForStrafe;
                }
        
                if (fieldsToEdit.Contains("onePerDeck"))
                {
                    info.onePerDeck = onePerDeck;
                }
        
                if (fieldsToEdit.Contains("appearanceBehaviour"))
                {
                    if (appearanceBehaviour != null)
                        info.appearanceBehaviour = appearanceBehaviour.Where(x => !string.IsNullOrEmpty(x)).Select(x => (CardAppearanceBehaviour.Appearance)Enum.Parse(typeof(CardAppearanceBehaviour.Appearance), x)).ToList();
                }
                
                if (fieldsToEdit.Contains("texture") && !string.IsNullOrEmpty(texture))
                {
                    Sprite Portrait = ScanImages.ParseImage(pluginName, texture);
    
                    if (Portrait != null)
                        info.SetPortrait(Portrait);
                }
    
                if (fieldsToEdit.Contains("altTexture") && !string.IsNullOrEmpty(altTexture))
                {
                    Sprite alternatePortrait = ScanImages.ParseImage(pluginName, altTexture);
    
                    if (alternatePortrait != null)
                        info.SetAltPortrait(alternatePortrait);
                }
    
                if (fieldsToEdit.Contains("emissionTexture") && !string.IsNullOrEmpty(emissionTexture))
                {
                    Sprite emissionPortrait = ScanImages.ParseImage(pluginName, emissionTexture);
    
                    if (emissionPortrait != null)
                        info.SetEmissivePortrait(emissionPortrait);
                }
    
                if (fieldsToEdit.Contains("titleGraphic") && !string.IsNullOrEmpty(titleGraphic))
                {
                    Sprite titleGraphicSprite = ScanImages.ParseImage(pluginName, titleGraphic);
    
                    if (titleGraphicSprite != null)
                        info.titleGraphic = titleGraphicSprite.texture;
                }
    
                if (fieldsToEdit.Contains("pixelTexture") && !string.IsNullOrEmpty(pixelTexture))
                {
                    Sprite pixelPortrait = ScanImages.ParseImage(pluginName, pixelTexture);
    
                    if (pixelPortrait != null)
                        info.SetPixelPortrait(pixelPortrait);
                }
    
                if (fieldsToEdit.Contains("decals") && decals != null)
                {
                    info.decals = new List<Texture>();
    
                    foreach (string texture2 in decals)
                    {
                        if (!string.IsNullOrEmpty(texture2))
                        {
                            Sprite decal = ScanImages.ParseImage(pluginName, texture2);
    
                            if (decal != null)
                                info.AddDecal(decal.texture);
                        }
                    }
                }
            }
        } else
        {
            info = CardManager.New(JSONLoader3.PluginGuid, name, displayedName, baseAttack, baseHealth, description);
            
            info.name = name;
            info.displayedName = displayedName;
            info.description = description;
    
            if (metaCategories != null)
                info.metaCategories = metaCategories.Where(x => !string.IsNullOrEmpty(x)).Select(x => (CardMetaCategory)Enum.Parse(typeof(CardMetaCategory), x)).ToList();
    
            if (!string.IsNullOrEmpty(cardComplexity))
                info.cardComplexity = (CardComplexity)Enum.Parse(typeof(CardComplexity), cardComplexity);
    
            if (!string.IsNullOrEmpty(temple))
                info.temple = (CardTemple)Enum.Parse(typeof(CardTemple), temple);
    
            info.baseAttack = baseAttack;
            info.baseHealth = baseHealth;
            info.hideAttackAndHealth = hideAttackAndHealth;
    
            info.cost = bloodCost;
            info.bonesCost = bonesCost;
            info.energyCost = energyCost;
    
            if (gemColors != null)
                info.gemsCost = gemColors.Where(x => !string.IsNullOrEmpty(x)).Select(x => (GemType)Enum.Parse(typeof(GemType), x)).ToList();
    
            if (!string.IsNullOrEmpty(specialStatIcon))
                info.specialStatIcon = (SpecialStatIcon)Enum.Parse(typeof(SpecialStatIcon), specialStatIcon);
    
            if (tribes != null)
                info.tribes = tribes.Where(x => !string.IsNullOrEmpty(x)).Select(x => (Tribe)Enum.Parse(typeof(Tribe), x)).ToList();
    
            if (traits != null)
                info.traits = traits.Where(x => !string.IsNullOrEmpty(x)).Select(x => (Trait)Enum.Parse(typeof(Trait), x)).ToList();
    
            if (specialAbilities != null)
                info.specialAbilities = specialAbilities.Where(x => !string.IsNullOrEmpty(x)).Select(x => (SpecialTriggeredAbility)Enum.Parse(typeof(SpecialTriggeredAbility), x)).ToList();
    
            if (abilities != null)
                info.abilities = abilities.Where(x => !string.IsNullOrEmpty(x)).Select(x => (Ability)Enum.Parse(typeof(Ability), x)).ToList();
    
            if (customAbilities != null)
            {
                foreach (AbilityData data in customAbilities)
                {
                    info.AddAbilities(data.GetAbility());
                }
            }
    
            if (customSpecialAbilities != null)
            {
                foreach (SpecialAbilityData data in customSpecialAbilities)
                {
                    info.AddSpecialAbilities(data.GetSpecialAbility());
                }
            }
    
            if (evolution != null)
            {
                if(CardUtils.allJLDRCardsPublic.FirstOrDefault(x => x.name.Replace(JSONLoader3.PluginGuid + "_", "") == evolution.name.Replace(JSONLoader3.PluginGuid + "_", "")) != null) 
                {
                    info.SetEvolve(JSONLoader3.PluginGuid + "_" + evolution.name, evolution.turnsToEvolve);
                }
                else
                {
                    info.SetEvolve(evolution.name, evolution.turnsToEvolve);
                }
            }
    
            if (!string.IsNullOrEmpty(defaultEvolutionName))
                info.defaultEvolutionName = defaultEvolutionName;
    
            if (tail != null)
            {
                if (tail != null)
                {
                    if(CardUtils.allJLDRCardsPublic.FirstOrDefault(x => x.name.Replace(JSONLoader3.PluginGuid + "_", "") == tail.name.Replace(JSONLoader3.PluginGuid + "_", "")) != null) 
                    {
                        info.SetTail(JSONLoader3.PluginGuid + "_" + tail.name, tail.tailLostPortrait);
                    }
                    else
                    {
                        info.SetTail(tail.name, tail.tailLostPortrait);
                    }
                }
            }
    
            if (iceCube != null)
            {
                if (iceCube != null)
                {
                    if(CardUtils.allJLDRCardsPublic.FirstOrDefault(x => x.name.Replace(JSONLoader3.PluginGuid + "_", "") == iceCube.creatureWithin.Replace(JSONLoader3.PluginGuid + "_", "")) != null) 
                    {
                        info.SetIceCube(JSONLoader3.PluginGuid + "_" + iceCube.creatureWithin);
                    }
                    else
                    {
                        info.SetIceCube(iceCube.creatureWithin);
                    }
                }
            }
    
            info.flipPortraitForStrafe = flipPortraitForStrafe;
            info.onePerDeck = onePerDeck;
    
            if (appearanceBehaviour != null)
                info.appearanceBehaviour = appearanceBehaviour.Where(x => !string.IsNullOrEmpty(x)).Select(x => (CardAppearanceBehaviour.Appearance)Enum.Parse(typeof(CardAppearanceBehaviour.Appearance), x)).ToList();
            
            if (!string.IsNullOrEmpty(texture))
            {
                Sprite Portrait = ScanImages.ParseImage(pluginName, texture);

                if (Portrait != null)
                    info.SetPortrait(Portrait);
            }

            if (!string.IsNullOrEmpty(altTexture))
            {
                Sprite alternatePortrait = ScanImages.ParseImage(pluginName, altTexture);

                if (alternatePortrait != null)
                    info.SetAltPortrait(alternatePortrait);
            }

            if (!string.IsNullOrEmpty(emissionTexture))
            {
                Sprite emissionPortrait = ScanImages.ParseImage(pluginName, emissionTexture);

                if (emissionPortrait != null)
                    info.SetEmissivePortrait(emissionPortrait);
            }

            if (!string.IsNullOrEmpty(titleGraphic))
            {
                Sprite titleGraphicSprite = ScanImages.ParseImage(pluginName, titleGraphic);

                if (titleGraphicSprite != null)
                    info.titleGraphic = titleGraphicSprite.texture;
            }

            if (!string.IsNullOrEmpty(pixelTexture))
            {
                Sprite pixelPortrait = ScanImages.ParseImage(pluginName, pixelTexture);

                if (pixelPortrait != null)
                    info.SetPixelPortrait(pixelPortrait);
            }

            if (decals != null)
            {
                info.decals = new List<Texture>();

                foreach (string texture2 in decals)
                {
                    if (!string.IsNullOrEmpty(texture2))
                    {
                        Sprite decal = ScanImages.ParseImage(pluginName, texture2);

                        if (decal != null)
                            info.AddDecal(decal.texture);
                    }
                }
            }
        }

        return info;
    }
}

/// <summary>
/// An Object Representing Modded Abilities
/// </summary>
/// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
public class AbilityData
{
    /// <summary>
    /// The Name of the Ability.
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// The GUID of the Ability.
    /// </summary>
    public string GUID { get; set; }
    
    /// <summary>
    /// A Constructor for Modded Abilities.
    /// </summary>
    /// <param name="name"><see cref="name"/></param>
    /// <param name="GUID"><see cref="GUID"/></param>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public AbilityData(string name, string GUID)
    {
        this.name = name;
        this.GUID = GUID;
    }
    
    /// <summary>
    /// A function that fetches the Modded Ability.
    /// </summary>
    /// <returns>An Ability associated wih the Modded Ability</returns>
    /// <remarks>This code is derived from code made by LilySylvie.</remarks>
    public Ability GetAbility()
    {
        return GuidManager.GetEnumValue<Ability>(GUID, name);
    }
}

/// <summary>
/// An Object Representing Modded Special Abilities
/// </summary>
/// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
public class SpecialAbilityData
{
    /// <summary>
    /// The Name of the Special Ability.
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// The GUID Of the Special Ability.
    /// </summary>
    public string GUID { get; set; }
    
    /// <summary>
    /// A Constructor for Modded Special Abilities.
    /// </summary>
    /// <param name="name"><see cref="name"/></param>
    /// <param name="GUID"><see cref="GUID"/></param>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public SpecialAbilityData(string name, string GUID)
    {
        this.name = name;
        this.GUID = GUID;
    }
    
    /// <summary>
    /// A function that fetches the Modded Special Ability.
    /// </summary>
    /// <returns>A SpecialTriggeredAbility associated wih the Modded Special Ability</returns>
    /// <remarks>This code is derived from code made by LilySylvie.</remarks>
    public SpecialTriggeredAbility GetSpecialAbility()
    {
        return GuidManager.GetEnumValue<SpecialTriggeredAbility>(GUID, name);
    }
}

/// <summary>
/// An Object representing the Evolution Parameters.
/// </summary>
/// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
public class EvolveData
{
    /// <summary>
    /// The Name of the Card this card will Evolve into.
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// The Amount of Turns this card needs in order to evolve.
    /// </summary>
    public int turnsToEvolve { get; set; }

    /// <summary>
    /// A Constructor for the Evolution Parameters.
    /// </summary>
    /// <param name="name"><see cref="name"/></param>
    /// <param name="turnsToEvolve"><see cref="turnsToEvolve"/></param>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public EvolveData(string name, int turnsToEvolve)
    {
        this.name = name;
        this.turnsToEvolve = turnsToEvolve;
    }
}

/// <summary>
/// An Object representing the Tail Parameters.
/// </summary>
/// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
public class TailData
{
    /// <summary>
    /// The Name of the Card that will become the Tail.
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// The Portrait the card gains after losing its tail.
    /// </summary>
    public string tailLostPortrait { get; set; }

    /// <summary>
    /// A Constructor for the Tail Parameters.
    /// </summary>
    /// <param name="name"><see cref="name"/></param>
    /// <param name="tailLostPortrait"><see cref="tailLostPortrait"/></param>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public TailData(string name, string tailLostPortrait)
    {
        this.name = name;
        this.tailLostPortrait = tailLostPortrait;
    }
}

/// <summary>
/// An Object representing the IceCube Parameters.
/// </summary>
/// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
public class IceCubeData
{
    /// <summary>
    /// The Card this will become on death.
    /// </summary>
    public string creatureWithin { get; set; }

    /// <summary>
    /// A constructor for the IceCue Parameters.
    /// </summary>
    /// <param name="creatureWithin"><see cref="creatureWithin"/></param>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public IceCubeData(string creatureWithin)
    {
        this.creatureWithin = creatureWithin;
    }
}