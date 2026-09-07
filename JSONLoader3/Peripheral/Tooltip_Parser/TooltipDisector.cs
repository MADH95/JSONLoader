using System.Collections.Generic;
using System.Reflection;
using System.Text;
using JSONLoader3.Peripheral.XML_Parser;
using UnityEngine;

namespace JSONLoader3.Peripheral.Tooltip_Parser;

/// <summary>
/// This class handles Tooltip Dissection for our JSON Objects.
///
/// This supports the following terms:
/// * REQUIRED - Mark this field as a Required field in the Schema.
/// * EXCLUDED - Mark this field as something to not include in the Schema.
///
/// VARIABLES!!!
///
/// All Variables will work as follows: VariableName(Definition), kinda like a KeyPairValue.
///
/// The following is a list of all Variables:
/// * MinimumLength - Int - Used in String and String Array - Mandates a Minimum Length.
/// * Pattern - Raw Regex - Used in String and String Array - Mandates a Pattern the Value must follow.
/// * Items - Boolean - Used in String Array and Object Array - Marks the fact the Array has items as true.
/// * ItemType - Type - Used in String Array and Object Array - Used to define the type of Array in which the items belong. (e.g. string or object)
/// * Enums - A List of Predefined Values - Used in String and String Array - This provides a Pre-Defined list of items users may use for defining the value.
/// * UniqueItems - Boolean - Used in String Array and Object Array - This mandates uniqueness among the values.
/// * Default - Value - Used in String, Int, and Boolean - This provides a default for Schema Validators.
/// * Minimum - Int - Used in Int - This mandates a Minimum Number.
/// * Maximum - Int - Used in Int - This mandates a Maximum Number.
/// * AdditionalProperties - Boolean - Used in Object and Object Array - Determines whether additional properties are valid.
///
/// If you inevitably need more as of present you'll need to code handling into the Schema and Linter.
///
/// MULTI-VAR!!!!
///
/// To use more than one variable all you need to do is add a '|' between each Variable, this acts as a Delimiter.
///
/// An example of such would be: [Tooltip("REQUIRED | MinimumLength(1) | Pattern(^[a-zA-Z\\d_]+$)")]
///
/// Notice the '//' in the Regex? That's because C# needs it to be escaped in quotes, but don't worry we properly escape it for JSON in <see cref="ReadDocumentationFile.EscapeJSON"/>
/// </summary>
/// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
/// <example>Hey, please go to the Original Class to view this properly.</example>
public class TooltipDisector
{
    /// <summary>
    /// This function returns A list of all tooltip properties on the Field.
    /// </summary>
    /// <param name="field">The Field to fetch the Tooltips off of.</param>
    /// <returns>A List of String. It returns an Empty List if there are no Tooltips.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static List<string> GetTooltipProperties(FieldInfo field)
    {
        List<string> tooltips = new List<string>();
        TooltipAttribute tooltipAttr = field.GetCustomAttribute<TooltipAttribute>();

        if (tooltipAttr == null)
            return tooltips;

        foreach (string tooltip in tooltipAttr.tooltip.Split('|'))
        {
            StringBuilder current = new StringBuilder();
            int depth = 0;

            foreach (char c in tooltipAttr.tooltip)
            {
                if (c == '(') depth++;
                if (c == ')') depth--;

                if (c == '|' && depth == 0)
                {
                    tooltips.Add(current.ToString().Trim());
                    current.Clear();
                }
                else
                {
                    current.Append(c);
                }
            }

            if (current.Length > 0)
                tooltips.Add(current.ToString().Trim());
        }

        return tooltips;
    }
}