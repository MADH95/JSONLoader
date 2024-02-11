using System.IO;
using UnityEngine;
using TinyJson;

namespace JLPlugin.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BepInEx;
    using Data;
    using JLPlugin.V2.Data;

    [Obsolete]
    public static class JLUtils
    {
        public static void LoadCardsFromFiles(List<string> files)
        {
            Dictionary<string, CardData> loadedJldrs = new();
            for (int index = files.Count - 1; index >= 0; index--)
            {
                string file = files[index];
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                if (!file.EndsWith(".jldr"))
                    continue;

                files.RemoveAt(index);

                CardData card = JSONParser.FromFilePath<CardData>(file);
                if (card == null)
                {
                    Plugin.Log.LogWarning($"Failed to load {filename}");
                    continue;
                }

                loadedJldrs.Add(file, card);
            }

            List<CardData> allCards = loadedJldrs.Values.ToList();
            foreach (var item in loadedJldrs)
            {
                string file = item.Key;
                CardData card = item.Value;

                // Convert to JLDR2 and write back for now
                CardSerializeInfo info = card.ConvertToV2(allCards);
                if (info != null)
                {
                    string newPath = info.WriteToFile(file, false);
                    files.Add(newPath);
                }
                else
                {
                    Plugin.Log.LogError($"{file} is a JLDR without a valid name");
                }
            }
        }

        public static Texture2D LoadTexture2D( string image )
        {
            string[] imagePaths = Directory.GetFiles( Paths.PluginPath, image, SearchOption.AllDirectories );

            if ( imagePaths.Length == 0 )
            {
                Plugin.Log.LogError( $"{ ErrorUtil.Card } - Couldn't find texture \"{ image }\" to load into { ErrorUtil.Field }" );
                return null;
            }

            if ( imagePaths.Length > 1 )
            {
                Plugin.Log.LogError( $"{ ErrorUtil.Card } - Couldn't load \"{ image }\" into { ErrorUtil.Field }, more than one file with that name found in the plugins folder" );
                return null;
            }

            byte[] imgBytes = File.ReadAllBytes( imagePaths[0] );

            Texture2D texture = new( 2, 2 );

            if ( !texture.LoadImage( imgBytes ) )
            {
                Plugin.Log.LogError( $"{ ErrorUtil.Card } - Couldn't load \"{ image }\" into { ErrorUtil.Field }" );
                return null;
            }

            return texture;

        }
    }
}
