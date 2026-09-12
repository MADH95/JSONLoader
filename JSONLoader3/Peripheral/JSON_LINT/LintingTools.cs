using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using JSONLoader3.Peripheral.JSON_SCHEMA;
using Sirenix.Utilities;
using UnityEngine.UIElements;

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
    /// <param name="JSONSchema">A List of String Resembling the full Schema.</param>
    /// <param name="LoaderName">The Loaders Identifier, this is used in the Schema and File Name.</param>
    /// <typeparam name="Class">The Class in which the Schema is being made for.</typeparam>
    /// <returns>A Tuple (of a List (of a int resembling JSON depth, a string resembling the field Name, a string resembling the string Value), and a bool saying whether it was valid or not).</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static (List<(int depth, string propertyName, string propertyValue)> validatedJSON, bool check)
        LintAgainstSchema<Class>(string file, List<string> JSONSchema, string LoaderName)
    {
        List<string> JSONFile = LoadInJSONItem(file);

        foreach (string line in JSONFile)
        {
            int currentLine = JSONFile.IndexOf(line);

            if (currentLine != JSONFile.Count - 1)
                if ((JSONFile[currentLine + 1].Trim().StartsWith("}") ||
                     JSONFile[currentLine + 1].Trim().EndsWith("}")) && JSONFile[currentLine].EndsWith(","))
                {
                    JSONLoader3.FormatLogger("ERROR", "LintingTools",
                        "Trailing Comma Detected on the Last Line of the JSON.");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        "This error occurs when the last Key in the JSON has a comma at the end prior to closing a brace.");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        $"To fix it just remove the comma at the end of line {currentLine}.");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        $"Here's the full path to the file the issue takes root in: {file}");
                }
        }

        List<(int depth, string propertyName, string propertyValue)> JSONProperties = DisectedJSON(JSONFile);
        List<(int depth, string propertyName, string propertyValue)> JSONSchemaProeprties = DisectedJSON(JSONSchema);

        List<string> requiredFields = GetRequiredFromSchema(JSONSchema);
        List<(string field, bool checkedField)> requiredFieldTicks = new List<(string field, bool checkedField)>();

        foreach (string field in requiredFields)
        {
            requiredFieldTicks.Add((field, false));
        }

        JSONLoader3.FormatLogger("Debug", "LintingTools", $"Required Properties Found: {string.Join(", ", requiredFields)}");

        foreach ((int depth, string propertyName, string propertyValue) prop in JSONProperties.Where(x => x.depth == 1))
        {
            string path = GetTraversalPath(JSONSchemaProeprties, prop, 2);
            if (path == string.Empty)
            {
                JSONLoader3.FormatLogger("ERROR", "LintingTools",
                    $"Something went wrong while doing PathTraversal for {prop.propertyName} the following should help identify what you did wrong.");
                if (GetTraversalPath(JSONSchemaProeprties,
                        (prop.depth, prop.propertyName.ToCamelCase(), prop.propertyValue), 2) != string.Empty)
                {
                    JSONLoader3.FormatLogger("ERROR", "LintingTools",
                        $"The casing of {prop.propertyName} within {Path.GetFileNameWithoutExtension(file)} is not correct, it should be in camelCase.");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        "Basically the Property needs to follow the correct convention which is first word in Lowercase, and the rest of the words first letters Capitalized.");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools", "likeThis");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        "In the JSON the KVP would look like: \"likeThis\": false");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        $"Here's the full path to the file the issue takes root in: {file}");
                    return (JSONProperties, false);
                }
                else
                {
                    if (!AllowExtraFieldsCheck(JSONSchema))
                    {
                        JSONLoader3.FormatLogger("ERROR", "LintingTools",
                            $"The Property of {prop.propertyName} is not apart of the JSON Schema, please remove it from the JSON entitled {Path.GetFileNameWithoutExtension(file)}.");
                        JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                            $"If you want to stop seeing these when you make your jsons, just cross check the JSON against the Schema saving it.");
                        JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                            "Theres a nifty site called JSONEditor that you may find useful, theres a guide in the README.");
                        JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                            "Additionally you can cross-reference against the Property List we provide in our API's documentation and WIKI's.");
                        JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                            $"Here's the full path to the file the issue takes root in: {file}");
                        JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                            $"You can also find the full Schema here: {Path.GetFullPath(LoadSchema.SchemaFolder + Path.DirectorySeparatorChar + LoaderName + Path.DirectorySeparatorChar + $"{LoaderName}_{typeof(Class).Name}_Schema.json")}");
                        return (JSONProperties, false);
                    }
                    else
                    {
                        JSONLoader3.FormatLogger("Warning", "LintingTools",
                            $"The Property of {prop.propertyName} is not apart of the JSON Schema, please remove it from the JSON entitled {Path.GetFileNameWithoutExtension(file)}.");
                        JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                            $"Here's the full path to the file the issue takes root in: {file}");
                        JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                            $"You can also find the full Schema here: {Path.GetFullPath(LoadSchema.SchemaFolder + Path.DirectorySeparatorChar + LoaderName + Path.DirectorySeparatorChar + $"{LoaderName}_{typeof(Class).Name}_Schema.json")}");
                        JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                            "This is a warning because additionalProperties was enabled for this Object thus we're letting it slide.");
                    }
                }
            }

            if (requiredFields.Contains(prop.propertyName))
            {
                for (int i = 0; i < requiredFieldTicks.Count; i++)
                {
                    if (requiredFieldTicks[i].Item1 == prop.propertyName)
                    {
                        requiredFieldTicks[i] = (prop.propertyName, true);

                        JSONLoader3.FormatLogger(
                            "Debug",
                            "LintingTools",
                            $"Ticked \"{prop.propertyName}\" as checked for the Required Properties."
                        );

                        break;
                    }
                }
            }

            if (path != string.Empty)
            {
                List<string> PropertySchema = GetJSONSchemaProperty(path, JSONSchema);

                if (!ValidatePropertyAgainstSchema(prop.propertyName, prop.propertyValue, PropertySchema, file,
                        Path.GetFullPath(LoadSchema.SchemaFolder + Path.DirectorySeparatorChar + LoaderName +
                                         Path.DirectorySeparatorChar +
                                         $"{LoaderName}_{typeof(Class).Name}_Schema.json")))
                {
                    return (JSONProperties, false);
                }
            }
        }

        foreach ((string field, bool check) in requiredFieldTicks)
        {
            if (!check)
            {
                JSONLoader3.FormatLogger("ERROR", "LintingTools",
                    $"Missing Required Field: {field} within {Path.GetFileNameWithoutExtension(file)}.");
                JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                    "To Fix this, simply add the field mentioned in the error to the json with the appropriate value.");
                JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                    $"Here's the full path to the file the issue takes root in: {file}");
                return (JSONProperties, false);
            }
        }

        return (JSONProperties, true);
    }

    /// <summary>
    /// A function to get a Path for Traversal in <see cref="GetJSONSchemaProperty"/>.
    /// </summary>
    /// <param name="JSONSchemaProperties">The List (of a int resembling JSON depth, a string resembling the field Name, a string resembling the string Value) resembling the JSON Schema.</param>
    /// <param name="JSON">List (of a int resembling JSON depth, a string resembling the field Name, a string resembling the string Value) resembling the JSON.</param>
    /// <param name="spelunkingDepth">The Multiplier to the Schema Level.</param>
    /// <returns>A string resembling a Schema Based Traversal Path</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static string GetTraversalPath(
        List<(int depth, string propertyName, string propertyValue)> JSONSchemaProperties,
        (int depth, string propertyName, string propertyValue) JSON, int spelunkingDepth)
    {
        List<string> traversalPath = new List<string>();

        int schemaIndex =
            JSONSchemaProperties.FindIndex(x =>
                x.propertyName == JSON.propertyName && x.depth == JSON.depth * spelunkingDepth);

        if (schemaIndex == -1)
        {
            return string.Empty;
        }

        traversalPath.Add(JSONSchemaProperties[schemaIndex].propertyName);

        int currentDepth = JSONSchemaProperties[schemaIndex].depth;

        for (int i = schemaIndex - 1; i >= 0; i--)
        {
            if (JSONSchemaProperties[i].depth < currentDepth)
            {
                traversalPath.Insert(0, JSONSchemaProperties[i].propertyName);
                currentDepth = JSONSchemaProperties[i].depth;
            }
        }

        return string.Join("/", traversalPath);
    }

    /// <summary>
    /// A function which dissects the JSON into the format expected by several functions.
    /// </summary>
    /// <param name="JSONFile">The List of String representing the JSON File.</param>
    /// <returns>List (of a int resembling JSON depth, a string resembling the field Name, a string resembling the string Value)</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static List<(int depth, string propertyName, string propertyValue)> DisectedJSON(List<string> JSONFile)
    {
        List<(int depth, string propertyName, string propertyValue)> JSONProperty =
            new List<(int depth, string propertyName, string propertyValue)>();

        int currentDepth = 0;

        foreach (string line in JSONFile)
        {
            int priorDepth = currentDepth;

            foreach (char character in line.Trim())
            {
                if (character == '{' || character == '[')
                {
                    currentDepth++;
                }
                else if (character == '}' || character == ']')
                {
                    currentDepth--;
                }
            }

            if (!line.Trim().StartsWith("{") &&
                !line.Trim().StartsWith("}") &&
                !line.Trim().StartsWith("[") &&
                !line.Trim().StartsWith("]") &&
                line.Trim().Contains(":"))
            {
                JSONProperty.Add((
                    priorDepth,
                    line.Trim().Substring(0, line.Trim().IndexOf(":")).Trim().Trim('"'),
                    line.Trim().Substring(line.Trim().IndexOf(":") + 1).Trim().TrimEnd(',')
                ));
            }
        }

        return JSONProperty;
    }


    /// <summary>
    /// Loads a JSON File into a List of String representing the JSON File.
    /// </summary>
    /// <param name="file">The full path to the JSON File.</param>
    /// <returns>A List of String representing the JSON File.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static List<string> LoadInJSONItem(string file)
    {
        FileStream ReadingStream = File.OpenRead(file);
        StreamReader Reader = new StreamReader(ReadingStream);

        List<string> toReturn = new List<string>();
        while (Reader.Peek() >= 0)
        {
            string line = Reader.ReadLine();
            toReturn.Add(line);
        }

        Reader.Dispose();

        return toReturn;
    }

    /// <summary>
    /// Gets a JSON Schema Property List from a Traversal List and the JSON Schema.
    /// </summary>
    /// <param name="propertyPath">The Traversal Path from <see cref="GetTraversalPath"/>.</param>
    /// <param name="JSONSchema">A List of String representing the JSON Schema.</param>
    /// <returns>A List of String representing all of the JSON Lines relevant to the given Property.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static List<string> GetJSONSchemaProperty(string propertyPath, List<string> JSONSchema)
    {
        List<string> path = propertyPath.Split('/').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

        List<string> currentPeice = JSONSchema;
        foreach (string pathItem in path)
        {
            currentPeice = DigThroughSchemaFindRelevant(pathItem, currentPeice);

            if (currentPeice.Count == 0)
            {
                break;
            }
        }

        return currentPeice;
    }

    /// <summary>
    /// A function that gets all of the Required Properties from the passed in JSON Schema.
    /// </summary>
    /// <param name="JSONSchema">A List of String representing the JSON Schema.</param>
    /// <returns>A List of String representing all of the Required Properties for the Object.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static List<string> GetRequiredFromSchema(List<string> JSONSchema)
    {
        List<string> toRequire = new List<string>();

        int start = -1;
        int end = -1;

        for (int i = 0; i < JSONSchema.Count; i++)
        {
            if (JSONSchema[i].Trim().StartsWith("\"required\": ["))
            {
                start = i + 1;
                break;
            }
        }

        if (start == -1)
            return toRequire;

        for (int i = start; i < JSONSchema.Count; i++)
        {
            if (JSONSchema[i].Trim().TrimEnd(',') == "]")
            {
                end = i;
                break;
            }
        }

        if (end == -1)
            return toRequire;

        for (int i = start; i < end; i++)
        {
            string field = JSONSchema[i].Trim().Trim('"', ',');

            if (!field.IsNullOrWhitespace() && !toRequire.Contains(field))
            {
                toRequire.Add(field);
            }
        }

        return toRequire;
    }

    /// <summary>
    /// A Boolean for whether or not Extra Fields are Allowed for the Object by the Schema.
    /// </summary>
    /// <param name="JSONSchema">A List of String representing the JSON Schema.</param>
    /// <returns>A true if allowed, and a false if disallowed</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static bool AllowExtraFieldsCheck(List<string> JSONSchema)
    {
        bool additionalProps = true;
        foreach (string line in JSONSchema)
        {
            if (line.Contains("\"additionalProperties\": "))
            {
                additionalProps = bool.Parse(line.Trim().Replace("\"additionalProperties\": ", "").Trim('"', ','));
                break;
            }
        }

        return additionalProps;
    }

    /// <summary>
    /// Digs Through an Array, Finds all of the Arrays Contents, and Sends it back.
    /// </summary>
    /// <param name="jsonArray">A string representing the JSON Array.</param>
    /// <returns>A List of String representing the JSON Arrays Contents. An empty string is returned if the Array has no Items.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    internal static List<string> DigThroughArrayFindRelevant(string jsonArray)
    {
        if (!jsonArray.Trim().StartsWith("[") || !jsonArray.Trim().EndsWith("]"))
        {
            return new List<string>();
        }

        string newJSONArray = jsonArray.Trim().Substring(1, jsonArray.Trim().Length - 2).Trim();

        List<string> arrayData = new List<string>();

        int pointA = 0;
        int depth = 0;
        for (int i = 0; i < newJSONArray.Length; i++)
        {
            if (newJSONArray[i] == '{' || newJSONArray[i] == '[')
            {
                depth++;
            }
            else if (newJSONArray[i] == '}' || newJSONArray[i] == ']')
            {
                depth--;
            }

            if (newJSONArray[i] == ',' && depth == 0)
            {
                arrayData.Add(newJSONArray.Substring(pointA, i - pointA).Trim());
                pointA = i + 1;
            }
        }

        if (pointA < newJSONArray.Length)
        {
            arrayData.Add(newJSONArray.Substring(pointA).Trim());
        }

        return arrayData;
    }

    /// <summary>
    /// A function that Digs through the JSON Schema and finds all Lines Relevant to a specific Property.
    /// </summary>
    /// <param name="propertyToFind">The Property in which you want to find in the Schema Sample passed in.</param>
    /// <param name="JSONSchema">A List Of String resembling the Schema Segment to Dig Through.</param>
    /// <returns>A List of String of all the Content related to the Property your after.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    internal static List<string> DigThroughSchemaFindRelevant(string propertyToFind, List<string> JSONSchema)
    {
        int pointA = -1;
        foreach (string item in JSONSchema)
        {
            if (item.Trim().StartsWith($"\"{propertyToFind}\": {{"))
            {
                pointA = JSONSchema.IndexOf(item);

                break;
            }
        }

        List<string> SchemaPeice = new List<string>();
        if (pointA == -1)
        {
            JSONLoader3.FormatLogger("Warning", "LintingTools", $"Could not find {propertyToFind} in Schema.");
            return SchemaPeice;
        }

        int braceDepth = 0;
        bool startedObject = false;

        for (int i = pointA; i < JSONSchema.Count; i++)
        {
            string currentLine = JSONSchema[i];

            SchemaPeice.Add(currentLine);

            foreach (char character in currentLine)
            {
                if (character == '{')
                {
                    braceDepth++;
                    startedObject = true;
                }
                else if (character == '}')
                {
                    braceDepth--;
                }
            }

            if (startedObject && braceDepth == 0)
            {
                break;
            }
        }

        return SchemaPeice;
    }

    /// <summary>
    /// This is a helper unused in this class directly, but useful if you need to GetProperties related to an Object in your Items Utilities.
    /// </summary>
    /// <param name="jsonObject">A String representing the JSON Object.</param>
    /// <returns>List (of a int resembling JSON depth, a string resembling the field Name, a string resembling the string Value)</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    internal static List<(int depth, string propertyName, string propertyValue)> GetObjectProperties(string jsonObject)
    {
        List<string> fakeJSON = new List<string>
        {
            "{"
        };

        fakeJSON.AddRange(jsonObject.Trim().Split('\n'));

        fakeJSON.Add("}");

        List<(int depth, string propertyName, string propertyValue)> properties =
            DisectedJSON(fakeJSON);

        if (properties.Count == 0)
        {
            return new List<(int depth, string propertyName, string propertyValue)>();
        }

        int objectDepth = properties[0].depth;

        return properties
            .Where(x => x.depth == objectDepth)
            .ToList();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="jsonObject">A String representing the JSON Object.</param>
    /// <param name="propertyToFind">The Property in which you want to find in the Schema Sample passed in.</param>
    /// <param name="file">The full path to the JSON File.</param>
    /// <returns></returns>
    internal static List<(int depth, string propertyName, string propertyValue)> GetObjectProperties(string jsonObject, string propertyToFind, string file)
    {
        if (jsonObject.Trim() != "{")
        {
            return GetObjectProperties(jsonObject);
        }

        List<string> JSONFile = File.ReadAllLines(file).ToList();

        int propertyIndex = JSONFile.FindIndex(x =>
            x.Trim().StartsWith($"\"{propertyToFind}\""));

        if (propertyIndex == -1)
        {
            return new List<(int depth, string propertyName, string propertyValue)>();
        }

        string completeObject = JSONFile[propertyIndex]
            .Substring(JSONFile[propertyIndex].IndexOf(":") + 1)
            .Trim();

        int depth = 0;

        foreach (char character in completeObject)
        {
            if (character == '{')
            {
                depth++;
            }
            else if (character == '}')
            {
                depth--;
            }
        }

        while (depth > 0 && propertyIndex + 1 < JSONFile.Count)
        {
            propertyIndex++;

            string nextLine = JSONFile[propertyIndex].Trim();
            completeObject += "\n" + nextLine;

            foreach (char character in nextLine)
            {
                if (character == '{')
                {
                    depth++;
                }
                else if (character == '}')
                {
                    depth--;
                }
            }
        }

        return GetObjectProperties(completeObject.TrimEnd(','));
    }

    /// <summary>
    /// See <see cref="DigThroughArrayFindRelevant"/> for more details.
    /// </summary>
    /// <param name="jsonArray">A String representing the JSON Array.</param>
    /// <param name="propertyName">The Name of the Property.</param>
    /// <param name="file">The full path to the file.</param>
    /// <returns>A List of String based on the results of <see cref="DigThroughArrayFindRelevant"/>.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    internal static List<string> GetArrayItems(string jsonArray, string propertyName, string file)
    {
        if (jsonArray.Trim() != "[")
        {
            return DigThroughArrayFindRelevant(jsonArray);
        }

        List<string> JSONFile = File.ReadAllLines(file).ToList();

        int propertyIndex = JSONFile.FindIndex(x =>
            x.Trim().StartsWith($"\"{propertyName}\""));

        if (propertyIndex == -1)
        {
            return new List<string>();
        }

        string completeArray = JSONFile[propertyIndex]
            .Substring(JSONFile[propertyIndex].IndexOf(":") + 1)
            .Trim();

        int depth = 0;

        foreach (char character in completeArray)
        {
            if (character == '[')
            {
                depth++;
            }
            else if (character == ']')
            {
                depth--;
            }
        }

        while (depth > 0 && propertyIndex + 1 < JSONFile.Count)
        {
            propertyIndex++;

            string nextLine = JSONFile[propertyIndex].Trim();
            completeArray += "\n" + nextLine;

            foreach (char character in nextLine)
            {
                if (character == '[')
                {
                    depth++;
                }
                else if (character == ']')
                {
                    depth--;
                }
            }
        }

        return DigThroughArrayFindRelevant(completeArray.TrimEnd(','));
    }
    
    /// <summary>
    /// Gets The Schemas under the AnyOf Type.
    /// </summary>
    /// <param name="JSONSchema">A List Of String resembling the Schema Segment to Dig Through.</param>
    /// <returns>A List of Schemas associated with the AnyOf Type.</returns>
    /// /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    internal static List<List<string>> GetAnyOfSchemas(List<string> JSONSchema)
    {
        List<List<string>> anyOfSchemas = new List<List<string>>();

        int anyOfIndex = JSONSchema.FindIndex(x =>
            x.Trim().StartsWith("\"anyOf\": ["));

        if (anyOfIndex == -1)
        {
            return anyOfSchemas;
        }

        List<string> currentSchema = null;
        int braceDepth = 0;

        for (int i = anyOfIndex + 1; i < JSONSchema.Count; i++)
        {
            string line = JSONSchema[i];

            if (line.Trim().StartsWith("{"))
            {
                if (braceDepth == 0)
                {
                    currentSchema = new List<string>();
                }

                braceDepth++;
            }

            if (currentSchema != null)
            {
                currentSchema.Add(line);
            }

            if (line.Trim().StartsWith("}"))
            {
                braceDepth--;

                if (braceDepth == 0 && currentSchema != null)
                {
                    anyOfSchemas.Add(currentSchema);
                    currentSchema = null;
                }
            }

            if (line.Trim() == "]" && braceDepth == 0)
            {
                break;
            }
        }

        return anyOfSchemas;
    }

    /// <summary>
    /// See <see cref="DigThroughArrayFindRelevant"/> for more details.
    /// </summary>
    /// <param name="jsonArray">A String representing the JSON Array.</param>
    /// <returns>A List of String based on the results of <see cref="DigThroughArrayFindRelevant"/>.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    internal static List<string> GetArrayItems(string jsonArray)
    {
        return DigThroughArrayFindRelevant(jsonArray);
    }

    /// <summary>
    /// This is the JSON Validator's core, it handles ensuring the JSON Itself is valid.
    /// </summary>
    /// <param name="jsonPropertyName">The Property we are Validating.</param>
    /// <param name="jsonPropertyValue">The Property Value we are Validating.</param>
    /// <param name="PropertySchema">The List of String representing the Schema.</param>
    /// <param name="file">The Full Path to the JSON File.</param>
    /// <param name="schemaFile">The Full Path to the JSON Schema File.</param>
    /// <returns>A true if valid, a false if invalid.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static bool ValidatePropertyAgainstSchema(string jsonPropertyName, string jsonPropertyValue,
        List<string> PropertySchema, string file, string schemaFile)
    {
        string type = "";
        string description = "";

        bool typeSet = false;
        bool descSet = false;

        List<string> newPropertySchema = new List<string>(PropertySchema);
        foreach (string line in PropertySchema)
        {
            if (PropertySchema.IndexOf(line) == 0)
            {
                newPropertySchema.RemoveAt(0);
            }

            if (line.Contains("\"type\": \"") && !typeSet)
            {
                type = line.Trim().Replace("\"type\": \"", "").Trim('"', ',');
                newPropertySchema.Remove(line);
                typeSet = true;
            }

            if (line.Contains("\"description\": \"") && !descSet)
            {
                description = line.Trim().Replace("\"description\": \"", "").Trim('"', ',');
                newPropertySchema.Remove(line);
                descSet = true;
            }
        }

        JSONLoader3.FormatLogger("Summary", "LintingTools",
            $"Property Description for {jsonPropertyName} for ease of access.");
        foreach (string line in description.Split('|').ToList())
            JSONLoader3.FormatLogger("Summary", "LintingTools", line.Trim().Replace("\\\"", "\""));

        if (type == "string" && PropertySchema.Any(x => x.Trim().StartsWith("\"anyOf\": [")))
        {
            return ValidateAnyOf(jsonPropertyName, jsonPropertyValue, PropertySchema, file, schemaFile);
        }

        if (type == "string")
        {
            return ValidateString(jsonPropertyName, jsonPropertyValue, newPropertySchema, file, schemaFile);
        }
        else if (type == "integer")
        {
            return ValidateInteger(jsonPropertyName, jsonPropertyValue, newPropertySchema, file, schemaFile);
        }
        else if (type == "boolean")
        {
            return ValidateBoolean(jsonPropertyName, jsonPropertyValue, newPropertySchema, file, schemaFile);
        }
        else if (type == "object")
        {
            return ValidateObject(jsonPropertyName, jsonPropertyValue, newPropertySchema, file, schemaFile);
        }
        else if (type == "array")
        {
            return ValidateArray(jsonPropertyName, jsonPropertyValue, newPropertySchema, file, schemaFile);
        }
        else
        {
            JSONLoader3.FormatLogger("Warning", "LintingTools",
                $"Could not find {type} support for validating {jsonPropertyName} within this Item Type for validating against the Schema, notify a maintainer if this is not intentional.");
        }

        return true;
    }

    /// <summary>
    /// This is the JSON Validator's AnyOf Handler, it handles ensuring the String Property is Valid.
    /// </summary>
    /// <param name="jsonPropertyName">The Property we are Validating.</param>
    /// <param name="jsonPropertyValue">The Property Value we are Validating.</param>
    /// <param name="PropertySchema">The List of String representing the Schema.</param>
    /// <param name="file">The Full Path to the JSON File.</param>
    /// <param name="schemaFile">The Full Path to the JSON Schema File.</param>
    /// <returns>A true if valid, a false if invalid.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static bool ValidateAnyOf(string jsonPropertyName, string jsonPropertyValue, List<string> PropertySchema, string file, string schemaFile)
    {
        List<List<string>> anyOfSchema = GetAnyOfSchemas(PropertySchema);

        JSONLoader3.FormatLogger("Debug", "LintingTools",
            $"Found {anyOfSchema.Count} Branches worth of Validators for validating {jsonPropertyName}. Attempting to validate against all of them, only one needs to pass to succeed.");

        foreach (List<string> validator in anyOfSchema)
        {
            if (ValidatePropertyAgainstSchema(jsonPropertyName, jsonPropertyValue, validator, file, schemaFile))
            {
                JSONLoader3.FormatLogger("Debug", "LintingTools",
                    $"This Validator has succeeded in validating {jsonPropertyName} with value of ({jsonPropertyValue})");
                return true;
            }
            else
            {
                JSONLoader3.FormatLogger("Debug", "LintingTools",
                    $"This Validator has failed in validating {jsonPropertyName} with value of ({jsonPropertyValue})");
            }
        }

        JSONLoader3.FormatLogger("ERROR", "LintingTools",
            $"We could not validate {jsonPropertyName} against any of the Validators available for {jsonPropertyName} ensure your value is valid. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
        JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
            $"This error triggers when none of the validators for {jsonPropertyName} returned errorless.");
        JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
            $"If you want to fix this, check how the Schema is trying to validate {jsonPropertyName} and ensure ({jsonPropertyValue} is valid against at least one of them.");
        JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
            $"Here's the full path to the file the issue takes root in: {file}");
        JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
            $"You can also find the full Schema here: {schemaFile}");

        return false;
    }

    /// <summary>
    /// This is the JSON Validator's String Handler, it handles ensuring the String Property is Valid.
    /// </summary>
    /// <param name="jsonPropertyName">The Property we are Validating.</param>
    /// <param name="jsonPropertyValue">The Property Value we are Validating.</param>
    /// <param name="PropertySchema">The List of String representing the Schema.</param>
    /// <param name="file">The Full Path to the JSON File.</param>
    /// <param name="schemaFile">The Full Path to the JSON Schema File.</param>
    /// <returns>A true if valid, a false if invalid.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static bool ValidateString(string jsonPropertyName, string jsonPropertyValue, List<string> PropertySchema,
        string file, string schemaFile)
    {
        int minLength = -1;
        Regex pattern = null;
        string @default = "";
        List<string> enums = new List<string>();
        for (int i = 0; i < PropertySchema.Count; i++)
        {
            string line = PropertySchema[i];

            if (line.Contains("\"minLength\": "))
            {
                minLength = int.Parse(
                    line.Trim()
                        .Replace("\"minLength\": ", "")
                        .Trim('"', ',')
                );
            }

            if (line.Contains("\"pattern\": \""))
            {
                pattern = new Regex(
                    Regex.Unescape(
                        line.Trim()
                            .Replace("\"pattern\": \"", "")
                            .Trim('"', ',')
                    )
                );
            }

            if (line.Contains("\"default\": \""))
            {
                @default = line.Trim()
                    .Replace("\"default\": \"", "").Replace(" ", "D")
                    .Trim('"', ',');
            }
            

            if (line.Contains("\"enum\": ["))
            {
                int start = i + 1;

                for (int j = start; j < PropertySchema.Count; j++)
                {
                    string enumLine = PropertySchema[j].Trim();

                    if (enumLine.TrimEnd(',') == "]")
                    {
                        break;
                    }

                    string enumValue = enumLine.Trim('"', ',');

                    if (!enumValue.IsNullOrWhitespace())
                    {
                        enums.Add(enumValue);
                    }
                }

                break;
            }
        }

        if (!(jsonPropertyValue.StartsWith("\"") && jsonPropertyValue.EndsWith("\"")))
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"Invalid String was Found. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "To fix this ensure the Value for this item is properly encases in Quotation marks.");
            return false;
        }

        if (minLength != -1 && jsonPropertyValue.Trim('"').Length < minLength)
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"The string is shorter than the Minimum Length ({minLength}) for {jsonPropertyName} according to the Schema for this Item. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"To fix this just simply extend the length of {jsonPropertyValue} above {minLength}.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Sadly as it broke here, I'm going to have to call a break, meaning we are going to stop proccessing this item and move on to the next one.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Hopefully you'll have it fixed for next load up!!");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Here's the full path to the file the issue takes root in: {file}");
            return false;
        }

        if (pattern != null && !pattern.IsMatch(jsonPropertyValue.Trim('"')))
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"The string value for {jsonPropertyName} is invalid based on the Regex specified by the Schema for this item ({pattern}). The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"To fix this copy the regex shown above and paste it into the expression box of https://regexr.com/.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Now take your value ({jsonPropertyValue}) and put it in the Text Box below it.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"The Regex will be described under tools, so now just edit your value for {jsonPropertyName} until it is valid.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Now just paste that value back into your JSON Key associated with {jsonPropertyName}, than your set for the next boot.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Keep in mind if it was an image, ensure the image matches that exact name. P.S. If your trying to route folders use '/' after the folders name to go in, and '../' to go in.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Sadly as it broke here, I'm going to have to call a break, meaning we are going to stop proccessing this item and move on to the next one.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Hopefully you'll have it fixed for next load up!!");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Here's the full path to the file the issue takes root in: {file}");
            return false;
        }

        if (@default.IsNullOrWhitespace() && jsonPropertyValue.Trim('"').IsNullOrWhitespace())
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"The string is empty and there is no default value for {jsonPropertyName} either define it or remove it. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"To fix this simply as the error states remove the KVP for {jsonPropertyName}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Alternatively define a value for {jsonPropertyName} you can toggle on Summary in The Config to show the Properties Description for further Information about it..");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Sadly as it broke here, I'm going to have to call a break, meaning we are going to stop proccessing this item and move on to the next one.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Hopefully you'll have it fixed for next load up!!");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Here's the full path to the file the issue takes root in: {file}");
            return false;
        }

        if (enums.Count > 0 && !enums.Any(enumValue => string.Equals(enumValue.Trim(), jsonPropertyValue.Trim().Trim('"'), StringComparison.Ordinal)))
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"The string does not match any of the required Enums for {jsonPropertyName} according to the Schema. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"To fix this just ensure the {jsonPropertyName} matches the actual Enums applicable for this field.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Look at the Description for {jsonPropertyName} for a list of possible values for this field.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Sadly as it broke here, I'm going to have to call a break, meaning we are going to stop proccessing this item and move on to the next one.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Hopefully you'll have it fixed for next load up!!");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Here's the full path to the file the issue takes root in: {file}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// This is the JSON Validator's Integer Handler, it handles ensuring the String Property is Valid.
    /// </summary>
    /// <param name="jsonPropertyName">The Property we are Validating.</param>
    /// <param name="jsonPropertyValue">The Property Value we are Validating.</param>
    /// <param name="PropertySchema">The List of String representing the Schema.</param>
    /// <param name="file">The Full Path to the JSON File.</param>
    /// <param name="schemaFile">The Full Path to the JSON Schema File.</param>
    /// <returns>A true if valid, a false if invalid.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static bool ValidateInteger(string jsonPropertyName, string jsonPropertyValue, List<string> PropertySchema,
        string file, string schemaFile)
    {
        int minimum = -1;
        int maximum = -1;
        int @default = -1;
        bool hasMinimum = false;
        bool hasMaximum = false;
        bool hasDefault = false;
        foreach (string line in PropertySchema)
        {
            if (line.Contains("\"minimum\": "))
            {
                minimum = int.Parse(line.Trim().Replace("\"minimum\": ", "").Trim('"', ','));
                hasMinimum = true;
            }

            if (line.Contains("\"maximum\": "))
            {
                maximum = int.Parse(line.Trim().Replace("\"maximum\": ", "").Trim('"', ','));
                hasMaximum = true;
            }

            if (line.Contains("\"default\": "))
            {
                @default = int.Parse(line.Trim().Replace("\"default\": ", "").Trim('"', ','));
                hasDefault = true;
            }
        }

        if ((jsonPropertyValue.StartsWith("\"") && jsonPropertyValue.EndsWith("\"")))
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"Invalid Int was Found. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "To fix this ensure the Value for this item is NOT encased in Quotation marks.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Sadly as it broke here, I'm going to have to call a break, meaning we are going to stop proccessing this item and move on to the next one.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Hopefully you'll have it fixed for next load up!!");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Here's the full path to the file the issue takes root in: {file}");
            return false;
        }

        if (jsonPropertyValue.IsNullOrWhitespace() && !hasDefault)
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"The value is empty and there is no default value for {jsonPropertyName} either define it or remove it. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"To fix this simply as the error states remove the KVP for {jsonPropertyName}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Alternatively define a value for {jsonPropertyName} you can toggle on Summary in The Config to show the Properties Description for further Information about it..");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Sadly as it broke here, I'm going to have to call a break, meaning we are going to stop proccessing this item and move on to the next one.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Hopefully you'll have it fixed for next load up!!");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Here's the full path to the file the issue takes root in: {file}");
            return false;
        }

        if (!int.TryParse(jsonPropertyValue, out int value))
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"The Value passed in with {jsonPropertyName} is not a valid Integer. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"The value associated with {jsonPropertyName} did not parse correctly as an Integer, the value being ({jsonPropertyValue})");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Sadly as it broke here, I'm going to have to call a break, meaning we are going to stop proccessing this item and move on to the next one.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Hopefully you'll have it fixed for next load up!!");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Here's the full path to the file the issue takes root in: {file}");
            return false;
        }

        if (value > maximum && hasMaximum)
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"The value of {jsonPropertyName} is Greater than the Maximum allowed by the Schema for this item, the maximum being {maximum}. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"To fix this simply set the value of {jsonPropertyName} to a Integer less than {maximum}.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Sadly as it broke here, I'm going to have to call a break, meaning we are going to stop proccessing this item and move on to the next one.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Hopefully you'll have it fixed for next load up!!");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Here's the full path to the file the issue takes root in: {file}");
            return false;
        }

        if (value < minimum && hasMinimum)
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"The value of {jsonPropertyName} is Less than the Minimum allowed by the Schema for this item, the minimum being {minimum}. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"To fix this simply set the value of {jsonPropertyName} to a Integer greater than {minimum}.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Sadly as it broke here, I'm going to have to call a break, meaning we are going to stop proccessing this item and move on to the next one.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Hopefully you'll have it fixed for next load up!!");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Here's the full path to the file the issue takes root in: {file}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// This is the JSON Validator's Boolean Handler, it handles ensuring the String Property is Valid.
    /// </summary>
    /// <param name="jsonPropertyName">The Property we are Validating.</param>
    /// <param name="jsonPropertyValue">The Property Value we are Validating.</param>
    /// <param name="PropertySchema">The List of String representing the Schema.</param>
    /// <param name="file">The Full Path to the JSON File.</param>
    /// <param name="schemaFile">The Full Path to the JSON Schema File.</param>
    /// <returns>A true if valid, a false if invalid.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static bool ValidateBoolean(string jsonPropertyName, string jsonPropertyValue, List<string> PropertySchema,
        string file, string schemaFile)
    {
        bool @default = false;
        bool hasDefault = false;
        foreach (string line in PropertySchema)
        {
            if (line.Contains("\"default\": "))
            {
                @default = bool.Parse(line.Trim().Replace("\"default\": ", "").Trim('"', ','));
                hasDefault = true;
            }
        }

        if ((jsonPropertyValue.StartsWith("\"") && jsonPropertyValue.EndsWith("\"")))
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"Invalid Boolean was Found. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "To fix this ensure the Value for this item is NOT encased in Quotation marks.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Sadly as it broke here, I'm going to have to call a break, meaning we are going to stop proccessing this item and move on to the next one.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Hopefully you'll have it fixed for next load up!!");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Here's the full path to the file the issue takes root in: {file}");
            return false;
        }

        if (jsonPropertyValue.IsNullOrWhitespace() && !hasDefault)
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"The value is empty and there is no default value for {jsonPropertyName} either define it or remove it. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"To fix this simply as the error states remove the KVP for {jsonPropertyName}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Alternatively define a value for {jsonPropertyName} you can toggle on Summary in The Config to show the Properties Description for further Information about it..");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Sadly as it broke here, I'm going to have to call a break, meaning we are going to stop proccessing this item and move on to the next one.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Hopefully you'll have it fixed for next load up!!");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Here's the full path to the file the issue takes root in: {file}");
            return false;
        }

        if (!bool.TryParse(jsonPropertyValue, out bool value))
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"The Value passed in with {jsonPropertyName} is not a valid Boolean. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"The value associated with {jsonPropertyName} did not parse correctly as an Boolean, the value being ({jsonPropertyValue})");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Sadly as it broke here, I'm going to have to call a break, meaning we are going to stop proccessing this item and move on to the next one.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Hopefully you'll have it fixed for next load up!!");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Here's the full path to the file the issue takes root in: {file}");
            return false;
        }

        if (jsonPropertyValue != jsonPropertyValue.ToLower())
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"The Value passed in with {jsonPropertyName} did not have the correct casing, the Value ({jsonPropertyValue}) must be all lowercase. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"To fix this just make sure the value associated with {jsonPropertyName} is in full lowercase.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Sadly as it broke here, I'm going to have to call a break, meaning we are going to stop proccessing this item and move on to the next one.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Hopefully you'll have it fixed for next load up!!");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Here's the full path to the file the issue takes root in: {file}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// This is the JSON Validator's Object Handler, it handles ensuring the String Property is Valid.
    /// </summary>
    /// <param name="jsonPropertyName">The Property we are Validating.</param>
    /// <param name="jsonPropertyValue">The Property Value we are Validating.</param>
    /// <param name="PropertySchema">The List of String representing the Schema.</param>
    /// <param name="file">The Full Path to the JSON File.</param>
    /// <param name="schemaFile">The Full Path to the JSON Schema File.</param>
    /// <returns>A true if valid, a false if invalid.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static bool ValidateObject(string jsonPropertyName, string jsonPropertyValue, List<string> PropertySchema,
        string file, string schemaFile)
    {
        List<string> fakeJSON = new List<string>();

        fakeJSON.Add("{");

        // If this is already an object (such as an object inside an array),
        // use its lines directly instead of wrapping it as a KVP.
        if (jsonPropertyValue.Trim().StartsWith("{"))
        {
            fakeJSON.AddRange(jsonPropertyValue.Trim().Split('\n'));
        }
        else
        {
            fakeJSON.Add($"\"{jsonPropertyName}\": {jsonPropertyValue}");
        }

        fakeJSON.Add("}");

        List<(int depth, string propertyName, string propertyValue)> JSONProperties = DisectedJSON(fakeJSON);

        if (JSONProperties.Count == 0)
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"Could not dissect object for {jsonPropertyName}. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            return false;
        }

        int objectDepth = JSONProperties[0].depth;

        // Only remove the wrapper property when we actually created one.
        if (!jsonPropertyValue.Trim().StartsWith("{"))
        {
            JSONProperties = JSONProperties.Where(x => x.propertyName != jsonPropertyName).ToList();
        }

        List<(int depth, string propertyName, string propertyValue)>
            JSONSchemaProperties = DisectedJSON(PropertySchema);

        List<string> requiredFields = GetRequiredFromSchema(PropertySchema);
        List<(string field, bool checkedField)> requiredFieldTicks = new List<(string field, bool checkedField)>();

        foreach (string field in requiredFields)
            requiredFieldTicks.Add((field, false));

        JSONLoader3.FormatLogger("Debug", "LintingTools", $"Required Fields: {string.Join(", ", requiredFields)}");

        foreach ((int depth, string propertyName, string propertyValue) prop in JSONProperties)
        {
            int normalizedDepth = prop.depth - objectDepth + 1;

            string path = GetTraversalPath(
                JSONSchemaProperties,
                (normalizedDepth, prop.propertyName, prop.propertyValue),
                2);

            if (path == string.Empty)
            {
                JSONLoader3.FormatLogger("ERROR", "LintingTools",
                    $"Something went wrong while doing PathTraversal for {prop.propertyName} the following should help identify what you did wrong.");

                if (GetTraversalPath(
                        JSONSchemaProperties,
                        (normalizedDepth, prop.propertyName.ToCamelCase(), prop.propertyValue),
                        2) != string.Empty)
                {
                    JSONLoader3.FormatLogger("ERROR", "LintingTools",
                        $"The casing of {prop.propertyName} within {Path.GetFileNameWithoutExtension(file)} is not correct, it should be in camelCase.");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        "Basically the Property needs to follow the correct convention which is first word in Lowercase, and the rest of the words first letters Capitalized.");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools", "likeThis");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        $"In the JSON the KVP would look like: \"likeThis\": false");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        $"Here's the full path to the file the issue takes root in: {file}");
                    return false;
                }

                if (!AllowExtraFieldsCheck(PropertySchema))
                {
                    JSONLoader3.FormatLogger("ERROR", "LintingTools",
                        $"The Property of {prop.propertyName} is not apart of the JSON Schema, please remove it from the JSON entitled {Path.GetFileNameWithoutExtension(file)}.");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        $"If you want to stop seeing these when you make your jsons, just cross check the JSON against the Schema saving it.");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        "Theres a nifty site called JSONEditor that you may find useful, theres a guide in the README.");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        "Additionally you can cross-reference against the Property List we provide in our API's documentation and WIKI's.");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        $"Here's the full path to the file the issue takes root in: {file}");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        $"You can also find the full Schema here: {schemaFile}");
                }
                else
                {
                    JSONLoader3.FormatLogger("Warning", "LintingTools",
                        $"The Property of {prop.propertyName} is not apart of the JSON Schema, please remove it from the JSON entitled {Path.GetFileNameWithoutExtension(file)}.");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        $"Here's the full path to the file the issue takes root in: {file}");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        $"You can also find the full Schema here: {schemaFile}");
                    JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                        "This is a warning because additionalProperties was enabled for this Object thus we're letting it slide.");
                }
            }

            if (requiredFields.Contains(prop.propertyName))
            {
                for (int i = 0; i < requiredFieldTicks.Count; i++)
                {
                    if (requiredFieldTicks[i].Item1 == prop.propertyName)
                    {
                        requiredFieldTicks[i] = (prop.propertyName, true);

                        JSONLoader3.FormatLogger(
                            "Debug",
                            "LintingTools",
                            $"Ticked \"{prop.propertyName}\" as checked for the Required Properties."
                        );

                        break;
                    }
                }
            }

            if (path != string.Empty)
            {
                List<string> propSchema = GetJSONSchemaProperty(path, PropertySchema);

                if (!ValidatePropertyAgainstSchema(
                        prop.propertyName,
                        prop.propertyValue,
                        propSchema,
                        file,
                        schemaFile))
                    return false;
            }
        }

        foreach ((string field, bool check) in requiredFieldTicks)
        {
            if (!check)
            {
                JSONLoader3.FormatLogger("ERROR", "LintingTools",
                    $"Missing Required Field: {field} within {Path.GetFileNameWithoutExtension(file)}.");
                JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                    "To Fix this, simply add the field mentioned in the error to the json with the appropriate value.");
                JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                    $"Here's the full path to the file the issue takes root in: {file}");
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// This is the JSON Validator's Array Handler, it handles ensuring the String Property is Valid.
    /// </summary>
    /// <param name="jsonPropertyName">The Property we are Validating.</param>
    /// <param name="jsonPropertyValue">The Property Value we are Validating.</param>
    /// <param name="PropertySchema">The List of String representing the Schema.</param>
    /// <param name="file">The Full Path to the JSON File.</param>
    /// <param name="schemaFile">The Full Path to the JSON Schema File.</param>
    /// <returns>A true if valid, a false if invalid.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static bool ValidateArray(string jsonPropertyName, string jsonPropertyValue, List<string> PropertySchema,
        string file, string schemaFile)
    {
        List<string> arrayItems = GetArrayItems(jsonPropertyValue, jsonPropertyName, file);

        if (arrayItems.Count == 0)
        {
            JSONLoader3.FormatLogger("Warning", "LintingTools",
                $"The Array is empty for {jsonPropertyName} either define it or remove it. The JSON this warming occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"To fix this simply as the error states remove the KVP for {jsonPropertyName}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Alternatively define a value for {jsonPropertyName} you can toggle on Summary in The Config to show the Properties Description for further Information about it..");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "This is a warning because we have never been strict about this, but if you want to stop seeing this warning do the above.");
            return true;
        }

        bool uniqueItems = false;
        string type = "";

        foreach (string line in PropertySchema)
        {
            if (line.Contains("\"uniqueItems\": "))
            {
                uniqueItems = bool.Parse(line.Trim().Replace("\"uniqueItems\": ", "").Trim('"', ','));
            }
        }

        if (uniqueItems && arrayItems.Distinct().Count() != arrayItems.Count)
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"There is duplicated Array Items in the Array {jsonPropertyName}, according to the Schema for this Item, all Array Items must be Unique for {jsonPropertyName}. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "To fix this error just ensure all Items of the Array are Unique, e.g. No Repeats within it.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Sadly as it broke here, I'm going to have to call a break, meaning we are going to stop proccessing this item and move on to the next one.");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                "Hopefully you'll have it fixed for next load up!!");
            JSONLoader3.FormatLogger("AdditionalInformation", "LintingTools",
                $"Here's the full path to the file the issue takes root in: {file}");
            return false;
        }

        List<string> itemSchema = DigThroughSchemaFindRelevant("items", PropertySchema);

        if (itemSchema.Count == 0)
        {
            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                $"Array {jsonPropertyName} has no item schema defined. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
            JSONLoader3.FormatLogger("Warning", "LintingTools",
                $"Could not find any Schema for validating the Items of the Array {jsonPropertyName}, notify a maintainer if you ever receive this, with a copy of your JSON file.");
            return false;
        }

        foreach (string line in itemSchema)
        {
            if (line.Contains("\"type\": \""))
            {
                type = line.Trim().Replace("\"type\": \"", "").Trim('"', ',');
                break;
            }
        }

        foreach (string item in arrayItems)
        {
            bool valid = false;

            // Objects are already complete JSON objects, so pass them directly
            // into ValidateObject instead of wrapping them in a fake KVP.
            if (type == "object")
            {
                valid = ValidateObject(
                    jsonPropertyName,
                    item,
                    itemSchema,
                    file,
                    schemaFile);
            }
            else
            {
                List<string> jsonDummy = new List<string>();

                jsonDummy.Add("{");
                jsonDummy.Add($"\"{jsonPropertyName}\": {item}");
                jsonDummy.Add("}");

                List<(int depth, string propertyName, string propertyValue)> dissectedItem =
                    DisectedJSON(jsonDummy);

                if (dissectedItem.Count == 0)
                {
                    JSONLoader3.FormatLogger("ERROR", "LintingTools",
                        $"Could not dissect array item for {jsonPropertyName}. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
                    return false;
                }
                
                if (itemSchema.Any(x => x.Trim().StartsWith("\"anyOf\": [")))
                {
                    foreach (string item2 in arrayItems)
                    {
                        List<string> jsonDummy2 = new List<string>
                        {
                            "{",
                            $"\"{jsonPropertyName}\": {item2}",
                            "}"
                        };

                        List<(int depth, string propertyName, string propertyValue)> dissectedItem2 =
                            DisectedJSON(jsonDummy2);

                        if (dissectedItem2.Count == 0)
                        {
                            JSONLoader3.FormatLogger("ERROR", "LintingTools",
                                $"Could not dissect array item for {jsonPropertyName}. The JSON this error occured in: {Path.GetFileNameWithoutExtension(file)}");
                            return false;
                        }

                        if (!ValidateAnyOf(
                                dissectedItem2[0].propertyName,
                                dissectedItem2[0].propertyValue,
                                itemSchema,
                                file,
                                schemaFile))
                        {
                            return false;
                        }
                    }

                    return true;
                }

                if (type == "string")
                {
                    valid = ValidateString(
                        dissectedItem[0].propertyName,
                        dissectedItem[0].propertyValue,
                        itemSchema,
                        file,
                        schemaFile);
                }
                else if (type == "integer")
                {
                    valid = ValidateInteger(
                        dissectedItem[0].propertyName,
                        dissectedItem[0].propertyValue,
                        itemSchema,
                        file,
                        schemaFile);
                }
                else if (type == "boolean")
                {
                    valid = ValidateBoolean(
                        dissectedItem[0].propertyName,
                        dissectedItem[0].propertyValue,
                        itemSchema,
                        file,
                        schemaFile);
                }
                else if (type == "array")
                {
                    valid = ValidateArray(
                        dissectedItem[0].propertyName,
                        dissectedItem[0].propertyValue,
                        itemSchema,
                        file,
                        schemaFile);
                }
                else
                {
                    JSONLoader3.FormatLogger("Warning", "LintingTools",
                        $"Could not find {type} support for validating {jsonPropertyName} within this Item Type for validating against the Schema, notify a maintainer if this is not intentional.");
                    valid = true;
                }
            }

            if (!valid)
            {
                return false;
            }
        }

        return true;
    }
}