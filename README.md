# JSON Loader

This is a BepInEx plugin made for Incryption to create custom cards, encounters, and regions using JSON files and the API by Cyantist.
It can parse custom cards, encounters, and regions, passing them to APIPlugin to load them into the game.

## Converting Existing Cards

In order to have compatability with mod manager, version 1.7.0 breaks backwards compatability. Don't worry though! all the contents of your files are fine, the only change is the extension on the end. the new extension JSONLoader looks for is .jldr, so you'll need to start making cards with that extension instead of .json (again, the **contents** are still in the json format). If you have a lot of cards to change, you can use the json to jldr [converter utility](https://inscryption.thunderstore.io/package/MADH95Mods/JSONRenameUtility/) to convert all cards in a directory from .json to .jldr!

## Installation

### Automatic
Simply download with Thunderstore Mod Manager!

### Manual
To install this plugin you first need to install BepInEx as a mod loader for Inscryption. A guide to do this can be found [here](https://docs.bepinex.dev/articles/user_guide/installation/index.html#where-to-download-bepinex)

You will also need version 1.13+ of the [InscryptionAPI](https://github.com/ScottWilson0903/InscryptionAPI) plugin.

Finally, you simply need to put the **JSONLoader.dll** folder in **BepInEx/plugins**.

## Custom Cards

To create your own cards you just create a .jldr file (written in JSON) with the '\_card' postfix *(e.g. MyCard_card.jldr)* and fill in all the fields you want your card to have (fields you don't include will be defaulted). The *name* field is required, and the rest are optional with default values (though that would be a boring card). Those fields and their values are specified in the table below. For reference, an example custom card (NewCard_card_example.jldr) is included in the **Cards** folder in this repo.

To edit existing cards, you similarly create a .jldr file and fill in the fields you want to edit on the card. You must include both the *name* field and the *fieldsToEdit* field with at least 1 field name in it (explained more on the table). Any fields you don't include in *fieldsToEdit* will not be changed from the base card.

You can use this [online JSON Schema validator](https://www.jsonschemavalidator.net/s/D3dmDn7L) to avoid syntax errors, and make sure the fields are correct in your json files.
There is also a [GUI](https://tinyurl.com/asxfrfbc) based verion that is an option, just copy the json from the right hand panel when done!

Files go anywhere in the plugins folder, along with the artwork required for the card.

Cards have lots of fields that can be filled - this is a list of all field names and their purpose. The fields you wish to include in the .jldr file should be copied exactly from this table, and any fields that refer to *Enums.md* or *Card Names.txt* should have their strings be copied exactly from there.

| Field | Description |
|------|-------------|
| fieldsToEdit | **[Required: Editing]** A string array for the fields you wish to edit. The fields must be the exact names as in the right hand side of this table |
| name | **[Required]** A string for the name the game will use to identify the card - should contain no spaces. When editing, this field must match the card's name (See *Card Names.txt* for a list of ingame card names) |
| displayedName | **[Optional]** **[Default: ""]** A string for the name displayed on the card |
| description | **[Optional]** **[Default: ""]** A string for the description Leshy gives when you find the card |
| metaCategories | **[Optional]** A string array of meta catagories the card has (See *Enums.txt* for a list of catagories) |
| cardComplexity | **[Optional]** **[Default: Vanilla]** A string for the complexity of the card (See *Enums.txt* for a list of levels of complexity) |
| temple | **[Optional]** **[Default: Nature]** A string for which Scrybe created the card |
| baseAttack | **[Optional]** **[Default: 0]** An integer value for the attack of a card |
| baseHealth | **[Optional]** **[Default: 1]** An integer value for the health of a card |
| hideAttackAndHealth | **[Default: false]** A boolean value to toggle if the cards attack and health are visible |
| bloodCost | **[Optional]** **[Default: 0]** An integer value for the blood cost of a card |
| bonesCost | **[Optional]** An integer value for the bones cost of a card |
| energyCost | **[Optional]** An integer value for the energy cost of a card |
| gemsCost | **[Optional]** A string array for the gems cost of a card (See *Enums.txt* for a list of gems) |
| specialStatIcon | **[Optional]** An string for which special stat icon the card has (See *Enums.txt* for a list of icons) |
| tribes | **[Optional]** An string array for the tribes the card belongs to (See *Enums.txt* for a list of tribes) |
| traits | **[Optional]** An string array for the traits a card has (See *Enums.txt* for a list of traits) |
| specialAbilities | **[Optional]** A string array for the special abilities a card has (See *Enums.txt* for a list of special abilities) |
| abilities | **[Optional]** A string array for the sigils a card has. (See *Enums.txt* for a list of sigil abilities). |
| customAbilities | **[Optional]** An array of objects for the custom ability name and mod GUID (It's children are in the table below this one) |
| customSpecialAbilities | **[Optional]** An array of objects for the custom special ability name and mod GUID (It's children are in the table below this one) |
| evolution | **[Optional]** A json object for the evolveParams of the card. (It's children are in the table below this one) |
| defaultEvolutionName | **[Optional]** The name the card will have when it evolves (when it doesn't have evolve_ fields set) |
| tail | **[Optional]** A json object for the tailParams of the card. (It's children are in the table below this one) |
| iceCube | **[Optional]** A json object for the iceCubeParams of the card. (It's children are in the table below this one) |
| flipPortraitForStrafe | **[Optional]** A boolean to determine if the cards portrait should flip when it uses one of the strafe sigils |
| onePerDeck | **[Optional]** A boolean value that toggles if there can be only one of the card per deck |
| appearanceBehaviour | **[Optional]** A string array for the behaviours the cards appearance should have (See enums.txt for a list of appearance behaviours) |
| texture | **[Optional]** A string for the name of the card's image (must be .png). If it is in a subfolder within *Artwork* the subfolder should preceed the file name seperated by a '/' (or your system equivelent) |
| altTexture | **[Optional]** A string for the name of the card's alternate image (must be .png) |
| emissionTexture | **[Optional]** A string for the name of the card's emission image (must be .png) |
| titleGraphic | **[Optional]** A string for the name of the card's title image (must be .png) |
| pixelTexture | **[Optional]** A string for the name of the card's act2 image (must be .png) |
| animatedPortrait | **[Unavailable]** |
| decals | **[Optional]** A string array for the texture names of a card decals (must be .png) |
___

### Custom Ability fields

| Field | Description |
|-|-|
| name | The name of the ability. This may be seperate form the name that appears in the book, check the mod description or ask in the discord for specifics |
| GUID | The GUID the mod maker made for their mod. This may be found in the mod description. It is usually in the layout of "MakerName.inscryption.ModName" |

___
### Custom Ability fields

| Field | Description |
|-|-|
| name | The name of the special ability. This may be seperate form the name that appears in the book, check the mod description or ask in the discord for specifics |
| GUID | The GUID the mod maker made to identify their mod. This may be found in the mod description. It is usually in the layout of "MakerName.inscryption.ModName" |

___
### Evolution fields

| Field | Description |
|-|-|
| name | The name of the card this card evolves into (See *Card Names.txt* for a list of ingame card names) |
| turnsToEvolve | The number of turns til the card evolves (The game supports sigil art for up to 3 turns) |

___
### Tail fields
| Field | Description |
|-|-|
| name | The name of the tail card this will produce (See *Card Names.txt* for a list of ingame card names) |
| tailLostPortrait | The portrait the card should have once it's tail is lost |

### IceCube fields
| Field | Description |
|-|-|
| creatureWithin | The name of the creature the card should turn into when it perishes (See *Card Names.txt* for a list of ingame card names) |

## Custom Encounters

For custom encounters, you create a .jldr file with the '\_encounter' postfix and fill in the desired fields. The *name* and *regions* values are required, the other fields are optional. You can find the list of fields in the table below. Currently, encounters can only be added to Act 1. For reference, an example custom encounter (squirrel template_encounter.jldr) is included in the **Encounters** folder in this repo.

| Field | Description |
|------|-------------|
| name | **[Required]** A string for the name the API will use to identify the encounter |
| regions | **[Required\*]** A string array of the regions this encounter will appear in (See *Enums.txt* for a list of regions) |
| dominantTribes | **[Optional]** A string array tribes mainly appearing in this encounter. Used for determining the totem in totem encounters |
| minDifficulty | **[Optional]** **[Default: 0]** The minimum difficulty at which this encounter will appear |
| maxDifficulty | **[Optional]** **[Default: 30]** The maximum difficulty at which this encounter will appear |
| turns | **[Optional]** A 2-dimensional array of CardBlueprint json objects for cards played each turn |
| randomReplacementCards | **[Optional]** A string array of cards that randomly replace the cards that have a randomReplacementChance larger than 0. Cards in this list need to be unlocked to appear |
| regular | **[Optional]** **[Default: opposite of bossPrep]** A boolean to determine whether this encounter appears in regular encounters |
| bossPrep | **[Optional]** **[Default: false]** A boolean to determine whether this encounter appears as the boss prep encounter of the given regions (the final encounter before the boss, if the requirements are met) |
| turnMods | **[Unsupported/Act 3 only]** An array of TurnMod json objects |
| redundantAbilities | **[Optional]** A string array of abilities that will not show up on totems for this encounter (Note that tribes have specific redundant abilities by default) |
| unlockedCardPrerequisites | **[Optional]** A string array of cards that need to be unlocked for this encounter to appear |

*\* You can leave regions empty, but if you do so, the encounter will never appear.*
___

### CardBlueprint fields
| Field | Description |
|-|-|
| card | **[Optional]** The name of the card that will be played (See *Card Names.txt* for a list of ingame card names) |
| replacement | **[Optional]** A json object for the replacement of the card |
| randomReplaceChance | **[Optional]** **[Default: 0]** The interger percentage chance that this card will be replaced by a card from *randomReplacementCards*  |

### Replacement fields
| Field | Description |
|-|-|
| card | The name of the card that will replace the original one (See *Card Names.txt* for a list of ingame card names) |
| randomReplaceChance | **[Default: 0]** The minimum difficulty at which this encounter will replace the original one |
___

### TurnMod fields [Unsupported/Act 3 only]
| Field | Description |
|-|-|
| turn | **[Optional]** **[Default: 0]** The turn at which this mod is applied  |
| applyAtDifficulty | **[Optional]** **[Default: 0]** The minimum difficulty at which this mod will apply |
| overclockCards | **[Optional]**  A boolean to determine whether cards this turn are overclocked |

## Custom Regions

For custom regions, you create a .jldr file with the '\_region' postfix and fill in the desired fields. The *name* and *tier* values are required, the other fields are optional. You can find the list of fields in the table below. Only Act 1 regions are supported. For reference, an example region (void_region.jldr) is included in the **Regions** folder in this repo, as well as a encounter for this region (void_encounter.jldr) in the **Encounters** folder.  
If not defined, the default values of the vanilla region of the given tier will be used.
| Field | Description |
|------|-------------|
| name | **[Required]** A string for the name the API will use to identify the region |
| tier | **[Required]** The tier this region will be added to, where 0 is the first map and 3 is the cabin |
| terrainCards | **[Optional]** A string array of cards that randomly appear on the board Cards in this list need to have the Terrain trait to appear |
| likelyCards | **[Optional]** A string array of cards that appear more often in choice nodes. Cards in this list need to have the ChoiceNode metacategory to appear |
| boardLightColor | **[Optional]** The hexadecimal RGBA color of the board light, prefixed by '#' |
| cardsLightColor | **[Optional]** The hexadecimal RGBA color of the cards light, prefixed by '#' |
| dustParticlesDisabled | **[Optional]** **[Default: false]** A boolean to determine whether to disable the dust particles |
| fogEnabled | **[Optional]** **[Default: false]** A boolean to determine whether to enable fog |
| fogAlpha | **[Optional]** A float to determine the alpha value of the fog |
| mapEmission | **[Optional]** A string for the texture of the emitted particles on the map (must be .png) |
| mapEmissionColor | **[Optional]** The hexadecimal RGBA color of the emitted particles, prefixed by '#' |
| silenceCabinAmbience | **[Optional]** A boolean to determine whether the 'silence cabin ambience' is played |
___



## Debugging
The easiest way to check if the plugin is working properly or to debug an error is to enable the console. This can be done by changing
```
[Logging.Console]
\## Enables showing a console for log output.
\# Setting type: Boolean
\# Default value: false
Enabled = false
```
to
```
[Logging.Console]
\## Enables showing a console for log output.
\# Setting type: Boolean
\# Default value: false
Enabled = true
```
in **Inscryption/BepInEx/Config/BepInEx.cfg**

___

To add cards to your starting hand to test if your cards work, you can download my [deck building mod](https://inscryption.thunderstore.io/package/MADH95Mods/DeckbuilderMod/)

___

If you want help debugging you can ask in the #card-creation channel in the [Inscryption modding discord](https://discord.gg/QrJEF5Denm).

## Development

Plans for the future:
 - Boss encounters
 - Modular abilities
