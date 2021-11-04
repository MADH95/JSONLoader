# JSON Card Loader

This is a BepInEx plugin made for Incryption to create custom cards using JSON files and the API by Cyantist.
It can currently parse custom cards and pass them to APIPlugin to load them into the game. Cards with custom EvolveParams, TailParams, and IceCubeParams are not yet supported.

## Installation
###Requirements
To install this plugin you first need to install BepInEx as a mod loader for Inscryption. A guide to do this can be found [here](https://docs.bepinex.dev/articles/user_guide/installation/index.html#where-to-download-bepinex)

You will also need the [InscryptionAPI](https://github.com/ScottWilson0903/InscryptionAPI) plugin.

To install this mod, you simply need to put the **JSONCardLoaderPlugin.dll** in **Inscryption/BepInEx/plugins/CardLoader** beside the **APIPlugin.dll**.

## Custom Cards

To create your own cards you just create a .json file and fill in all the parameters you want your card to have (parameters you don't include will be defaulted). An example file is included in this repo.

All custom cards must have their json files be in a **Cards** folder, and their art in an **Artwork** folder in the same path as the dll.

Cards have lots of fields that can be edited - this is a list of all fields names and what they should include. The names must be exactly as written below into the json file.

| Parameter | Description |
|------|-------------|
| priority | Currently Unused |
| name | The name the game will use to identify the card - should contain no spaces |
| displayedName | The name displayed on the card |
| discription | The description Leshy gives when you find the card |
| metaCategories | An integer array of meta catagories the card has (See enums.txt for which values relate to what category) |
| cardComplexity | An integer value for the complexity of the card (See enums.txt for which values relate to which level of complexity) |
| temple | An integer value for which Scrybe created the card |
| baseAttack | An integer value for the attack of a card |
| baseHealth | An integer value for the health of a card |
| hideAttackAndHealth | A boolean value to toggle if the cards attack and health are visible |
| cost | An integer value for the blood cost of a card |
| bonesCost | An integer value for the bones cost of a card |
| energyCost | An integer value for the energy cost of a card |
| gemsCost | An integer array for the gems cost of a card (See enums.txt for what values relate to which gem) |
| specialStatIcon | An integer value for which special stat icon the card has (See enums.txt for which value relates to which icon) |
| tribes | An integer array for the tribes the card belongs to (See enums.txt for which values relate to which tribe) |
| traits | An integer array for the traits a card has (See enums.txt for which values relate to which trait) |
| specialAbilities | An integer array for the special abilities a card has (See enums.txt for which values relate to which special ability) |
| abilities | An integer array for the sigils a card has. (See enums.txt for which values relate to which sigil ability).
| evolveParams | Currently Unavailable |
| defaultEvolutionName | The default name the evolution will take |
| tailParams | Currently Unavailable |
| iceCubeParams | Currently Unavailable |
| flipPortraitForStrafe | A boolean to determine if the cards portrait should flip when it uses one of the strafe sigils |
| onePerDeck | A boolean value that toggles if there can be only one of the card per deck |
| appearanceBehaviour | An integer array for the behaviours the cards appearance should have (See enums.txt for which values relate to which appearance behaviours) |
| texture | A string for the image name. If it is in a subfolder within **Artwork** the subfolder can proceed the file name seperated by a '/' |
| altTexture | A string for cards alternate image name |
| titleGraphic | A string for the cards title image name (like Tentacle cards in act1) |
| pixelTexture | A string for the cards act2 image name |
| animatedPortrait | Currently Unavailable |
| decals | A string array for the texture names of a card decals | 

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

If you want help debugging you can find me on [Inscryption modding discord](https://discord.gg/QrJEF5Denm) as Cyantist.

## Development

Plans for the future:
 - Add functionality for updating existing cards
 - Add functionality for EvolveParams, TailParams, and IceCubeParams
 - Figure out how to Parse strings to enums for better usability
