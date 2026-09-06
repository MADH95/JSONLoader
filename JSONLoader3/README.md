# JSON Loader V3
The below documentation is likely going to become outdated, the most up to date documentation will be provided via GitHub and Thunderstore WIKI Systems.

This package is now maintained by Chaosyr as of V2.7.0.

If you wish to contribute to the project the GitHub is [MADH95/JSONLoader](https://github.com/MADH95/JSONCardLoaderPlugin). The Development Branch is: [MADH95/JSONLoader/Refactor-JSONLoader-3](https://github.com/MADH95/JSONLoader/tree/Refactor-JSONLoader-3).

Thank you to the following folks for their amazing Contributations to this Project:

* [MADH95](https://github.com/MADH95)
* [JamesVeug](https://github.com/JamesVeug)
* [LilySylvee](https://github.com/LilySylvee)
* [kbmackenzie](https://github.com/kbmackenzie)
* [Chaosyr](https://github.com/Chaosyr)
* [divisionbyz0rro](https://github.com/divisionbyz0rro)
* [IngoHHacks](https://github.com/IngoHHacks)
* [Khaomi](https://github.com/Khaomi)
* [vladdeSV](https://github.com/vladdeSV)
* [TVFLabs](https://github.com/TVFLabs)
* [UwUMacaroniTime](https://github.com/UwUMacaroniTime)
* [Windows10CE](https://github.com/Windows10CE)

JSONLoader and API 3.0.0 are on the Horizon, JSONLoader's will be first as it's the simpler of the two to upgrade.

## Tools:
For a Live Schema Based Editor for JSONLoader open up [JSON-Editor](https://json-editor.github.io/json-editor/?data=N4Ig9gDgLglmB2BnEAuUBXRBTA+vAhgLa75RQBOMARulFsigGb4A22ANCBOWIdDlSyMw5XABMsLLHVTM2WTpnFD86FlBwA3VunqoKuzoXwAPHBOgALVAAZONMghyIopXITATUARk4BjfGxnLCQYWE1cbkgscigAT2D8cj9rJlYOEERLMAB3HBiecgYQFPx4AHMsEE4ovg0IQLocYzMpCqhLHFF8FKwxWXSFEFEPCJw5IKiIGNg9NPlOEOFk3D8EFwGF8H4YeBwcmDFKqFQSyyw/AGsqMBNqkBg1+BZqU8RpvwoqzjAqACsLhoWPg4mBaKd4CJjCx7ohesZUMAAL6cDpYYhvD5fe6iACO6BgojEAgSEmYahO8wyYhg72BCTxBKJOAQLDimwyWVyLJ28H05EMIEhOHwYhpsAQrBwUxmMDmEyG+D+pn5gppiHwVCk+XFOD+iAQqqG6s12rWLGBEGwHONtNNkR401icoYBltGq1JHI5BBIrFNs4Js9Iu9vtEIgk5ADICD2qSPtJkmkVSpQxC9pDCZwawg7JQbs48d9TwoYDYOCgkCNgbtwaLiakTVYLC6uVdAvdGfr5iTTWBLlbOSNSKRQA=).

### Instructions For JSON-Editor:
* First navigate to where your JSONLoader Schemas are set to be created, be default it will be the `[JSONLoader3.dll Path]/Schemas` folder.
* Find the Schema relevant to what you want to edit. It will follow the pattern of `LOADER_Type_Schema.json`, next open up the file.
* Once open in a text editor press `CTRL+A`, or your OS Equivalent of Select All, followed by `CTRL+C` or your OS Equivalent of Copy.
* Now, navigate to the JSON-Editor linked above.
* What you'll do now, is scroll down to the bottom of the page where it says `Schema`, click where it shows `1 | {}` specifically the curly brace.
* Press `CTRL+A` again but now follow it with `CTRL+V`, or your OS Equivalent of Paste.
* Now that the Schemas in place press `Update Schema`, this will update the page above to have an Editor based on the Schema you inserted.

### JSON-Editor Tips
* Press in the Box to Edit a Property.
* Press the Checkbox to enable editing for that property.
* If there are multiple items to select for a given Property it has 2 way's of showing up.
  * The first a scrolling pane with all the options.
    * For this you will want to click to select a property. `CTRL-Click` to select another property. `SHIFT-Click` will allow you to select the property you clicked first, until the one you clicked while holding `SHIFT`.
  * The other way is an Array of Tick Boxes.
    * To apply an item you'll just Tick the Box next to it.
* If you see `+ Item` by a Properties Name that means it is an Array of Objects, to add an Object to it just click the button.
  * To delete an Item hit `[Insert Trashcan Here] item`.
  * To copy the Item hit `[Insert 2 Overlapped Squares Here] item`.
* You can press the upside down `^` to minimize the property. To unminimize press the sideways `^`.
* Each property has an associated description, these are meant to help you understand what the field does, if you don't understand it you can always ask for help in the [Inscryption Modding](https://discord.gg/ZQPvfKEpwM) Discord.

## JSON and CSV Loader API Documentation
The below sections serve to document the support of each Version of JSONLoader, for more detail or to expand the detail between Updates refer to the [JSONLoader Wiki](https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki) or [JSONLoader GitHub Wiki](https://github.com/MADH95/JSONLoader/wiki). ***Notes for Contributing to the Wiki are Outlined on their respective Home Pages***.

### JSON Inscrybing
All JSONLoader versions require the same things so, heres a unified basics for making things with JSONLoader. First off make sure you have a Keyboard, Mouse, Monitor, File Explorer, and a Text Editor. These are more or less all you need to make JSON's for this mod. However there are some mandatory steps to get your environments prepared.

#### File Explorer (Windows)
In order for you to make the actual JLDR extension for your cards you'll need to follow the below steps in your File Explorer.
1. Open File Explorer
2. Find the `…` (or 3 dots in a row) button, and press it.
3. Press `[Insert a Wrench Here] Options`.
4. In the menu that just popped up you'll see 3 Tabs at the top, press the one labeled `View`.
5. Under `Advanced Settings:` toggle off `Hide extensions for known file types`, another useful one to toggle would be `Show hidden files, folders, and drives`.
6. After you've toggled these press `Apply to Folders`.
7. Next, press `OK`.

Now you should see File Extensions alongside all of your files. As stated before this will allow you to change the File Extension for the mod. 

#### Getting the Path's
Next up you'll likely want to grab a path, namely the one to your Plugins folder. This folder will lie wherever your BepInEx folder is. 

If you use a Mod Manager, go to one of the following places:
* R2ModMan: `Settings` -> `Directories` -> `Profile Folder` -> `Browse` -> Navigate via File Explorer to `BepInEx` -> Navigate via File Explorer to `plugins` -> Go to the File Explorer Address Bar -> Click It -> Hit `CTRL+C` or the OS Equivalent. 
* GaleModManager: Click `File` in the Top Bar -> `Browse Profile Folder` -> Navigate via File Explorer to `BepInEx` -> Navigate via File Explorer to `plugins` -> Go to the File Explorer Address Bar -> Click It -> Hit `CTRL+C` or the OS Equivalent.
* ThunderstoreModManager: `Settings` -> `Directories` -> `Profile Folder` -> `Browse` -> Navigate via File Explorer to `BepInEx` -> Navigate via File Explorer to `plugins` -> Go to the File Explorer Address Bar -> Click It -> Hit `CTRL+C` or the OS Equivalent.

If your manual it should be something like:
1. Navigate to the Games Local Install Folder
  * XboxGames: `C:\XboxGames\Inscryption\Content`
  * Steam: `\steamapps\common\Inscryption` after you get to the Steam Install Folder. 
2. Next navigate to `BepInEx/plugins`
3. Go to the File Explorer Address Bar -> Click It -> Hit `CTRL+C` or the OS Equivalent.

Now store that path somewhere you'll remember it, you'll be coming back here a lot over the course of your mod.

#### Text Editor
The recommended File Editor for JSONLoader is [VisualStudioCode](https://code.visualstudio.com/) as it has built in handlers for both JSON Syntax and CSV Syntax, if your working with JSONLoader at any point this should be your go-to editor, but if you have a preficed editor nothings stopping you from using it.

#### Adding the File Extensions to the Context Menu (Windows 11)
I'm going to include this for those on Windows 11 for other OS's the next section should work fine.

1. In A Text Editor Create a new File.
2. Enter the following into the file:
   ```ini
   Windows Registry Editor Version 5.00

   [HKEY_CLASSES_ROOT\.md]
   @="markdownfile"
   
   [HKEY_CLASSES_ROOT\.md\ShellNew]
   "NullFile"=""
   
   [HKEY_CLASSES_ROOT\markdownfile]
   @="Markdown Document"
   
   [HKEY_CLASSES_ROOT\markdownfile\DefaultIcon]
   @="\"C:\\Users\\Chaos\\AppData\\Local\\Programs\\Microsoft VS Code\\a44adf7f53\\resources\\app\\resources\\win32\\markdown.ico\""
   
   [HKEY_CLASSES_ROOT\.json]
   @="jsonfile"
   
   [HKEY_CLASSES_ROOT\.json\ShellNew]
   "NullFile"=""
   
   [HKEY_CLASSES_ROOT\jsonfile]
   @="JSON File"
   
   [HKEY_CLASSES_ROOT\jsonfile\DefaultIcon]
   @="\"C:\\Users\\Chaos\\AppData\\Local\\Programs\\Microsoft VS Code\\a44adf7f53\\resources\\app\\resources\\win32\\json.ico\""
   
   [HKEY_CLASSES_ROOT\.jldr]
   @="jldrfile"
   
   [HKEY_CLASSES_ROOT\.jldr\ShellNew]
   "NullFile"=""
   
   [HKEY_CLASSES_ROOT\jldrfile]
   @="JSONLoader File"
   
   [HKEY_CLASSES_ROOT\jldrfile\DefaultIcon]
   @="\"C:\\Users\\Chaos\\AppData\\Local\\Programs\\Microsoft VS Code\\a44adf7f53\\resources\\app\\resources\\win32\\json.ico\""
   
   [HKEY_CLASSES_ROOT\.jldr2]
   @="jldr2file"
   
   [HKEY_CLASSES_ROOT\.jldr2\ShellNew]
   "NullFile"=""
   
   [HKEY_CLASSES_ROOT\jldr2file]
   @="JSONLoader2 File"
   
   [HKEY_CLASSES_ROOT\jldr2file\DefaultIcon]
   @="\"C:\\Users\\Chaos\\AppData\\Local\\Programs\\Microsoft VS Code\\a44adf7f53\\resources\\app\\resources\\win32\\json.ico\""
   
   [HKEY_CLASSES_ROOT\.jldr3]
   @="jldr3file"
   
   [HKEY_CLASSES_ROOT\.jldr3\ShellNew]
   "NullFile"=""
   
   [HKEY_CLASSES_ROOT\jldr3file]
   @="JSONLoader3 File"
   
   [HKEY_CLASSES_ROOT\jldr3file\DefaultIcon]
   @="\"C:\\Users\\Chaos\\AppData\\Local\\Programs\\Microsoft VS Code\\a44adf7f53\\resources\\app\\resources\\win32\\json.ico\""
   
   [HKEY_CLASSES_ROOT\.csv]
   @="csvfile"
   
   [HKEY_CLASSES_ROOT\.csv\ShellNew]
   "NullFile"=""
   
   [HKEY_CLASSES_ROOT\csvfile]
   @="CSV File"
   
   [HKEY_CLASSES_ROOT\csvfile\DefaultIcon]
   @="\"C:\\Users\\Chaos\\AppData\\Local\\Programs\\Microsoft VS Code\\a44adf7f53\\resources\\app\\resources\\win32\\html.ico\""
   ```
3. Save the file as a `[SomeName].reg`, then run it.
4. Next Restart your File Explorer via Task Manager

What this did was add the following file types to your Right Click Context Menu: `.md`, `.json`, `.jldr`, `.jldr2`, `.jldr3`, and `.csv`. So that now when you want to make a new JSONLoader file you can press `New` -> `JSONLoader(X) File` in the Context Menu. Note for the Icons this is set to utilize those of [Visual Studio Code](https://code.visualstudio.com/)

#### Creating the JSON File

Now you'll need to make the actual file for your Item added by JSONLoader. Go to the Plugins folder, then you'll make a new directory or folder under it, this will be your Mod's folder. Make another directory under it called simply `plugins` this will make your life a little easier when uploading your mods, as the folders will be sticky. Now make a folder called `Scripts`, this will be where your JSON's are expected to live unless you explicitly define it in a file included in your mod, that's not relevant now though.

Once that's done, Right-Click the window explorer pane in the folder, Select New `Text Document` or New `JSONLoader(X) File`, ensure the extension of the file matches the Item your trying to create. Now Open the file in a Text Editor, and insert `{}` into the file, this is so you have a valid JSON base. Each Support area of the Documentation will cover what to put into this file.

Oh, before I leave you, give this a watch: [Web Dev Simplified: Learn JSON in 10 Minutes](https://www.youtube.com/watch?v=iiADhChRriM), this will give you a overview of what JSON is and how to work with it, and it will teach you the terminology.

### JSONLoaderV1 Support:
This version of JSONLoader supports Cards Exclusively and limited support for Modded Libraries. This is a Maintenance Version, outside of Bug Fixes it will NOT be updated.

#### JSONLoaderV1 Cards:
JSONLoaderV1 Cards support allows you to well make JSON Based Cards for the Game, note they aren't the most Complex things in this version of the mod.

The following are all of the fields available for JSONLoaderV1 Cards and what they do:

##### Card Fields

|           Key            | Description                                                                                                                                                                                                                                                                                                                                                                                                                                         |                     Type |
|:------------------------:|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------:|
|      `fieldsToEdit`      | Any items applied within this field will be used for overwriting the In-Game card associated with the field 'name'.                                                                                                                                                                                                                                                                                                                                 |             String Array |
|          `name`          | The In-Code name for the card, please append on a Prefix unique to your mod if you are NOT editing a base game card. For example; "JSONFanMod5_Gorilla".                                                                                                                                                                                                                                                                                            |                   String |
|     `displayedName`      | The In-Game name for the card, it can be anything as long as this font can display it; https://font.download/font/heavyweight                                                                                                                                                                                                                                                                                                                       |                   String |
|      `description`       | The In-Game flavor for the card, this will show when receiving the card for the first time, if you want to prevent it being seen from saving use; https://thunderstore.io/c/inscryption/p/creator/Fuck_Dialouge_Saving/                                                                                                                                                                                                                             |                   String |
|     `metaCategories`     | These Meta-Categories control how your card will show up within the game, see the following page for what each of them do; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums                                                                                                                                                                                                                                |             String Array |
|     `cardComplexity`     | This controls WHEN your card can show up in the game, see the following page for what each of them do; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums                                                                                                                                                                                                                                                    |                   String |
|         `temple`         | This controls which temple in Act 2 the card is apart of, as well as meant to determine which Act outside Act 2 the card shows up in, whether mods follow the convention is up to question, but that's what these do. So, Nature is Act 1 and the Nature Temple, Tech is Act 3 and the Technology Temple, Undead is the Grimora Portion of the Finale and the Undead Temple, Wizard is the Magnificus Portion of the Finale and the Magicks Temple. |                   String |
|       `baseAttack`       | This value determines the attack value of the card, it cannot be negative.                                                                                                                                                                                                                                                                                                                                                                          |                      Int |
|       `baseHealth`       | This value determines the health value of the card, it cannot be negative or 0.                                                                                                                                                                                                                                                                                                                                                                     |                      Int |
|  `hideAttackAndHealth`   | This boolean value determines whether the Attack and Health of the card should be hidden or not.                                                                                                                                                                                                                                                                                                                                                    |                  Boolean |
|       `bloodCost`        | This value determines the amount of Blood this card will cost.                                                                                                                                                                                                                                                                                                                                                                                      |                      Int |
|       `bonesCost`        | This value determines the amount of Bones this card will cost.                                                                                                                                                                                                                                                                                                                                                                                      |                      Int |
|       `energyCost`       | This value determines the amount of Energy this card will cost.                                                                                                                                                                                                                                                                                                                                                                                     |                      Int | 
|       `gemColors`        | The following 3 values are accepted here: Green for the Green Gem, Orange for the Orange Gem, and Blue for the Blue Gem. Each of these correlates to the Gem Cost of a card. This version of JSONLoader does not support multiple of the same color of gem.                                                                                                                                                                                         |             String Array |
|    `specialStatIcon`     | This determines which Stat Icon to show on the card, this must be used alongside the associated Special Ability.                                                                                                                                                                                                                                                                                                                                    |                   String |
|         `tribes`         | This List determines what Tribes are applied to the card, this works with Base Game tribes only. Use a newer version of JSONLoader for Modded Tribes. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums                                                                                                                                                                    |             String Array |
|         `traits`         | This List determines what Traits are applied to this card, this works with Base Game traits only. Use a newer version of JSONLoader for Modded Traits. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums                                                                                                                                                                   |             String Array |
|    `specialAbilities`    | This List determines what Special Abilities are applied to this card, this works specifically with Base Game Special Abilities. For Modded Special Abilities utilize the 'customSpecialAbilities' field. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums                                                                                                                 |             String Array |
|       `abilities`        | This List determines what Abilities are applied to this card, this works specifically with Base Game Abilities. For Modded Abilities utilize the 'customAbilities' field. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums                                                                                                                                                |             String Array |
|    `customAbilities`     | This List determines the Modded Abilities that will be applied to this card. You may find this to be a useful resource; https://github.com/Chaosyr/SaxbyModEnums/wiki                                                                                                                                                                                                                                                                               |        AbilityData Array |
| `customSpecialAbilities` | This List determines the Modded Special Abilities that will be applied to this card. You may find this to be a useful resource; https://github.com/Chaosyr/SaxbyModEnums/wiki                                                                                                                                                                                                                                                                       | SpecialAbilityData Array |
|       `evolution`        | This Object determines the Evolution related Parameters for this card, such as what it will turn into, and how long it will take to turn into it.                                                                                                                                                                                                                                                                                                   |               EvolveData |
|  `defaultEvolutionName`  | This determines what the Default Evolution Name will be, note it will appear in the format of; '[defaultEvolutionName] [displayedName]', just replace the variables with your JSON's values.                                                                                                                                                                                                                                                        |                   String |
|          `tail`          | This Object determines the LooseTail related Parameters for this card, such as this cards Texture after losing its tail, or the Card the Tail Will Be.                                                                                                                                                                                                                                                                                              |                 TailData |
|        `iceCube`         | This Object determines the IceCube related Parameters for this card, namely what card it will be turned into, if left empty the default is an Opossum.                                                                                                                                                                                                                                                                                              |              IceCubeData |
| `flipPortraitForStrafe`  | A bool determining whether this cards portrait will flip when the card moves. (like the sigil icon does)                                                                                                                                                                                                                                                                                                                                            |                  Boolean |
|       `onePerDeck`       | A bool determining if there can only be one copy of this card within the Player's deck.                                                                                                                                                                                                                                                                                                                                                             |                  Boolean |
|   `appearanceBehavior`   | This List determines the Appearance Behaviors in which will be applied to this card. Use a newer version of JSONLoader for Modded Appearance Behaviors. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums                                                                                                                                                                  |             String Array |
|        `texture`         | The Path to your cards Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image.                                                                                                                                                                                                                                         |                   String |
|       `altTexture`       | The Path to your cards Alternative Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies in the case you have a Goat's Eye or possibly some other cases.                                                                                                                                                |                   String |
|    `emissionTexture`     | The Path to your cards Emissive Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies in the case you've transferred a sigil at the Sacrificial Stones onto this card.                                                                                                                                  |                   String |
|      `titleGraphic`      | The Path to your cards Title Graphic, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '113x28' image. This applies specifically over your card name as a way of obscuring it like the Tentacle Cards are.                                                                                                                                |                   String |
|      `pixelTexture`      | The Path to your cards Pixel Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '41x28' image. This applies specifically in Act 2, its just that act's version of the card portrait.                                                                                                                                              |                   String |
|         `decals`         | This is a list of all the Decal Images in which will be stacked onto your card, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '125x190' image.                                                                                                                                                                                         |             String Array |

###### AbilityData Object

|  Key   | Description                                                                       |   Type |
|:------:|-----------------------------------------------------------------------------------|-------:|
| `name` | This is the In-Code name of the Ability.                                          | String |
| `GUID` | This is the Ability Libraries GUID, it's a similar concept to your card's prefix. | String |

###### SpecialAbilityData Object

|  Key   | Description                                                                               |   Type |
|:------:|-------------------------------------------------------------------------------------------|-------:|
| `name` | This is the In-Code name of the Special Ability.                                          | String |
| `GUID` | This is the Special Ability Libraries GUID, it's a similar concept to your card's prefix. | String |

###### EvolveData Object

|       Key       | Description                                                                                                                                                           |   Type |
|:---------------:|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------:|
|     `name`      | This represents the In-Code name of the card this card is meant to evolve into.                                                                                       | String |
| `turnsToEvolve` | This value represents the amount of turns it takes for this card to evolve. This version's Turn Count must be between 1-3 for more use a newer version of JSONLoader. |    Int |

###### TailData Object

|        Key         | Description                                                                                                                                                                                                                                                                                 |   Tupe |
|:------------------:|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------:|
|       `name`       | This represents the In-Code name of the card this card will leave in its old lane if Loose Tail triggers.                                                                                                                                                                                   | String |
| `tailLostPortrait` | The Path to your cards Tail Lost Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies specifically when this card is struck and lost its tail. | String |

###### IceCubeData Object

|       Key        | Description                                                                                              |   Type |
|:----------------:|----------------------------------------------------------------------------------------------------------|-------:|
| `creatureWithin` | This represents the In-Code name of the card this card will leave behind in its place when it is to die. | String |

### JSONLoaderV2 Support:

### JSONLoaderV3 Support:

### CSVLoader Support:

___

## JSONLoader Maintainer Documentation
The below sections serve to document internal Information relevant for anyone maintaining JSONLoader, for more detail or to expand the detail between Updates refer to the [JSONLoader Wiki](https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki) or [JSONLoader GitHub Wiki](https://github.com/MADH95/JSONLoader/wiki). ***Notes for Contributing to the Wiki are Outlined on their respective Home Pages***.

### JSON Object Tooltip Language
This section goes over our Homemade `JSON Object Tooltip Language` used for creating our Schemas on the fly.

#### HARD-CODED VALUES
* REQUIRED - Mark this field as a Required field in the Schema.
* EXCLUDED - Mark this field as something to not include in the Schema.

#### VARIABLES
All Variables will work as follows: VariableName(Definition), kinda like a KeyPairValue.
The following is a list of all Variables:
* MinimumLength - Int - Used in String and String Array - Mandates a Minimum Length.
* Pattern - Raw Regex - Used in String and String Array - Mandates a Pattern the Value must follow.
* Items - Boolean - Used in String Array and Object Array - Marks the fact the Array has items as true.
* ItemType - Type - Used in String Array and Object Array - Used to define the type of Array in which the items belong. (e.g. string or object)
* Enums - A List of Predefined Values - Used in String and String Array - This provides a Pre-Defined list of items users may use for defining the value.
* UniqueItems - Boolean - Used in String Array and Object Array - This mandates uniqueness among the values.
* Default - Value - Used in String, Int, and Boolean - This provides a default for Schema Validators.
* Minimum - Int - Used in Int - This mandates a Minimum Number.
* Maximum - Int - Used in Int - This mandates a Maximum Number.
* AdditionalProperties - Boolean - Used in Object and Object Array - Determines whether additional properties are valid.

If you inevitably need more as of present you'll need to code handling into the Schema and Linter.

#### MULTI-VARIABLE
To use more than one variable all you need to do is add a '|' between each Variable, this acts as a Delimiter.

An example of such would be: 

```
[Tooltip("REQUIRED | MinimumLength(1) | Pattern(^[a-zA-Z\\d_]+$)")]
```

Notice the `//` in the Regex? That's because C# needs it to be escaped in quotes, but don't worry we properly escape it for JSON in `ReadDocumentationFile.EscapeJSON()`.