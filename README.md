# JSON Card Loader

This is a BepInEx plugin made for Incryption to create custom cards using JSON files and the API by Cyantist.
It can parse custom cards and pass them to APIPlugin to load them into the game.

## Installation

To install this plugin you first need to install BepInEx as a mod loader for Inscryption. A guide to do this can be found [here](https://docs.bepinex.dev/articles/user_guide/installation/index.html#where-to-download-bepinex)

You will also need a version of the [InscryptionAPI](https://github.com/ScottWilson0903/InscryptionAPI) plugin. (v1.2.0 and below require version 1.7.2 of API, v1.2.1 is compatable with version 1.8+ of API)

To install this mod, you simply need to put the **JSONLoader** folder in **BepInEx/plugins**.

## Custom Cards

To create your own cards you just create a .json file and fill in all the fields you want your card to have (fields you don't include will be defaulted). Some fields are required, and some are optional with default values. Those fields and their values are specified in the table below. For reference, an example card is included in this repo.

You can use this [online JSON IDE](https://jsoneditoronline.org/#left=local.duxaza&right=local.napoto) to avoid syntax errors in your json files.

All custom cards must have their json files in a **Cards** folder, and their art in an **Artwork** folder in the **JSONLoader** folder.

Cards have lots of fields that can be edited - this is a list of all field names and their purpose. The fields you wish to include in the json file should be copied exactly from this table, and any fields that refer to *Enum.txt* should have their strings be copied exactly from there.

**Note:** Fields that have a prefix followed by an underscore (e.g. "evolve_") must all be included together. You either include them all, or none of them.

| Parameter | Description |
|------|-------------|
| priority | **[Unused]** |
| name | **[Required]** A string for the name the game will use to identify the card - should contain no spaces |
| displayedName | **[Required]** A string for the name displayed on the card |
| description | **[Required]** A string for the description Leshy gives when you find the card |
| metaCategories | **[Required]** A string array of meta catagories the card has (See *Enums.txt* for a list of catagories) |
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
| abilities | **[Optional]** A string array for the sigils a card has. (See *Enums.txt* for a list of sigil abilities).
| evolve_evolutionName | **[Optional]** The name field of the card this will evolve into (See *Card Names.txt* for a list of ingame card names) |
| evolve_turnsToEvolve | **[Optional]** The number of turns the card will take to evolve. The game only supports are for 1-3 but can permit higher numbers |
| defaultEvolutionName | **[Optional]** The name the card will have when it evolves (when it doesn't have evolve_ fields set) |
| tail_cardName | **[Optional]** The name field of the tail card this will produce (See *Card Names.txt* for a list of ingame card names) |
| tail_tailLostPortrait | **[Optional]** The portrait the card should have once it's tail is lost |
| iceCube_creatureWithin | **[Optional]** The name field of the creature the card should turn into when it perishes (See *Card Names.txt* for a list of ingame card names) |
| flipPortraitForStrafe | **[Optional]** A boolean to determine if the cards portrait should flip when it uses one of the strafe sigils |
| onePerDeck | **[Optional]** A boolean value that toggles if there can be only one of the card per deck |
| appearanceBehaviour | **[Optional]** A string array for the behaviours the cards appearance should have (See enums.txt for a list of appearance behaviours) |
| texture | **[Optional]** A string for the name of the card's image (including extension). If it is in a subfolder within *Artwork* the subfolder should preceed the file name seperated by a '/' (or your system equivelent) |
| altTexture | **[Optional]** A string for the name of the card's alternate image (including file extension) |
| titleGraphic | **[Optional]** A string for the name of the card's title image (including file extension) |
| pixelTexture | **[Optional]** A string for the name of the card's act2 image (including file extension) |
| animatedPortrait | **[Unavailable]** |
| decals | **[Optional]** A string array for the texture names of a card decals (including file extension) |

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
 - Add functionality for updating existing cards
 - Add functionality for adding custom sigils loaded by other mods
