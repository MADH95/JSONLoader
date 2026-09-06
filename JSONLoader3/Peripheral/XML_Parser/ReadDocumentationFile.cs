using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace JSONLoader3.Peripheral.XML_Parser;

/// <summary>
/// This class handles the reading of the XML Documentation.
/// </summary>
public class ReadDocumentationFile
{
    /// <summary>
    /// The Path to the Executing Assembly.
    /// </summary>
    private static Assembly assembly = Assembly.GetExecutingAssembly();
    /// <summary>
    /// The Path to the JSONLoader DLL.
    /// </summary>
    internal static string DLLPath = Path.GetDirectoryName(assembly.Location);
    /// <summary>
    /// The Path to the JSONLoader DLL.
    /// </summary>
    internal static string DLLName = Path.GetFileNameWithoutExtension(assembly.Location);
    /// <summary>
    /// The Full Contents of the XML Documentation File.
    /// </summary>
    internal static List<string> DocFileXML = new List<string>();
    
    /// <summary>
    /// A function in which reads and stores the Documentation File.
    /// </summary>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static void FetchDocFile()
    {
        if (File.Exists(DLLPath + Path.DirectorySeparatorChar + $"{DLLName}.xml"))
        {
            StreamReader reader = new StreamReader(DLLPath + Path.DirectorySeparatorChar + $"{DLLName}.xml");

            while (reader.Peek() >= 0)
            {
                DocFileXML.Add(reader.ReadLine());
            }
        }
    }

    /// <summary>
    /// This function fetches the full ItemSummary for a passed in Item.
    /// </summary>
    /// <param name="tup">A tuple from <see cref="GetInfo"/> or the Generic Version.</param>
    /// <returns>The Full Item Summary in non-JSON Form.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static List<string> FetchItemSummary((string Namespace, string className, string ItemName) tup)
    {
        if (DocFileXML.Count == 0) return null;
        int index = DocFileXML.FindIndex(x => x.Contains("F:" + tup.Namespace + "." + tup.className + "." + tup.ItemName));
        List<string> newDoc = new List<string>();

        for (int i = index +1; i < DocFileXML.FindIndex(index, x => x.Trim() == "</summary>"); i++)
        {
            if (DocFileXML[i].Trim() == "</summary>" || DocFileXML[i].Trim() == "<summary>")
            {
                continue;
            }
            
            newDoc.Add(DocFileXML[i]);
        }

        return newDoc;
    }

    /// <summary>
    /// Gets the JSON Formatted variant of the XML Summary.
    /// </summary>
    /// <param name="tup">A tuple from <see cref="GetInfo"/> or the Generic Version.</param>
    /// <returns>A single-line, trimmed String of the XML Summary.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static string GetJSONSummary((string Namespace, string className, string ItemName) tup)
    {
        List<string> JSONSummaryBase = FetchItemSummary((tup.Namespace, tup.className, tup.ItemName));

        string JSONSummary = "";

        foreach (string line in JSONSummaryBase)
        {
            JSONSummary += line.Trim() + " ";
        }
        
        return JSONSummary.Trim();
    }

    /// <summary>
    /// This function gets all the info you need regarding the passed in Item. This is the Generic Version.
    /// </summary>
    /// <param name="ItemName">The String Name of the Item specifically: <see cref="FieldInfo"/> specifically the Name field.</param>
    /// <typeparam name="Class">The Class in which the Item comes from.</typeparam>
    /// <returns>A tuple persisting of the Namespace, ClassName and the ItemName you passed in.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static (string Namespace, string className, string ItemName) GetInfo<Class>(string ItemName)
    {
        string Namespace = typeof(Class).Namespace;
        string ClassName = typeof(Class).Name;
        return (Namespace, ClassName, ItemName);
    }
    
    /// <summary>
    /// This function gets all the info you need regarding the passed in Item. This is the version where you have the actual Type.
    /// </summary>
    /// <param name="ItemName">The String Name of the Item specifically: <see cref="FieldInfo"/> specifically the Name field.</param>
    /// <param name="Class">The Class in which the Item comes from.</param>
    /// <returns>A tuple persisting of the Namespace, ClassName and the ItemName you passed in.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static (string Namespace, string className, string ItemName) GetInfo(string ItemName, Type Class)
    {
        string Namespace = Class.Namespace;
        string ClassName = Class.Name;
        return (Namespace, ClassName, ItemName);
    }
    
    /// <summary>
    /// A small function that handles Escaping for JSON's.
    /// </summary>
    /// <param name="value">The value in which needs Escape Handling.</param>
    /// <returns>An escaped version of the value passed in, if something wasn't escaped properly have a dev update this function.</returns>
    public static string EscapeJSON(string value)
    {
        return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}