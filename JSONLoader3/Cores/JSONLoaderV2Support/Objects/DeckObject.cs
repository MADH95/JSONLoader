// using System.Collections.Generic;
// using System.Linq;
// using DiskCardGame;
// using InscryptionAPI.Ascension;
// using InscryptionAPI.Card;
// using JSONLoader3.Peripheral.ImageHandling;
// using Sirenix.Utilities;
// using UnityEngine;
//
// namespace JSONLoader3.Cores.JSONLoaderV2Support.Objects;
//
// public class DeckObject
// {
//     public List<DeckDataObject> decks {get; set;}
//     internal string file;
//     internal string pluginName;
//
//     public DeckObject(List<DeckDataObject> decks, string file, string pluginName)
//     {
//         this.decks = decks;
//         this.file = file;
//         this.pluginName = pluginName;
//     }
//
//     public List<StarterDeckInfo> ConvertDeckObjectToStarterDeckInfo()
//     {
//         List<StarterDeckInfo> infos = new List<StarterDeckInfo>();
//         foreach (DeckDataObject deck in decks)
//         {
//             StarterDeckInfo info = deck.ConvertDeckDataObjectToStarterDeckInfo();
//             infos.Add(info);
//         }
//
//         return infos;
//     }
// }
//
// public class DeckDataObject
// {
//     public List<string> fieldsToEdit { get; set; }
//     public string name { get; set; }
//     public string modPrefix { get; set; }
//     public string title { get; set; }
//     public List<string> cards { get; set; }
//     public string iconTexture { get; set; }
//     public int unlockLevel { get; set; }
//     internal string file;
//     internal string pluginName;
//
//     public DeckDataObject(List<string> fieldsToEdit, string name, string modPrefix, string title = "",
//         List<string> cards = null, string iconTexture = "", int unlockLevel = 0, string file = "", string pluginName = "")
//     {
//         this.fieldsToEdit = fieldsToEdit;
//         this.name = modPrefix + "_" + name;
//         this.modPrefix = modPrefix;
//         this.title = title;
//         this.cards = cards;
//         this.iconTexture = iconTexture;
//         this.unlockLevel = unlockLevel;
//         this.file = file;
//         this.pluginName = pluginName;
//     }
//
//     public StarterDeckInfo ConvertDeckDataObjectToStarterDeckInfo()
//     {
//         StarterDeckInfo info = null;
//         if (!fieldsToEdit.IsNullOrEmpty())
//         {
//             StarterDeckInfo infoTemp = StarterDeckManager.AllDeckInfos.FirstOrDefault(x => x.name == name.Replace(modPrefix + "_", ""));
//             if (infoTemp != null)
//             {
//                 info = infoTemp;
//
//                 if (fieldsToEdit.Contains("title"))
//                 {
//                     if (!string.IsNullOrEmpty(title))
//                         info.title = title;
//                 }
//
//                 if (fieldsToEdit.Contains("cards"))
//                 {
//                     if (cards != null)
//                     {
//                         info.cards = new List<CardInfo>();
//                         foreach (string card in cards)
//                         {
//                             if (!string.IsNullOrEmpty(card))
//                                 info.cards.Add(CardManager.AllCardsCopy.Find(x => x.name == card));
//                         }
//                     }
//                 }
//
//                 if (fieldsToEdit.Contains("iconTexture"))
//                 {
//                     if (!string.IsNullOrEmpty(iconTexture))
//                     {
//                         Sprite sprite = ScanImages.ParseImage(pluginName, iconTexture);
//
//                         if (sprite != null)
//                             info.SetIconTexture(sprite);
//                     }
//                 }
//
//                 if (fieldsToEdit.Contains("unlockLevel"))
//                 {
//                     info.SetUnlockLevel(unlockLevel);
//                 }
//             }
//         }
//         else
//         {
//             StarterDeckManager.FullStarterDeck info2 = StarterDeckManager.New(modPrefix, name, title, ScanImages.ParseImage(pluginName, iconTexture).texture, cards, unlockLevel);
//
//             info = info2.Info;
//         }
//
//         return info;
//     }
// }