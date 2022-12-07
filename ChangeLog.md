## 2.2.5
- Fixed altTexture not working
- Debug prints are now logged to debug instead of info

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

## 2.1
- Added starter deck support with help from Lily

## 2.0.1
- Fixed a defect with converting JLDR to JLDR2 as it relates to editing base game cards
- Base game cards are now edited directly instead of using the event. This fixes issues where copies of those cards still existed in other places but wouldn't get properly edited (example: Pack Rat).

## 2.0.0
- Updated to align with API 2.0

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
