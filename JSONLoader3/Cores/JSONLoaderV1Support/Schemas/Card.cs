using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace JSONLoader3.Cores.JSONLoaderV1Support.Schemas;

/// <summary>
/// The main data Object for JSONLoader V1's Card system.
/// </summary>
[Serializable]
public class Card
{
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string array for the fields you wish to edit. The fields must be the exact names as in the right hand side of this table" |
    ///
    /// New Description from JSONLoader v3.0.0: Any items applied within this field will be used for overwriting the In-Game card associated with the field 'name'.
    /// </summary>
    [Tooltip("Items(True) | ItemType(string) | UniqueItems(true) | Enums(displayedName, description, metaCategories, cardComplexity, temple, baseAttack, baseHealth, hideAttackAndHealth, bloodCost, bonesCost, energyCost, gemColors, specialStatIcon, tribes, traits, specialAbilities, abilities, customAbilities, customSpecialAbilities, evolution, defaultEvolutionName, tail, iceCube, flipPortraitForStrafe, onePerDeck, appearanceBehavior, texture, altTexture, emissionTexture, titleGraphic, pixelTexture, decals)")]
    public List<string> fieldsToEdit;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string for the name the game will use to identify the card - should contain no spaces. When editing, this field must match the card's name (See Card Names.txt for a list of ingame card names)" |
    ///
    /// New Description from JSONLoader v3.0.0: The In-Code name for the card, please append on a Prefix unique to your mod if you are NOT editing a base game card. For example; "JSONFanMod5_Gorilla".
    /// </summary>
    [Tooltip("REQUIRED | MinimumLength(1) | Pattern(^[a-zA-Z\\d_]+$)")]
    public string name;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string for the name displayed on the card" |
    ///
    /// New Description from JSONLoader v3.0.0: The In-Game name for the card, it can be anything as long as this font can display it; https://font.download/font/heavyweight
    /// </summary>
    public string displayedName;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string for the description Leshy gives when you find the card" |
    ///
    /// New Description from JSONLoader v3.0.0: The In-Game flavor for the card, this will show when receiving the card for the first time, if you want to prevent it being seen from saving use; https://thunderstore.io/c/inscryption/p/creator/Fuck_Dialouge_Saving/
    /// </summary>
    public string description;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string array of meta catagories the card has (See Enums.txt for a list of catagories)" |
    ///
    /// New Description from JSONLoader v3.0.0: These Meta-Categories control how your card will show up within the game, see the following page for what each of them do; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums
    /// </summary>
    [Tooltip("Items(true) | ItemType(string) | Enums(ChoiceNode, GBCPack, GBCPlayable, Part3Random, Rare, TraderOffer, AscensionUnlock) | UniqueItems(true)")]
    public List<string> metaCategories;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string for the complexity of the card (See Enums.txt for a list of levels of complexity)" |
    ///
    /// New Description from JSONLoader v3.0.0: This controls WHEN your card can show up in the game, see the following page for what each of them do; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums
    /// </summary>
    [Tooltip("Default(Vanilla) | Enums(Vanilla, Simple, Intermediate, Advanced)")]
    public string cardComplexity;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string for which Scrybe created the card" |
    ///
    /// New Description from JSONLoader v3.0.0: This controls which temple in Act 2 the card is apart of, as well as meant to determine which Act outside Act 2 the card shows up in, whether mods follow the convention is up to question, but that's what these do. So, Nature is Act 1 and the Nature Temple, Tech is Act 3 and the Technology Temple, Undead is the Grimora Portion of the Finale and the Undead Temple, Wizard is the Magnificus Portion of the Finale and the Magicks Temple.
    /// </summary>
    [Tooltip("Default(Nature) | Enums(Nature, Undead, Tech, Wizard)")]
    public string temple;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "An integer value for the attack of a card" |
    ///
    /// New Description from JSONLoader v3.0.0: This value determines the attack value of the card, it cannot be negative.
    /// </summary>
    [Tooltip("Default(0) | Minimum(0)")]
    public int baseAttack;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "An integer value for the health of a card" |
    ///
    /// New Description from JSONLoader v3.0.0: This value determines the health value of the card, it cannot be negative or 0.
    /// </summary>
    [Tooltip("Default(1) | Minimum(1)")]
    public int baseHealth;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A boolean value to toggle if the cards attack and health are visible" |
    ///
    /// New Description from JSONLoader v3.0.0: This boolean value determines whether the Attack and Health of the card should be hidden or not.
    /// </summary>
    [Tooltip("Default(false)")]
    public bool hideAttackAndHealth;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "An integer value for the blood cost of a card" |
    ///
    /// New Description from JSONLoader v3.0.0: This value determines the amount of Blood this card will cost.
    /// </summary>
    [Tooltip("Default(0) | Minimum(0)")]
    public int bloodCost;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "An integer value for the bones cost of a card" |
    ///
    /// New Description from JSONLoader v3.0.0: This value determines the amount of Bones this card will cost.
    /// </summary>
    [Tooltip("Default(0) | Minimum(0)")]
    public int bonesCost;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "An integer value for the energy cost of a card" |
    ///
    /// New Description from JSONLoader v3.0.0: This value determines the amount of Energy this card will cost.
    /// </summary>
    [Tooltip("Default(0) | Minimum(0)")]
    public int energyCost;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string array for the gems cost of a card (See Enums.txt for a list of gems)" |
    ///
    /// New Description from JSONLoader v3.0.0: The following 3 values are accepted here: Green for the Green Gem, Orange for the Orange Gem, and Blue for the Blue Gem. Each of these correlates to the Gem Cost of a card. This version of JSONLoader does not support multiple of the same color of gem.
    /// </summary>
    [Tooltip("Items(true) | ItemType(String) | Enums(Green, Orange, Blue) | UniqueItems(true)")]
    public List<string> gemColors;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "An string for which special stat icon the card has (See Enums.txt for a list of icons)" |
    ///
    /// New Description from JSONLoader v3.0.0: This determines which Stat Icon to show on the card, this must be used alongside the associated Special Ability.
    /// </summary>
    [Tooltip("Enums(Ants, Bell, Bones, CardsInHand, GreenGems, Mirror, SacrificesThisTurn)")]
    public string specialStatIcon;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "An string array for the tribes the card belongs to (See Enums.txt for a list of tribes)" |
    ///
    /// New Description from JSONLoader v3.0.0: This List determines what Tribes are applied to the card, this works with Base Game tribes only. Use a newer version of JSONLoader for Modded Tribes. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums
    /// </summary>
    [Tooltip("Items(true) | ItemType(String) | Enums(Bird, Canine, Hooved, Insect, Reptile, Squirrel) | UniqueItems(true)")]
    public List<string> tribes;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "An string array for the traits a card has (See Enums.txt for a list of traits)" |
    ///
    /// New Description from JSONLoader v3.0.0: This List determines what Traits are applied to this card, this works with Base Game traits only. Use a newer version of JSONLoader for Modded Traits. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums
    /// </summary>
    [Tooltip("Items(true) | ItemType(String) | Enums(Ant, Bear, Blind, DeathcardCreationNonOption, EatsWarrens, FeedsStoat, Fused, Gem, Giant, Goat, Juvenile, KillsSurvivors, Lice, LikesHoney, Pelt, ProtectsCub, SatisfiesRingTrial, Structure, Terrain, Uncuttable, Undead, Wolf) | UniqueItems(true)")]
    public List<string> traits;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string array for the special abilities a card has (See Enums.txt for a list of special abilities)" |
    ///
    /// New Description from JSONLoader v3.0.0: This List determines what Special Abilities are applied to this card, this works specifically with Base Game Special Abilities. For Modded Special Abilities utilize the 'customSpecialAbilities' field. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums
    /// </summary>
    [Tooltip("Items(true) | ItemType(String) | Enums(Ant, BellProximity, BountyHunter, BrokenCoinLeft, BrokenCoinRight, CagedWolf, CardsInHand, Cat, Daus, GiantCard, GiantMoon, GiantShip, GreenMage, JerseyDevil, Lammergeier, Mirror, Ouroboros, PackMule, RandomCard, SacrificesThisTurn, ShapeShifter, SpawnLice, TalkingCardChooser, TrapSpawner) | UniqueItems(true)")]
    public List<string> specialAbilities;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string array for the sigils a card has. (See Enums.txt for a list of sigil abilities)." |
    ///
    /// New Description from JSONLoader v3.0.0: This List determines what Abilities are applied to this card, this works specifically with Base Game Abilities. For Modded Abilities utilize the 'customAbilities' field. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums
    /// </summary>
    [Tooltip("Items(true) | ItemType(String) | Enums(ActivatedDealDamage, ActivatedDrawSkeleton, ActivatedEnergyToBones, ActivatedHeal, ActivatedRandomPowerBone, ActivatedRandomPowerEnergy, ActivatedSacrificeDrawCards, ActivatedStatsUp, ActivatedStatsUpEnergy, AllStrike, Apparition, BeesOnHit, BombSpawner, BoneDigger, Brittle, BuffEnemy, BuffGems, BuffNeighbors, CellBuffSelf, CellDrawRandomCardOnDeath, CellTriStrike, ConduitBuffAttack, ConduitEnergy, ConduitFactory, ConduitHeal, ConduitNull, ConduitSpawnGems, CorpseEater, CreateBells, CreateDams, CreateEgg, DeathShield, Deathtouch, DebuffEnemy, DeleteFile, DoubleDeath, DoubleStrike, DrawAnt, DrawCopy, DrawCopyOnDeath, DrawNewHand, DrawRabbits, DrawRandomCardOnDeath, DrawVesselOnHit, DropRubyOnDeath, EdaxioArms, EdaxioHead, EdaxioLegs, EdaxioTorso, Evolve, ExplodeGems, ExplodeOnDeath, ExplodingCorpse, FileSizeDamage, Flying, GainAttackOnKill, GainBattery, GainGemBlue, GainGemGreen, GainGemOrange, GainGemTriple, GemDependant, GemsDraw, GuardDog, Haunter, HydraEgg, IceCube, LatchBrittle, LatchDeathShield, LatchExplodeOnDeath, Loot, MadeOfStone, Morsel, MoveBeside, OpponentBones, PermaDeath, PreventAttack, QuadrupleBones, RandomAbility, RandomConsumable, Reach, Sacrificial, Sentry, Sharp, ShieldGems, Sinkhole, SkeletonStrafe, Sniper, SplitStrike, SquirrelOrbit, SquirrelStrafe, SteelTrap, Strafe, StrafePush, StrafeSwap, Submerge, SubmergeSquid, SwapStats, TailOnHit, Transformer, TripleBlood, TriStrike, Tutor, VirtualReality, WhackAMole) | UniqueItems(true)")]
    public List<string> abilities;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "An array of objects for the custom ability name and mod GUID (It's children are in the table below this one)" |
    ///
    /// New Description from JSONLoader v3.0.0: This List determines the Modded Abilities that will be applied to this card. You may find this to be a useful resource; https://github.com/Chaosyr/SaxbyModEnums/wiki
    /// </summary>
    [Tooltip("Items(true) | ItemType(Object) | AdditionalProperties(false) | UniqueItems(true)")]
    public List<AbilityData> customAbilities;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "An array of objects for the custom special ability name and mod GUID (It's children are in the table below this one)" |
    ///
    /// New Description from JSONLoader v3.0.0: This List determines the Modded Special Abilities that will be applied to this card. You may find this to be a useful resource; https://github.com/Chaosyr/SaxbyModEnums/wiki
    /// </summary>
    [Tooltip("Items(true) | ItemType(Object) | AdditionalProperties(false) | UniqueItems(true)")]
    public List<SpecialAbilityData> customSpecialAbilities;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A json object for the evolveParams of the card. (It's children are in the table below this one)" |
    ///
    /// New Description from JSONLoader v3.0.0: This Object determines the Evolution related Parameters for this card, such as what it will turn into, and how long it will take to turn into it.
    /// </summary>
    [Tooltip("AdditionalProperties(false)")]
    public EvolveData evolution;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "The name the card will have when it evolves (when it doesn't have evolve_ fields set)" |
    ///
    /// New Description from JSONLoader v3.0.0: This determines what the Default Evolution Name will be, note it will appear in the format of; '[defaultEvolutionName] [displayedName]', just replace the variables with your JSON's values.
    /// </summary>
    public string defaultEvolutionName;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A json object for the tailParams of the card. (It's children are in the table below this one)" |
    ///
    /// New Description from JSONLoader v3.0.0: This Object determines the LooseTail related Parameters for this card, such as this cards Texture after losing its tail, or the Card the Tail Will Be.
    /// </summary>
    [Tooltip("AdditionalProperties(false)")]
    public TailData tail;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A json object for the iceCubeParams of the card. (It's children are in the table below this one)" |
    ///
    /// New Description from JSONLoader v3.0.0: This Object determines the IceCube related Parameters for this card, namely what card it will be turned into, if left empty the default is an Opossum.
    /// </summary>
    [Tooltip("AdditionalProperties(false)")]
    public IceCubeData iceCube;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A boolean to determine if the cards portrait should flip when it uses one of the strafe sigils" |
    ///
    /// New Description from JSONLoader v3.0.0: A bool determining whether this cards portrait will flip when the card moves. (like the sigil icon does)
    /// </summary>
    [Tooltip("Default(false)")]
    public bool flipPortraitForStrafe;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A boolean value that toggles if there can be only one of the card per deck" |
    ///
    /// New Description from JSONLoader v3.0.0: A bool determining if there can only be one copy of this card within the Player's deck.
    /// </summary>
    [Tooltip("Default(false)")]
    public bool onePerDeck;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string array for the behaviours the cards appearance should have (See enums.txt for a list of appearance behaviours)" |
    ///
    /// New Description from JSONLoader v3.0.0: This List determines the Appearance Behaviors in which will be applied to this card. Use a newer version of JSONLoader for Modded Appearance Behaviors. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums
    /// </summary>
    [Tooltip("Items(true) | ItemType(String) | Enums(AddSnelkDecals, AlternatingBloodDecal, AnimatedPortrait, DynamicPortrait, FullCardPortrait, GiantAnimatedPortrait, GoldEmission, HologramPortrait, RareCardBackground, RareCardColors, SexyGoat, StaticGlitch, TerrainBackground, TerrainLayout, RedEmission, DefaultEmission, MoonParticleEffects) | UniqueItems(true)")]
    public List<string> appearanceBehavior;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string for the name of the card's image (must be .png). If it is in a subfolder within Artwork the subfolder should preceed the file name seperated by a '/' (or your system equivelent)" |
    ///
    /// New Description from JSONLoader v3.0.0: The Path to your cards Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image.
    /// </summary>
    [Tooltip("Pattern(^[a-zA-Z\\d_]+.png$)")]
    public string texture;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string for the name of the card's alternate image (must be .png)" |
    ///
    /// New Description from JSONLoader v3.0.0: The Path to your cards Alternative Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies in the case you have a Goat's Eye or possibly some other cases.
    /// </summary>
    [Tooltip("Pattern(^[a-zA-Z\\d_]+.png$)")]
    public string altTexture;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string for the name of the card's emission image (must be .png)" |
    ///
    /// New Description from JSONLoader v3.0.0: The Path to your cards Emissive Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies in the case you've transferred a sigil at the Sacrificial Stones onto this card.
    /// </summary>
    [Tooltip("Pattern(^[a-zA-Z\\d_]+.png$)")]
    public string emissionTexture;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string for the name of the card's title image (must be .png)" |
    ///
    /// New Description from JSONLoader v3.0.0: The Path to your cards Title Graphic, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '113x28' image. This applies specifically over your card name as a way of obscuring it like the Tentacle Cards are.
    /// </summary>
    [Tooltip("Pattern(^[a-zA-Z\\d_]+.png$)")]
    public string titleGraphic;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string for the name of the card's act2 image (must be .png)" |
    ///
    /// New Description from JSONLoader v3.0.0: The Path to your cards Pixel Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '41x28' image. This applies specifically in Act 2, its just that act's version of the card portrait.
    /// </summary>
    [Tooltip("Pattern(^[a-zA-Z\\d_]+.png$)")]
    public string pixelTexture;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "A string array for the texture names of a card decals (must be .png)" |
    ///
    /// New Description from JSONLoader v3.0.0: This is a list of all the Decal Images in which will be stacked onto your card, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '125x190' image.
    /// </summary>
    [Tooltip("Pattern(^[a-zA-Z\\d_]+.png$) | UniqueItems(true)")]
    public List<string> decals;
}

/// <summary>
/// An Object for the Custom Ability related Data for JSONLoader V1's Card system.
/// </summary>
[Serializable]
public class AbilityData
{
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "The name of the ability. This may be seperate form the name that appears in the book, check the mod description or ask in the discord for specifics" |
    ///
    /// New Description from JSONLoader v3.0.0: This is the In-Code name of the Ability.
    /// </summary>
    [Tooltip("REQUIRED | MinimumLength(1) | Pattern(^[a-zA-Z\\d_]+$)")]
    public string name;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "The GUID the mod maker made for their mod. This may be found in the mod description. It is usually in the layout of 'MakerName.inscryption.ModName'" |
    ///
    /// New Description from JSONLoader v3.0.0: This is the Ability Libraries GUID, it's a similar concept to your card's prefix.
    /// </summary>
    [Tooltip("REQUIRED | MinimumLength(1) | Pattern(^[a-zA-Z\\d_]+$)")]
    public string GUID;
}

/// <summary>
/// An Object for the Custom Special Ability related Data for JSONLoader V1's Card system.
/// </summary>
[Serializable]
public class SpecialAbilityData
{
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "The name of the special ability. This may be seperate form the name that appears in the book, check the mod description or ask in the discord for specifics" |
    ///
    /// New Description from JSONLoader v3.0.0: This is the In-Code name of the Special Ability.
    /// </summary>
    [Tooltip("REQUIRED | MinimumLength(1) | Pattern(^[a-zA-Z\\d_]+$)")]
    public string name;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "The GUID the mod maker made to identify their mod. This may be found in the mod description. It is usually in the layout of 'MakerName.inscryption.ModName'" |
    ///
    /// New Description from JSONLoader v3.0.0: This is the Special Ability Libraries GUID, it's a similar concept to your card's prefix.
    /// </summary>
    [Tooltip("REQUIRED | MinimumLength(1) | Pattern(^[a-zA-Z\\d_]+$)")]
    public string GUID;
}

/// <summary>
/// An Object for the Evolve related Data for JSONLoader V1's Card system.
/// </summary>
[Serializable]
public class EvolveData
{
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "The name of the card this card evolves into (See Card Names.txt for a list of ingame card names)" |
    ///
    /// New Description from JSONLoader v3.0.0: This represents the In-Code name of the card this card is meant to evolve into. 
    /// </summary>
    [Tooltip("REQUIRED | Pattern(^[a-zA-Z\\d_]+$)")]
    public string name;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "The number of turns til the card evolves (The game supports sigil art for up to 3 turns)" |
    ///
    /// New Description from JSONLoader v3.0.0: This value represents the amount of turns it takes for this card to evolve. This version's Turn Count must be between 1-3 for more use a newer version of JSONLoader.
    /// </summary>
    [Tooltip("REQUIRED | Default(1) | Minimum(1) | Maximum(3)")]
    public int turnsToEvolve;
}

/// <summary>
/// An Object for the Tail related Data for JSONLoader V1's Card system.
/// </summary>
[Serializable]
public class TailData
{
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "The name of the tail card this will produce (See Card Names.txt for a list of ingame card names)" |
    ///
    /// New Description from JSONLoader v3.0.0: This represents the In-Code name of the card this card will leave in its old lane if Loose Tail triggers.
    /// </summary>
    [Tooltip("REQUIRED | Pattern(^[a-zA-Z\\d_]+$)")]
    public string name;
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "The portrait the card should have once it's tail is lost" |
    ///
    /// New Description from JSONLoader v3.0.0: The Path to your cards Tail Lost Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies specifically when this card is struck and lost its tail.
    /// </summary>
    [Tooltip("Pattern(^[a-zA-Z\\d_]+.png$)")]
    public string tailLostPortrait;
}

/// <summary>
/// An Object for the IceCube related Data for JSONLoader V1's Card system.
/// </summary>
[Serializable]
public class IceCubeData
{
    /// <summary>
    /// Original Description from JSONLoader v1.7.2: "The name of the creature the card should turn into when it perishes (See Card Names.txt for a list of ingame card names)" |
    ///
    /// New Description from JSONLoader v3.0.0: This represents the In-Code name of the card this card will leave behind in its place when it is to die.
    /// </summary>
    [Tooltip("REQUIRED | Pattern(^[a-zA-Z\\d_]+$)")]
    public string creatureWithin;
}