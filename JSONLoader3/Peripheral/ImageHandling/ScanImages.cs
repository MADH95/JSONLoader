using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InscryptionAPI.Helpers;
using UnityEngine;

namespace JSONLoader3.Peripheral.ImageHandling;

/// <summary>
/// A class to handle the scanning of Images into the Game.
/// </summary>
public class ScanImages
{
    /// <summary>
    /// A function that handles and determines how to Parse the Image.
    /// </summary>
    /// <param name="plugin">The full path to the Plugin.</param>
    /// <param name="imagePath">Either the Relative Image Path or Base64 of the Image.</param>
    /// <returns>A Sprite of the Image.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    public static Sprite ParseImage(string plugin, string imagePath)
    {
        if (imagePath.StartsWith("data:image/png;base64,") || imagePath.StartsWith("base64:"))
        {
            return GetTextureFromBase64(imagePath);
        }

        if (!imagePath.Contains("/"))
        {
            return GetSpriteFromFileName(imagePath, plugin);
        }

        return GetTextureFromPluginAndPath(plugin, imagePath);
    }
    
    /// <summary>
    /// Gets a Texture2D from the Base64 Image.
    /// </summary>
    /// <param name="path">The Base64 Path of the Image.</param>
    /// <returns>A Texture2D from the Base64.</returns>
    private static Texture2D GetTextureFromString(string path)
    {
        if (path.StartsWith("data:image/png;base64,"))
        {
            try
            {
                string contents = path.Substring("data:image/png;base64,".Length);
                byte[] bytes = Convert.FromBase64String(contents);

                Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                texture.filterMode = FilterMode.Point;
                texture.LoadImage(bytes);

                return texture;
            }
            catch (Exception e)
            {
                JSONLoader3.FormatLogger("Error", "ScanImages", $"Failed to convert base64 to texture: {path}");
                JSONLoader3.FormatLogger("Debug", "ScanImages", "Printing Message of Exception and Source");
                JSONLoader3.FormatLogger("Debug", "ScanImages", e.Message + " " + e.Source);
                JSONLoader3.FormatLogger("Debug", "ScanImages", "Printing Stack Trace");
                JSONLoader3.FormatLogger("Debug", "ScanImages", e.StackTrace);
            }
        }

        if (path.StartsWith("base64:"))
        {
            try
            {
                string contents = path.Substring("base64:".Length);
                byte[] bytes = Convert.FromBase64String(contents);

                Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
                texture.filterMode = FilterMode.Point;
                texture.LoadImage(bytes);

                return texture;
            }
            catch (Exception e)
            {
                JSONLoader3.FormatLogger("Error", "ScanImages", $"Failed to convert base64 to texture: {path}");
                JSONLoader3.FormatLogger("Debug", "ScanImages", "Printing Message of Exception and Source");
                JSONLoader3.FormatLogger("Debug", "ScanImages", e.Message + " " + e.Source);
                JSONLoader3.FormatLogger("Debug", "ScanImages", "Printing Stack Trace");
                JSONLoader3.FormatLogger("Debug", "ScanImages", e.StackTrace);
            }
        }

        return TextureHelper.GetImageAsTexture(path);
    }

    /// <summary>
    /// Converts the Base64 into a Sprite.
    /// </summary>
    /// <param name="texture">The Base64's Texture2D.</param>
    /// <returns>A Sprite of the Base64 Texture.</returns>
    private static Sprite GetTextureFromBase64(string texture)
    {
        if (!string.IsNullOrEmpty(texture))
        {
            Texture2D imageAsTexture = GetTextureFromString(texture);
            if (imageAsTexture != null)
            {
                return imageAsTexture.ConvertTexture();
            }
        }

        return null;
    }

    /// <summary>
    /// Gets the Texture from a Plugin with a Path.
    /// </summary>
    /// <param name="plugin">The Full Path to the Plugin.</param>
    /// <param name="filePath">The Relative Path to the File.</param>
    /// <returns>A Sprite from the Plugin and Path.</returns>
    /// <remarks>This code is provided by Creator/Chaosyr/SaxbyMod/The Stoat Lord.</remarks>
    private static Sprite GetTextureFromPluginAndPath(string plugin, string filePath)
    {
        string fullPath = Path.GetFullPath(
            Path.Combine(plugin, filePath.Replace('/', Path.DirectorySeparatorChar))
        );

        if (!File.Exists(fullPath))
        {
            fullPath = Path.GetFullPath(Path.Combine(plugin, "plugins",
                filePath.Replace('/', Path.DirectorySeparatorChar)));

            if (!File.Exists(fullPath))
            {
                JSONLoader3.FormatLogger("Error", "ScanImages", $"File not found at path: {filePath} when appended onto the Plugins Path.");
                JSONLoader3.FormatLogger("AdditionalInformation", "ScanImages", "We tried the following 2 paths: ");
                JSONLoader3.FormatLogger("AdditionalInformation", "ScanImages", Path.GetFullPath(Path.Combine(plugin, filePath.Replace('/', Path.DirectorySeparatorChar))));
                JSONLoader3.FormatLogger("AdditionalInformation", "ScanImages", Path.GetFullPath(Path.Combine(plugin, "plugins", filePath.Replace('/', Path.DirectorySeparatorChar))));
                JSONLoader3.FormatLogger("AdditionalInformation", "ScanImages", "To resolve this, just make sure the Path leads to where your asset is located, we don't recursively scan in this case.");

                Texture2D fallbackTexture = new Texture2D(114, 94);

                return Sprite.Create(
                    fallbackTexture,
                    new Rect(0f, 0f, 114f, 94f),
                    new Vector2(0.5f, 0.5f)
                );
            }
        }

        return GetCustomImage(
            Path.GetFileNameWithoutExtension(filePath),
            Path.GetDirectoryName(fullPath)
        );
    }

    /// <summary>
    /// Gets the Image when all you have is the FileName and PluginPath.
    /// </summary>
    /// <param name="fileName">The Name of the File.</param>
    /// <param name="pluginPath">The Plugin in which is originating the request for the file.</param>
    /// <returns>A Sprite if Successful it will have the image requested, if it failed it will be an empty image.</returns>
    public static Sprite GetSpriteFromFileName(string fileName, string pluginPath)
    {
        List<string> Files = Directory.GetFiles(pluginPath, "*", SearchOption.AllDirectories).ToList();
        foreach (string file in Files)
        {
            if (file.EndsWith(".png"))
            {
                if (Path.GetFileName(file) == fileName)
                {
                    return GetCustomImage(file.Replace(".png", ""), pluginPath);
                }
            }
        }

        Texture2D fallbackTexture = new Texture2D(114, 94);

        return Sprite.Create(
            fallbackTexture,
            new Rect(0f, 0f, 114f, 94f),
            new Vector2(0.5f, 0.5f)
        );
    }
    
    /// <summary>
    /// This essentially is LoadCustomTexture but from a PNG File.
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="imagePath">The path to the file.</param>
    /// <returns>A Texture2D of the inputted Image</returns>
    /// <remarks>This code is provided by the amazing dark dragoon on nexus mods and discord.</remarks>
    private static Texture2D LoadCustomPNG(string fileName, string imagePath)
    {
        return LoadCustomTexture(fileName, imagePath); // Reuse LoadCustomTexture for PNG files
    }
    
    /// <summary>
    /// This is a simple function that converts a file into a Texture2D
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="imagePath">The path to the file.</param>
    /// <returns>A Texture2D of the inputted Image</returns>
    /// <remarks>This code is provided by the amazing dark dragoon on nexus mods and discord.</remarks>
    private static Texture2D LoadCustomTexture(string fileName, string imagePath)
    {
        string imageToLoad = Path.Combine(imagePath, fileName + ".png");
        if (File.Exists(imageToLoad))
        {
            byte[] data = File.ReadAllBytes(imageToLoad);
            Texture2D texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false); // Use RGBA32 format for better color representation
            texture2D.LoadImage(data);
            return texture2D;
        }
        JSONLoader3.FormatLogger("Error", "ScanImages", $"Texture not found at path: {imageToLoad}");
        return null;
    }
    
    /// <summary>
    /// This gets a sprite from the passed in file, specifically a PNG.
    /// </summary>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="imagePath">The path to the file.</param>
    /// <returns>A Sprite of the inputted Image</returns>
    /// <remarks>This code is provided by the amazing dark dragoon on nexus mods and discord.</remarks>
    private static Sprite GetCustomImage(string fileName, string imagePath)
    {
        Texture2D texture2D = LoadCustomPNG(fileName, imagePath);
        if (texture2D != null)
        {
            Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2((float)0.5, (float)0.5));
            sprite.name = fileName; // Name the sprite for easy identification
            return sprite;
        }
        JSONLoader3.FormatLogger("Error", "ScanImages", $"Sprite creation failed for: {fileName} at path: {imagePath}");
        return null;
    }
}