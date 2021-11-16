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

You can use this [online JSON Schema validator](https://www.jsonschemavalidator.net/s/7TVTP3Y9) to avoid syntax errors, and make sure the fields are correct in your json files.
There is also a [GUI](https://tinyurl.com/asxfrfbc) based verion that is an option, just copy the json from the right hand panel when done!

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
