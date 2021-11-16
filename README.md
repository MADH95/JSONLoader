# JSON Loader

This is a BepInEx plugin made for Incryption to create custom cards using JSON files and the API by Cyantist.
It can parse custom cards and pass them to APIPlugin to load them into the game.

## Installation

***Warning:*** Incompatible with thunderstore mod loader

To install this plugin you first need to install BepInEx as a mod loader for Inscryption. A guide to do this can be found [here](https://docs.bepinex.dev/articles/user_guide/installation/index.html#where-to-download-bepinex)

You will also need version 1.11+ of the [InscryptionAPI](https://github.com/ScottWilson0903/InscryptionAPI) plugin.

To install this mod, you simply need to put the **JSONLoader** folder in **BepInEx/plugins**.

## Custom Cards

To create your own cards you just create a .json file and fill in all the fields you want your card to have (fields you don't include will be defaulted). The *name* field is required, and the rest are optional with default values (though that would be a boring card). Those fields and their values are specified in the table below. For reference, an example custom card (8 f\*cking bears.json) is included in the **Cards** folder in this repo.

To edit existing cards, you similarly create a .json file and fill in the fields you want to edit on the card. You must include both the *name* field and the *fieldsToEdit* field with at least 1 field name in it (explained more on the table). Any fields you don't include in *fieldsToEdit* will not be changed from the base card.

You can use this [online JSON Schema validator](https://www.jsonschemavalidator.net/s/vGGNV7fO) to avoid syntax errors, and make sure the fields are correct in your json files.
There is also a [GUI](https://json-editor.github.io/json-editor/?data=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=) based verion that is an option, just copy the json from the right hand panel when done!

All cards (custom or edited) must have their json files in the **Cards** folder, and their art in the **Artwork** folder in the **JSONLoader** folder.

Cards have lots of fields that can be filled - this is a list of all field names and their purpose. The fields you wish to include in the json file should be copied exactly from this table, and any fields that refer to *Enum.txt* should have their strings be copied exactly from there.

**Note:** Fields that have a prefix followed by an underscore (e.g. "evolve_") must all be included together. You either include them all, or none of them.

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
| cost | **[Optional]** **[Default: 0]** An integer value for the blood cost of a card |
| bonesCost | **[Optional]** An integer value for the bones cost of a card |
| energyCost | **[Optional]** An integer value for the energy cost of a card |
| gemsCost | **[Optional]** A string array for the gems cost of a card (See *Enums.txt* for a list of gems) |
| specialStatIcon | **[Optional]** An string for which special stat icon the card has (See *Enums.txt* for a list of icons) |
| tribes | **[Optional]** An string array for the tribes the card belongs to (See *Enums.txt* for a list of tribes) |
| traits | **[Optional]** An string array for the traits a card has (See *Enums.txt* for a list of traits) |
| specialAbilities | **[Optional]** A string array for the special abilities a card has (See *Enums.txt* for a list of special abilities) |
| abilities | **[Optional]** A string array for the sigils a card has. (See *Enums.txt* for a list of sigil abilities). |
| evolution | **[Optional]** A json object for the evolveParams of the card. (It's children are in the table below this one) |
| defaultEvolutionName | **[Optional]** The name the card will have when it evolves (when it doesn't have evolve_ fields set) |
| tail | **[Optional]** A json object for the tailParams of the card. (It's children are in the table below this one) |
| iceCube | **[Optional]** A json object for the iceCubeParams of the card. (It's children are in the table below this one) |
| flipPortraitForStrafe | **[Optional]** A boolean to determine if the cards portrait should flip when it uses one of the strafe sigils |
| onePerDeck | **[Optional]** A boolean value that toggles if there can be only one of the card per deck |
| appearanceBehaviour | **[Optional]** A string array for the behaviours the cards appearance should have (See enums.txt for a list of appearance behaviours) |
| texture | **[Optional]** A string for the name of the card's image (including extension). If it is in a subfolder within *Artwork* the subfolder should preceed the file name seperated by a '/' (or your system equivelent) |
| altTexture | **[Optional]** A string for the name of the card's alternate image (including file extension) |
| titleGraphic | **[Optional]** A string for the name of the card's title image (including file extension) |
| pixelTexture | **[Optional]** A string for the name of the card's act2 image (including file extension) |
| animatedPortrait | **[Unavailable]** |
| decals | **[Optional]** A string array for the texture names of a card decals (including file extension) |

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

You can change **MADH95.inscryption.JSONLoader.cfg** (same location as above) to load up to 4 of your custom cards in the starting deck of a new run. You simply have to enable it by changing
```
## Load start deck with specified cards
# Setting type: Boolean
# Default value: false
TestDeck = false
```
to
```
## Load start deck with specified cards
# Setting type: Boolean
# Default value: false
TestDeck = true
```
then change the card names to the *name* parameter you set in the json file. The game should load a chapter 2 (one after the tutorial) run with those cards in the starting deck!

___

If you want help debugging you can find me on [Inscryption modding discord](https://discord.gg/QrJEF5Denm) as MADH95.

## Development

Plans for the future:
 - Add functionality for adding custom sigils loaded by other mods
 - Implement support for Thunderstore mod loader
 - Use a better Parser to allow for more Natural json files (i.e. param fields are objects with their own fields)
