using System;
using System.Collections.Generic;
using JSONLoader3.Cores.JSONLoaderV1Support.Schemas;
using JSONLoader3.Peripheral.JSON_LINT;

namespace JSONLoader3.Cores.JSONLoaderV1Support.Utilities;

/// <summary>
/// A class for <see cref="JSONLoaderV1Support.Schemas.Card"/> related Utilities.
/// </summary>
public class CardUtils
{
    /// <summary>
    /// A list of all the <see cref="JSONLoaderV1Support.Schemas.Card"/> Files in which we need to load.
    /// </summary>
    internal static List<String> CardsToLoad = new List<string>();

    /// <summary>
    /// A function to handle the loading of <see cref="JSONLoaderV1Support.Schemas.Card"/>'s.
    /// </summary>
    public static void HandleCards()
    {
        if (CardsToLoad.Count == 0)
        {
            return;
        }
        
        foreach (string file in CardsToLoad)
        {
            JSONLoader3.FormatLogger("Debug", "CardUtils", $"Linting {file} against Schema.");
            LintingTools.LintAgainstSchema(file, file);
        }
    }
}