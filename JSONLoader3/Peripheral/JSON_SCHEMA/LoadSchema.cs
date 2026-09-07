using System.Collections.Generic;
using System.IO;
using JSONLoader3.Peripheral.FILE_Loader;
using JSONLoader3.Subperipheral.JSONLoader_Configuration;

namespace JSONLoader3.Peripheral.JSON_SCHEMA;

/// <summary>
/// This class will handle the loading of a Schema File.
/// </summary>
public class LoadSchema
{
    /// <summary>
    /// This resembles the Schema Folder Location.
    /// </summary>
    public static string SchemaFolder = FindFiles.DLLPath + Path.DirectorySeparatorChar + DefineConfiguration.SchemaSavePath.Value;

    /// <summary>
    /// This function Loads the Given Item into a List of String resembling its Schema.
    /// </summary>
    /// <param name="LoaderName">The Loaders Identifier, this is used in the Schema and File Name.</param>
    /// <typeparam name="Class">The Class in which the Schema is being made for.</typeparam>
    /// <returns>A List of String resembling the entire JSON Schema Line by Line.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static List<string> FindAndLoadSchema<Class>(string LoaderName)
    {
        string schemaPath = GetFullSchemaPath<Class>(LoaderName);
        
        FileStream ReadingStream = File.OpenRead(schemaPath);
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
    /// This gets the full Schema Path for the Given Item.
    /// </summary>
    /// <param name="LoaderName">The Loaders Identifier, this is used in the Schema and File Name.</param>
    /// <typeparam name="Class">The Class in which the Schema is being made for.</typeparam>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static string GetFullSchemaPath<Class>(string LoaderName)
    {
        return SchemaFolder + Path.DirectorySeparatorChar + LoaderName + Path.DirectorySeparatorChar + $"{LoaderName}_{typeof(Class).Name}_Schema.json";
    }
}