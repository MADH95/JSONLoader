using System;
using System.Collections.Generic;
using System.IO;
using BepInEx;
using DiskCardGame;
using InscryptionAPI.Masks;
using JLPlugin;
using TinyJson;

namespace JSONLoader.V2Code
{
    [Serializable]
    public class MaskData
    {
        public enum AdditionType
        {
            Override,
            Add,
            Random
        }
        
        public string maskName;
        public string texturePath;
        public string maskType;
        public string modelType;
        public string type;
        
        public static void LoadAllMasks(List<string> files)
        {
            for (int index = 0; index < files.Count; index++)
            {
                string file = files[index];
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                if (!filename.EndsWith("_mask.jldr2")) 
                    continue;
                
                files.RemoveAt(index--);
                
                Plugin.VerboseLog($"Loading JLDR2 (mask) {filename}");
                MaskData mask = JSONParser.FromFilePath<MaskData>(file);

                LeshyAnimationController.Mask? bossType = GetMask(mask.maskType);
                if (!bossType.HasValue)
                    continue;
                
                if (!Enum.TryParse(mask.type, out AdditionType additionType))
                {
                    Plugin.Log.LogError($"Could not parse mask {mask.maskName} type '{additionType}'!");
                    continue;
                }

                CustomMask customMask = null;
                if (additionType == AdditionType.Override)
                    customMask = MaskManager.Override(Plugin.PluginGuid, mask.maskName, bossType.Value, mask.texturePath);
                else if (additionType == AdditionType.Random)
                    customMask = MaskManager.AddRandom(Plugin.PluginGuid, mask.maskName, bossType.Value, mask.texturePath);
                else
                    customMask = MaskManager.Add(Plugin.PluginGuid, mask.maskName, mask.texturePath);
                
                MaskManager.ModelType? modelType = GetModelType(mask.modelType);
                if (modelType.HasValue)
                    customMask.SetModelType(modelType.Value);

                Plugin.VerboseLog($"Loaded JSON mask from {filename}!");
            }
        }

        private static MaskManager.ModelType? GetModelType(string modelType)
        {
            if (string.IsNullOrEmpty(modelType))
                return MaskManager.ModelType.FlatMask;
            
            if (Enum.TryParse(modelType, out MaskManager.ModelType bossType))
                return bossType;
            
            // TODO: Custom model types
            
            Plugin.Log.LogWarning($"Could not parse mask model type '{modelType}'!");
            return null;
        }

        private static LeshyAnimationController.Mask? GetMask(string maskOverrideMask)
        {
            if (Enum.TryParse(maskOverrideMask, out LeshyAnimationController.Mask bossType))
                return bossType;
            
            foreach (CustomMask mask in MaskManager.CustomMasks)
            {
                if (maskOverrideMask.StartsWith(mask.GUID) && maskOverrideMask.EndsWith(mask.Name))
                {
                    return mask.ID;
                }
            }
            
            Plugin.Log.LogError($"Could not parse mask type '{maskOverrideMask}'!");
            return LeshyAnimationController.Mask.Prospector;
        }

        private static bool TryGetFullPath(string fileName, out string fullFilePath)
        {
            DirectoryInfo directory = new DirectoryInfo(Paths.PluginPath);
            FileInfo[] filesInDir = directory.GetFiles(fileName, SearchOption.AllDirectories);
            if (filesInDir.Length == 0)
            {
                fullFilePath = null;
                return false;
            }
            else if (filesInDir.Length > 1)
            {
                Plugin.Log.LogWarning($"More than 1 file with the filename {fileName}! Using first one!");
            }

            fullFilePath = filesInDir[0].FullName;
            return true;
        }
    }
}