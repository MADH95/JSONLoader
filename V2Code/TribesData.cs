using System;
using System.Collections.Generic;
using BepInEx;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
using System.IO;
using System.Linq;
using DiskCardGame;
using TinyJson;
using UnityEngine;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class TribeList
    {
        public class TribeInfo
        {
            public string name;
            public string guid;
            public string tribeIcon;
            public bool appearInTribeChoices;
            public string choiceCardBackTexture;
        }

        public TribeInfo[] tribes;

        public static void LoadAllTribes(List<string> files)
        {
            for (int index = 0; index < files.Count; index++)
            {
                string file = files[index];
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (!filename.EndsWith("_tribe.jldr2"))
                    continue;

                files.RemoveAt(index--);

                Plugin.Log.LogDebug($"Loading JLDR2 (tribes) {filename}");
                TribeList tribeInfo = JSONParser.FromJson<TribeList>(File.ReadAllText(file));

                foreach (var tribedata in tribeInfo.tribes)
                {
                    Texture2D backTex;
                    Texture2D iconTex;

                    if (!tribedata.tribeIcon.IsNullOrWhiteSpace())
                    {
                        iconTex = TextureHelper.GetImageAsTexture(tribedata.tribeIcon);
                    }
                    else
                    {
                        iconTex = null;
                    }

                    if (tribedata.choiceCardBackTexture.IsNullOrWhiteSpace())
                    {
                        backTex = TextureHelper.GetImageAsTexture("default_card_rewardback_blank.png");
                        if (iconTex != null)
                        {
                            Color32[] iconPixels = iconTex.GetPixels32();
                            for (int i = 0; i < iconPixels.Length; i++)
                            {
                                if (iconPixels[i].a >= 1)
                                    backTex.SetPixel((i % iconTex.width) + 12, (i / iconTex.width) + 12, iconPixels[i]);
                            }

                            backTex.Apply(false);
                        }
                    }
                    else
                    {
                        backTex = TextureHelper.GetImageAsTexture(tribedata.choiceCardBackTexture);
                    }

                    TribeManager.Add(tribedata.guid, tribedata.name, iconTex, tribedata.appearInTribeChoices, backTex);
                }

                Plugin.Log.LogDebug(
                    $"Loaded JSON tribes {string.Join(",", tribeInfo.tribes.Select(s => s.name).ToList())}");
            }
        }

        public static void ExportAllTribes()
        {
            Plugin.Log.LogInfo($"Exporting {((int)Tribe.NUM_TRIBES-1+TribeManager.NewTribes.Count)} Tribes to JSON");
            foreach (Tribe tribe in Enum.GetValues(typeof(Tribe)))
            {
                if (tribe is Tribe.None or Tribe.NUM_TRIBES)
                    continue;
                
                TribeManager.TribeInfo tribeInfo = new TribeManager.TribeInfo();
                tribeInfo.name = tribe.ToString();
                tribeInfo.guid = "";
                tribeInfo.tribeChoice = true;
                tribeInfo.cardback = ResourceBank.Get<Texture2D>("Art/Cards/RewardBacks/card_rewardback_" + tribe.ToString().ToLowerInvariant());
                tribeInfo.icon = ResourceBank.Get<Sprite>("Art/Cards/TribeIcons/tribeicon_" + tribe.ToString().ToLowerInvariant());
                ExportTribe(tribeInfo);
            }
            
            foreach (TribeManager.TribeInfo tribe in TribeManager.NewTribes)
            {
                ExportTribe(tribe);
            }
        }
        
        public static void ExportTribe(TribeManager.TribeInfo info)
        {
            string path = Path.Combine(Plugin.ExportDirectory, "Tribes");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            TribeInfo serializedTribe = new TribeInfo();
            serializedTribe.name = info.name;
            serializedTribe.guid = info.guid;
            serializedTribe.appearInTribeChoices = info.tribeChoice;
            
            ImportExportUtils.ApplyEnumList(ref info.icon, ref serializedTribe.tribeIcon, false, "Tribes", $"{serializedTribe.guid}_{serializedTribe.name}_icon");
            ImportExportUtils.ApplyEnumList(ref info.cardback, ref serializedTribe.choiceCardBackTexture, false, "Tribes", $"{serializedTribe.guid}_{serializedTribe.name}_choiceCardBackTexture");
            
            string json = JSONParser.ToJSON(serializedTribe);
            File.WriteAllText(Path.Combine(path, serializedTribe.name + ".jldr2"), json);
        }
    }
}
