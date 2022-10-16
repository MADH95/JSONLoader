using BepInEx;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
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

        public static void LoadAllTribes()
        {
            foreach (string file in Directory.EnumerateFiles(Paths.PluginPath, "*.jldr2", SearchOption.AllDirectories))
            {
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (filename.EndsWith("_tribe.jldr2"))
                {
                    Plugin.Log.LogDebug($"Loading JLDR2 (tribes) {filename}");
                    TribeList tribeInfo = JSONParser.FromJson<TribeList>(File.ReadAllText(file));

                    foreach (var tribedata in tribeInfo.tribes)
                    {
                        Texture2D backTex;
                        Texture2D iconTex;

                        if (!tribedata.tribeIcon.IsNullOrWhiteSpace())
                            iconTex = TextureHelper.GetImageAsTexture(tribedata.tribeIcon);
                        else iconTex = null;

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
                        else backTex = TextureHelper.GetImageAsTexture(tribedata.choiceCardBackTexture);

                        TribeManager.Add(tribedata.guid, tribedata.name, iconTex, tribedata.appearInTribeChoices, backTex);
                    }

                    Plugin.Log.LogDebug($"Loaded JSON tribes {string.Join(",", tribeInfo.tribes.Select(s => s.name).ToList())}");
                }
            }
        }
    }
}
