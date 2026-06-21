using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

                if (!filename.ToLower().EndsWith("_tribe.jldr2") && !filename.EndsWith("_tribes.jldr2"))
                    continue;

                ImportExportUtils.SetDebugPath(file);
                files.RemoveAt(index--);
                
                try
                {
                    Plugin.VerboseLog($"Loading JLDR2 (tribes) {filename}");
                    TribeList tribeInfo = JSONParser.FromFilePath<TribeList>(file);
                    foreach (TribeInfo tribedata in tribeInfo.tribes)
                    {
                        Texture2D backTex = null;
                        Texture2D iconTex = null;

                        ImportExportUtils.SetID(tribedata.name);
                        ImportExportUtils.ApplyValue(ref iconTex, ref tribedata.tribeIcon, true, "Tribes", "tribeIcon");

                        if (!string.IsNullOrEmpty(tribedata.choiceCardBackTexture))
                        {
                            Plugin.VerboseLog($"Loading {tribedata.name} back " + tribedata.choiceCardBackTexture);
                            ImportExportUtils.ApplyValue(ref backTex, ref tribedata.choiceCardBackTexture, true,
                                "Tribes",
                                "choiceCardBackTexture");
                        }

                        if (backTex == null)
                        {
                            backTex = TextureHelper.GetImageAsTexture("default_card_rewardback_blank.png");
                            if (iconTex != null)
                            {
                                Color32[] iconPixels = iconTex.GetPixels32();
                                for (int i = 0; i < iconPixels.Length; i++)
                                {
                                    if (iconPixels[i].a >= 1)
                                        backTex.SetPixel((i % iconTex.width) + 12, (i / iconTex.width) + 12,
                                            iconPixels[i]);
                                }

                                backTex.Apply(false);
                            }
                        }

                        TribeManager.Add(tribedata.guid, tribedata.name, iconTex, tribedata.appearInTribeChoices,
                            backTex);
                    }
                }
                catch (Exception e)
                {
                    Plugin.Log.LogError($"Error loading tribe from {file}");
                    Plugin.Log.LogError(e);
                }
            }
        }

        public static void ExportAllTribes()
        {
            Plugin.Log.LogInfo($"Exporting {((int)Tribe.NUM_TRIBES - 1 + TribeManager.NewTribes.Count)} Tribes to JSON");
            foreach (Tribe tribe in Enum.GetValues(typeof(Tribe)))
            {
                if (tribe is Tribe.None or Tribe.NUM_TRIBES)
                    continue;

                TribeManager.TribeInfo tribeInfo = new TribeManager.TribeInfo();
                tribeInfo.name = tribe.ToString();
                tribeInfo.guid = tribeInfo.guid;
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

            ImportExportUtils.SetID(info.name);
            ImportExportUtils.ApplyValue(ref info.icon, ref serializedTribe.tribeIcon, false, "Tribes", "tribeIcon");
            ImportExportUtils.ApplyValue(ref info.cardback, ref serializedTribe.choiceCardBackTexture, false, "Tribes", "choiceCardBackTexture");


            string json = JSONParser.ToJSON(serializedTribe);
            File.WriteAllText(Path.Combine(path, serializedTribe.name + "_tribe.jldr2"), json);
        }
    }
}
