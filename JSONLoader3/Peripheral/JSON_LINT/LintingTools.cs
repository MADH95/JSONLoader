using System.IO;

namespace JSONLoader3.Peripheral.JSON_LINT;

/// <summary>
/// This class handles JSON Linting.
/// </summary>
public class LintingTools
{
    /// <summary>
    /// A function intended to lint a JSON File against a JSON Schema.
    /// </summary>
    /// <param name="file">The Full Path to the JSON File.</param>
    /// <param name="schemaFile">The Full Path to the JSON Schema File.</param>
    public static void LintAgainstSchema(string file, string schemaFile)
    {
        using FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
        using StreamReader reader = new StreamReader(stream);
    }
}