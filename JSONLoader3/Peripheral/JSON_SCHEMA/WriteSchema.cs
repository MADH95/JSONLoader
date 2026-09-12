using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using JSONLoader3.Peripheral.FILE_Loader;
using JSONLoader3.Peripheral.Tooltip_Parser;
using JSONLoader3.Peripheral.XML_Parser;
using JSONLoader3.Subperipheral.JSONLoader_Configuration;
using Sirenix.Utilities;

namespace JSONLoader3.Peripheral.JSON_SCHEMA;

/// <summary>
/// This class handles the Writing of JSON Schemas.
/// </summary>
public class WriteSchema
{
    /// <summary>
    /// This resembles the Schema Folder Location.
    /// </summary>
    public static string SchemaFolder = FindFiles.DLLPath + Path.DirectorySeparatorChar + DefineConfiguration.SchemaSavePath.Value;

    /// <summary>
    /// A simple function to handle the creation of the Schema Directory
    /// </summary>
    /// <param name="LoaderName">The Loaders Identifier, this is used in the Schema and File Name.</param>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static void CreateSchemaDirectors(string LoaderName)
    {
        if (!Directory.Exists(SchemaFolder))
        {
            Directory.CreateDirectory(SchemaFolder);
        }
        if (!Directory.Exists(SchemaFolder + Path.DirectorySeparatorChar + LoaderName))
        {
            Directory.CreateDirectory(SchemaFolder + Path.DirectorySeparatorChar + LoaderName);
        }
    }

    /// <summary>
    /// A function to create and open the Schema.
    /// </summary>
    /// <param name="LoaderName">The Loaders Identifier, this is used in the Schema and File Name.</param>
    /// <typeparam name="Class">The Class in which the Schema is being made for.</typeparam>
    /// <returns>An opened <see cref="StreamWriter"/>.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static StreamWriter CreateAndOpenSchema<Class>(string LoaderName)
    {
        // Create the Schema File
        string SchemaPath = SchemaFolder + Path.DirectorySeparatorChar + LoaderName + Path.DirectorySeparatorChar + $"{LoaderName}_{typeof(Class).Name}_Schema.json";
        FileStream WritingStream = File.Create(SchemaPath);
        return new StreamWriter(WritingStream);
    }

    /// <summary>
    /// This function closes the passed in StreamWriter.
    /// </summary>
    /// <param name="Writer">The StreamWriter associated with <see cref="CreateAndOpenSchema"/></param>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static void CloseWriterAndFile(StreamWriter Writer)
    {
        Writer.Dispose();
    }

    /// <summary>
    /// Extracts all the Tooltips and puts them in a tuple List.
    /// </summary>
    /// <param name="fields">All of the fieldInfo's relevant for the Tooltip List.</param>
    /// <returns>A List of (a Tuple of (a Field Info, and a List of String resembling the tooltips)).</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static List<(FieldInfo field, List<string> tooltips)> GetToolTips(List<FieldInfo> fields)
    {
        List<(FieldInfo field, List<string> tooltips)> fieldTooltipList = new List<(FieldInfo field, List<string> tooltips)>();
        foreach (FieldInfo field in fields)
        {
            fieldTooltipList.Add((field, TooltipDisector.GetTooltipProperties(field)));
        }

        return fieldTooltipList;
    }

    /// <summary>
    /// Gets all the Required fields from a passed in Field to Tooltip List.
    /// </summary>
    /// <param name="fieldTooltipList">The full Field to Tooltip List from <see cref="GetToolTips"/>.</param>
    /// <returns>A list of the required Fields.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static List<string> GetRequired(List<(FieldInfo field, List<string> tooltips)> fieldTooltipList)
    {
        return fieldTooltipList.Where(x => x.tooltips.Contains("REQUIRED", StringComparer.Ordinal) && !x.tooltips.Contains("EXCLUDED", StringComparer.Ordinal)).Select(x => $"\"{x.field.Name}\"").ToList();
    }

    /// <summary>
    /// Gets all the Writable fields from a passed in Field to Tooltip List.
    /// </summary>
    /// <param name="fieldTooltipList">The full Field to Tooltip List from <see cref="GetToolTips"/>.</param>
    /// <returns>All of the Writable fields in the format in which it came in.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static List<(FieldInfo field, List<string> tooltips)> GetWritable(List<(FieldInfo field, List<string> tooltips)> fieldTooltipList)
    {
        return fieldTooltipList.Where(x => !x.tooltips.Contains("EXCLUDED", StringComparer.Ordinal)).ToList();
    }

    /// <summary>
    /// This function handles the Schema Writing logic for AnyOf[] fields within JSON Schema.
    /// </summary>
    /// <param name="anyOf">The String Value associated with the AnyOf Tooltip.</param>
    /// <param name="Indentation">The amount of excess indentation needed.</param>
    /// <returns>A Multi-Line String representing the Handled AnyOf Array.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static string HandleAnyOf(string anyOf, int Indentation)
    {
        string toWrite = "";
        List<string> anyOfProps = anyOf.Replace('(', ' ').Replace(')', ' ').Split(';').ToList();
        foreach (string anyOfProp in anyOfProps)
        {
            toWrite += $$"""
                         
                                         {{new string(' ', Indentation * 4)}}{
                         """;
            List<string> anyOfPropProps = anyOfProp.Replace('[', ' ').Replace(']', ' ').Split(',').ToList();
            
            
            List<(string field, string value)> anyOfPropPropsFields = new List<(string field, string value)>();
            
            foreach (string item in anyOfPropProps)
            {
                int separatorIndex = item.IndexOf(':');

                if (separatorIndex >= 0)
                {
                    anyOfPropPropsFields.Add((
                        item.Substring(0, separatorIndex).Trim(),
                        item.Substring(separatorIndex + 1).Trim()
                    ));
                }
            }
            
            // Handle Title
            (string field, string value) title = anyOfPropPropsFields.FirstOrDefault(x => x.field == "Title");
            toWrite += $$"""
                         
                                             {{new string(' ', Indentation * 4)}}"title": "{{title.value}}",
                         """;
            
            // Handle Description
            (string field, string value) description = anyOfPropPropsFields.FirstOrDefault(x => x.field == "Description");
            toWrite += $$"""
                         
                                             {{new string(' ', Indentation * 4)}}"description": "{{description.value}}",
                         """;
            
            (string field, string value) type = anyOfPropPropsFields.FirstOrDefault(x => x.field == "Type");
            toWrite += $$"""
                         
                                             {{new string(' ', Indentation * 4)}}"type": "{{type.value}}",
                         """;

            if (type.value == "string")
            {
                // Optional Enums Values
                (string field, string value) enums = anyOfPropPropsFields.FirstOrDefault(x => x.field == "Enums");
                if (enums != (null, null))
                {
                    toWrite += $$"""

                                                     {{new string(' ', Indentation * 4)}}"enum": [
                                                         {{new string(' ', Indentation * 4)}}{{string.Join($", {Environment.NewLine}                        {new string(' ', Indentation * 4)}", enums.value.Split('>').Select(x => $"\"{x.Trim()}\""))}}
                                                     {{new string(' ', Indentation * 4)}}],
                                 """;
                }
            }

            toWrite = toWrite.TrimEnd();
            toWrite = toWrite.TrimEnd(',');
            toWrite += $$"""
                         
                                         {{new string(' ', Indentation * 4)}}},
                         """;
        }
        
        toWrite = toWrite.TrimEnd();
        toWrite = toWrite.TrimEnd(',');

        return toWrite;
    }

    /// <summary>
    /// This function handles String related JSON Schema Components.
    /// </summary>
    /// <param name="writableFields">A list of all the writable fields, we namely use it here for comma assurance.</param>
    /// <param name="field">The specific field of the property in which needs to be Handled.</param>
    /// <param name="tooltips">The list of tooltips associated with that field.</param>
    /// <param name="Indentation">The amount of excess indentation needed.</param>
    /// <param name="Class">The class in which this property belongs.</param>
    /// <returns>A Multi-Line String representing the Handled String Property.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static string HandleString(List<(FieldInfo field, List<string> tooltips)> writableFields, FieldInfo field, List<string> tooltips, int Indentation, Type Class)
    {
        string toWrite = "";
        toWrite += $$"""

                             {{new string(' ', Indentation * 4)}}"{{field.Name}}": {
                                 {{new string(' ', Indentation * 4)}}"type": "string",
                                 {{new string(' ', Indentation * 4)}}"description": "{{ReadDocumentationFile.EscapeJSON(ReadDocumentationFile.GetJSONSummary(ReadDocumentationFile.GetInfo(field.Name, Class)))}}",
                     """;

        // Optional Minimum Length
        string minLength = tooltips.FirstOrDefault(x => x.StartsWith("MinimumLength(", StringComparison.Ordinal));
        if (minLength != null)
            toWrite += $$"""

                                     {{new string(' ', Indentation * 4)}}"minLength": {{minLength.Split('(').Last().Trim('(', ')')}},
                         """;

        // Optional Regex Pattern
        string pattern = tooltips.FirstOrDefault(x => x.StartsWith("Pattern(", StringComparison.Ordinal));
        if (pattern != null)
            toWrite += $$"""

                                     {{new string(' ', Indentation * 4)}}"pattern": "{{ReadDocumentationFile.EscapeJSON(pattern.Substring(pattern.IndexOf('(') + 1, pattern.LastIndexOf(')') - pattern.IndexOf('(') - 1))}}",
                         """;

        // Optional Default Value
        string @default = tooltips.FirstOrDefault(x => x.StartsWith("Default(", StringComparison.Ordinal));
        if (@default != null)
            toWrite += $$"""

                                     {{new string(' ', Indentation * 4)}}"default": "{{@default.Split('(').Last().Trim('(', ')')}}",
                         """;

        // Optional Enums Values
        string enums = tooltips.FirstOrDefault(x => x.StartsWith("Enums(", StringComparison.Ordinal));
        if (enums != null)
        {
            toWrite += $$"""

                                     {{new string(' ', Indentation * 4)}}"enum": [
                                         {{new string(' ', Indentation * 4)}}{{string.Join($", {Environment.NewLine}                {new string(' ', Indentation * 4)}", enums.Split('(').Last().Trim('(', ')').Split(',').Select(x => $"\"{x.Trim()}\""))}}
                                     {{new string(' ', Indentation * 4)}}],
                         """;
        }
        
        // Optional AnyOf
        string anyOf = tooltips.FirstOrDefault(x => x.StartsWith("AnyOf(", StringComparison.Ordinal));
        if (anyOf != null)
        {
            toWrite += $$"""

                                     {{new string(' ', Indentation * 4)}}"anyOf": [
                         """;

            toWrite += HandleAnyOf(anyOf, Indentation);

            toWrite += $$"""

                                     {{new string(' ', Indentation * 4)}}],
                         """;
        }

        toWrite = toWrite.TrimEnd();
        toWrite = toWrite.TrimEnd(',');
        toWrite += $$"""

                             {{new string(' ', Indentation * 4)}}}
                     """;

        if (writableFields.IndexOf((field, tooltips)) != writableFields.Count - 1)
        {
            toWrite += ",";
        }

        return toWrite;
    }

    /// <summary>
    /// This function handles Int related JSON Schema Components.
    /// </summary>
    /// <param name="writableFields">A list of all the writable fields, we namely use it here for comma assurance.</param>
    /// <param name="field">The specific field of the property in which needs to be Handled.</param>
    /// <param name="tooltips">The list of tooltips associated with that field.</param>
    /// <param name="Indentation">The amount of excess indentation needed.</param>
    /// <param name="Class">The class in which this property belongs.</param>
    /// <returns>A Multi-Line String representing the Handled Int Property.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static string HandleInt(List<(FieldInfo field, List<string> tooltips)> writableFields, FieldInfo field, List<string> tooltips, int Indentation, Type Class)
    {
        string toWrite = "";
        toWrite += $$"""

                             {{new string(' ', Indentation * 4)}}"{{field.Name}}": {
                                 {{new string(' ', Indentation * 4)}}"type": "integer",
                                 {{new string(' ', Indentation * 4)}}"description": "{{ReadDocumentationFile.EscapeJSON(ReadDocumentationFile.GetJSONSummary(ReadDocumentationFile.GetInfo(field.Name, Class)))}}",
                     """;

        // Optional Default Value
        string @default = tooltips.FirstOrDefault(x => x.StartsWith("Default(", StringComparison.Ordinal));
        if (@default != null)
            toWrite += $$"""

                                     {{new string(' ', Indentation * 4)}}"default": {{@default.Split('(').Last().Trim('(', ')')}},
                         """;

        // Option Minimum Value
        string minimum = tooltips.FirstOrDefault(x => x.StartsWith("Minimum(", StringComparison.Ordinal));
        if (minimum != null)
            toWrite += $$"""

                                     {{new string(' ', Indentation * 4)}}"minimum": {{minimum.Split('(').Last().Trim('(', ')')}},
                         """;

        // Option Maximum Value
        string maximum = tooltips.FirstOrDefault(x => x.StartsWith("Maximum(", StringComparison.Ordinal));
        if (maximum != null)
            toWrite += $$"""

                                     {{new string(' ', Indentation * 4)}}"maximum": {{maximum.Split('(').Last().Trim('(', ')')}},
                         """;

        toWrite = toWrite.TrimEnd();
        toWrite = toWrite.TrimEnd(',');
        toWrite += $$"""

                             {{new string(' ', Indentation * 4)}}}
                     """;

        if (writableFields.IndexOf((field, tooltips)) != writableFields.Count - 1)
        {
            toWrite += ",";
        }

        return toWrite;
    }

    /// <summary>
    /// This function handles Boolean related JSON Schema Components.
    /// </summary>
    /// <param name="writableFields">A list of all the writable fields, we namely use it here for comma assurance.</param>
    /// <param name="field">The specific field of the property in which needs to be Handled.</param>
    /// <param name="tooltips">The list of tooltips associated with that field.</param>
    /// <param name="Indentation">The amount of excess indentation needed.</param>
    /// <param name="Class">The class in which this property belongs.</param>
    /// <returns>A Multi-Line String representing the Handled Boolean Property.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static string HandleBoolean(List<(FieldInfo field, List<string> tooltips)> writableFields, FieldInfo field, List<string> tooltips, int Indentation, Type Class)
    {
        string toWrite = "";
        toWrite += $$"""

                             {{new string(' ', Indentation*4)}}"{{field.Name}}": {
                                 {{new string(' ', Indentation*4)}}"type": "boolean",
                                 {{new string(' ', Indentation*4)}}"description": "{{ReadDocumentationFile.EscapeJSON(ReadDocumentationFile.GetJSONSummary(ReadDocumentationFile.GetInfo(field.Name, Class)))}}",
                     """;
                
        // Optional Default Value
        string @default = tooltips.FirstOrDefault(x => x.StartsWith("Default(", StringComparison.Ordinal));
        if (@default != null)
            toWrite += $$"""

                                     {{new string(' ', Indentation*4)}}"default": {{@default.Split('(').Last().Trim('(', ')').ToLower()}},
                         """;
                
        toWrite = toWrite.TrimEnd();
        toWrite = toWrite.TrimEnd(',');
        toWrite += $$"""

                             {{new string(' ', Indentation*4)}}}
                     """;

        if (writableFields.IndexOf((field, tooltips)) != writableFields.Count - 1)
        {
            toWrite += ",";
        }

        return toWrite;
    }

    /// <summary>
    /// This function handles String Array related JSON Schema Components.
    /// </summary>
    /// <param name="writableFields">A list of all the writable fields, we namely use it here for comma assurance.</param>
    /// <param name="field">The specific field of the property in which needs to be Handled.</param>
    /// <param name="tooltips">The list of tooltips associated with that field.</param>
    /// <param name="Indentation">The amount of excess indentation needed.</param>
    /// <param name="Class">The class in which this property belongs.</param>
    /// <returns>A Multi-Line String representing the Handled String Array Property.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static string HandleStringArray(List<(FieldInfo field, List<string> tooltips)> writableFields, FieldInfo field, List<string> tooltips, int Indentation, Type Class)
    {
        string toWrite = "";
        toWrite += $$"""

                             {{new string(' ', Indentation * 4)}}"{{field.Name}}": {
                                 {{new string(' ', Indentation * 4)}}"type": "array",
                                 {{new string(' ', Indentation * 4)}}"description": "{{ReadDocumentationFile.EscapeJSON(ReadDocumentationFile.GetJSONSummary(ReadDocumentationFile.GetInfo(field.Name, Class)))}}",
                     """;

        // Optional Items Value
        string items = tooltips.FirstOrDefault(x => x.StartsWith("Items(", StringComparison.Ordinal));
        if (items != null && items.Split('(').Last().Trim('(', ')').ToLower() == "true")
        {
            toWrite += $$"""

                                     {{new string(' ', Indentation * 4)}}"items": {
                         """;

            // Optional ItemType Value
            string itemType = tooltips.FirstOrDefault(x => x.StartsWith("ItemType(", StringComparison.Ordinal));
            if (itemType != null)
                toWrite += $$"""

                                             {{new string(' ', Indentation * 4)}}"type": "{{itemType.Split('(').Last().Trim('(', ')').ToLower()}}",
                             """;

            // Optional Regex Pattern
            string pattern = tooltips.FirstOrDefault(x => x.StartsWith("Pattern(", StringComparison.Ordinal));
            if (pattern != null)
                toWrite += $$"""

                                             {{new string(' ', Indentation * 4)}}"pattern": "{{ReadDocumentationFile.EscapeJSON(pattern.Substring(pattern.IndexOf('(') + 1, pattern.LastIndexOf(')') - pattern.IndexOf('(') - 1))}}",
                             """;

            // Optional Enums Values
            string enums = tooltips.FirstOrDefault(x => x.StartsWith("Enums(", StringComparison.Ordinal));
            if (enums != null)
            {
                toWrite += $$"""

                                             {{new string(' ', Indentation * 4)}}"enum": [
                                                 {{new string(' ', Indentation * 4)}}{{string.Join($", {Environment.NewLine}                    {new string(' ', Indentation * 4)}", enums.Split('(').Last().Trim('(', ')').Split(',').Select(x => $"\"{x.Trim()}\""))}}
                                             {{new string(' ', Indentation * 4)}}],
                             """;
            }
            
            // Optional AnyOf
            string anyOf = tooltips.FirstOrDefault(x => x.StartsWith("AnyOf(", StringComparison.Ordinal));
            if (anyOf != null)
            {
                toWrite += $$"""

                                             {{new string(' ', Indentation * 4)}}"anyOf": [
                             """;

                toWrite += HandleAnyOf(anyOf, Indentation + 1);

                toWrite += $$"""

                                             {{new string(' ', Indentation * 4)}}],
                             """;
            }

            toWrite = toWrite.TrimEnd();
            toWrite = toWrite.TrimEnd(',');
            toWrite += $$"""

                                     {{new string(' ', Indentation * 4)}}},
                         """;
        }

        // Optional UniqueItems Value
        string UniqueItems = tooltips.FirstOrDefault(x => x.StartsWith("UniqueItems(", StringComparison.Ordinal));
        if (UniqueItems != null)
            toWrite += $$"""

                                     {{new string(' ', Indentation * 4)}}"uniqueItems": {{UniqueItems.Split('(').Last().Trim('(', ')').ToLower()}},
                         """;

        toWrite = toWrite.TrimEnd();
        toWrite = toWrite.TrimEnd(',');
        toWrite += $$"""

                             {{new string(' ', Indentation * 4)}}}
                     """;

        if (writableFields.IndexOf((field, tooltips)) != writableFields.Count - 1)
        {
            toWrite += ",";
        }

        return toWrite;
    }

    /// <summary>
    /// This function handles Object Array related JSON Schema Components.
    /// </summary>
    /// <param name="writableFields">A list of all the writable fields, we namely use it here for comma assurance.</param>
    /// <param name="field">The specific field of the property in which needs to be Handled.</param>
    /// <param name="tooltips">The list of tooltips associated with that field.</param>
    /// <param name="Indentation">The amount of excess indentation needed.</param>
    /// <param name="Class">The class in which this property belongs.</param>
    /// <returns>A Multi-Line String representing the Handled Object Array Property.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static string HandleObjectArray(List<(FieldInfo field, List<string> tooltips)> writableFields, FieldInfo field, List<string> tooltips, int Indentation, Type Class)
    {
        Type elementType = field.FieldType.GetGenericArguments()[0];
                string toWrite = "";
                toWrite += $$"""
                             
                                     {{new string(' ', Indentation*4)}}"{{field.Name}}": {
                                         {{new string(' ', Indentation*4)}}"type": "array",
                                         {{new string(' ', Indentation*4)}}"description": "{{ReadDocumentationFile.EscapeJSON(ReadDocumentationFile.GetJSONSummary(ReadDocumentationFile.GetInfo(field.Name, Class)))}}",
                             """;
                
                // Optional Items Value
                string items = tooltips.FirstOrDefault(x => x.StartsWith("Items(", StringComparison.Ordinal));
                if (items != null && items.Split('(').Last().Trim('(', ')').ToLower() == "true")
                {
                    toWrite += $$"""
                                 
                                             {{new string(' ', Indentation*4)}}"items": {
                                 """;

                    // Optional ItemType Value
                    string itemType = tooltips.FirstOrDefault(x => x.StartsWith("ItemType(", StringComparison.Ordinal));
                    if (itemType != null)
                        toWrite += $$"""
                                     
                                                     {{new string(' ', Indentation*4)}}"type": "{{itemType.Split('(').Last().Trim('(', ')').ToLower()}}",
                                     """;
                    
                    // Optional AdditionalProperties Value
                    string additionalProperties = tooltips.FirstOrDefault(x => x.StartsWith("AdditionalProperties(", StringComparison.Ordinal));
                    if (additionalProperties != null)
                    {
                        string value = additionalProperties.Split('(').Last().Trim('(', ')').ToLower();
                        if (value == "true" || value == "false")
                            toWrite += $$"""

                                                     {{new string(' ', Indentation*4)}}"additionalProperties": {{value}},
                                         """;
                        else
                            toWrite += $$"""

                                                     {{new string(' ', Indentation*4)}}"additionalProperties": {
                                                         {{new string(' ', Indentation*4)}}"type": "{{value}}"
                                                     {{new string(' ', Indentation*4)}}},
                                         """;
                    }

                    if (elementType != typeof(object))
                    {
                        string returnedJSON = RecursiveWrite(elementType, Indentation+3);

                        toWrite += returnedJSON;
                    } 
                    
                    toWrite = toWrite.TrimEnd();
                    toWrite = toWrite.TrimEnd(',');
                    toWrite += $$"""
                                 
                                             {{new string(' ', Indentation*4)}}},
                                 """;
                }
                
                // Optional UniqueItems Value
                string UniqueItems = tooltips.FirstOrDefault(x => x.StartsWith("UniqueItems(", StringComparison.Ordinal));
                if (UniqueItems != null)
                    toWrite += $$"""
                                 
                                             {{new string(' ', Indentation*4)}}"uniqueItems": {{UniqueItems.Split('(').Last().Trim('(', ')').ToLower()}},
                                 """;

                toWrite = toWrite.TrimEnd();
                toWrite = toWrite.TrimEnd(',');
                toWrite += $$"""
                             
                                     {{new string(' ', Indentation*4)}}}
                             """;
                
                if (writableFields.IndexOf((field, tooltips)) != writableFields.Count - 1)
                {
                    toWrite += ",";
                }

                return toWrite;
    }

    /// <summary>
    /// This function handles Object related JSON Schema Components.
    /// </summary>
    /// <param name="writableFields">A list of all the writable fields, we namely use it here for comma assurance.</param>
    /// <param name="field">The specific field of the property in which needs to be Handled.</param>
    /// <param name="tooltips">The list of tooltips associated with that field.</param>
    /// <param name="Indentation">The amount of excess indentation needed.</param>
    /// <param name="Class">The class in which this property belongs.</param>
    /// <returns>A Multi-Line String representing the Handled Object Property.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static string HandleObject(List<(FieldInfo field, List<string> tooltips)> writableFields, FieldInfo field, List<string> tooltips, int Indentation, Type Class)
    {
        string toWrite = "";
        toWrite += $$"""

                             {{new string(' ', Indentation*4)}}"{{field.Name}}": {
                                 {{new string(' ', Indentation*4)}}"type": "object",
                                 {{new string(' ', Indentation*4)}}"description": "{{ReadDocumentationFile.EscapeJSON(ReadDocumentationFile.GetJSONSummary(ReadDocumentationFile.GetInfo(field.Name, Class)))}}",
                     """;
            // Optional AdditionalProperties Value
            string additionalProperties = tooltips.FirstOrDefault(x => x.StartsWith("AdditionalProperties(", StringComparison.Ordinal));
            if (additionalProperties != null)
            {
                string value = additionalProperties.Split('(').Last().Trim('(', ')').ToLower();
                if (value == "true" || value == "false")
                    toWrite += $$"""

                                             {{new string(' ', Indentation*4)}}"additionalProperties": {{value}},
                                 """;
                else
                    toWrite += $$"""

                                             {{new string(' ', Indentation*4)}}"additionalProperties": {
                                                 {{new string(' ', Indentation*4)}}"type": "{{value}}"
                                             {{new string(' ', Indentation*4)}}},
                                 """;
            }

            if (field.FieldType != typeof(object))
            {
                string returnedJSON = RecursiveWrite(field.FieldType, Indentation + 2);

                toWrite += returnedJSON;
            } 

        toWrite = toWrite.TrimEnd();
        toWrite = toWrite.TrimEnd(',');
        toWrite += $$"""

                             {{new string(' ', Indentation*4)}}}
                     """;

        if (writableFields.IndexOf((field, tooltips)) != writableFields.Count - 1)
        {
            toWrite += ",";
        }

        return toWrite;
    }

    /// <summary>
    /// The Non-Recursive JSON Schema Writer
    /// </summary>
    /// <param name="LoaderName">The Loaders Identifier, this is used in the Schema and File Name.</param>
    /// <typeparam name="Class">The Class in which the Schema is being made for.</typeparam>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static void WriteJSONSchema<Class>(string LoaderName)
    {
        CreateSchemaDirectors(LoaderName);
        StreamWriter WritingWriter = CreateAndOpenSchema<Class>(LoaderName);
        
        // Initiate the Schema
        WritingWriter.Write($$"""
                            {
                                "$schema": "http://json-schema.org/draft-04/schema#",
                                "$id": "https://github.com/MADH95/JSONLoader",
                                "title": "New {{LoaderName}} {{typeof(Class).Name}}",
                                "description": "Schema to verify the input data for a new {{LoaderName}} {{typeof(Class).Name}}.",
                                "type": "object",
                                "additionalProperties": false,
                            """);
        
        List<(FieldInfo field, List<string> tooltips)> fieldTooltipList = GetToolTips(typeof(Class).GetFields().ToList());
        
        // Handle Required Properties.
        List<string> required = GetRequired(fieldTooltipList);
        if (!required.IsNullOrEmpty())
            WritingWriter.Write($$"""
                                  
                                      "required": [
                                          {{string.Join($",{Environment.NewLine}        ", required)}}
                                      ],
                                  """);
        
        // Open the Objects Properties
        WritingWriter.Write($$"""
                              
                                  "properties": {
                              """);
        
        // Get All Non-Excluded Fields and Write them to the Schema
        List<(FieldInfo field, List<string> tooltips)> writableFields = GetWritable(fieldTooltipList);
        foreach ((FieldInfo field, List<string> tooltips) in writableFields)
        {
            // Handles String JSON Schema Types
            if (field.FieldType == typeof(string))
            {
                WritingWriter.Write(HandleString(writableFields, field, tooltips, 0, typeof(Class)));
            }

            // Handles Int JSON Schema Types
            else if (field.FieldType == typeof(int))
            {
                WritingWriter.Write(HandleInt(writableFields, field, tooltips, 0, typeof(Class)));
            }
            
            // Handles Boolean JSON Schema Types
            else if (field.FieldType == typeof(bool))
            {
                WritingWriter.Write(HandleBoolean(writableFields, field, tooltips, 0, typeof(Class)));
            }
            
            // Handles Array JSON Schema Types
            else if (field.FieldType == typeof(List<string>))
            {
                WritingWriter.Write(HandleStringArray(writableFields, field, tooltips, 0, typeof(Class)));
            }

            // Handles Object Array JSON Schema Types
            else if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
            {
                
                WritingWriter.Write(HandleObjectArray(writableFields, field, tooltips, 0, typeof(Class)));
            }
            
            // Handles Object JSON Schema Types
            else if (field.FieldType.IsClass)
            {
                WritingWriter.Write(HandleObject(writableFields, field, tooltips, 0, typeof(Class)));
            }
        }

        WritingWriter.Write($$"""
                              
                                  }
                              }
                              """);

        CloseWriterAndFile(WritingWriter);
    }

    /// <summary>
    /// The Recursive JSON Schema Writer
    /// </summary>
    /// <param name="Class">The Class in which the Object Schema Extension is being made for.</param>
    /// <param name="Indentation">The amount of excess indentation needed.</param>
    /// <returns>A Multi-Line String resembling the Object.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static string RecursiveWrite(Type Class, int Indentation)
    {
        string toSendOut = "";
        
         // Get All the Fields of the Class, and get their associated Tooltips.
        List<FieldInfo> fields = Class.GetFields().ToList();
        
        List<(FieldInfo field, List<string> tooltips)> fieldTooltipList = GetToolTips(fields);
        
        // Handle Required Properties.
        List<string> required = GetRequired(fieldTooltipList);
        if (!required.IsNullOrEmpty())
            toSendOut += $$"""
                           
                               {{new string(' ', Indentation*4)}}"required": [
                                   {{new string(' ', Indentation*4)}}{{string.Join($",{Environment.NewLine}        {new string(' ', Indentation*4)}", required)}}
                               {{new string(' ', Indentation*4)}}],
                           """;
        
        // Open the Objects Properties
        toSendOut += $$"""
                       
                           {{new string(' ', Indentation*4)}}"properties": {
                       """;
        
        // Get All Non-Excluded Fields and Write them to the Schema
        List<(FieldInfo field, List<string> tooltips)> writableFields = GetWritable(fieldTooltipList);
        foreach ((FieldInfo field, List<string> tooltips) in writableFields)
        {
            
            // Handles String JSON Schema Types
            if (field.FieldType == typeof(string))
            {
                toSendOut += HandleString(writableFields, field, tooltips, Indentation, Class);
            }

            // Handles Int JSON Schema Types
            else if (field.FieldType == typeof(int))
            {
                toSendOut += HandleInt(writableFields, field, tooltips, Indentation, Class);
            }
            
            // Handles Boolean JSON Schema Types
            else if (field.FieldType == typeof(bool))
            {
                toSendOut += HandleBoolean(writableFields, field, tooltips, Indentation, Class);
            }
            
            // Handles Array JSON Schema Types
            else if (field.FieldType == typeof(List<string>))
            {
                toSendOut += HandleStringArray(writableFields, field, tooltips, Indentation, Class);
            }

            // Handles Object Array JSON Schema Types
            else if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(List<>))
            {
                toSendOut += HandleObjectArray(writableFields, field, tooltips, Indentation, Class);
            }
            
            // Handles Object JSON Schema Types
            else if (field.FieldType.IsClass)
            {
                toSendOut += HandleObject(writableFields, field, tooltips, Indentation, Class);
            }
        }

        toSendOut += $$"""
                       
                           {{new string(' ', Indentation*4)}}},
                       """;
        
        return toSendOut;
    }
}