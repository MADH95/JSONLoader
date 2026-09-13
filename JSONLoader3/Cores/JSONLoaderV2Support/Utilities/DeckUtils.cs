// using System.Collections.Generic;
// using System.Collections.ObjectModel;
// using System.IO;
// using System.Linq;
// using DiskCardGame;
// using JSONLoader3.Cores.JSONLoaderV2Support.Objects;
// using JSONLoader3.Peripheral.JSON_LINT;
// using JSONLoader3.Peripheral.JSON_SCHEMA;
// using Sirenix.Utilities;
//
// namespace JSONLoader3.Cores.JSONLoaderV2Support.Utilities;
//
// /// <summary>
// /// A class for <see cref="JSONLoaderV2Support.Schemas.Deck"/> related Utilities.
// /// </summary>
// public class DeckUtils
// {
//     /// <summary>
//     /// A list of all the <see cref="JSONLoaderV2Support.Schemas.Deck"/> Files in which we need to load.
//     /// </summary>
//     internal static List<(string pluginName, string file)> StarterDecksToLoad = new List<(string pluginName, string file)>();
//
//     /// <summary>
//     /// An internal facing List of all JLDR2 <see cref="JSONLoaderV2Support.Schemas.Deck"/>s passed to the API.
//     /// </summary>
//     internal static List<StarterDeckInfo> allJLDR2Decks = new List<StarterDeckInfo>();
//
//     /// <summary>
//     /// A public facing read-only collection of all JLDR2 <see cref="JSONLoaderV2Support.Schemas.Deck"/>s passed to the API.
//     /// </summary>
//     public static ReadOnlyCollection<StarterDeckInfo> allJLDR2DecksPublic => allJLDR2Decks.AsReadOnly();
//
//     /// <summary>
//     /// A function to handle the loading of <see cref="JSONLoaderV2Support.Schemas.Card"/>'s.
//     /// </summary>
//     /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
//     public static void HandleDecks()
//     {
//         if (StarterDecksToLoad.Count == 0)
//         {
//             return;
//         }
//
//         JSONLoader3.FormatLogger("Debug", "DeckUtils", $"Writing JSONLoaderV2's {typeof(Deck).Name}'s Schema so that we can Lint against it.");
//         WriteSchema.WriteJSONSchema<Deck>("JSONLoaderV2");
//         JSONLoader3.FormatLogger("Debug", "DeckUtils", $"Loading JSONLoaderV2's {typeof(Deck).Name}'s Schema so that we can Lint against it.");
//         List<string> Schema = LoadSchema.FindAndLoadSchema<Deck>("JSONLoaderV2");
//         
//         foreach ((string pluginName, string file) in StarterDecksToLoad)
//         {
//             JSONLoader3.FormatLogger("Debug", "DeckUtils", $"Linting {file} against Schema.");
//             (List<(int depth, string propertyName, string propertyValue)> validatedJSON, bool check) = LintingTools.LintAgainstSchema<Deck>(file, Schema, "JSONLoaderV2");
//             if (!check)
//             {
//                 continue;
//             }
//
//             JSONLoader3.FormatLogger("Debug", "DeckUtils", $"Linting {file} against Schema was Successful, attempting to Parse into a JSONLoaderV2's {typeof(Deck).Name}.");
//             Parse(validatedJSON, file, pluginName);
//         }
//     }
//
//     /// <summary>
//     /// This function handles the Parsing of a Starter Deck into the Game.
//     /// </summary>
//     /// <param name="JSONStarterDeck">A List (of a int resembling JSON depth, a string resembling the field Name, a string resembling the string Value) representing the Starter Deck.</param>
//     /// <param name="file">The full Path to the File.</param>
//     /// <param name="pluginName">The full Path to the Plugin.</param>
//     /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
//     public static void Parse(List<(int depth, string propertyName, string propertyValue)> JSONStarterDeck, string file, string pluginName)
//     {
//         List<(int depth, string propertyName, string propertyValue)> decks = new List<(int depth, string propertyName, string propertyValue)>();
//         List<DeckDataObject> decksToPass = new List<DeckDataObject>();
//         foreach ((int depth, string propertyName, string propertyValue) in JSONStarterDeck)
//         {
//             if (propertyName == "decks" && depth == 1)
//             {
//                 decks = LintingTools.GetObjectProperties(propertyValue, propertyName, file).ToList();
//                 JSONLoader3.FormatLogger("Debug", "CardUtils", $"Found {propertyName} with value of ({string.Join(", ", decks.Select(x => $"{x.propertyName}: {x.propertyValue}"))}) successfully while parsing {typeof(JSONLoaderV2Support.Schemas.Deck).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
//             }
//             
//             List<string> deckObjects = LintingTools.GetArrayItems(propertyValue, propertyName, file);
//
//             foreach (string deck in deckObjects)
//             {
//                 List<string> fieldsToEdit = new List<string>();
//                 string name = "";
//                 string modPrefix = "";
//                 string title = " ";
//                 List<string> cards = new List<string>();
//                 string iconTexture = "";
//                 int unlockLevel = 0;
//                 foreach ((int depth2, string propertyName2, string propertyValue2) in decks)
//                 {
//                     if (propertyName2 == "fieldsToEdit" && depth2 == 1)
//                     {
//                         fieldsToEdit = LintingTools.GetArrayItems(propertyValue2, propertyName2, file).Select(x => x.Trim().Trim('"')).Where(x => !string.IsNullOrEmpty(x)).ToList();
//                         JSONLoader3.FormatLogger("Debug", "CardUtils", $"Found {propertyName2} with value of ({string.Join(", ", fieldsToEdit)}) successfully while parsing {typeof(Deck).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
//                     }
//
//                     if (propertyName2 == "name" && depth2 == 1)
//                     {
//                         name = propertyValue2.Trim('\"');
//                         JSONLoader3.FormatLogger("Debug", "CardUtils", $"Found {propertyName} with value of ({name}) successfully while parsing {typeof(Deck).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
//                     }
//
//                     if (propertyName2 == "modPrefix" && depth2 == 1)
//                     {
//                         modPrefix = propertyValue2.Trim('\"');
//                         JSONLoader3.FormatLogger("Debug", "CardUtils", $"Found {propertyName} with value of ({modPrefix}) successfully while parsing {typeof(Deck).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
//                     }
//
//                     if (propertyName2 == "title")
//                     {
//                         title = propertyValue2.Trim('\"');
//                         JSONLoader3.FormatLogger("Debug", "CardUtils", $"Found {propertyName} with value of ({title}) successfully while parsing {typeof(Deck).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
//                     }
//                     
//                     if (propertyName2 == "cards" && depth2 == 1)
//                     {
//                         cards = LintingTools.GetArrayItems(propertyValue2, propertyName2, file).Select(x => x.Trim().Trim('"')).Where(x => !string.IsNullOrEmpty(x)).ToList();
//                         JSONLoader3.FormatLogger("Debug", "CardUtils", $"Found {propertyName2} with value of ({string.Join(", ", cards)}) successfully while parsing {typeof(Deck).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
//                     }
//
//                     if (propertyName2 == "iconTexture" && depth2 == 1)
//                     {
//                         iconTexture = propertyValue2.Trim('\"');
//                         JSONLoader3.FormatLogger("Debug", "CardUtils", $"Found {propertyName} with value of ({iconTexture}) successfully while parsing {typeof(Deck).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
//                     }
//
//                     if (propertyName2 == "unlockLevel" && depth2 == 1)
//                     {
//                         unlockLevel = int.Parse(propertyValue2);
//                         JSONLoader3.FormatLogger("Debug", "CardUtils", $"Found {propertyName} with value of ({unlockLevel}) successfully while parsing {typeof(Deck).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
//                     }
//                 }
//                 
//                 if (modPrefix.IsNullOrWhitespace())
//                 {
//                     modPrefix = JSONLoader3.PluginGuid;
//                 }
//                 
//                 DeckDataObject deckDataObject = new DeckDataObject(fieldsToEdit, name, modPrefix, title, cards, iconTexture, unlockLevel, file, pluginName);
//                 decksToPass.Add(deckDataObject);
//             }
//         }
//
//         DeckObject deckObject = new DeckObject(decksToPass, file, pluginName);
//         List<StarterDeckInfo> infos = deckObject.ConvertDeckObjectToStarterDeckInfo();
//         foreach (StarterDeckInfo info in infos)
//             allJLDR2Decks.Add(info);
//     }
// }