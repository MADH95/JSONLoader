# JSON Loader V2

This is a BepInEx plugin made for Incryption to create custom cards using JSON files and the API.
It can parse custom cards and pass them to APIPlugin to load them into the game.

Version 2.0 of this mod is designed to create full compatibility with Version 2.0+ of the API. JSON card files compatible with this API will have an extension of .JLDR2

## Reloading

In the newest version of JSON Loader you can press "shift + r" to apply the changes you made to your cards and/or sigils without restarting the game.

## Configils

Besides cards JSON Loader also allows you to create sigils. To do this, your file needs to end in '_sigil.jldr2'.

Here is the [documentation](https://docs.google.com/document/d/1QLAfomaTcatm-foU2P1ZoqGQFFvhCfmEnN4jIxAWceQ/edit?usp=sharing) for making sigils.

## Talking Cards

The latest version of JSONLoader allows you to create talking cards! To do this, your file needs to end in '\_talk.jldr2'.

All of the documentation for that can be found [here](https://github.com/KBMackenzie/InscryptionJSONDump/blob/main/Documentation/Talking_Card_Guide.md)!

## Converting Existing Cards to JLDR2

In order to have compatability with mod manager, version 2.0.0 breaks backwards compatability by default. Don't worry though! This mod comes with a backwards compatibility mode that can optionally be turned on. When turned on, all existing .JLDR files will be read in, converted to .JLDR2 files, and dumped back into the folder where the original JLDR was found. However, I cannot guarantee that the version will be 100% correct, especially when it comes to assigning the appropriate prefix to the card (see the first heading below under 'Custom Cards').

If you are a card creator, the best course of action is to set the config option to convert all JLDR files, manually inspect the JLDR2 files generated to ensure they are correct, then publish your mod with a brand new set of JLDR2 files.

For the most part, JLDR2 and JLDR are the same. The key differences are outlined here:

### Abilities and Special Abilities

Previously, base game abilities and mod-added abilities were handled differently; they were completely separate parts of the file. Now, base game abilities and mod-added abilities are kept in the same list. Base game abilities are referred to by their enumerated name, such as "Flying" or "Reach." Mod-added abilities are referred to by a combination of the Mod GUID and their name. For example, the "Deathburst" ability from Void's sigil pack (part of the popular "All the Sigils" mod) is referred by the string "extraVoid.inscryption.voidSigils.Deathburst"; in this example, "Deathburst" is the name of the ability, and "extraVoid.inscryption.voidSigils" is the GUID of the mod. 

So to create a card with both Flying and Deathburst, you would do something like the following:

```json
"abilities": [ "Flying", "extraVoid.inscryption.voidSigils.Deathburst" ]
```

This holds true for all custom enumerations on a card. In the case of Abilities and Special Abilities, this is very different behaviour from previous versions of JSON Loader. However, all enumerations are treated this way. For example, mod added Traits, Metacategories, special stat icons, etc can all be handled exactly the same way.

### Evolve, Ice Cube, and Tail

Previously, these parameters were set as individual objects on the card. In a JLDR2, they have been "flattened" into the main card definition. The best way to understand this is to see an example:

**JLDR**
```json
{
    "evolution": {
        "name": "Bear",
        "turnsToEvolve": 1
    },
    "tail": {
        "name": "BearTail",
        "tailLostPortrait": "taillessbear.png"
    },
    "iceCube": {
        "creatureWithin": "FrozenBear"
    }
}
```

**JLDR2**
```json
{
    "evolveIntoName": "Bear",
    "evolveTurns": 1,
    "tailName": "BearTail",
    "tailLostPortrait": "taillessbear.png",
    "iceCubeName": "FrozenBear"
}
```

As you can see, the JLDR2 file has fewer child objects that have to be defined; the evolve, tail, and ice cube parameters are now on the card itself, reducing the overall size of the card file.

## Installation

### Automatic
Simply download with Thunderstore Mod Manager!

### Manual
To install this plugin you first need to install BepInEx as a mod loader for Inscryption. A guide to do this can be found [here](https://docs.bepinex.dev/articles/user_guide/installation/index.html#where-to-download-bepinex)

You will also need version 1.12+ of the [InscryptionAPI](https://github.com/ScottWilson0903/InscryptionAPI) plugin.

Finally, you simply need to put the **JSONLoader.dll** folder in **BepInEx/plugins**.

## Custom Cards

To create your own cards you just create a .jldr2 file (written in json) and fill in all the fields you want your card to have (fields you don't include will be defaulted). The *name* field is required, and the rest are optional with default values (though that would be a boring card). Those fields and their values are specified in the table below. For reference, an example custom card (8 f\*cking bears.jldr) is included in the **Cards** folder in this repo.

### New Cards and Card Prefixes

If you are creating a new card, you need to consider how to name your card so that it does not end up accidentally sharing a name with someone else's card in the future. The accepted way to prevent naming collisions in the community is to prefix the names of your cards with a simple name or code representing your card pack. For example, if you are adding a bunch of cards for Act 3 themed around the all-time classic action movie The Matrix, you might prefix all of your cards with "matrix_" - for example, "matrix_Neo" and "matrix_Trinity." Then, if someone comes along after you and creates a card pack based on mythology and religion, and they also want to create a card called Trinity, they will name their card "myth_Trinity," and we no longer have any issues with having two cards *named* "Trinity." Note that they can both still be called "Trinity" on the card, but the internal game name is different.

Cards that are loaded via JSON should indicate what their specific prefix is using the *modPrefix* field. This helps the card loading process recognize that you have properly prefixed your card and aren't simply using snake_case naming for multi-word cards. More specifically, we can't tell if a card named "Snow_Man" represents a snowman card, or if it's a card named "Man" with a mod prefix of "Snow."

Note that if your card's name and prefix don't match, the game will force it to match. So if your card's name is "StrongBad" and your card prefix is "HSR", your card will be named "HSR_StrongBad" in game. 

### Editing base game cards

To edit a card from the base game, you similarly create a .jldr2 file and fill in the fields you want to edit on the card. You must include the *name* to be able to identify the card; the mod prefix is not necessary. Any fields you fill out will be changed, and everything else will stay the same. Note that you can only edit cards from the base game this way: you cannot edit cards from other mods.

### Validation and GUI editing

You can use this [online JSON Schema validator](https://www.jsonschemavalidator.net/s/T7KtwMc7) to avoid syntax errors, and make sure the fields are correct in your json files.

There is also a [GUI](https://tinyurl.com/492ytnj7) based verion that is an option, just copy the json from the right hand panel when done!

### Where do my files go?

Files go anywhere in the plugins folder, along with  the artwork required for the card.

# The JLDR2 File

Cards have lots of fields that can be filled - this is a list of all field names and their purpose. The fields you wish to include in the .jldr file should be copied exactly from this table, and any fields that refer to *Enums.md* or *Card Names.txt* should have their strings be copied exactly from there.

| Field | Description |
|------|-------------|
| fieldsToEdit | **[Required: Editing]** A string array for the fields you wish to edit. The fields must be the exact names as in the left hand side of this table |
| name | **[Required]** A string for the name the game will use to identify the card - should contain no spaces. When editing, this field must match the card's name (See *Card Names.txt* for a list of ingame card names) |
| displayedName | **[Optional]** **[Default: ""]** A string for the name displayed on the card |
| description | **[Optional]** **[Default: ""]** A string for the description Leshy gives when you find the card |
| metaCategories | **[Optional]** A string array of meta catagories the card has. See *Enums.md* for the list that the game ships with. These can also be fully qualified guid+ability strings if they were added by another mod. |
| cardComplexity | **[Optional]** **[Default: Vanilla]** A string for the complexity of the card (See *Enums.md* for a list of levels of complexity) |
| temple | **[Optional]** **[Default: Nature]** A string for which Scrybe created the card |
| baseAttack | **[Optional]** **[Default: 0]** An integer value for the attack of a card |
| baseHealth | **[Optional]** **[Default: 1]** An integer value for the health of a card |
| hideAttackAndHealth | **[Default: false]** A boolean value to toggle if the cards attack and health are visible |
| bloodCost | **[Optional]** **[Default: 0]** An integer value for the blood cost of a card |
| bonesCost | **[Optional]** An integer value for the bones cost of a card |
| energyCost | **[Optional]** An integer value for the energy cost of a card |
| gemsCost | **[Optional]** A string array for the gems cost of a card (See *Enums.md* for a list of gems) |
| specialStatIcon | **[Optional]** An string for which special stat icon the card has. See *Enums.md* for the list that the game ships with. These can also be fully qualified guid+ability strings if they were added by another mod. |
| tribes | **[Optional]** An string array for the tribes the card belongs to. See *Enums.md* for the list that the game ships with. These can also be fully qualified guid+ability strings if they were added by another mod. |
| traits | **[Optional]** An string array for the traits a card has. See *Enums.md* for the list that the game ships with. These can also be fully qualified guid+ability strings if they were added by another mod. |
| specialAbilities | **[Optional]** A string array for the special abilities a card has. See *Enums.md* for the list that the game ships with. These can also be fully qualified guid+ability strings if they were added by another mod. |
| abilities | **[Optional]** A string array for the sigils a card has. See *Enums.md* for the list that the game ships with. These can also be fully qualified guid+ability strings if they were added by another mod. |
| evolveIntoName | **[Optional]** The name of the card that this card will evolve into when it has the Evolve sigil |
| evolveTurns | **[Optional]** The number of turns to evolve |
| defaultEvolutionName | **[Optional]** The name the card will have when it evolves (when it doesn't have evolve_ fields set) |
| tailName | **[Optional]** The name of the tail card produced when this card has TailOnHit |
| tailLostPortrait | **[Optional]** The .png file to switch the card's art with when the tail is lost |
| iceCubeName | **[Optional]** The name of the card generated when the card has the IceCube ability |
| flipPortraitForStrafe | **[Optional]** A boolean to determine if the cards portrait should flip when it uses one of the strafe sigils |
| onePerDeck | **[Optional]** A boolean value that toggles if there can be only one of the card per deck |
| appearanceBehaviour | **[Optional]** A string array for the behaviours the cards appearance should have. See *Enums.md* for the list that the game ships with. These can also be fully qualified guid+ability strings if they were added by another mod. |
| texture | **[Optional]** A string for the name of the card's image (must be .png). If it is in a subfolder within *Artwork* the subfolder should preceed the file name seperated by a '/' (or your system equivelent) |
| altTexture | **[Optional]** A string for the name of the card's alternate image (must be .png) |
| emissionTexture | **[Optional]** A string for the name of the card's emission image (must be .png) |
| titleGraphic | **[Optional]** A string for the name of the card's title image (must be .png) |
| pixelTexture | **[Optional]** A string for the name of the card's act2 image (must be .png) |
| animatedPortrait | **[Unavailable]** |
| decals | **[Optional]** A string array for the texture names of a card decals (must be .png) |

## Starter Decks

The latest version of JSON Loader allows you to create starter decks. To do this, your file needs to end in '_deck.jldr2' and should look like this:

```json
{
    "decks": [
        {
            "name": "DeckName1",
            "iconTexture": "icon.png",
            "cards": [ "Card1", "Card2", "Card3" ]
        },
        {
            "name": "DeckName2",
            "iconTexture": "icon2.png",
            "cards": [ "Card4", "Card5", "Card6" ]
        }
    ]
}
```

Note that you can define any number of starter decks in a single '*_deck.jldr2' file, and that the expected format of a '_deck.jldr2' file looks very different than other jldr2 files.

## Tribes

The latest version of JSON Loader allows you to create tribes. To do this, your file needs to end in '_tribe.jldr2' and should look like this:

```json
{
    "tribes": [
    {
      "name": "TribeName1",
	  "guid": "YourModGuid",
	  "tribeIcon": "tribeicon_custom1.png",
	  "appearInTribeChoices": true,
	  "choiceCardBackTexture": "card_rewardback_custom1.png"
    },
	{
      "name": "TribeName2",
	  "guid": "YourModGuid",
	  "appearInTribeChoices": false
    }
  ]
}
```

Note that much like starter decks, any number of tribes can be defined in a single '*_tribe.jldr2' file. Also note that if a tribe doesn't have a choiceCardBackTexture, one will be auto-generated based on the tribe's icon.

## Encounters

The latest version of JSON Loader allows you to create encounters. To do this, your file needs to end in '_encounter.jldr2' and should look like this:

```json
{
	"name": "",
	"minDifficulty": 0,
	"maxDifficulty": 0,
	"regions": [""],
	"dominantTribes": [""],
	"randomReplacementCards": [""],
	"redundantAbilities": [""],
	"turns": [{
		"cardInfo": [{
			"card": "",
			"randomReplaceChance": 0,
			"difficultyReq": 0,
			"difficultyReplacement": ""
		}]
	}]
}
```

These are all vanilla regions:
Alpine, Forest, Midnight, Midnight_Ascension, Pirateville, Wetlands

## Gramophone

The latest version of JSON Loader allows you to add music tracks to the Gramophone in Leshy's cabin.
To do this, your file needs to end in '_gram.jldr2' and should look like this:

```json
{
  "Prefix": "Example",
  "Tracks": [
    {
      "Track": "MyTrack.mp3",
      "Volume": 1
    },
    {
      "Track": "AnotherTrack.wav",
      "Volume": 1
    }
  ]
}
```
You should put your mod's prefix in the "Prefix" field. You can add as many tracks as you want inside of "Tracks", following the example above.

"Track" should be the name of your audio file. The audio file should be located inside of the `BepInEx/plugins` folder. The supported audio formats currently are MP3, OGG, WAV and AIFF.

"Volume" should be the volume of your track, from 0 to 1, where 0 is silence and 1 is full volume. If you want your track to be at half volume, for example, you can put 0.5 in the Volume field.

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

If you want help debugging you can ask in the #jsonloader channel in the [Inscryption modding discord](https://discord.gg/QrJEF5Denm).
