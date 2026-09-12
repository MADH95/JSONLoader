using System;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using JSONLoader3.Peripheral.ImageHandling;
using Sirenix.Utilities;
using UnityEngine;

namespace JSONLoader3.Cores.JSONLoaderV2Support.Objects;

/// <summary>
/// An Object representing an <see cref="JSONLoaderV2Support.Schemas.Card"/>.
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
    /// The cards In-Code ModPrefix (this is to help you extract the raw name, as name automatically has the prefix attached.
    /// </summary>
    public string modPrefix { get; set; }
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
    /// An Int representing the Bones Cost of the card.
    /// </summary>
    public int bonesCost { get; set; }
    /// <summary>
    /// An Int representing the Energy Cost of the card.
    /// </summary>
    public int energyCost { get; set; }
    /// <summary>
    /// A List representing all the Gem Colors applied to the card.
    /// </summary>
    public List<string> gemsCost { get; set; }
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
    /// A List of all Abilities that the card has.
    /// </summary>
    public List<string> abilities { get; set; }
    /// <summary>
    /// A List of all Special Abilities that the card has.
    /// </summary>
    public List<string> specialAbilities { get; set; }
    /// <summary>
    /// The Name of the Card this card will Evolve into.
    /// </summary>
    public string evolveIntoName { get; set; }
    /// <summary>
    /// The Amount of Turns this card needs in order to evolve.
    /// </summary>
    public int evolveTurns { get; set; }
    /// <summary>
    /// The Default Evolution Name for the Card if it has no Evolve Ability Related Parameters.
    /// </summary>
    public string defaultEvolutionName { get; set; }
    /// <summary>
    /// The Name of the Card that will become the Tail.
    /// </summary>
    public string tailName { get; set; }
    /// <summary>
    /// The Portrait the card gains after losing its tail.
    /// </summary>
    public string tailLostPortrait { get; set; }
    /// <summary>
    /// The Card this will become on death.
    /// </summary>
    public string iceCubeName { get; set; }
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
    /// The Emission Version of the Card's Portrait.
    /// </summary>
    public string emissionTexture { get; set; }
    /// <summary>
    /// The Alternate Version of the Card's Portrait.
    /// </summary>
    public string altTexture { get; set; }
    /// <summary>
    /// The Alternate Emission Version of the Card's Portrait.
    /// </summary>
    public string altEmissionTexture { get; set; }
    /// <summary>
    /// The Act 2 Version of the Card's Portrait.
    /// </summary>
    public string pixelTexture { get; set; }
    /// <summary>
    /// The Title Graphic of the Card, this appears overlayed on the card's name.
    /// </summary>
    public string titleGraphic { get; set; }
    /// <summary>
    /// A list of all Decal paths to be on the card.
    /// </summary>
    public List<string> decals { get; set; }
    /// <summary>
    /// A Dictionary of Extension Properties applied upon this card.
    /// </summary>
    public Dictionary<string, string> extensionProperties { get; set; }
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
    /// <param name="modPrefix"><see cref="modPrefix"/></param>
    /// <param name="displayedName"><see cref="displayedName"/></param>
    /// <param name="description"><see cref="description"/></param>
    /// <param name="metaCategories"><see cref="metaCategories"/></param>
    /// <param name="baseAttack"><see cref="baseAttack"/></param>
    /// <param name="baseHealth"><see cref="baseHealth"/></param>
    /// <param name="hideAttackAndHealth"><see cref="hideAttackAndHealth"/></param>
    /// <param name="bloodCost"><see cref="bloodCost"/></param>
    /// <param name="bonesCost"><see cref="bonesCost"/></param>
    /// <param name="energyCost"><see cref="energyCost"/></param>
    /// <param name="gemsCost"><see cref="gemsCost"/></param>
    /// <param name="specialStatIcon"><see cref="specialStatIcon"/></param>
    /// <param name="tribes"><see cref="tribes"/></param>
    /// <param name="traits"><see cref="traits"/></param>
    /// <param name="abilities"><see cref="abilities"/></param>
    /// <param name="specialAbilities"><see cref="specialAbilities"/></param>
    /// <param name="evolveIntoName"><see cref="evolveIntoName"/></param>
    /// <param name="evolveTurns"><see cref="evolveTurns"/></param>
    /// <param name="defaultEvolutionName"><see cref="defaultEvolutionName"/></param>
    /// <param name="tailName"><see cref="tailName"/></param>
    /// <param name="tailLostPortrait"><see cref="tailLostPortrait"/></param>
    /// <param name="iceCubeName"><see cref="iceCubeName"/></param>
    /// <param name="flipPortraitForStrafe"><see cref="flipPortraitForStrafe"/></param>
    /// <param name="onePerDeck"><see cref="onePerDeck"/></param>
    /// <param name="appearanceBehaviour"><see cref="appearanceBehaviour"/></param>
    /// <param name="texture"><see cref="texture"/></param>
    /// <param name="emissionTexture"><see cref="emissionTexture"/></param>
    /// <param name="altTexture"><see cref="altTexture"/></param>
    /// <param name="altEmissionTexture"><see cref="altEmissionTexture"/></param>
    /// <param name="pixelTexture"><see cref="pixelTexture"/></param>
    /// <param name="titleGraphic"><see cref="titleGraphic"/></param>
    /// <param name="decals"><see cref="decals"/></param>
    /// <param name="extensionProperties"><see cref="extensionProperties"/></param>
    /// <param name="file"><see cref="file"/></param>
    /// <param name="pluginName"><see cref="pluginName"/></param>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public CardObject(List<string> fieldsToEdit, string name, string modPrefix = "", string displayedName = "",
        string description = "", List<string> metaCategories = null, int baseAttack = 0, int baseHealth = 0,
        bool hideAttackAndHealth = false,
        int bloodCost = 0, int bonesCost = 0, int energyCost = 0, List<string> gemsCost = null,
        string specialStatIcon = "", List<string> tribes = null, List<string> traits = null,
        List<string> abilities = null, List<string> specialAbilities = null, string evolveIntoName = "",
        int evolveTurns = 1, string defaultEvolutionName = "", string tailName = "", string tailLostPortrait = "",
        string iceCubeName = "", bool flipPortraitForStrafe = false, bool onePerDeck = false,
        List<string> appearanceBehaviour = null, string texture = "", string emissionTexture = "",
        string altTexture = "", string altEmissionTexture = "", string pixelTexture = "", string titleGraphic = "",
        List<string> decals = null, Dictionary<string, string> extensionProperties = null, string file = "", string pluginName = "")
    {
        this.fieldsToEdit = fieldsToEdit;
        this.name = modPrefix + "_" + name;
        this.modPrefix = modPrefix;
        this.displayedName = displayedName;
        this.description = description;
        this.metaCategories = metaCategories;
        this.baseAttack = baseAttack;
        this.baseHealth = baseHealth;
        this.hideAttackAndHealth = hideAttackAndHealth;
        this.bloodCost = bloodCost;
        this.bonesCost = bonesCost;
        this.energyCost = energyCost;
        this.gemsCost = gemsCost;
        this.specialStatIcon = specialStatIcon;
        this.tribes = tribes;
        this.traits = traits;
        this.abilities = abilities;
        this.specialAbilities = specialAbilities;
        this.evolveIntoName = evolveIntoName;
        this.evolveTurns = evolveTurns;
        this.defaultEvolutionName = defaultEvolutionName;
        this.tailName = tailName;
        this.tailLostPortrait = tailLostPortrait;
        this.iceCubeName = iceCubeName;
        this.flipPortraitForStrafe = flipPortraitForStrafe;
        this.onePerDeck = onePerDeck;
        this.appearanceBehaviour = appearanceBehaviour;
        this.texture = texture;
        this.emissionTexture = emissionTexture;
        this.altTexture = altTexture;
        this.altEmissionTexture = altEmissionTexture;
        this.pixelTexture = pixelTexture;
        this.titleGraphic = titleGraphic;
        this.decals = decals;
        this.extensionProperties = extensionProperties;
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
            CardInfo infoTemp = CardManager.AllCardsCopy.FirstOrDefault(x => x.name == name.Replace(modPrefix + "_", ""));
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
                    {
                        info.metaCategories = new List<CardMetaCategory>();
                        foreach (string meta in metaCategories)
                        {
                            if (!string.IsNullOrEmpty(meta))
                                info.AddMetaCategories(GetEnumValue<CardMetaCategory>(meta));

                        }
                    }
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
        
                if (fieldsToEdit.Contains("gemsCost"))
                {
                    if (gemsCost != null)
                    {
                        info.gemsCost = new List<GemType>();
                        info.gemsCost = gemsCost.Where(x => !string.IsNullOrEmpty(x)).Select(x => (GemType)Enum.Parse(typeof(GemType), x)).ToList();
                    }
                }
        
                if (fieldsToEdit.Contains("specialStatIcon"))
                {
                    if (!string.IsNullOrEmpty(specialStatIcon))
                        info.specialStatIcon = GetEnumValue<SpecialStatIcon>(specialStatIcon);
                }
        
                if (fieldsToEdit.Contains("tribes"))
                {
                    if (tribes != null)
                    {
                        info.tribes = new List<Tribe>();
                        foreach (string tribe in tribes)
                        {
                            if (!string.IsNullOrEmpty(tribe))
                                info.AddTribes(GetEnumValue<Tribe>(tribe));
                        }
                    }
                }
        
                if (fieldsToEdit.Contains("traits"))
                {
                    if (traits != null)
                    {
                        info.traits = new List<Trait>();
                        foreach (string trait in traits)
                        {
                            if (!string.IsNullOrEmpty(trait))
                                info.AddTraits(GetEnumValue<Trait>(trait));
                        }
                    }
                }
                
                if (fieldsToEdit.Contains("abilities"))
                {
                    if (abilities != null)
                    {
                        info.abilities = new List<Ability>();
                        foreach (string ability in abilities)
                        {
                            if (!string.IsNullOrEmpty(ability))
                                info.AddAbilities(GetEnumValue<Ability>(ability));
                        }
                    }
                }
        
                if (fieldsToEdit.Contains("specialAbilities"))
                {
                    if (specialAbilities != null)
                    {
                        info.specialAbilities = new List<SpecialTriggeredAbility>();
                        foreach (string special in specialAbilities)
                        {
                            if (!string.IsNullOrEmpty(special))
                                info.AddSpecialAbilities(GetEnumValue<SpecialTriggeredAbility>(special));
                        }
                    }
                }

                if (fieldsToEdit.Contains("evolveIntoName"))
                {
                    info.evolveParams = new EvolveParams();
                    if (!string.IsNullOrEmpty(evolveIntoName))
                    {
                        info.evolveParams.evolution = CardManager.AllCardsCopy.Find(x => x.name == evolveIntoName);
                    }
                    
                    if (fieldsToEdit.Contains("evolveTurns"))
                    {
                        info.evolveParams.turnsToEvolve = evolveTurns;
                    }
                }
        
                if (fieldsToEdit.Contains("defaultEvolutionName"))
                {
                    if (!string.IsNullOrEmpty(defaultEvolutionName))
                        info.defaultEvolutionName = defaultEvolutionName;
                }

                if (fieldsToEdit.Contains("tailName"))
                {
                    info.tailParams = new TailParams();
                    if (!string.IsNullOrEmpty(tailName))
                    {
                        info.tailParams.tail = CardManager.AllCardsCopy.Find(x => x.name == tailName);
                    }
                    
                    if (fieldsToEdit.Contains("tailLostPortrait"))
                    {
                        if (!string.IsNullOrEmpty(tailLostPortrait))
                            info.tailParams.tailLostPortrait = ScanImages.ParseImage(pluginName, tailLostPortrait);
                    }
                }


                if (fieldsToEdit.Contains("iceCubeName"))
                {
                    info.iceCubeParams = new IceCubeParams();
                    if (!string.IsNullOrEmpty(iceCubeName))
                    {
                        info.iceCubeParams.creatureWithin = CardManager.AllCardsCopy.Find(x => x.name == iceCubeName);
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
                    {
                        info.appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
                        foreach (string appearance in appearanceBehaviour)
                        {
                            if (!string.IsNullOrEmpty(appearance))
                                info.AddAppearances(GetEnumValue<CardAppearanceBehaviour.Appearance>(appearance));
                        }
                    }
                }
                
                if (fieldsToEdit.Contains("texture"))
                {
                    Sprite sprite = ScanImages.ParseImage(pluginName, texture);

                    if (sprite != null)
                        info.SetEmissivePortrait(sprite);
                }
                
                if (fieldsToEdit.Contains("emissionTexture"))
                {
                    if (!string.IsNullOrEmpty(emissionTexture))
                    {
                        Sprite sprite = ScanImages.ParseImage(pluginName, emissionTexture);
    
                        if (sprite != null)
                            info.SetEmissivePortrait(sprite);
                    }
                }
    
                if (fieldsToEdit.Contains("altTexture"))
                {
                    if (!string.IsNullOrEmpty(altTexture))
                    {
                        Sprite sprite = ScanImages.ParseImage(pluginName, altTexture);
    
                        if (sprite != null)
                            info.SetAltPortrait(sprite);
                    }
                }
                
                if (fieldsToEdit.Contains("altEmissionTexture"))
                {
                    if (!string.IsNullOrEmpty(altEmissionTexture))
                    {
                        Sprite sprite = ScanImages.ParseImage(pluginName, altEmissionTexture);
    
                        if (sprite != null)
                            info.SetEmissiveAltPortrait(sprite);
                    }
                }
    
                if (fieldsToEdit.Contains("pixelTexture"))
                {
                    if (!string.IsNullOrEmpty(pixelTexture))
                    {
                        Sprite sprite = ScanImages.ParseImage(pluginName, pixelTexture);
    
                        if (sprite != null)
                            info.pixelPortrait = sprite;
                    }
                }
    
                if (fieldsToEdit.Contains("titleGraphic"))
                {
                    if (!string.IsNullOrEmpty(titleGraphic))
                    {
                        Sprite sprite = ScanImages.ParseImage(pluginName, titleGraphic);
    
                        if (sprite != null)
                            info.titleGraphic = sprite.texture;
                    }
                }
    
                if (fieldsToEdit.Contains("decals"))
                {
                    if (decals != null)
                    {
                        info.decals = new List<Texture>();
                        foreach (string texture2 in decals)
                        {
                            if (!string.IsNullOrEmpty(texture2))
                            {
                                Sprite sprite = ScanImages.ParseImage(pluginName, texture2);
    
                                if (sprite != null)
                                    info.decals.Add(sprite.texture);
                            }
                        }
                    }
                }

                if (fieldsToEdit.Contains("extensionProperties"))
                {
                    if (extensionProperties != null)
                        foreach (KeyValuePair<string, string> kvp in extensionProperties)
                        {
                            info.SetExtendedProperty(kvp.Key, kvp.Value);
                        }
                }
            }
        } else
        {
            info = CardManager.New(modPrefix, name, displayedName, baseAttack, baseHealth, description);
            
            info.metaCategories = new List<CardMetaCategory>();
            info.gemsCost = new List<GemType>();
            info.tribes = new List<Tribe>();
            info.traits = new List<Trait>();
            info.abilities = new List<Ability>();
            info.specialAbilities = new List<SpecialTriggeredAbility>();
            info.appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            info.decals = new List<Texture>();
            
            info.name = name;
            info.displayedName = displayedName;
            info.description = description;
    
            if (metaCategories != null)
                foreach (string meta in metaCategories)
                {
                    if (!string.IsNullOrEmpty(meta))
                        info.AddMetaCategories(GetEnumValue<CardMetaCategory>(meta));
                }
    
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
    
            if (gemsCost != null)
                info.gemsCost = gemsCost.Where(x => !string.IsNullOrEmpty(x)).Select(x => (GemType)Enum.Parse(typeof(GemType), x)).ToList();
    
            if (!string.IsNullOrEmpty(specialStatIcon))
                info.specialStatIcon = GetEnumValue<SpecialStatIcon>(specialStatIcon);
    
            if (tribes != null)
                foreach (string tribe in tribes)
                {
                    if (!string.IsNullOrEmpty(tribe))
                        info.AddTribes(GetEnumValue<Tribe>(tribe));
                }
    
            if (traits != null)
                foreach (string trait in traits)
                {
                    if (!string.IsNullOrEmpty(trait))
                        info.AddTraits(GetEnumValue<Trait>(trait));
                }
            
            if (abilities != null)
                foreach (string ability in abilities)
                {
                    if (!string.IsNullOrEmpty(ability))
                        info.AddAbilities(GetEnumValue<Ability>(ability));
                }
    
            if (specialAbilities != null)
                foreach (string special in specialAbilities)
                {
                    if (!string.IsNullOrEmpty(special))
                        info.AddSpecialAbilities(GetEnumValue<SpecialTriggeredAbility>(special));
                }
            
            if (!string.IsNullOrEmpty(evolveIntoName))
            {
                info.evolveParams = new EvolveParams();
                info.evolveParams.evolution = CardManager.AllCardsCopy.Find(x => x.name == evolveIntoName);
                info.evolveParams.turnsToEvolve = evolveTurns;
            }
    
            if (!string.IsNullOrEmpty(defaultEvolutionName))
                info.defaultEvolutionName = defaultEvolutionName;
            
            if (!string.IsNullOrEmpty(tailName))
            {
                info.tailParams = new TailParams();
                info.tailParams.tail = CardManager.AllCardsCopy.Find(x => x.name == tailName);
                if (!string.IsNullOrEmpty(tailLostPortrait))
                    info.tailParams.tailLostPortrait = ScanImages.ParseImage(pluginName, tailLostPortrait);
            }
    
            if (!string.IsNullOrEmpty(iceCubeName))
            {
                info.iceCubeParams = new IceCubeParams();
                info.iceCubeParams.creatureWithin = CardManager.AllCardsCopy.Find(x => x.name == iceCubeName);
            }
            
            info.flipPortraitForStrafe = flipPortraitForStrafe;
            info.onePerDeck = onePerDeck;
    
            if (appearanceBehaviour != null)
                foreach (string appearance in appearanceBehaviour)
                {
                    if (!string.IsNullOrEmpty(appearance))
                        info.AddAppearances(GetEnumValue<CardAppearanceBehaviour.Appearance>(appearance));
                }
            
            if (!string.IsNullOrEmpty(texture))
            {
                Sprite Portrait = ScanImages.ParseImage(pluginName, texture);

                if (Portrait != null)
                    info.SetPortrait(Portrait);
            }

            if (!string.IsNullOrEmpty(emissionTexture))
            {
                Sprite emissionPortrait = ScanImages.ParseImage(pluginName, emissionTexture);

                if (emissionPortrait != null)
                    info.SetEmissivePortrait(emissionPortrait);
            }

            if (!string.IsNullOrEmpty(altTexture))
            {
                Sprite alternatePortrait = ScanImages.ParseImage(pluginName, altTexture);

                if (alternatePortrait != null)
                    info.SetAltPortrait(alternatePortrait);
            }
            
            if (!string.IsNullOrEmpty(altEmissionTexture))
            {
                Sprite alternateEmissionPortrait = ScanImages.ParseImage(pluginName, altEmissionTexture);

                if (alternateEmissionPortrait != null)
                    info.SetEmissiveAltPortrait(alternateEmissionPortrait);
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
            
            if (extensionProperties != null)
                foreach (KeyValuePair<string, string> kvp in extensionProperties)
                {
                    info.SetExtendedProperty(kvp.Key, kvp.Value);
                }
        }

        return info;
    }
    
    /// <summary>
    /// Gets the Enum Value compared to the Type Param.
    /// </summary>
    /// <param name="value">The Value we want an Enum From.</param>
    /// <typeparam name="T">The Type to check it against.</typeparam>
    /// <returns>The Parsed Enum Value.</returns>
    private static T GetEnumValue<T>(string value) where T : unmanaged, Enum
    {
        int firstUnderscore = value.IndexOf('_');

        if (firstUnderscore != -1)
        {
            string prefix = value.Substring(0, firstUnderscore);
            string actualValue = value.Substring(firstUnderscore + 1);

            T enumValue = GuidManager.GetEnumValue<T>(prefix, actualValue);

            Debug.Log(
                $"[CardObject] Resolved '{value}' -> GUID: '{prefix}', Name: '{actualValue}', Enum: '{enumValue}', Value: {Convert.ToInt32(enumValue)} String Value '{actualValue}'"
            );

            return enumValue;
        }
        
        int lastDotIndex = value.LastIndexOf('.');

        if (lastDotIndex != -1)
        {
            string prefix = value.Substring(0, lastDotIndex);
            string actualValue = value.Substring(lastDotIndex + 1);

            T enumValue = GuidManager.GetEnumValue<T>(prefix, actualValue);

            Debug.Log(
                $"[CardObject] Resolved '{value}' -> GUID: '{prefix}', Name: '{actualValue}', Enum: '{enumValue}', Value: {Convert.ToInt32(enumValue)} String Value '{actualValue}'"
            );

            return enumValue;
        }

        T fallbackValue = (T)Enum.Parse(typeof(T), value);

        Debug.Log(
            $"[CardObject] Resolved '{value}' -> Enum: '{fallbackValue}', Value: {Convert.ToInt32(fallbackValue)} String Value '{value}'"
        );

        return fallbackValue;
    }
}