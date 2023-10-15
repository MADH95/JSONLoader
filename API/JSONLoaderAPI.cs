using DiskCardGame;
using JLPlugin;
using JLPlugin.V2.Data;
using System;
using System.Collections.Generic;
using TinyJson;

namespace JSONLoader.API
{
    public static class JSONLoaderAPI
    {

        public static void AddCard(string json) { AddCards(json); }

        public static void AddCards(params string[] json)
        {
            foreach (string card in json)
            {
                try
                {
                    CardSerializeInfo cardInfo = JSONParser.FromJson<CardSerializeInfo>(card);
                    cardInfo.Apply();
                    Plugin.Log.LogDebug($"Added card {cardInfo.name} using JSONLoader API");
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError($"Failed to add card using JSONLoader API: {ex.Message}");
                    Plugin.Log.LogError(ex);
                }
            }
        }

        public static void RemoveCard(string json) { RemoveCards(json); }

        public static void RemoveCards(params string[] json)
        {
            foreach (string card in json)
            {
                try
                {
                    CardSerializeInfo cardInfo = JSONParser.FromJson<CardSerializeInfo>(card);
                    cardInfo.Remove();
                    Plugin.Log.LogDebug($"Removed card {cardInfo.name} using JSONLoader API");
                }
                catch (Exception ex)
                {
                    Plugin.Log.LogError($"Failed to remove card using JSONLoader API: {ex.Message}");
                    Plugin.Log.LogError(ex);
                }
            }
        }

        public static CardInfo ParseCard(string json)
        {
            try
            {
                CardSerializeInfo cardInfo = JSONParser.FromJson<CardSerializeInfo>(json);
                return cardInfo.ToCardInfo();
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError($"Failed to parse card using JSONLoader API: {ex.Message}");
                Plugin.Log.LogError(ex);
            }
            return null;
        }

        public static List<CardInfo> ParseCards(params string[] json)
        {
            List<CardInfo> cards = new List<CardInfo>();
            foreach (string card in json)
            {
                cards.Add(ParseCard(card));
            }
            return cards;
        }
    }
}
