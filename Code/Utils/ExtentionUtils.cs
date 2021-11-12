using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace JLPlugin.Utils
{
    public static class ExtensionUtils
    {
        public static List<T> AsEnum<T>( this List<string> list, Dictionary<string, T> dict, Func<string, string> message ) where T : struct, IConvertible
        {
            List<T> output = new();

            foreach ( string elem in list )
            {
                try
                {
                    output.Add( dict[ elem ] );
                }
                catch
                {
                    Plugin.Log.LogError( message( elem ) );
                }
            }

            return output;
        }

        public static Texture2D WithImage( this Texture2D texture, string image )
        {
            byte[] imgBytes = File.ReadAllBytes( Path.Combine( Plugin.ArtPath, image ) );
            texture.LoadImage( imgBytes );
            return texture;
        }

        public static Sprite AsSprite( this Texture2D tex, string name )
        {
            tex.name = "portrait_" + name;
            tex.filterMode = FilterMode.Point;
            Sprite sprite = Sprite.Create(tex, new(0.0f, 0.0f, tex.width, tex.height), new(0.5f, 0.5f));
            sprite.name = "portrait_" + name;

            return sprite;
        }
    }
}
