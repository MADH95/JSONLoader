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
___
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
The recommended File Editor for JSONLoader is [VisualStudioCode](https://code.visualstudio.com/) as it has built in handlers for both JSON Syntax and CSV Syntax, if your working with JSONLoader at any point this should be your go-to editor, but if you have a preficed editor nothings stopping you from using it. If you don't want to download anything there is a Website called [JSONEditorOnline](https://jsoneditoronline.org/) which does similar.

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
___
### Artwork Form Support:
We support 3 ways in which you can load images for your Plugin. Below outline each form.

#### Plugins Folder Scanning:
This is the simplest form, simply just define your pngs name, at the root of the plugins folder. This is for the case where you don't want to make an art folder, and are not releasing the mod.

#### Relative Path to Image:
What we mean by this is that you can hand us a relative path from your plugins folder to your png. See the Example Card "MyCardWithPathing_example.jldr" for an example of how you can do this.

But the syntax is pretty much just;

* `/` after the folders name to indicate we need to go into that folder.
* `../` before the file or folders name to indicate we need to go up a folder.
* Plain Text is treated as a folder name.
* Plain Text followed by `.png` is treated as the file.

If you want to make sure your syntax is valid open [Regexer](https://regexr.com/) and place in the Regex: `^(?:(?:\.\.\/|[a-zA-Z\d_-\s]+\/)*[a-zA-Z\d_-\s]+\.png|data:image\/png;base64,[A-Za-z0-9+/]+={0,2}|base64:[A-Za-z0-9+/]+={0,2})$` into the Expression box, and your path into the Text box. If your wondering why it's so long, we are validating 3 data forms in that Regex Pattern.

#### Base64 Encoded Images:
You may not be as familiar with this type of image, but more or less its an image where the image itself is in textual form, specifically encrypted in the format of a 64imal number system (basically there are 64 numbers instead of the usual 10), this is great if you want to keep your art from being included in another person's mod while keeping it in your own (Though someone could always go in the json and copy the base64). It also removed the need to make a physical image for the texture.

Anyways there are two ways in which we validly accept base64 in Texture related fields. The first would be:

* `base64:`

This is how its allowed in JSONLoader2's setup (as of writing the Non-Nightly version of JSONLoader) But we also allow for the path variant of base64 as well. E.G.

* `data:image/png;base64,`

Note the start of the string if you do it this way **MUST MATCH THIS FORMAT**. An example Base64 you could pass in would be:

<details>
<summary>Raw Base64</summary>

```
data:image/png;base64,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"
```

</details>
This would decrypt to an image of the Archivist from the game. Though notably with JSONLoader alone this would be an All black image as it contains color, plus with the way we handle things, it portrait breaks, take it as a unintended quirk.

If you want to make sure your syntax is valid open [Regexer](https://regexr.com/) and place in the Regex: `^(?:(?:\.\.\/|[a-zA-Z\d_-\s]+\/)*[a-zA-Z\d_-\s]+\.png|data:image\/png;base64,[A-Za-z0-9+/]+={0,2}|base64:[A-Za-z0-9+/]+={0,2})$` into the Expression box, and your path into the Text box. If your wondering why it's so long, we are validating 3 data forms in that Regex Pattern.
___
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
|       `baseHealth`       | This value determines the health value of the card, it cannot be negative or 1.                                                                                                                                                                                                                                                                                                                                                                     |                      Int |
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

|        Key         | Description                                                                                                                                                                                                                                                                                 |   Type |
|:------------------:|---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------:|
|       `name`       | This represents the In-Code name of the card this card will leave in its old lane if Loose Tail triggers.                                                                                                                                                                                   | String |
| `tailLostPortrait` | The Path to your cards Tail Lost Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies specifically when this card is struck and lost its tail. | String |

###### IceCubeData Object

|       Key        | Description                                                                                              |   Type |
|:----------------:|----------------------------------------------------------------------------------------------------------|-------:|
| `creatureWithin` | This represents the In-Code name of the card this card will leave behind in its place when it is to die. | String |
___
### JSONLoaderV2 Support:
This version of JSONLoader supports several types of data, and supports Modded Libraries pretty well. This is a Maintenance Version, outside of Bug Fixes it will NOT be updated.

#### JSONLoaderV2 Cards:
JSONLoaderV2 Cards support allows you to well make JSON Based Cards for the Game, these can be pretty complex but overall relatively simple.

The following are all of the fields available for JSONLoaderV1 Cards and what they do:

##### Card Fields

|           Key           | Description                                                                                                                                                                                                                                                                                                                                                                                                                                         |         Type |
|:-----------------------:|-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------:|
|     `fieldsToEdit`      | Any items applied within this field will be used for overwriting the In-Game card associated with the field 'name'.                                                                                                                                                                                                                                                                                                                                 | String Array |
|         `name`          | The In-Code name for the card, when referencing this card, it is the piece that comes after the 'modPrefix' field.                                                                                                                                                                                                                                                                                                                                  |       String |
|       `modPrefix`       | The In-Code identifier for the card, when referencing this card, it is the piece that comes before the 'name' field.                                                                                                                                                                                                                                                                                                                                |       String |
|     `displayedName`     | The In-Game name for the card, it can be anything as long as this font can display it; https://font.download/font/heavyweight                                                                                                                                                                                                                                                                                                                       |       String |
|      `description`      | The In-Game flavor for the card, this will show when receiving the card for the first time, if you want to prevent it being seen from saving use; https://thunderstore.io/c/inscryption/p/creator/Fuck_Dialouge_Saving/                                                                                                                                                                                                                             |       String |
|    `metaCategories`     | These Meta-Categories control how your card will show up within the game, see the following page for what each of them do; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums                                                                                                                                                                                                                                | String Array |
|    `cardComplexity`     | This controls WHEN your card can show up in the game, see the following page for what each of them do; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums                                                                                                                                                                                                                                                    |
|        `temple`         | This controls which temple in Act 2 the card is apart of, as well as meant to determine which Act outside Act 2 the card shows up in, whether mods follow the convention is up to question, but that's what these do. So, Nature is Act 1 and the Nature Temple, Tech is Act 3 and the Technology Temple, Undead is the Grimora Portion of the Finale and the Undead Temple, Wizard is the Magnificus Portion of the Finale and the Magicks Temple. |       String |
|      `baseAttack`       | This value determines the attack value of the card, it cannot be negative.                                                                                                                                                                                                                                                                                                                                                                          |      Integer |
|      `baseHealth`       | This value determines the health value of the card, it cannot be negative or 0.                                                                                                                                                                                                                                                                                                                                                                     |      Integer |
|  `hideAttackAndHealth`  | This boolean value determines whether the Attack and Health of the card should be hidden or not.                                                                                                                                                                                                                                                                                                                                                    |      Boolean |
|       `bloodCost`       | This value determines the amount of Blood this card will cost.                                                                                                                                                                                                                                                                                                                                                                                      |      Integer |
|       `bonesCost`       | This value determines the amount of Bones this card will cost.                                                                                                                                                                                                                                                                                                                                                                                      |      Integer |
|      `energyCost`       | This value determines the amount of Energy this card will cost.                                                                                                                                                                                                                                                                                                                                                                                     |      Integer |
|       `gemsCost`        | The following 3 values are accepted here: Green for the Green Gem, Orange for the Orange Gem, and Blue for the Blue Gem. Each of these correlates to the Gem Cost of a card. This version of JSONLoader does not support multiple of the same color of gem.                                                                                                                                                                                         | String Array |
|    `specialStatIcon`    | This determines which Stat Icon to show on the card, this must be used alongside the associated Special Ability.                                                                                                                                                                                                                                                                                                                                    |       String |
|        `tribes`         | This List determines what Tribes are applied to the card. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums                                                                                                                                                                                                                                                                | String Array |
|        `traits`         | This List determines what Traits are applied to this card. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums                                                                                                                                                                                                                                                               | String Array |
|       `abilities`       | This List determines what Abilities are applied to this card. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums                                                                                                                                                                                                                                                            | String Array |
|   `specialAbilities`    | This List determines what Special Abilities are applied to this card. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums                                                                                                                                                                                                                                                    | String Array |
|    `evolveIntoName`     | This represents the In-Code name of the card this card is meant to evolve into. It should match the following: [Mod Prefix]_[Name].                                                                                                                                                                                                                                                                                                                 |       String |
|      `evolveTurns`      | This value represents the amount of turns it takes for this card to evolve. This version's Turn Count must be greater than 1.                                                                                                                                                                                                                                                                                                                       |      Integer |
| `defaultEvolutionName`  | This determines what the Default Evolution Name will be, note it will appear in the format of; '[defaultEvolutionName] [displayedName]', just replace the variables with your JSON's values.                                                                                                                                                                                                                                                        |       String |
|       `tailName`        | This represents the In-Code name of the card this card will leave in its old lane if Loose Tail triggers. It should match the following: [Mod Prefix]_[Name].                                                                                                                                                                                                                                                                                       |       String |
|   `tailLostPortrait`    | The Path to your cards Tail Lost Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies specifically when this card is struck and lost its tail.                                                                                                                                                         |       String |
|      `iceCubeName`      | This represents the In-Code name of the card this card will leave behind in its place when it is to die. It should match the following: [Mod Prefix]_[Name].                                                                                                                                                                                                                                                                                        |       String |
| `flipPortraitForStrafe` | A bool determining whether this cards portrait will flip when the card moves. (like the sigil icon does)                                                                                                                                                                                                                                                                                                                                            |      Boolean |
|      `onePerDeck`       | A bool determining if there can only be one copy of this card within the Player's deck.                                                                                                                                                                                                                                                                                                                                                             |      Boolean |
|  `appearanceBehaviour`  | This List determines the Appearance Behaviors in which will be applied to this card. Use a newer version of JSONLoader for Modded Appearance Behaviors. You can find the full list here; https://thunderstore.io/c/inscryption/p/MADH95Mods/JSONCardLoader/wiki/5396-vanilla-enums                                                                                                                                                                  | String Array |
|        `texture`        | The Path to your cards Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image.                                                                                                                                                                                                                                         |       String |
|    `emissionTexture`    | The Path to your cards Emissive Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies in the case you've transferred a sigil at the Sacrificial Stones onto this card.                                                                                                                                  |       String |
|      `altTexture`       | The Path to your cards Alternative Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies in the case you have a Goat's Eye or possibly some other cases.                                                                                                                                                |       String |
|  `altEmissionTexture`   | The Path to your cards Alternative Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '114x94' image. This applies in the case you have a Goat's Eye or possibly some other cases and, you've transferred a sigil at the Sacrificial Stones onto this card.                                                                       |       String |
|     `pixelTexture`      | The Path to your cards Pixel Portrait, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '41x28' image. This applies specifically in Act 2, its just that act's version of the card portrait.                                                                                                                                              |       String |
|     `titleGraphic`      | The Path to your cards Title Graphic, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '113x28' image. This applies specifically over your card name as a way of obscuring it like the Tentacle Cards are.                                                                                                                                |       String |
|        `decals`         | This is a list of all the Decal Images in which will be stacked onto your card, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '125x190' image.                                                                                                                                                                                         | String Array |
|  `extensionProperties`  | This is a list of all Extended Properties to this Card. You'll need to supply your own Field:Value pairs according to the mods specifications. If using the Editor, hit edit by the property to edit this Object.                                                                                                                                                                                                                                   |       Object |

To Utilize Extension Properties, add a `"{Field}": "{Value}"`, for each Extension Property you wish to have on the card, note it must be within the fields `{}` braces.
___
### JSONLoaderV3 Support:
___
### CSVLoader Support:
___
### Configuration
With the API we offer some Configuration which you can find located in: `Chaosyr.MADH95.Inscryption.JSON.CSVLoader.cfg`. The following is an overview of what you can configure and what they will do affecting the API of JSON and CSV Loader.

#### JSON Loading Origination Path
This is effectively a CSV as a value. All values passed into it must be Paths using similar logic to that seen in the Artwork Form Support section of this README.

By default, this value is set to: `Scripts, Plugins/Scripts, Cards, Plugins/Cards` to make mods work without the User needing to configure this. But if you need more Paths just add them to the end of the CSV.

These paths are Relative to your mods specific folder under the `plugins` folder, well more so any mod specific folder under the `plugins` folder but yes.

This value effects what folders JSON Loader will recursively load JSON's from.

#### CSV Loading Origination Path
This is effectively a CSV as a value. All values passed into it must be Paths using similar logic to that seen in the Artwork Form Support section of this README.

By default, this value is set to: `Sheets, Plugins/Sheets` to make mods work without the User needing to configure this. But if you need more Paths just add them to the end of the CSV.

These paths are Relative to your mods specific folder under the `plugins` folder, well more so any mod specific folder under the `plugins` folder but yes.

This value effects what folders CSV Loader will recursively load CSV's from.

#### Schema Save Path
This must represent one SINGULAR path, similar to those above. This path is relative to the DLL this API takes root within.

The default value is `/Schemas`.

#### Show Verbose Logging
It's less of a Verbose Logging but when set to `true`, the API will output Debug information in the Console and in the Log File.

#### Show Additional Information
When this value is set to `true` the API will output some Additional Information with common errors with the API. Think o it as a modmakers tooling. This will be outputted to the File and Console.

#### Show Summary Information
When this value is set to `true` when the API is validating Item's against their related Schema's, it will print the description of those properties as well. Again both to the Console and Log File.

### Recursively Scan At Plugin Level
Compatibility mode that makes the File Finder recursively scan from the Plugin Level rather than from the specified Path's levels in their respective configs.
___
## Installation

<u>**FOR LINUX AND STEAMDECK:** Ensure that the game is set to run using `Proton` in he game settings on steam. Should be a setting like this: `Change launch behaviour` -> `Proton`.</u>

### Installing with a Mod Manager
1. Download and install [Thunderstore Mod Manager](https://www.overwolf.com/app/Thunderstore-Thunderstore_Mod_Manager), [Gale](https://thunderstore.io/c/inscryption/p/Kesomannen/GaleModManager/) or [r2modman](https://thunderstore.io/c/inscryption/p/ebkr/r2modman/).
2. Click the **Install with Mod Manager** button on the top of [BepInEx's](https://thunderstore.io/c/inscryption/p/BepInEx/BepInExPack_Inscryption/) page.
3. Run the game via the mod manager.

If you have issues with Mod Managers head to one of these discords;

* **Thunderstore Support Discord:** [Here](https://discord.gg/Fbz54kQAxg)
* **R2ModMan Support Discord:** [Here](https://discord.gg/R85wjqa4WN)
* **Gale Mod Manager Support Discord:** [Here](https://discord.gg/sfuWXRfeTt)

### Installing Manually
1. Install [BepInEx](https://thunderstore.io/package/download/BepInEx/BepInExPack_Inscryption/5.4.2305/) by pressing `Manual Download` and extract the contents into a folder. **Do not extract into the game folder!**
2. Move the contents of the `BepInExPack_Inscryption` folder into the game folder (where the game executable is; usually found here: `C:\Program Files (x86)\Steam\steamapps\common\Inscryption`).
3. Run the game. If everything was done correctly, you will see the BepInEx console appear on your desktop. Close the game after it finishes loading.
4. Install [MonoModLoader](https://inscryption.thunderstore.io/package/BepInEx/MonoMod_Loader_Inscryption/) and extract the contents into a folder.
5. Move the contents of the `patchers` folder into `BepInEx/patchers` (If any of the mentioned BepInEx folders don't exist, just create them).
6. Install [Inscryption API](https://inscryption.thunderstore.io/package/API_dev/API/) and extract the contents into a folder.
7. Move the contents of the `plugins` folder into `BepInEx/plugins` and the contents of the `monomod` folder into the `BepInEx/monomod` folder.
8. Run the game again. If everything runs correctly, a message will appear in the console telling you that the API was loaded.
9. For any additional mods create a new subfolder, it can be called anything and extract the zips archive into it and if there is a `BepInEx` folder within the zip instead drop the contents of that folder into the `BepInEx` root for the modding instance. EX;
    ```
    BepInEx // These go within the BepInEx root folder
    |-- config
    |-- patchers
    |-- plugins
    |-- monomod
    |-- core
    plugins // Files within go into the created plugin subfolder that was created for the mod
    |-- Art
    |-- Scripts
    |-- MyMod.dll
    manifest.json // Ignorable, but if kept, goes in plugin subfolder
    README.md // Ignorable, but if kept, goes in plugin subfolder
    CHANGELOG.md // Ignorable, but if kept, goes in plugin subfolder
    icon.png // Ignorable, but if kept, goes in plugin subfolder
    ```
10. Run the game once more and everything should be correct and working.

### Installing Manually (XBOX Game-Pass)
1. Install [BepInEx](https://thunderstore.io/package/download/BepInEx/BepInExPack_Inscryption/5.4.2305/) by pressing `Manual Download` and extract the contents into a folder. **Do not extract into the game folder!**
2. Move the contents of the `BepInExPack_Inscryption` folder into the game folder (where the game executable is; usually found here: `C:\XboxGames\Inscryption\Content`).
3. Install the following package: [XboxSilencio](https://thunderstore.io/c/inscryption/p/CORE_API_TEAM/GamepassSilencio/), this is a package which aims to solve frictions with XBOX Gamepass specific installations, such as HueyFS related errors.
4. Run the game. If everything was done correctly, you will see the BepInEx console appear on your desktop. Close the game after it finishes loading.
5. Install [MonoModLoader](https://inscryption.thunderstore.io/package/BepInEx/MonoMod_Loader_Inscryption/) and extract the contents into a folder.
6. Move the contents of the `patchers` folder into `BepInEx/patchers` (If any of the mentioned BepInEx folders don't exist, just create them).
7. Install [Inscryption API](https://inscryption.thunderstore.io/package/API_dev/API/) and extract the contents into a folder.
8. Move the contents of the `plugins` folder into `BepInEx/plugins` and the contents of the `monomod` folder into the `BepInEx/monomod` folder.
9. Run the game again. If everything runs correctly, a message will appear in the console telling you that the API was loaded.
10. For any additional mods create a new subfolder, it can be called anything and extract the zips archive into it and if there is a `BepInEx` folder within the zip instead drop the contents of that folder into the `BepInEx` root for the modding instance. EX;
    ```
    BepInEx // These go within the BepInEx root folder
    |-- config
    |-- patchers
    |-- plugins
    |-- monomod
    |-- core
    plugins // Files within go into the created plugin subfolder that was created for the mod
    |-- Art
    |-- Scripts
    |-- MyMod.dll
    manifest.json // Ignorable, but if kept, goes in plugin subfolder
    README.md // Ignorable, but if kept, goes in plugin subfolder
    CHANGELOG.md // Ignorable, but if kept, goes in plugin subfolder
    icon.png // Ignorable, but if kept, goes in plugin subfolder
    ```
11. Run the game once more and everything should be correct and working.

### Installing on the Steam Deck
1. Download [r2modman](https://thunderstore.io/c/inscryption/p/ebkr/r2modman/) on the Steam Deck's Desktop Mode and open it from its download using its `AppImage` file.
2. Download the mods you plan on using and their dependencies.
3. Go to the setting of the profile you are using for the mods and click `Browse Profile Folder`.
4. Copy the BepInEx folder, then go to Steam and open Inscryption's Properties menu
5. Go to `Installed Files` click `Browse` to open the folder containing Inscryption's local files; paste the BepInEx folder there.
6. Enter Gaming Mode and check 'Force the use of a specific Steam Play compatibility tool' in the Properties menu under `Compatibility`.
7. Go to the launch parameters and enter `WINEDLLOVERRIDES="winhttp.dll=n,b" %command%`.
8. Open Inscryption. If everything was done correctly, you should see a console appear on your screen.

### Mac & Linux
1. Follow the steps here first: <https://docs.bepinex.dev/articles/user_guide/installation/index.html>
2. Next do steps 4-10 of the Manual Installation
3. Your game should be setup for inscryption modding now

If you have any issues with Mac/Linux, Steam Deck, or Manual head over to the discord for this game:

* **Inscryption Modding Discord:** [Here](https://discord.gg/ZQPvfKEpwM)
___
## Installing from the GitHub Package:

<u>***If your using `MadH95-JSONCardLoader` for JLDR2 based mods, please make sure to go into `MADH.inscryption.JSONLoader.cfg` and disable JLDR Conversion, you may need to do this after launching the first time. If you don't `JSONLoader` in combination with `JSONLoader-Nightly` will register the card Twice to the Game.***</u>

### Manager

1. Find the package that correlates with your system, below is a quick reference of the RID's and what they correlate to;
- Win-x64: XBOX Gamepass Version of the game (based on the game on Windows)
- Win-x86: For the Steam Version of the game (based on the game on Windows)
- We also offer versions for the following:
  - Linux-Arm64
  - Linux-Arm
  - Linux-Musl-Arm64
  - Linux-Musl-X64
  - Linux-X64
  - OSX-Arm64 (Mac)
  - OSX-X64 (Mac)
  - Win-Arm64
2. Enter your Mod Manager and Import as a Local Mod via the Following Steps:
- R2ModMan
  1. Open the Application, Navigate to Inscryption, Navigate to the Profile you wish to Install to.
  2. Press `Settings`.
  3. Type `Import` into the Search box as highlighted below

      <img width="1919" height="1079" alt="image" src="https://github.com/user-attachments/assets/1ae6e189-b80e-42fc-bf23-fd6305502353" />
  4. Press `Import Local Mod`.
  5. Press `Select File` and Navigate to and Select the File you just downloaded from this Release.
  6. If you followed the steps correctly it should appear as follows (with the correct version of this GitHub release,

      <img width="649" height="599" alt="image" src="https://github.com/user-attachments/assets/ecafca6f-999c-4991-a9a7-8232db56b65b" />

     <u>*For this version it should so `0.0.6` when importing.*</u>
  7. Where it says `Author`: `Unknown` replace the `Unknown` with `MADH95`.
  8. Press `Import Local Mod`.
  9. Wait for Dependencies to Resolve and your set. (Note for any mods on Thunderstore needing an older version of JSONLoader you may want to also install them directly from the website, or do these steps after you finished installing those mods, you must remove ONLY the old version of the mod.)
- Gale
  1. Open the Application, Navigate to the Game, Than the Profile.
  2. At the top of the screen (on the left) press `Import` than `... Local Mod`.
  3. Navigate to and Select the File you just downloaded from this Release.
  4. Wait for Dependencies to Resolve and your set. (Note for any mods on Thunderstore needing an older version of JSONLoader you may want to also install them directly from the website, or do these steps after you finished installing those mods, you must remove ONLY the old version of the mod.)

### Manual

1. Find the package that correlates with your system, below is a quick reference of the RID's and what they correlate to;
- Win-x64: XBOX Gamepass Version of the game (based on the game on Windows)
- Win-x86: For the Steam Version of the game (based on the game on Windows)
- We also offer versions for the following:
  - Linux-Arm64
  - Linux-Arm
  - Linux-Musl-Arm64
  - Linux-Musl-X64
  - Linux-X64
  - OSX-Arm64 (Mac)
  - OSX-X64 (Mac)
  - Win-Arm64
2. Extract the Zip to a new folder.
3. Take the files and folders under `plugins` of the zip and move them up a folder.
4. Delete the `plugins` folder from the Extracted directory.
5. Now navigate to the location of your BepInEx install from your earlier setup for the API.
6. Navigate to `plugins` and add a folder entitled `MadH95-JSON_and_CSV_Loader_Nightly`.
7. Move the setup from steps `2-4` into the folder you created in step `7`.
___
## Support
If you need help with anything related to this API or Package head to the [Inscryption Modding](https://discord.gg/ZQPvfKEpwM).

If its related to Installation and not Mod Manager related please use Modding Help, otherwise if it is Mod Manager related head to their respective discords as linked earlier.

If its related to the Behavior of the API in relation to the mod your making use the JSONLoader channel, if its related to mods by others use Modding Help.

If you need anything specific from the maintainer of this Package contact: `@thincreator3483` on discord.