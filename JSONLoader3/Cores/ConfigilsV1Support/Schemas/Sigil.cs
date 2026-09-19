using System.Collections.Generic;
using UnityEngine;

namespace JSONLoader3.Cores.ConfigilsV1Support.Schemas;

/// <summary>
/// This class represents the Configil Data Type.
/// </summary>
public class Sigil
{
    /// <summary>
    /// The In-Code name of the Sigil.
    /// </summary>
    [Tooltip("REQUIRED | MinimumLength(1) | Pattern(^[a-zA-Z\\d_]+$)")]
    public string name;
    /// <summary>
    /// The In-Code GUID/ModPrefix of the Sigil.
    /// </summary>
    [Tooltip("REQUIRED | AlternativeNames(modPrefix) | MinimumLength(1) | Pattern(^[a-zA-Z\\d_]+$)")]
    public string GUID;
    /// <summary>
    /// The In-Game name of the Sigil.
    /// </summary>
    [Tooltip("Default( )")]
    public string displayName;
    /// <summary>
    /// The Description of the Sigil shown in the Rulebook.
    /// </summary>
    [Tooltip("Default( )")]
    public string description;
    /// <summary>
    /// The Meta Categories which Apply to this Sigil.
    /// </summary>
    [Tooltip("Items(true) | UniqueItems(true) | AnyOf([Title: Base Game Meta Category, Description: A Meta Category from the Base Game, Type: string, Enums: Part1Rulebook, Part1Modular, Part3BuildACard, Part3Rulebook, Part3Modular, BountyHunter, GrimoraRulebook, MagnificusRulebook, AscensionUnlocked];[Title: Modded Meta Category, Description: Format is {Mod GUID}.{Meta Category Name}, Type: string])")]
    public List<string> metaCategories;
    /// <summary>
    /// The Path to your sigils Icon, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '49x49' image. This is your sigils Icon within 3D acts.
    /// </summary>
    [Tooltip("Pattern(^(?:(?:\\.\\.\\/|[a-zA-Z\\d_\\s-]+\\/)*[a-zA-Z\\d_\\s-]+\\.png|data:image\\/png;base64,[A-Za-z0-9+/]+={0,2}|base64:[A-Za-z0-9+/]+={0,2})$)")]
    public string texture;
    /// <summary>
    /// The Path to your sigils Act 2 Icon, this is localized to your Plugins Folder. It's your job to keep it organized, do it as you would these 'JLDR' files. This must be a PNG File and must be a '49x49' image. This is your sigils Icon within 2D acts.
    /// </summary>
    [Tooltip("Pattern(^(?:(?:\\.\\.\\/|[a-zA-Z\\d_\\s-]+\\/)*[a-zA-Z\\d_\\s-]+\\.png|data:image\\/png;base64,[A-Za-z0-9+/]+={0,2}|base64:[A-Za-z0-9+/]+={0,2})$)")]
    public string pixelTexture;
    /// <summary>
    /// The Sigils Power Level.
    /// </summary>
    [Tooltip("Default(0)")]
    public int powerLevel;
    /// <summary>
    /// The Dialogue in which will appear when the Sigil is learned for the first time.
    /// </summary>
    [Tooltip("Default( )")]
    public string abilityLearnedDialouge;
    /// <summary>
    /// How high in priority this sigils activation is.
    /// </summary>
    [Tooltip("Default(0)")]
    public int priority;
    /// <summary>
    /// Whether this sigil should be usable by the Opponent.
    /// </summary>
    [Tooltip("Default(false)")]
    public bool opponentUsable;
    /// <summary>
    /// Whether this sigil should be able to be stacked.
    /// </summary>
    [Tooltip("Default(false)")]
    public bool canStack;
    /// <summary>
    /// Whether this sigil is actually a Special Ability.
    /// </summary>
    [Tooltip("Default(false)")]
    public bool isSpecialAbility;
    /// <summary>
    /// Whether this Special Ability is actually a Power Stat as well.
    /// </summary>
    [Tooltip("Default(false)")]
    public bool isPowerStat;
    /// <summary>
    /// Whether this Power Stat applies to Attack.
    /// </summary>
    [Tooltip("Default(false)")]
    public bool appliesToAttack;
    /// <summary>
    /// Whether this Power Stat applies to Health.
    /// </summary>
    [Tooltip("Default(false)")]
    public bool appliesToHealth;
    /// <summary>
    /// The Underarching Schema for a Configil.
    /// </summary>
    [Tooltip("EXTENDS")]
    public UnderarchingConfigilSchema configilSchema;
}