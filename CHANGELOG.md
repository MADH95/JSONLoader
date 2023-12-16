## 2.5.3
- fixed abilityLearnedDialogue
- rewrote buffCards (again) to make it cleaner and work faster
- added the changeAppearance field which has this format:

"changeAppearance": [{
    "changePortrait": "portrait.png",
    "changeName": "Elder ([BaseCard.Info.displayedName])",
    "addDecals": [ "decal1.png", "decal2.png" ],
    "removeDecals": [ "decal3.png", "decal4.png" ]
}]

- added the OnDamageDirectly trigger which triggers when a card deals direct damage to the opponent, it has two variables [HitSlot] and [DamageAmount]
- added the [DeathSlot] variable to the OnPreKill and OnPreDeath triggers
- added a field called isPermanent to buffCards
- made buffCards and transformCards point the camera at the hand instead of the board if the card that they're modifying is in the hand
- added a function called SetVar which has this format: SetVar('function_name', 5) (currently gets resets after all actions caused by the current trigger have happened, but i will try to fix that later though)
- added support for comments in any jldr2 file, comments are to be used in this format:

//This changes the appearance of a card
"changeAppearance": [{
    "changePortrait": "portrait.png", //This changes the portrait of the card
    "changeName": "Elder ([BaseCard.Info.displayedName])", //This is just like evolve!
    "addDecals": [ "decal.png" ] //And last but not least i add a decal to the card here
}]

- fixed generated variables causing an error if one of their variables were null
- updated NCalcExtensions to the latest version
- made it so all fields in configils can be empty or only whitespace without it causing an error
- cleaned up some code
- rewrote the readme and some file names for better consistency
- updated the card names file, changelog and the configils documentation

--changes made by James <3--
- fields in JSONLoader are no longer case sensitive and can be upper or lower case to avoid errors.
- Tribe file extensions now support tribe.jldr2 and tribes.jldr2 to avoid errors.
- Fixed reload and export hotkeys inverted.
- Fixed JSONLoader not loading cards with non-english characters.

## 2.5.2
- Fixed Emissions not working
- Fixed mods with a custom tribe failing to load
- Fixed cards overriden with JSON missing fields that aren't specified
- Fixed Configils breaking due to parameters not being parsed correctly
- Exposed hotkeys as configs to fit individual needs. (See config for more info)
- _example cards are ignored yet again (This wasn't meant to be removed)

## 2.5.1
- Fixed Encounters throwing errors when importing cards form mods
- Incorrectly named cards will now throw an error and suggest names of similarly named cards.
- Errors in .jldr2 files now log more information when they occur.

## 2.5.0
- Bumped API requirement to 2.18.2
- Added custom mask support (With example .jldr2 and .schema!)
- Added custom language support (With example .jldr2 and .schema!)
- Added translation support for cards using *displayName* and *description*.
- Added support for overriding existing encounters
- Added hotkey to export all compatible content to .jldr2  (Left Control + Right Control + X)
- Added config to enable verbose logging to help debugging
- Added Tribe .jldr2 example
- Reduced the amount of times JSONLoader looks for files when starting the game
- Moved Change notes from Readme to ChangeLog.md
- Updated 8 Fucking bears card example. Includes custom tribe example

## 2.4.5 (configil patchnotes)
- rewrote and cleaned up a ton of code
- added the triggers: OnPlayerStartOfTurn, OnOpponentStartOfTurn, OnPlayerEndOfTurn and OnOpponentEndOfTurn
- fixed retainMods removing the evolve sigil from the card because of some left over code from evolve
- added targetCard to the card object
- fixed a bug where the game would softlock when infused was not set in addAbilities
- fixed targetCard only working with singular variables and not with things like functions, this does mean that targetCard will now require variables to be encased in parentheses to enable NCalc
- fixed a bug where if you set one sigil to be infused in addAbilities that it would make all infused
- changed removeAbilities to now be a list in this format:
  "removeAbilities": [{
  "list": "",
  "name": "",
  "all": ""
  }]
- added a new field to removeAbilities called "all", when this field is set to true all instances of a sigil will be removed from a card instead of just one (this also means that the default for removing sigils isn't removing all instances of that sigil anymore, but now just one)
- added a new field called "list" to both addAbilities and removeAbilities, this field can be set to any list and will add/remove any sigils from that list from the targeted card
- added a "damageSource" field to damageCards which functions like targetCard but changes what card the game will think the damage came from, this field can be set to "null" to have there be no damage source
- changed gemCost in activationCost to be gemsCost instead to be in line with the card jldr2's
- added a function called ListContains() which can be used to see if a list contains a certain object
- added a couple of general use functions to check if a card has a certain "modifier" called: HasAbility(card, ability), HasTribe(card, tribe), HasTrait(card, trait) and HasSpecialAbility(card, specialAbility)
- added a couple of functions to get the object of a specific type from it's name: Ability('GuidPlusName'), Tribe('GuidPlusName'), Trait('GuidPlusName') and SpecialAbility('GuidPlusName'). Here's an example of how you could use the Ability() function: (HasAbility([BaseCard], Ability('Submerge')))

(an object is basically just a container with information about something, so for example the object of a sigil contains things like: the name, the guid, the description, the metacategories etc.)

## 2.4.3
- Actually added the file now

## 2.4.2
- Readded an important file that i deleted because i thought it wasn't important :P

## 2.4.1
- Fixed the patchnotes

## 2.4.0 (configil patchnotes)
- Added a maxEnergy field to gainCurrency (coded by UwUMacaroniTime)
- Made configils run much faster and have better performance (coded by kelly betty)
- Fixed any errors relating to a sigil trying to access the card that it is on after it has died or has been removed from the board
- Fixed the passive trigger spamming the console and cards dying when their attack was set to 0 when using the passive trigger
- Fixed gainCurrency foils only being visual and not actually increasing the foil amount
- Added a [DamageAmount] variable to both OnStruck and OnDamage
- Fixed runOnCondition not working for sendMessage
- Added two new variables [card.TemporaryMods] and [card.AllAbilities]

## 2.3.0
- Added talking card support!

## 2.3.0
- Added talking card support!
	
## 2.2.4
- Added gramophone support

## 2.2.3
- Added encounter support
- Removed the [PlayerSlot()] and [OpponentSlot()] variable and replaced it with the GetSlot() function with this format:
  GetSlot(index, isOpponentSlot, fields) 
  (fields is everything that you would have after the first dot of the original variable in single quotation marks)
- Fixed a ton more configil bugs

## 2.2.2
- Added tribe support

## 2.2.1
- Added card and sigil reloading and fixed a ton of bugs with configils

## 2.2.0
- Added API for adding cards

## 2.1.1
- Added configils (made by Lily#7394)

## 2.1.0
- Added starter deck support with help from Lily

## 2.0.1
- Fixed a defect with converting JLDR to JLDR2 as it relates to editing base game cards
- Base game cards are now edited directly instead of using the event. This fixes issues where copies of those cards still existed in other places but wouldn't get properly edited (example: Pack Rat).

## 2.0.0
- Rewritten to be compatible with API 2.0

## 1.7.2
- Added check for "_example" on the end of file name to remove example files from loading

## 1.7.1
- Fixed discrepancies in ancillary files

## 1.7.0
- Fixed error when assigning temple on custom cards
- Removed TestDeck to be released as an individual mods
- Support for customSpecialAbilities in API v1.12
- Support for mod manager with custom .jldr extension on card files
- cost changed to bloodCost in accordance with API
- Refactored JLUtils into clearer classes
- Removed redundant variables (evolve_evolutionName, etc.)
- Alphabetized Card Names.txt
- Made Enums.md more infomrative

## v1.6.1
- Fixed error if abilities array is present but empty

## v1.6.0
- Added support for adding custom abilities implemented by other mods

## v1.5.3
- Changed default "metaCategories" to be an empty list
- Added ChangeLog.md

## v1.5.0
- Compatability patch for InscryptionAPI v1.11
- Refactored Code:
	+ Error checking no longer uses exception handling to funciton
	+ Game will load default deck if the names in the config for testdeck don't exist
	+ baseHealth and metaCategories are handled before the call to NewCard.Add to avoid bloat in the function call
	+ Removed Evolve/Tail/IceCubeParams handling to utilise API's handling of those aspects instead
	+ Added ErrorUtil to better help with error logging
	+ Made use of new json parser to read in Evolve/Tail/IceCubeData from json objects

## v1.4.0
- Changed json parser from Unity's JSONUtility to TinyJson

## v1.3.11
- Fixed error with png check

## v1.3.10
- Added check for textures being png files

## v1.3.9
- Fixed bug where textures were being assigned wrong

## v1.3.8
- Fixed bug where "altTexture", "pixelTexture" and "titleGraphic" were being set to "texture"

## v1.3.6
- Fixed linking error from update v1.3.5
Refactored Code:
	+ Seperated EvolveData, TailData, and IceCubeData into their own files and gave them functions to handle their own generation and conversion to Param varients
	+ Added class CDUtils to seperate the data from card adding/editing funcitons
	+ seperated ExtensionUtils into it's own file

## v1.3.5
- Compatablity patch for InscryptionAPI v1.10
- Refactored code:
	+ CardData has it's own file
	+ Dicts has it's own file
	+ EvolveData, TailData, and IceCubeData are now in their own file together
	+ JLUtils was created to host utility functions like assignment helpers and validity checks

## v1.3.3
- updated error logging for user

## v1.3.2
- Added ability to edit existing cards with the use of "fieldsToEdit" with example card *OP-ossum.json*
- Added better error logging for user

## v1.2.1
- Compatablity patch for InscryptionAPI v1.8

## v1.2.0
- Added validity check for TestDeck cards
- Added functionality for:
	+ EvolveParams via "evolve_evolutionName" & "evolve_turnsToEvolve"
	+ TailParams via "tail_cardName" & "tail_tailLostPortrait"
	+ IceCubeParams via "iceCube_creatureWithin"
- Patched LoadScreenManager.LoadGameData and ChapterSelectMenu_OnChapterConfirmed to facilitate above implementation
- Minor refactor of NewCard call

## v1.1.1
- Added ability to use strings for the enum values on a card
- Added ability to add cards to starting deck using config file in order to test functionality

## v1.0.1
- Fixed issue with hard coded file paths

## v1.0.0
- Handles cards that don't use:
	+ EvolveParams
	+ TailParams
	+ IceCubeParams
