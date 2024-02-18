# JSON Loader V2

This is a mod made for Incryption to create custom cards, sigils, starter decks, tribes, encounters and more using JSON files.

Version 2.0 of this mod is designed to create full compatibility with Version 2.0+ of the API. JSON files compatible with this API will have an extension of .JLDR2

## Reloading

As a mod creator using JSONLoader you can press "shift + r" to apply the changes you've made to any jldr2 files without having to restart the game.

## Validation and GUI editing

You can use this [online JSON Schema validator](https://www.jsonschemavalidator.net) to avoid syntax errors, and make sure the fields are correct in your jldr2 files. Just put the contents of the corresponding schema from the Schema folder located in the JSONLoader files or on the github page in the left hand panel, and the contents of the jldr2 file that you want to validate in the right hand panel. If the json appears to be invalid the website will tell you where the error is and what exactly is wrong.

There is also a [GUI](https://json-editor.github.io/json-editor/) based version that is an option, just input a schema in the panel at the bottom of the page and copy the json from the right hand panel when done!

## Where do my files go?

Files go anywhere in the plugins folder, along with the artwork required for said file.

## Converting Existing Cards to JLDR2

In order to have compatability with mod manager, version 2.0.0 breaks backwards compatability by default. Don't worry though! This mod comes with a backwards compatibility mode that can optionally be turned on. When turned on, all existing .JLDR files will be read in, converted to .JLDR2 files, and dumped back into the folder where the original JLDR was found. However, I cannot guarantee that the version will be 100% correct, especially when it comes to assigning the appropriate prefix to the card (see the first heading below under 'Custom Cards').

If you are a card creator, the best course of action is to set the config option to convert all JLDR files, manually inspect the JLDR2 files generated to ensure they are correct, then publish your mod with a brand new set of JLDR2 files.

For the most part, JLDR2 and JLDR are the same. The key differences are outlined here:

### Abilities and Special Abilities

Previously, base game abilities and mod-added abilities were handled differently; they were completely separate parts of the file. Now, base game abilities and mod-added abilities are kept in the same list. Base game abilities are referred to by their enumerated name, such as "Flying" or "Reach." Mod-added abilities are referred to by a combination of the Mod GUID and their name. For example, the "Deathburst" ability from Void's sigil pack (part of the popular "All the Sigils" mod) is referred by the string "ATS.Deathburst"; in this example, "Deathburst" is the name of the ability, and "ATS" is the GUID of the mod. 

So to create a card with both Flying and Deathburst, you would do something like the following:

```json
"abilities": [ "Flying", "ATS.Deathburst" ]
```

This holds true for all custom enumerations on a card. In the case of Abilities and Special Abilities, this is very different behaviour from previous versions of JSONLoader. However, all enumerations are treated this way. For example, mod added Traits, Metacategories, special stat icons, etc can all be handled exactly the same way.

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

## Custom Cards

To create your own cards you just create a .jldr2 file (written in json) and fill in all the fields you want your card to have (fields you don't include will be defaulted). The *name* field is required, and the rest are optional with default values (though that would be a boring card). Those fields and their values are specified in the table below. For reference, an example custom card (8 more f\*cking bears_example.jldr2) is included in the **Cards** folder in this repo.

### New Cards and Card Prefixes

If you are creating a new card, you need to consider how to name your card so that it does not end up accidentally sharing a name with someone else's card in the future. The accepted way to prevent naming collisions in the community is to prefix the names of your cards with a simple name or code representing your card pack. For example, if you are adding a bunch of cards for Act 3 themed around the all-time classic action movie The Matrix, you might prefix all of your cards with "matrix_" - for example, "matrix_Neo" and "matrix_Trinity." Then, if someone comes along after you and creates a card pack based on mythology and religion, and they also want to create a card called Trinity, they will name their card "myth_Trinity," and we no longer have any issues with having two cards *named* "Trinity." Note that they can both still be called "Trinity" on the card, but the internal game name is different.

Cards that are loaded via JSONLoader should indicate what their specific prefix is using the *modPrefix* field. This helps the card loading process recognize that you have properly prefixed your card and aren't simply using snake_case naming for multi-word cards. More specifically, we can't tell if a card named "Snow_Man" represents a snowman card, or if it's a card named "Man" with a mod prefix of "Snow."

Note that if your card's name and prefix don't match, the game will force it to match. So if your card's name is "StrongBad" and your card prefix is "HSR", the internal name of your card will be "HSR_StrongBad".

### Editing base game cards

To edit a card from the base game, you similarly create a .jldr2 file and fill in the fields you want to edit on the card. You must include the *name* to be able to identify the card; the mod prefix is not necessary. Any fields you fill out will be changed, and everything else will stay the same. Note that you can only edit cards from the base game this way: you cannot edit cards from other mods.

## The fields

Cards have lots of fields that can be filled - this is a list of all field names and their purposes. The fields you wish to include in the .jldr2 file should be copied exactly from this table, and any fields that refer to *[Enums.md](https://github.com/MADH95/JSONLoader/blob/master/Enums.md)* or *[Card Names.md](https://github.com/MADH95/JSONLoader/blob/master/Card%20Names.md)* should have their strings be copied exactly from there.

| Field | Description |
|------|-------------|
| name | **[Required]** A string for the name the game will use to identify the card - should contain no spaces. When editing, this field must match the card's name (See *[Card Names.md](https://github.com/MADH95/JSONLoader/blob/master/Card%20Names.md)* for a list of ingame card names) |
| displayedName | **[Optional]** **[Default: ""]** A string for the name displayed on the card |
| description | **[Optional]** **[Default: ""]** A string for the description Leshy gives when you find the card |
| metaCategories | **[Optional]** A string array of meta catagories the card has. See *[Enums.md](https://github.com/MADH95/JSONLoader/blob/master/Enums.md)* for the list that the game ships with. These can also be fully qualified guid+ability strings if they were added by another mod. |
| cardComplexity | **[Optional]** **[Default: Vanilla]** A string for the complexity of the card (See *[Enums.md](https://github.com/MADH95/JSONLoader/blob/master/Enums.md)* for a list of levels of complexity) |
| temple | **[Optional]** **[Default: Nature]** A string for which Scrybe created the card |
| baseAttack | **[Optional]** **[Default: 0]** An integer value for the attack of a card |
| baseHealth | **[Optional]** **[Default: 1]** An integer value for the health of a card |
| hideAttackAndHealth | **[Default: false]** A boolean value to toggle if the cards attack and health are visible |
| bloodCost | **[Optional]** **[Default: 0]** An integer value for the blood cost of a card |
| bonesCost | **[Optional]** An integer value for the bones cost of a card |
| energyCost | **[Optional]** An integer value for the energy cost of a card |
| gemsCost | **[Optional]** A string array for the gems cost of a card (See *[Enums.md](https://github.com/MADH95/JSONLoader/blob/master/Enums.md)* for a list of gems) |
| specialStatIcon | **[Optional]** An string for which special stat icon the card has. See *[Enums.md](https://github.com/MADH95/JSONLoader/blob/master/Enums.md)* for the list that the game ships with. These can also be fully qualified guid+ability strings if they were added by another mod. |
| tribes | **[Optional]** An string array for the tribes the card belongs to. See *[Enums.md](https://github.com/MADH95/JSONLoader/blob/master/Enums.md)* for the list that the game ships with. These can also be fully qualified guid+ability strings if they were added by another mod. |
| traits | **[Optional]** An string array for the traits a card has. See *[Enums.md](https://github.com/MADH95/JSONLoader/blob/master/Enums.md)* for the list that the game ships with. These can also be fully qualified guid+ability strings if they were added by another mod. |
| specialAbilities | **[Optional]** A string array for the special abilities a card has. See *[Enums.md](https://github.com/MADH95/JSONLoader/blob/master/Enums.md)* for the list that the game ships with. These can also be fully qualified guid+ability strings if they were added by another mod. |
| abilities | **[Optional]** A string array for the sigils a card has. See *[Enums.md](https://github.com/MADH95/JSONLoader/blob/master/Enums.md)* for the list that the game ships with. These can also be fully qualified guid+ability strings if they were added by another mod. |
| evolveIntoName | **[Optional]** The name of the card that this card will evolve into when it has the Evolve sigil |
| evolveTurns | **[Optional]** The number of turns to evolve |
| defaultEvolutionName | **[Optional]** The name the card will have when it evolves (when it doesn't have evolve_ fields set) |
| tailName | **[Optional]** The name of the tail card produced when this card has TailOnHit |
| tailLostPortrait | **[Optional]** The .png file to switch the card's art with when the tail is lost |
| iceCubeName | **[Optional]** The name of the card generated when the card has the IceCube ability |
| flipPortraitForStrafe | **[Optional]** A boolean to determine if the cards portrait should flip when it uses one of the strafe sigils |
| onePerDeck | **[Optional]** A boolean value that toggles if there can be only one of the card per deck |
| appearanceBehaviour | **[Optional]** A string array for the behaviours the cards appearance should have. See *[Enums.md](https://github.com/MADH95/JSONLoader/blob/master/Enums.md)* for the list that the game ships with. These can also be fully qualified guid+ability strings if they were added by another mod. |
| texture | **[Optional]** A string for the name of the card's image (must be .png). If it is in a subfolder within *Artwork* the subfolder should preceed the file name seperated by a '/' (or your system equivelent) |
| altTexture | **[Optional]** A string for the name of the card's alternate image (must be .png) |
| emissionTexture | **[Optional]** A string for the name of the card's emission image (must be .png) |
| titleGraphic | **[Optional]** A string for the name of the card's title image (must be .png) |
| pixelTexture | **[Optional]** A string for the name of the card's act2 image (must be .png) |
| animatedPortrait | **[Unavailable]** |
| decals | **[Optional]** A string array for the texture names of a card decals (must be .png) |

## Configils

Besides cards JSONLoader also allows you to create sigils. To do this, your file needs to end in '_sigil.jldr2'.

Here is the [documentation](https://docs.google.com/document/d/1QLAfomaTcatm-foU2P1ZoqGQFFvhCfmEnN4jIxAWceQ/edit?usp=sharing) for making sigils.

## Talking Cards

JSONLoader also allows you to create talking cards! To do this, your file needs to end in '\_talk.jldr2'.

All of the documentation for that can be found [here](https://github.com/KBMackenzie/InscryptionJSONDump/blob/main/Documentation/Talking_Card_Guide.md)!

## Starter Decks

JSONLoader also allows you to create starter decks. To do this, your file needs to end in '_deck.jldr2' and should look like this:

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

Note that you can define any number of starter decks in a single '_deck.jldr2' file, and that the expected format of a '_deck.jldr2' file looks very different than that of other jldr2 files.

## Tribes

JSONLoader also allows you to create tribes. To do this, your file needs to end in '_tribe.jldr2' or '_tribes.jldr2' and should look like this:

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

Note that much like starter decks, any number of tribes can be defined in a single '_tribe.jldr2' file. Also note that if a tribe doesn't have a choiceCardBackTexture, one will be auto-generated based on the tribe's icon.

## Encounters

JSONLoader also allows you to create encounters. To do this, your file needs to end in '_encounter.jldr2' and should look like this:

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

These are all the vanilla regions that you can use for your encounters:
Alpine, Forest, Midnight, Midnight_Ascension, Pirateville, Wetlands

## Gramophone

JSONLoader also allows you to add music tracks to the Gramophone in Leshy's cabin.
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


## Localization

If you want to translate your cards into other languages, add the language suffix to the end of the field name. 

For example, if you want to translate the *displayedName* field into French, you would add a *displayedName_fr* field to your card. 

### Card localisation
```json
{
  "name": "JSON_SuperHypeMan", 
  "modPrefix": "ExampleMod", 
  "baseAttack": 6, 
  "baseHealth": 9, 
  "displayedName": "Super Hype Man", 
  "displayedName_fr": "Super Animateur", 
  "displayedName_it": "Super Uomo dell'Eccitazione", 
  "displayedName_de": "Super Stimmungsmacher", 
  "displayedName_es": "Super Animador", 
  "displayedName_pt": "Super Animador", 
  "description_tr": "Süper Coşku Adamı", 
  "description_ru": "Супер Человек-Аниматор", 
  "description_ja": "スーパーハイプマン", 
  "description_ko": "슈퍼 하이프 맨", 
  "description_zhcn": "超级炒作男", 
  "description_zhtw": "超級炒作男"
}
```

### New Languages
```json
{
    "languageName": "Polish",
    "languageCode": "nl",
    "resetButtonText": "Reset with Polish",
    "stringTablePath": "stringtable.csv"
}
```
### New Language Fonts
```json
{
    "fontReplacementPaths": [
        {
        "Type": "Liberation",
        "AssetBundlePath": "en_mainfont.assetbundle",
        "FontAssetName": "en_mainfont",
        "TMPFontAssetName": "en_mainfont"
        }
    ]
}
```

## Masks

To replace a mask that a boss puts on their face you can do it in a few ways.

### Replace a mask with a texture

This will replace the angler mask model with a flat surface and apply a texture to it. The image dimensions are 1000x1500
```json
{
  "maskName": "JSON_TestMask",
  "type": "Override",
  "texturePath": "testmask.png",
  "maskType": "Angler"
}
```
If you want to keep the original model but replace the texture you can add a field specifying the model type as below.
```json
  "modelType": "Angler"
```

## Regions

All custom regions need their files to be named ending with `_region.jldr2`.

JLDR2
```json
{
  "name": "TestRegion",
  "tier": 0,
  "addToPool": true,
  "terrainCards": ["BaitBucket"],
  "encounters": ["Skinks"],
  "likelyCards": ["Bullfrog"],
  "dominantTribes": ["Insect"],
  "bossPrepEncounter": "Submerge",
  "boardLightColor": "0,193,122,255",
  "cardsLightColor": "0,129,255,255",
  "mapAlbedo": "customRegion_mapAlbedo.png",
  "bosses": ["ProspectorBoss"],
  "fillerScenery": [
    {
      "minScale": {"x": 0.06, "y": 0.05},
      "maxScale": {"x": 0.09, "y": 0.22},
      "prefabNames": ["Tree_3_Mossy"],
      "radius": 0.06,
      "perlinNoiseHeight": true
    }
  ],
  "scarceScenery": [
    {
      "minDensity": 0.10,
      "minInstances": 40,
      "maxInstances": 50,
      "minScale": {"x": 40.00, "y": 40.00},
      "maxScale": {"x": 50.00, "y": 50.00},
      "prefabNames": ["Fern_1"],
      "radius": 0.05,
      "perlinNoiseHeight": true
    }
  ],
  "predefinedScenery": [{
    "minScale": {"x": 0.06, "y": 0.05},
    "maxScale": {"x": 0.09, "y": 0.22},
    "prefabNames": ["Tree_3_Mossy"],
    "radius": 0.06,
    "perlinNoiseHeight": true,
    "rotation": {"x": 0, "y": 0, "z": 0},
    "scale": {"x": 1, "y": 1, "z": 1}
  }],
  "dialogueEvent": {
    "eventName": "TestRegion",
    "mainLines": ["The rank smell of rot and mold permeated the humid air.", "Every step forward was answered by some nearby slip or slither."],
    "repeatLines": [["The air grew thick with moisture...", "The buzzing and chirping of insects drowned out the sound of your footfalls..."],
      ["As the air grew humid your boots became harder to pull from the mud.", "The dank smell of tepid water invaded your nostrils."]]
  }
}
```

### tier
Which position in the run the region will appear in.
(Broken as of API 2.19.3)
0. Any order
1. First region in the run
2. Second region in the run
3. Third region in the run


### addToPool
If set to true then the region will be added to the pool of regions available to be randomly chosen in ascension runs.

### terrainCards
List of terrain cards that can be placed on the board when starting fights.

NOTE: cards listed here need ot have the Terrain trait. 

### encounters
Encounters that that can appear during fights.

### likelyCards
Extra Cards that can appear during ThreeChoice event nodes to be added to your deck.

### dominantTribes
Tribes that decide what card will appear in the Oil painting and in ThreeChoice event nodes.

### bossPrepEncounter (Optional)
The encounter that will be used to for the boss fight.
If not specified then a random encounter will be chosen according to the node and games difficulty

### boardLightColor
Color that the map will show when moving between nodes

### cardsLightColor
Color tint cards will have.

### mapAlbedo
Name of an image that will be used on the map.

### bosses
Name of bosses/opponents that can appear when in this region.

### fillerScenery
List of Props that are scattered around the region.
- **minScale** and **maxScale** are the minimum and maximum scale of the prop.
- **prefabNames** are the names of the props that can appear on the map. see MapScenery.png for list of props.
- **radius** is the radius of the area the prop that no other props can appear in.
- **perlinNoiseHeight** is a boolean that determines if the prop position is randomized or not

### scarceScenery
Main props that are put on the map.
- **minDensity** Not used
- **minInstances** Minimum amount of instances that can spawn per map
- **maxInstances** Maximum amount of instances that can spawn per map
- **minScale** and **maxScale** are the minimum and maximum scale of the prop. Various per prop
- **prefabNames** are the names of the props that can appear on the map. see MapScenery.png for list of props.
- **radius** is the radius of the area the prop that no other props can appear in.
- **perlinNoiseHeight** is a boolean that determines if the prop position is randomized or not


### predefinedScenery
Props that will always appear in the map.
- **minScale** and **maxScale** are the minimum and maximum scale of the prop. Various per prop
- **prefabNames** are the names of the props that can appear on the map. see MapScenery.png for list of props.
- **radius** is the radius of the area the prop that no other props can appear in.
- **perlinNoiseHeight** is a boolean that determines if the prop position is randomized or not
- **rotation** Set rotation for all the props 
- **scale** Set scale for all the props


### dialogueEvent
- **eventName** Name of the dialogue event that plays when entering the region. Use the same name as the region.
- **mainLines** The dialogue that plays when first entering the region
- **repeatLines** Dialogue that plays every other time you enter the region or start a new map in the same region.

## Consumable Items

Items are powerful tools that can be awarded.
Similar to configils this is how to create an item to appear in the game.

Create a file with `_item.jldr2` at the end of the file name.

```json
{
  "GUID": "TestMod",
  "rulebookName": "Geck Army",
  "rulebookDescription": "Puts 4 Gecks in your hand",
  "description": "The Geck Army is a powerful force to be reckoned with. It's a good thing you're the one in control of it.",
  "icon": "base64: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",
  "abilityBehaviour": [
    {
      "trigger": {
        "triggerType": "OnActivate"
      },
      "drawCards": [
        {
          "card": {
            "name": "Geck",
            "retainMods": "false"
          }
        },
        {
          "card": {
            "name": "Geck",
            "retainMods": "false"
          }
        },
        {
          "card": {
            "name": "Geck",
            "retainMods": "false"
          }
        },
        {
          "card": {
            "name": "Geck",
            "retainMods": "false"
          }
        }
      ]
    }
  ]
}
```


## Bottled Cards

Vanilla inscyrption has items that are just a card in a bottle. When you activate it duriong a battle it gives you that card.
This is what you need to do to add a new bottled card to the game.


Make a file with `_item.jldr2` at the end of the file name
```json
{
  "GUID": "TestMod",
  "bottledCardName": "Urayuli",
  "icon": "base64:iVBORw0KGgoAAAANSUhEUgAAAGQAAACWAgMAAABV1sXVAAAAAXNSR0IB2cksfwAAAAlwSFlzAAALEwAACxMBAJqcGAAAAAlQTFRFAAAAAAAAAQEB7L2CIAAAAAN0Uk5TAP//RFDWIQAAAflJREFUeJzt10tyhCAQBmC0io3r4Q4pT8ERWMh9rFnmFNasLE6ZBnx0w08yr81UhUrMyKd00zJGlXqpae8tBJ8asC7L9JikLZLcB0S1xTfFtKVr5KZMK+tNfA15pto7JCpWtZbcp00txiXRSKaW2CimLd1jcqHtBUr685jkioEaUEFdLBGqTjwJybZCwFWgPsrBIjHKKW3hNbVxWUNxlB5cO5rENSRukaS6oBWfOz9OdFNMEnh9nPZTB24i1NdRReE6OKOBMMcBMsw2aDncEbr6dh+jmHI4sx9a5X0OXwTS55FGBmKjF3mzjGQgsSfyFiOIHRFVlEFm2gz6S6LtyXGRBTGnyAS68z9K52XSXEQJpEwMnOZy3DvSByZ2pzyQOUXJ9pJUNygh/dyH+HFcKllCuAVqtYS93S/DIQurgZRViFWjEJ7/++Ua1gHK0lMvkpVmO1P2SAaaJZaRZEw14EJ1o5+VfudatibFMRGj0Y1/uENWvmKKc9jN9G+5tiRWDcuyuZQxrxoFRWUJWGLt+J2ZJE8+BVLsWUS7Pi+AFG6uJEBJE51zIYCsoSV74Z6Q9QkBo8XJ3IrcJpUP/x6L6lBxx1ydr7jqpfD2+eJbou4V84zYf2mKfMCwHPie4c8K4h3IHHvle9P2vuTr1ybNwXLhJ0lgVva/qf0AKeWOroyqESEAAAAASUVORK5CYII="
}
```



## Installation

### Automatic
Simply download with Thunderstore Mod Manager!

### Manual
To install this plugin you first need to install BepInEx as a mod loader for Inscryption. A guide to do this can be found [here](https://docs.bepinex.dev/articles/user_guide/installation/index.html#where-to-download-bepinex)

You will also need the newest version of the [InscryptionAPI](https://github.com/ScottWilson0903/InscryptionAPI) plugin.

Finally, you simply need to put the **JSONLoader.dll** folder in **BepInEx/plugins**.

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

To add cards to your deck to test if your cards work, you can download the [debug menu mod](https://inscryption.thunderstore.io/package/JamesGames/DebugMenu/)

___

If you want help debugging you can ask in the #jsonloader channel in the [Inscryption modding discord server](https://discord.gg/QrJEF5Denm).
