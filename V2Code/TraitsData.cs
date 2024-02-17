using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InscryptionAPI.Guid;
using TinyJson;
using UnityEngine;
using UnityEngine.Serialization;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class TraitList
    {
        public class TraitInfo
        {
            public string name;
            public string guid;
        }

        public TraitInfo[] traits;

        public static void LoadAllTraits(List<string> files)
        {
            for (int index = 0; index < files.Count; index++)
            {
                string file = files[index];
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (!filename.EndsWith("_traits.jldr2") && !filename.EndsWith("_trait.jldr2"))
                    continue;

                ImportExportUtils.SetDebugPath(file);
                files.RemoveAt(index--);
                
                try
                {
                    Plugin.VerboseLog($"Loading JLDR2 (traits) {filename}");
                    TraitList traitList = JSONParser.FromFilePath<TraitList>(file);
                    foreach (TraitInfo trait in traitList.traits)
                    {
                        GuidManager.GetEnumValue<Trait>(trait.guid ?? Plugin.PluginGuid, trait.name);
                    }
                }
                catch (Exception e)
                {
                    Plugin.Log.LogError($"Error loading trait from {file}");
                    Plugin.Log.LogError(e);
                }
            }
        }
    }
}
