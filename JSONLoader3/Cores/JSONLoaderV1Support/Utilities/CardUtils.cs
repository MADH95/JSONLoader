using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using DiskCardGame;
using JSONLoader3.Cores.JSONLoaderV1Support.Objects;
using JSONLoader3.Peripheral.JSON_LINT;
using JSONLoader3.Peripheral.JSON_SCHEMA;
using Card = JSONLoader3.Cores.JSONLoaderV1Support.Schemas.Card;

namespace JSONLoader3.Cores.JSONLoaderV1Support.Utilities;

/// <summary>
/// A class for <see cref="JSONLoaderV1Support.Schemas.Card"/> related Utilities.
/// </summary>
public class CardUtils
{
    /// <summary>
    /// A list of all the <see cref="JSONLoaderV1Support.Schemas.Card"/> Files in which we need to load.
    /// </summary>
    internal static List<(string pluginName, string file)> CardsToLoad = new List<(string pluginName, string file)>();

    /// <summary>
    /// An internal facing List of all JLDR Cards passed to the API.
    /// </summary>
    internal static List<CardInfo> allJLDRCards = new List<CardInfo>();

    /// <summary>
    /// A public facing read-only collection of all JLDR Cards passed to the API.
    /// </summary>
    public static ReadOnlyCollection<CardInfo> allJLDRCardsPublic => allJLDRCards.AsReadOnly();

    /// <summary>
    /// A function to handle the loading of <see cref="JSONLoaderV1Support.Schemas.Card"/>'s.
    /// </summary>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static void HandleCards()
    {
        if (CardsToLoad.Count == 0)
        {
            return;
        }

        JSONLoader3.FormatLogger("Debug", "CardUtils",
            $"Writing JSONLoaderV1's {typeof(Card).Name}'s Schema so that we can Lint against it.");
        WriteSchema.WriteJSONSchema<Card>("JSONLoaderV1");
        JSONLoader3.FormatLogger("Debug", "CardUtils",
            $"Loading JSONLoaderV1's {typeof(Card).Name}'s Schema so that we can Lint against it.");
        List<string> Schema = LoadSchema.FindAndLoadSchema<Card>("JSONLoaderV1");

        foreach ((string pluginName, string file) in CardsToLoad)
        {
            JSONLoader3.FormatLogger("Debug", "CardUtils", $"Linting {file} against Schema.");
            (List<(int depth, string propertyName, string propertyValue)> validatedJSON, bool check) =
                LintingTools.LintAgainstSchema<Card>(file, Schema, "JSONLoaderV1");
            if (!check)
            {
                continue;
            }

            JSONLoader3.FormatLogger("Debug", "CardUtils",
                $"Linting {file} against Schema was Successful, attempting to Parse into a JSONLoaderV1's {typeof(Card).Name}.");
            Parse(validatedJSON, file, pluginName);
        }
    }

    /// <summary>
    /// This function handles the Parsing of a Card into the Game.
    /// </summary>
    /// <param name="JSONCard">A List (of a int resembling JSON depth, a string resembling the field Name, a string resembling the string Value) representing the Card.</param>
    /// <param name="file">The full Path to the File.</param>
    /// <param name="pluginName">The full Path to the Plugin.</param>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static void Parse(List<(int depth, string propertyName, string propertyValue)> JSONCard, string file,
        string pluginName)
    {
        List<string> fieldsToEdit = new List<string>();
        string name = string.Empty;
        string displayedName = string.Empty;
        string description = string.Empty;
        List<string> metaCategories = new List<string>();
        string cardComplexity = string.Empty;
        string temple = string.Empty;
        int baseAttack = 0;
        int baseHealth = 0;
        bool hideAttackAndHealth = false;
        int bloodCost = 0;
        int bonesCost = 0;
        int energyCost = 0;
        List<string> gemColors = new List<string>();
        string specialStatIcon = string.Empty;
        List<string> tribes = new List<string>();
        List<string> traits = new List<string>();
        List<string> specialAbilities = new List<string>();
        List<string> abilities = new List<string>();
        List<AbilityData> customAbilities = new List<AbilityData>();
        List<SpecialAbilityData> customSpecialAbilities = new List<SpecialAbilityData>();
        EvolveData evolution = null;
        string defaultEvolutionName = string.Empty;
        TailData tail = null;
        IceCubeData iceCube = null;
        bool flipPortraitForStrafe = false;
        bool onePerDeck = false;
        List<string> appearanceBehaviour = new List<string>();
        string texture = string.Empty;
        string altTexture = string.Empty;
        string emissionTexture = string.Empty;
        string titleGraphic = string.Empty;
        string pixelTexture = string.Empty;
        List<string> decals = new List<string>();

        foreach ((int depth, string propertyName, string propertyValue) in JSONCard)
        {
            JSONLoader3.FormatLogger("Debug", "CardUtils", $"Found {propertyName} with value of ({propertyValue}), depth of ({depth}) while parsing {typeof(Card).Name}.");
            
            if (propertyName == "fieldsToEdit" && depth==1)
            {
                fieldsToEdit = LintingTools.GetArrayItems(propertyValue, propertyName, file)
                    .Select(x => x.Trim().Trim('"'))
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({string.Join(", ", fieldsToEdit)}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "name" && depth==1)
            {
                name = propertyValue.Trim('\"');
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({name}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "displayedName" && depth==1)
            {
                displayedName = propertyValue.Trim('\"');
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({displayedName}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "description" && depth==1)
            {
                description = propertyValue.Trim('\"');
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({description}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "metaCategories" && depth==1)
            {
                metaCategories = LintingTools.GetArrayItems(propertyValue, propertyName, file)
                    .Select(x => x.Trim().Trim('"'))
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({string.Join(", ", metaCategories)}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "cardComplexity" && depth==1)
            {
                cardComplexity = propertyValue.Trim('\"');
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({cardComplexity}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "temple" && depth==1)
            {
                temple = propertyValue.Trim('\"');
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({temple}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "baseAttack" && depth==1)
            {
                baseAttack = int.Parse(propertyValue);
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({baseAttack}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "baseHealth" && depth==1)
            {
                baseHealth = int.Parse(propertyValue);
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({baseHealth}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "hideAttackAndHealth" && depth==1)
            {
                hideAttackAndHealth = bool.Parse(propertyValue);
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({hideAttackAndHealth}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "bloodCost" && depth==1)
            {
                bloodCost = int.Parse(propertyValue);
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({bloodCost}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "bonesCost" && depth==1)
            {
                bonesCost = int.Parse(propertyValue);
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({bonesCost}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "energyCost" && depth==1)
            {
                energyCost = int.Parse(propertyValue);
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({energyCost}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "gemColors" && depth==1)
            {
                gemColors = LintingTools.GetArrayItems(propertyValue, propertyName, file)
                    .Select(x => x.Trim().Trim('"'))
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({string.Join(", ", gemColors)}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "specialStatIcon" && depth==1)
            {
                specialStatIcon = propertyValue.Trim('\"');
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({specialStatIcon}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "tribes" && depth==1)
            {
                tribes = LintingTools.GetArrayItems(propertyValue, propertyName, file)
                    .Select(x => x.Trim().Trim('"'))
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({string.Join(", ", tribes)}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "traits" && depth==1)
            {
                traits = LintingTools.GetArrayItems(propertyValue, propertyName, file)
                    .Select(x => x.Trim().Trim('"'))
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({string.Join(", ", traits)}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "specialAbilities" && depth==1)
            {
                specialAbilities = LintingTools.GetArrayItems(propertyValue, propertyName, file)
                    .Select(x => x.Trim().Trim('"'))
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({string.Join(", ", specialAbilities)}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "abilities" && depth==1)
            {
                abilities = LintingTools.GetArrayItems(propertyValue, propertyName, file)
                    .Select(x => x.Trim().Trim('"'))
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({string.Join(", ", abilities)}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "customAbilities" && depth == 1)
            {
                List<string> customAbilityObjects =
                    LintingTools.GetArrayItems(propertyValue, propertyName, file);

                foreach (string customAbilityObject in customAbilityObjects)
                {
                    List<(int depth, string propertyName, string propertyValue)> ability =
                        LintingTools.GetObjectProperties(customAbilityObject);
                    
                    JSONLoader3.FormatLogger(
                        "Debug",
                        "CardUtils",
                        $"customAbilityObject = ({customAbilityObject})");

                    JSONLoader3.FormatLogger(
                        "Debug",
                        "CardUtils",
                        $"ability properties = ({string.Join(", ", ability.Select(x => $"[{x.propertyName}] = [{x.propertyValue}]"))})");

                    string abilityName = ability
                        .First(x => x.propertyName == "name")
                        .propertyValue
                        .Trim('"');

                    string guid = ability
                        .First(x => x.propertyName == "GUID")
                        .propertyValue
                        .Trim('"');

                    customAbilities.Add(new AbilityData(abilityName, guid));
                }

                JSONLoader3.FormatLogger(
                    "Debug",
                    "CardUtils",
                    $"Found {propertyName} with value of ({string.Join(", ", customAbilities)}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "customSpecialAbilities" && depth == 1)
            {
                List<string> customSpecialAbilityObjects =
                    LintingTools.GetArrayItems(propertyValue, propertyName, file);

                foreach (string customSpecialAbilityObject in customSpecialAbilityObjects)
                {
                    List<(int depth, string propertyName, string propertyValue)> specialAbility =
                        LintingTools.GetObjectProperties(customSpecialAbilityObject);

                    string abilityName = specialAbility
                        .First(x => x.propertyName == "name")
                        .propertyValue
                        .Trim('"');

                    string guid = specialAbility
                        .First(x => x.propertyName == "GUID")
                        .propertyValue
                        .Trim('"');

                    customSpecialAbilities.Add(new SpecialAbilityData(abilityName, guid));
                }

                JSONLoader3.FormatLogger(
                    "Debug",
                    "CardUtils",
                    $"Found {propertyName} with value of ({string.Join(", ", customSpecialAbilities)}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "evolution" && depth==1)
            {
                List<(int depth, string propertyName, string propertyValue)> evolutionData = LintingTools.GetObjectProperties(propertyValue);

                string evolutionName = evolutionData
                    .First(x => x.propertyName == "evolutionName")
                    .propertyValue
                    .Trim('"');
                
                int turnsToEvolve = int.Parse(evolutionData
                    .First(x => x.propertyName == "turnsToEvolve")
                    .propertyValue
                    .Trim('"'));

                evolution = new EvolveData(
                    evolutionName,
                    turnsToEvolve
                );

                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({evolution}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "defaultEvolutionName" && depth==1)
            {
                defaultEvolutionName = propertyValue.Trim('\"');
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({defaultEvolutionName}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "tail" && depth==1)
            {
                List<(int depth, string propertyName, string propertyValue)> tailData = LintingTools.GetObjectProperties(propertyValue);

                string tailName = tailData
                    .First(x => x.propertyName == "tailName")
                    .propertyValue
                    .Trim('"');
                
                string tailLostPortrait = tailData
                    .First(x => x.propertyName == "tailLostPortrait")
                    .propertyValue
                    .Trim('"');

                tail = new TailData(
                    tailName,
                    tailLostPortrait
                );

                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({tail}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "iceCube" && depth == 1)
            {
                List<(int depth, string propertyName, string propertyValue)> iceCubeData = LintingTools.GetObjectProperties(propertyValue);

                string creatureWithin = iceCubeData
                    .First(x => x.propertyName == "creatureWithin")
                    .propertyValue
                    .Trim('"');

                iceCube = new IceCubeData(creatureWithin);

                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({iceCube}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "flipPortraitForStrafe" && depth==1)
            {
                flipPortraitForStrafe = bool.Parse(propertyValue);
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({flipPortraitForStrafe}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "onePerDeck" && depth==1)
            {
                onePerDeck = bool.Parse(propertyValue);
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({onePerDeck}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "appearanceBehaviour" && depth==1)
            {
                appearanceBehaviour = LintingTools.GetArrayItems(propertyValue, propertyName, file)
                    .Select(x => x.Trim().Trim('"'))
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({string.Join(", ", appearanceBehaviour)}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "texture" && depth==1)
            {
                texture = propertyValue.Trim('\"');
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({texture}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "altTexture" && depth==1)
            {
                altTexture = propertyValue.Trim('\"');
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({altTexture}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "emissionTexture" && depth==1)
            {
                emissionTexture = propertyValue.Trim('\"');
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({emissionTexture}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "titleGraphic" && depth==1)
            {
                titleGraphic = propertyValue.Trim('\"');
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({titleGraphic}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "pixelTexture" && depth==1)
            {
                pixelTexture = propertyValue.Trim('\"');
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({pixelTexture}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }

            if (propertyName == "decals" && depth==1)
            {
                decals = LintingTools.GetArrayItems(propertyValue, propertyName, file)
                    .Select(x => x.Trim().Trim('"'))
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();
                JSONLoader3.FormatLogger("Debug", "CardUtils",
                    $"Found {propertyName} with value of ({string.Join(", ", decals)}) successfully while parsing {typeof(Card).Name} from {Path.GetFileNameWithoutExtension(pluginName)} specifically {Path.GetFileNameWithoutExtension(file)}.");
            }
        }

        CardObject objectCard = new CardObject(fieldsToEdit, name, displayedName, description, metaCategories,
            cardComplexity, temple, baseAttack, baseHealth, hideAttackAndHealth, bloodCost, bonesCost, energyCost,
            gemColors, specialStatIcon, tribes, traits, specialAbilities, abilities, customAbilities,
            customSpecialAbilities, evolution, defaultEvolutionName, tail, iceCube, flipPortraitForStrafe, onePerDeck,
            appearanceBehaviour, texture, altTexture, emissionTexture, titleGraphic, pixelTexture, decals, file,
            pluginName);
        CardInfo info = objectCard.ConvertCardObjectToCardInfo();
        allJLDRCards.Add(info);
    }
}