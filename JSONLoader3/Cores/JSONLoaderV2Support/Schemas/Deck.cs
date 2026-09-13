// using System.Collections.Generic;
// using UnityEngine;
//
// namespace JSONLoader3.Cores.JSONLoaderV2Support.Schemas;
//
// /// <summary>
// /// The main data Object for JSONLoader V2's Card system.
// /// </summary>
// public class Deck
// {
//     /// <summary>
//     /// A List of all Decks this Object holds.
//     /// </summary>
//     [Tooltip("REQUIRED | AdditionalProperties(false) | Items(true) | ItemType(object) | UniqueItems(false)")]
//     public List<DeckData> decks;
// }
//
// /// <summary>
// /// The data Object for each Individual Deck.
// /// </summary>
// public class DeckData
// {
//     [Tooltip("Items(True) | ItemType(String) | UniqueItems(true) | Enums(title, cards, iconTexture, unlockLevel)")]
//     public List<string> fieldsToEdit;
//     [Tooltip("REQUIRED | MinimumLength(1) | Pattern(^[a-zA-Z\\d_]+$)")]
//     public string name;
//     [Tooltip("MinimumLength(1) | Pattern(^[a-zA-Z\\d_]+$)")]
//     public string modPrefix;
//     [Tooltip("Default( )")]
//     public string title;
//     [Tooltip("Pattern(^[a-zA-Z\\d_]+$) | Items(True) | UniqueItems(true) | AnyOf([Title: Base Game Card, Description: Select a card from the Base Game., Type: String, Enums: !BOUNTYHUNTER_BASE > !BUILDACARD_BASE > !CORRUPTED > !DEATHCARD_BASE > !DEATHCARD_LESHY > !DEATHCARD_PIXEL_BASE > !DEATHCARD_VICTORY > !FRIENDCARD_BASE > !GIANTCARD_MOON > !GIANTCARD_SHIP > !INSPECTOR > !MELTER > !MYCO_OLD_DATA > !MYCOCARD_BASE > !STATIC!GLITCH > AboveCurve > Adder > AlarmBot > Alpha > Amalgam > Amoeba > Amoebot > Angler_Fish_Bad > Angler_Fish_Good > Angler_Fish_More > Angler_Talking > AnnoyTower > Ant > AntFlying > AntQueen > AquaSquirrel > AttackConduit > Automaton > BaitBucket > Banshee > Bat > BatteryBot > Beaver > Bee > Beehive > Bloodhound > BlueMage > BlueMage_Fused > BlueMage_Talking > BoltHound > Bombbot > BombMaiden > Bonehound > BonelordHorn > Bonepile > Boulder > BridgeRailing > BrokenBot > BrokenEgg > Bull > Bullfrog > BurrowingTrap > BustedPrinter > CagedWolf > CaptiveFile > CardMergeStones > Cat > CatUndead > CellBuff > CellGift > CellTri > CloserBot > Cockroach > CoinLeft > CoinRight > ConduitTower > Coyote > Cuckoo > CXformerAdder > CXformerElk > CXformerRaven > CXformerWolf > Dam > Daus > DausBell > DeadHand > DeadPets > DeadTree > DefaultTail > DireWolf > DireWolfCub > Draugr > DrownedSoul > DUMMY_5-5 > Elk > ElkCub > EmptyVessel > EmptyVessel_BlueGem > EmptyVessel_GreenGem > EmptyVessel_OrangeGem > EnergyConduit > EnergyRoller > FactoryConduit > Family > FieldMouse > FieldMouse_Fused > FlyingMage > ForceMage > FrankNStein > FrozenOpossum > Geck > GemExploder > GemFiend > GemRipper > GemsConduit > GemShielder > GhostShip > GiftBot > Goat > GoldNugget > Gravedigger > Gravedigger_Fused > GreenMage > Grizzly > Hawk > HeadlessHorseman > HealerConduit > Hodag > Hrokkall > Hydra > Ijiraq > Ijiraq_UnlockScreen > Insectodrone > JerseyDevil > JuniorSage > Kingfisher > Kraken > Lammergeier > LatcherBomb > LatcherBrittle > LatcherShield > LeapBot > Librarian > Lice > MageKnight > Maggots > Magpie > Mantis > MantisGod > MarrowMage > MasterBleene > MasterGoranj > MasterOrlu > MealWorm > MeatBot > MineCart > Mole > Mole_Telegrapher > MoleMan > MoleSeaman > Moose > Mothman_Stage1 > Mothman_Stage2 > Mothman_Stage3 > MoxDualBG > MoxDualGO > MoxDualOB > MoxEmerald > MoxRuby > MoxSapphire > MoxTriple > MudTurtle > Mule > Mummy > Mummy_Telegrapher > MuscleMage > Necromancer > NullConduit > Opossum > OrangeMage > Otter > Ouroboros > Ouroboros_Part3 > PackRat > PeltGolden > PeltHare > PeltWolf > PlasmaGunner > Porcupine > PracticeMage > PracticeMageSmall > Pronghorn > Pupil > Rabbit > Raccoon > RatKing > Rattler > Raven > RavenEgg > RedHart > Revenant > RingWorm > RoboMice > RoboSkeleton > RubyGolem > Salmon > Sarcophagus > SentinelBlue > SentinelGreen > SentinelOrange > SentryBot > SentryBot_Fused > Shark > Shieldbot > Shutterbug > Skeleton > SkeletonMage > SkeletonParrot > SkeletonPirate > Skink > SkinkTail > Skunk > Smoke > Smoke_Improved > Smoke_NoBones > Snapper > Snek_Neck > Snelk > Sniper > Sparrow > SquidBell > SquidCards > SquidMirror > Squirrel > SquirrelBall > Starvation > Steambot > StimMage > Stinkbug_Talking > Stoat > Stoat_Talking > Stump > SwapBot > Tadpole > Tail_Bird > Tail_Furry > Tail_Insect > TechMoxTriple > Thickbot > TombRobber > TombStone > Trap > TrapFrog > Tree > Tree_Hologram > Tree_Hologram_SnowCovered > Tree_SnowCovered > Urayuli > Vulture > Warren > Wolf > Wolf_Talking > WolfCub > Wolverine > XformerBatBeast > XformerBatBot > XformerGrizzlyBeast > XformerGrizzlyBot > XformerPorcupineBeast > XformerPorcupineBot > Zombie];[Title: Mod Added Card, Description: To add a card in which has been added by a mod use the following format {Mod Prefix}_{In-Code Card Name}., Type: String]")]
//     public List<string> cards;
//     [Tooltip("Pattern(^(?:(?:\\.\\.\\/|[a-zA-Z\\d_\\s-]+\\/)*[a-zA-Z\\d_\\s-]+\\.png|data:image\\/png;base64,[A-Za-z0-9+/]+={0,2}|base64:[A-Za-z0-9+/]+={0,2})$)")]
//     public string iconTexture;
//     [Tooltip("Default(0)")]
//     public int unlockLevel;
// }