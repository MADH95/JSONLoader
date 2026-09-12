using System;
using System.Collections.Generic;
using UnityEngine;

namespace JSONLoader3.Cores.JSONLoaderV2Support.Schemas;

/// <summary>
/// The main data Object for JSONLoader V2's Card system.
/// </summary>
public class Card
{
    /// <summary>
    /// Any items applied within this field will be used for overwriting the In-Game card associated with the field 'name'.
    /// </summary>
    [Tooltip("Items(True) | ItemType(string) | UniqueItems(true) | Enums(displayedName, description, metaCategories, cardComplexity, temple, baseAttack, baseHealth, hideAttackAndHealth, bloodCost, bonesCost, energyCost, gemsCost, specialStatIcon, tribes, traits, abilities, specialAbilities, evolveIntoName, evolveTurns, defaultEvolutionName, tailName, tailLostPortrait, iceCubeName, flipPortraitForStrafe, onePerDeck, appearanceBehaviour, texture, emissionTexture, altTexture, altEmissionTexture, pixelTexture, titleGraphic, decals, extensionProperties)")]
    public List<string> fieldsToEdit;
    /// <summary>
    /// The In-Code name for the card, when referencing this card, it is the piece that comes after the 'modPrefix' field.
    /// </summary>
    [Tooltip("REQUIRED | MinimumLength(1) | Pattern(^[a-zA-Z\\d_]+$)")]
    public string name;
    /// <summary>
    /// The In-Code identifier for the card, when referencing this card, it is the piece that comes before the 'name' field.
    /// </summary>
    [Tooltip("REQUIRED | MinimumLength(1) | Pattern(^[a-zA-Z\\d_]+$)")]
    public string modPrefix;
    /// <summary>
    /// The In-Game name for the card, it can be anything as long as this font can display it; https://font.download/font/heavyweight
    /// </summary>
    public string displayedName;
    /// <summary>
    /// The In-Game flavor for the card, this will show when receiving the card for the first time, if you want to prevent it being seen from saving use; https://thunderstore.io/c/inscryption/p/creator/Fuck_Dialouge_Saving/
    /// </summary>
    public string description;
    /// <summary>
    /// These Meta-Categories control how your card will show up within the game, see the following page for what each of them do; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums
    /// </summary>
    [Tooltip("Items(true) | UniqueItems(true) | AnyOf([Title: Base Game Meta Category, Description: A Meta Category from the Base Game, Type: string, Enums: ChoiceNode > TraderOffer > Part3Random > Rare > GBCPack > GBCPlayable > AscensionUnlock];[Title: Modded Meta Category, Description: Format is {Mod GUID}.{Meta Category Name}, Type: string])")]
    public List<string> metaCategories;
    /// <summary>
    /// This controls WHEN your card can show up in the game, see the following page for what each of them do; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums
    /// </summary>
    [Tooltip("Default(Vanilla) | Enums(Vanilla, Simple, Intermediate, Advanced)")]
    public string cardComplexity;
    /// <summary>
    /// This controls which temple in Act 2 the card is apart of, as well as meant to determine which Act outside Act 2 the card shows up in, whether mods follow the convention is up to question, but that's what these do. So, Nature is Act 1 and the Nature Temple, Tech is Act 3 and the Technology Temple, Undead is the Grimora Portion of the Finale and the Undead Temple, Wizard is the Magnificus Portion of the Finale and the Magicks Temple.
    /// </summary>
    [Tooltip("Default(Nature) | Enums(Nature, Undead, Tech, Wizard)")]
    public string temple;
    /// <summary>
    /// This value determines the attack value of the card, it cannot be negative.
    /// </summary>
    [Tooltip("Default(0) | Minimum(0)")]
    public int baseAttack;
    /// <summary>
    /// This value determines the health value of the card, it cannot be negative or 0.
    /// </summary>
    [Tooltip("Default(1) | Minimum(0)")]
    public int baseHealth;
    /// <summary>
    /// This boolean value determines whether the Attack and Health of the card should be hidden or not.
    /// </summary>
    [Tooltip("Default(false)")]
    public bool hideAttackAndHealth;
    /// <summary>
    /// This value determines the amount of Blood this card will cost.
    /// </summary>
    [Tooltip("Default(0) | Minimum(0)")]
    public int bloodCost;
    /// <summary>
    /// This value determines the amount of Bones this card will cost.
    /// </summary>
    [Tooltip("Default(0) | Minimum(0)")]
    public int bonesCost;
    /// <summary>
    /// This value determines the amount of Energy this card will cost.
    /// </summary>
    [Tooltip("Default(0) | Minimum(0)")]
    public int energyCost;
    /// <summary>
    /// The following 3 values are accepted here: Green for the Green Gem, Orange for the Orange Gem, and Blue for the Blue Gem. Each of these correlates to the Gem Cost of a card. This version of JSONLoader does not support multiple of the same color of gem.
    /// </summary>
    [Tooltip("Items(true) | ItemType(String) | Enums(Green, Orange, Blue) | UniqueItems(true)")]
    public List<string> gemsCost;
    /// <summary>
    /// This determines which Stat Icon to show on the card, this must be used alongside the associated Special Ability.
    /// </summary>
    [Tooltip("AnyOf([Title: Base Game Special Stat Icon, Description: A Special Stat Icon from the Base Game, Type: string, Enums: None > Ants > Bones > Mirror > Bell > GreenGems > CardsInHand > SacrificesThisTurn];[Title: Modded Special Stat Icon, Description: Format is {Mod GUID}.{Special Stat Icon Name}, Type: string])")]
    public string specialStatIcon;
    /// <summary>
    /// This List determines what Tribes are applied to the card. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums
    /// </summary>
    [Tooltip("Items(true) | UniqueItems(true) | AnyOf([Title: Base Game Tribe, Description: A Tribe from the Base Game, Type: string, Enums: None > Squirrel > Bird > Canine > Hooved > Reptile > Insect];[Title: Modded Tribe, Description: Format is {Mod GUID}.{Tribe Name}, Type: string])")]
    public List<string> tribes;
    /// <summary>
    /// This List determines what Traits are applied to this card. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums
    /// </summary>
    [Tooltip("Items(true) | UniqueItems(true) | AnyOf([Title: Base Game Trait, Description: A Trait from the Base Game, Type: string, Enums: None > EatsWarrens > FeedsStoat > LikesHoney > Wolf > Bear > Juvenile > ProtectsCub > Undead > Structure > Blind > Ant > Terrain > Pelt > Uncuttable > SatisfiesRingTrial > Giant > Gem > Fused > KillsSurvivors > Goat > DeathcardCreationNonOption > Lice];[Title: Modded Trait, Description: Format is {Mod GUID}.{Trait Name}, Type: string])")]
    public List<string> traits;
    /// <summary>
    /// This List determines what Abilities are applied to this card. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums
    /// </summary>
    [Tooltip("Items(true) | UniqueItems(true) | AnyOf([Title: Base Game Ability, Description: A Ability from the Base Game, Type: string, Enums: None > DrawRabbits > BeesOnHit > Strafe > Deathtouch > Evolve > CreateDams > Tutor > WhackAMole > DrawCopy > TailOnHit > CorpseEater > QuadrupleBones > Submerge > DrawCopyOnDeath > Sharp > StrafePush > DrawAnt > GuardDog > Flying > Sacrificial > PreventAttack > TripleBlood > Reach > SplitStrike > TriStrike > IceCube > Sinkhole > BoneDigger > RandomConsumable > SteelTrap > RandomAbility > SquirrelOrbit > AllStrike > BuffNeighbours > Brittle > SkeletonStrafe > GainGemGreen > GainGemOrange > GainGemBlue > BuffGems > DropRubyOnDeath > GemsDraw > GemDependant > GainGemTriple > DrawNewHand > SquirrelStrafe > ConduitBuffAttack > ConduitFactory > ConduitHeal > ConduitNull > GainBattery > ExplodeOnDeath > Sniper > DeathShield > PermaDeath > LatchExplodeOnDeath > LatchBrittle > LatchDeathShield > FileSizeDamage > DeleteFile > Transformer > Sentry > ExplodeGems > ShieldGems > DrawVesselOnHit > ConduitEnergy > BombSpawner > DoubleDeath > ActivatedRandomPowerEnergy > ActivatedRandomPowerBone > ActivatedStatsUp > SwapStats > ActivatedDrawSkeleton > ActivatedDealDamage > CreateBells > BuffEnemy > ConduitSpawnGems > DrawRandomCardOnDeath > Loot > ActivatedSacrificeDrawCards > ActivatedStatsUpEnergy > ActivatedHeal > DebuffEnemy > CellBuffSelf > CellDrawRandomCardOnDeath > CellTriStrike > ActivatedEnergyToBones > MoveBeside > SubmergeSquid > BloodGuzzler > Haunter > ExplodingCorpse > Apparition > VirtualReality > EdaxioHead > EdaxioArms > EdaxioLegs > EdaxioTorso > CreateEgg > DoubleStrike > OpponentBones > StrafeSwap > Morsel > GainAttackOnKill > MadeOfStone > HydraEgg];[Title: Modded Ability, Description: Format is {Mod GUID}.{Ability Name}, Type: string])")]
    public List<string> abilities;
    /// <summary>
    /// This List determines what Special Abilities are applied to this card. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums
    /// </summary>
    [Tooltip("Items(true) | UniqueItems(true) | AnyOf([Title: Base Game Special Ability, Description: A Special Ability from the Base Game, Type: string, Enums: None > Cat > EMPTY3 > EMPTY4 > EMPTY5 > EMPTY6 > Ant > RandomCard > Lammergeier > TalkingCardChooser > PackMule > Mirror > BellProximity > CagedWolf > TrapSpawner > GiantCard > GiantMoon > GreenMage > JerseyDevil > Daus > BountyHunter > BrokenCoinLeft > BrokenCoinRight > CardsInHand > Ouroboros > Shapeshifter > SacrificesThisTurn > SpawnLice > GiantShip];[Title: Modded Special Ability, Description: Format is {Mod GUID}.{Special Ability Name}, Type: string])")]
    public List<string> specialAbilities;
    /// <summary>
    /// This represents the In-Code name of the card this card is meant to evolve into. It should match the following: [Mod Prefix]_[Name].
    /// </summary>
    [Tooltip("Pattern(^[a-zA-Z\\d_]+$)")]
    public string evolveIntoName;
    /// <summary>
    /// This value represents the amount of turns it takes for this card to evolve. This version's Turn Count must be greater than 1.
    /// </summary>
    [Tooltip("Default(1) | Minimum(1)")]
    public int evolveTurns;
    /// <summary>
    /// This determines what the Default Evolution Name will be, note it will appear in the format of; '[defaultEvolutionName] [displayedName]', just replace the variables with your JSON's values.
    /// </summary>
    public string defaultEvolutionName;
    /// <summary>
    /// This represents the In-Code name of the card this card will leave in its old lane if Loose Tail triggers. It should match the following: [Mod Prefix]_[Name].
    /// </summary>
    [Tooltip("Pattern(^[a-zA-Z\\d_]+$)")]
    public string tailName;
    /// <summary>
    /// The Path to your cards Tail Lost Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies specifically when this card is struck and lost its tail.
    /// </summary>
    [Tooltip("Pattern(^(?:(?:\\.\\.\\/|[a-zA-Z\\d_\\s-]+\\/)*[a-zA-Z\\d_\\s-]+\\.png|data:image\\/png;base64,[A-Za-z0-9+/]+={0,2}|base64:[A-Za-z0-9+/]+={0,2})$)")]
    public string tailLostPortrait;
    /// <summary>
    /// This represents the In-Code name of the card this card will leave behind in its place when it is to die. It should match the following: [Mod Prefix]_[Name].
    /// </summary>
    [Tooltip("Pattern(^[a-zA-Z\\d_]+$)")]
    public string iceCubeName;
    /// <summary>
    /// A bool determining whether this cards portrait will flip when the card moves. (like the sigil icon does)
    /// </summary>
    [Tooltip("Default(false)")]
    public bool flipPortraitForStrafe;
    /// <summary>
    /// A bool determining if there can only be one copy of this card within the Player's deck.
    /// </summary>
    [Tooltip("Default(false)")]
    public bool onePerDeck;
    /// <summary>
    /// This List determines the Appearance Behaviors in which will be applied to this card. Use a newer version of JSONLoader for Modded Appearance Behaviors. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums
    /// </summary>
    [Tooltip("Items(true) | UniqueItems(true) | AnyOf([Title: Base Game Appearance Behaviour, Description: An Appearance Behaviour from the Base Game, Type: string, Enums: StaticGlitch > FullCardPortrait > TerrainBackground > RareCardColors > AddSnelkDecals > AnimatedPortrait > GoldEmission > RareCardBackground > AlternatingBloodDecal > TerrainLayout > DynamicPortrait > GiantAnimatedPortrait > HologramPortrait > SexyGoat > RedEmission > DefaultEmission > MoonParticleEffects];[Title: Modded Appearance Behaviour, Description: Format is {Mod GUID}.{Appearance Behaviour Name}, Type: string])")]
    public List<string> appearanceBehaviour;
    /// <summary>
    /// The Path to your cards Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image.
    /// </summary>
    [Tooltip("Pattern(^(?:(?:\\.\\.\\/|[a-zA-Z\\d_\\s-]+\\/)*[a-zA-Z\\d_\\s-]+\\.png|data:image\\/png;base64,[A-Za-z0-9+/]+={0,2}|base64:[A-Za-z0-9+/]+={0,2})$)")]
    public string texture;
    /// <summary>
    /// The Path to your cards Emissive Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies in the case you've transferred a sigil at the Sacrificial Stones onto this card.
    /// </summary>
    [Tooltip("Pattern(^(?:(?:\\.\\.\\/|[a-zA-Z\\d_\\s-]+\\/)*[a-zA-Z\\d_\\s-]+\\.png|data:image\\/png;base64,[A-Za-z0-9+/]+={0,2}|base64:[A-Za-z0-9+/]+={0,2})$)")]
    public string emissionTexture;
    /// <summary>
    /// The Path to your cards Alternative Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies in the case you have a Goat's Eye or possibly some other cases.
    /// </summary>
    [Tooltip("Pattern(^(?:(?:\\.\\.\\/|[a-zA-Z\\d_\\s-]+\\/)*[a-zA-Z\\d_\\s-]+\\.png|data:image\\/png;base64,[A-Za-z0-9+/]+={0,2}|base64:[A-Za-z0-9+/]+={0,2})$)")]
    public string altTexture;
    /// <summary>
    /// The Path to your cards Alternative Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies in the case you have a Goat's Eye or possibly some other cases and, you've transferred a sigil at the Sacrificial Stones onto this card.
    /// </summary>
    [Tooltip("Pattern(^(?:(?:\\.\\.\\/|[a-zA-Z\\d_\\s-]+\\/)*[a-zA-Z\\d_\\s-]+\\.png|data:image\\/png;base64,[A-Za-z0-9+/]+={0,2}|base64:[A-Za-z0-9+/]+={0,2})$)")]
    public string altEmissionTexture;
    /// <summary>
    /// The Path to your cards Pixel Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '41x28' image. This applies specifically in Act 2, its just that act's version of the card portrait.
    /// </summary>
    [Tooltip("Pattern(^(?:(?:\\.\\.\\/|[a-zA-Z\\d_\\s-]+\\/)*[a-zA-Z\\d_\\s-]+\\.png|data:image\\/png;base64,[A-Za-z0-9+/]+={0,2}|base64:[A-Za-z0-9+/]+={0,2})$)")]
    public string pixelTexture;
    /// <summary>
    /// The Path to your cards Title Graphic, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '113x28' image. This applies specifically over your card name as a way of obscuring it like the Tentacle Cards are.
    /// </summary>
    [Tooltip("Pattern(^(?:(?:\\.\\.\\/|[a-zA-Z\\d_\\s-]+\\/)*[a-zA-Z\\d_\\s-]+\\.png|data:image\\/png;base64,[A-Za-z0-9+/]+={0,2}|base64:[A-Za-z0-9+/]+={0,2})$)")]
    public string titleGraphic;
    /// <summary>
    /// This is a list of all the Decal Images in which will be stacked onto your card, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '125x190' image.
    /// </summary>
    [Tooltip("Items(True) | ItemType(string) | Pattern(^(?:(?:\\.\\.\\/|[a-zA-Z\\d_\\s-]+\\/)*[a-zA-Z\\d_\\s-]+\\.png|data:image\\/png;base64,[A-Za-z0-9+/]+={0,2}|base64:[A-Za-z0-9+/]+={0,2})$) | UniqueItems(true)")]
    public List<string> decals;
    /// <summary>
    /// This is a list of all Extended Properties to this Card. You'll need to supply your own Field:Value pairs according to the mods specifications. If using the Editor, hit edit by the property to edit this Object.
    /// </summary>
    [Tooltip("AdditionalProperties(string)")]
    public object extensionProperties;
}