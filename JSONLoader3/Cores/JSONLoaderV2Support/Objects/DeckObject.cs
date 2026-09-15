using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using InscryptionAPI.Ascension;
using InscryptionAPI.Card;
using JSONLoader3.Peripheral.ImageHandling;
using Sirenix.Utilities;
using UnityEngine;

namespace JSONLoader3.Cores.JSONLoaderV2Support.Objects;

/// <summary>
/// An Object resembling the StarterDeck List.
/// </summary>
public class DeckObject
{
    /// <summary>
    /// The List of Decks in the DeckObject.
    /// </summary>
    public List<DeckDataObject> decks {get; set;}
    /// <summary>
    /// The JSON File.
    /// </summary>
    internal string file { get; set; }
    /// <summary>
    /// The Plugin In Which the Starter Deck originates.
    /// </summary>
    internal string pluginName { get; set; }

    /// <summary>
    /// Creates the Deck Object.
    /// </summary>
    /// <param name="decks"><see cref="decks"/></param>
    /// <param name="file"><see cref="file"/></param>
    /// <param name="pluginName"><see cref="pluginName"/></param>
    public DeckObject(List<DeckDataObject> decks, string file, string pluginName)
    {
        this.decks = decks;
        this.file = file;
        this.pluginName = pluginName;
    }

    /// <summary>
    /// Converts the Deck Object to a List of <see cref="StarterDeckInfo"/>
    /// </summary>
    /// <returns>A List of <see cref="StarterDeckInfo"/></returns>
    public List<StarterDeckInfo> ConvertDeckObjectToStarterDeckInfo()
    {
        List<StarterDeckInfo> infos = new List<StarterDeckInfo>();
        foreach (DeckDataObject deck in decks)
        {
            StarterDeckInfo info = deck.ConvertDeckDataObjectToStarterDeckInfo();
            infos.Add(info);
        }

        return infos;
    }
}

/// <summary>
/// An Object resembling the Individual Starter Deck.
/// </summary>
public class DeckDataObject
{
    /// <summary>
    /// The Fields In Which Will be overwrote for the deck with the associated name.
    /// </summary>
    public List<string> fieldsToEdit { get; set; }
    /// <summary>
    /// The Starter Decks Name.
    /// </summary>
    public string name { get; set; }
    /// <summary>
    /// The Starter Decks Mod Prefix.
    /// </summary>
    public string modPrefix { get; set; }
    /// <summary>
    /// The Starter Decks title.
    /// </summary>
    public string title { get; set; }
    /// <summary>
    /// The Starter Decks Cards.
    /// </summary>
    public List<string> cards { get; set; }
    /// <summary>
    /// The Starter Decks IconTexture.
    /// </summary>
    public string iconTexture { get; set; }
    /// <summary>
    /// The Starter Decks UnlockLevel.
    /// </summary>
    public int unlockLevel { get; set; }
    /// <summary>
    /// The JSON File.
    /// </summary>
    internal string file { get; set; }
    /// <summary>
    /// The Plugin In Which the Starter Deck originates.
    /// </summary>
    internal string pluginName { get; set; }

    /// <summary>
    /// Creates A DeckDataObject.
    /// </summary>
    /// <param name="fieldsToEdit"><see cref="fieldsToEdit"/></param>
    /// <param name="name"><see cref="name"/></param>
    /// <param name="modPrefix"><see cref="modPrefix"/></param>
    /// <param name="title"><see cref="title"/></param>
    /// <param name="cards"><see cref="cards"/></param>
    /// <param name="iconTexture"><see cref="iconTexture"/></param>
    /// <param name="unlockLevel"><see cref="unlockLevel"/></param>
    /// <param name="file"><see cref="file"/></param>
    /// <param name="pluginName"><see cref="pluginName"/></param>
    public DeckDataObject(List<string> fieldsToEdit, string name, string modPrefix, string title = "",
        List<string> cards = null, string iconTexture = "", int unlockLevel = 0, string file = "", string pluginName = "")
    {
        this.fieldsToEdit = fieldsToEdit;
        this.name = modPrefix + "_" + name;
        this.modPrefix = modPrefix;
        this.title = title;
        this.cards = cards;
        this.iconTexture = iconTexture;
        this.unlockLevel = unlockLevel;
        this.file = file;
        this.pluginName = pluginName;
    }

    /// <summary>
    /// Converts the DeckDataObject into a <see cref="StarterDeckInfo"/>.
    /// </summary>
    /// <returns>A <see cref="StarterDeckInfo"/>.</returns>
    public StarterDeckInfo ConvertDeckDataObjectToStarterDeckInfo()
    {
        StarterDeckInfo info = null;
        if (!fieldsToEdit.IsNullOrEmpty())
        {
            StarterDeckInfo infoTemp = StarterDeckManager.AllDeckInfos.FirstOrDefault(x => x.name == name.Replace(modPrefix + "_", ""));
            if (infoTemp != null)
            {
                info = infoTemp;

                if (fieldsToEdit.Contains("title"))
                {
                    if (!string.IsNullOrEmpty(title))
                        info.title = title;
                }

                if (fieldsToEdit.Contains("cards"))
                {
                    if (cards != null)
                    {
                        info.cards = new List<CardInfo>();
                        foreach (string card in cards)
                        {
                            if (!string.IsNullOrEmpty(card))
                                info.cards.Add(CardManager.AllCardsCopy.Find(x => x.name == card));
                        }
                    }
                }

                if (fieldsToEdit.Contains("iconTexture"))
                {
                    if (!string.IsNullOrEmpty(iconTexture))
                    {
                        Sprite sprite = ScanImages.ParseImage(pluginName, iconTexture);

                        if (sprite != null)
                            info.SetIconTexture(sprite);
                    }
                }

                if (fieldsToEdit.Contains("unlockLevel"))
                {
                    info.SetUnlockLevel(unlockLevel);
                }
            }
        }
        else
        {
            StarterDeckManager.FullStarterDeck info2 = StarterDeckManager.New(modPrefix, name, title, ScanImages.ParseImage(pluginName, iconTexture).texture, cards.ToArray(), unlockLevel);

            info = info2.Info;
        }

        return info;
    }
}