using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

namespace JSONLoader.V1Code.Utils
{
        public static class ImageLoader
        {
            // Load a custom texture from the specified path
            public static Texture2D LoadCustomTexture(string fileName, string imagePath)
            {
                string imageToLoad = Path.Combine(imagePath, fileName + ".png");
                if (File.Exists(imageToLoad))
                {
                    byte[] data = File.ReadAllBytes(imageToLoad);
                    Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false); // Use RGBA32 format for better color representation
                    texture2D.LoadImage(data);
                    return texture2D;
                }
                Debug.LogWarning($"Texture not found at path: {imageToLoad}");
                return null;
            }

            // Load a custom PNG texture from the specified path
            public static Texture2D LoadCustomPNG(string fileName, string imagePath)
            {
                return LoadCustomTexture(fileName, imagePath); // Reuse LoadCustomTexture for PNG files
            }

            // Create a Sprite from a custom image file
            public static Sprite GetCustomImage(string fileName, string imagePath)
            {
                Texture2D texture2D = LoadCustomPNG(fileName, imagePath);
                if (texture2D != null)
                {
                    Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), Vector2.zero);
                    sprite.name = fileName; // Name the sprite for easy identification
                    return sprite;
                }
                Debug.LogWarning($"Sprite creation failed for: {fileName} at path: {imagePath}");
                return null;
            }
        }
}
