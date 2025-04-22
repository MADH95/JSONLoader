using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TinyJson;
using System.Linq;
using Sirenix.Utilities;
using JSONLoader.V1Code.Utils;
using DiskCardGame;
using InscryptionAPI.Helpers;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class AppearenceBehaviors
    {
        public class AppearenceBackgroundInfo
        {
            public Sprite CardBack {get; set;}
        }

        public class AppearenceCardBackground(AppearenceBackgroundInfo info) : CardAppearanceBehaviour
        {
            public override void ApplyAppearance()
            {
                Texture2D ExampleBG = info.CardBack.texture;
                ExampleBG.filterMode = FilterMode.Point;
                base.Card.RenderInfo.baseTextureOverride = ExampleBG;
            }

            // When this card is chosen via a sequencer, run the following method. This example does nothing, but think *Ijaraq*.
            public override void OnCardAddedToDeck()
            {

            }
        }

        public class AppearenceBehaviorInfo
        {
            public string Name;
            public string GUID;
            public string Layer;
            public List<string> ImageList = new List<string>();
            public bool Randomize;
            public string RandomizeConditions;
        }

        public static AppearenceBehaviorInfo[] AppearenceBehavior;

        public static void LoadAllAppearences(List<string> files)
        {
            for (int index = 0; index < files.Count; index++)
            {
                string file = files[index];
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar));
                string FilePath = Path.GetFullPath(file);

                if (!filename.ToLower().EndsWith("_appearence.jldr2"))
                    continue;

                ImportExportUtils.SetDebugPath(file);
                files.RemoveAt(index--);

                try
                {
                    Plugin.VerboseLog($"Loading JLDR2 (appearence behavior) {filename}");
                    AppearenceBehaviorInfo AppearenceBehaviors = JSONParser.FromFilePath<AppearenceBehaviorInfo>(file);
                    foreach (AppearenceBehaviorInfo appearencedata in AppearenceBehavior)
                    {
                        // Define an Appearence Info for the Appearence
                        AppearenceBehaviorInfo Appearence = new AppearenceBehaviorInfo
                        {
                            Name = appearencedata.Name,
                            GUID = appearencedata.GUID,
                            Layer = appearencedata.Layer,
                            ImageList = appearencedata.ImageList,
                            Randomize = appearencedata.Randomize,
                            RandomizeConditions = appearencedata.RandomizeConditions
                        };
                        // Create a List of Sprites for the Appearence
                        List<Sprite> Imagelist = new List<Sprite> { };

                        // Get the Image Sprite for each Image in the ImageList
                        foreach (string Image in Appearence.ImageList)
                        {
                            string[] SplitImagePath = Image.Split('/');
                            int PathCount = SplitImagePath.Count();
                            string ImageName = SplitImagePath.ElementAt(PathCount);
                            string NewImagePath = Image.Replace('/' + ImageName, "");
                            if (ImageName.EndsWith(".png"))
                            {
                                Sprite MyNewSprite = ImageLoader.GetCustomImage(FilePath + NewImagePath, ImageName);
                                Imagelist.AppendWith(MyNewSprite);
                            } else
                            {
                                Debug.LogError($"{FilePath + NewImagePath + ImageName} must be of type '.png'.");
                            }
                        }

                        // Create Appearence ID
                        string ID = Appearence.GUID + Appearence.Name;

                        // Define Layer Logic + Randomization Logic

                        if (Appearence.Randomize == true)
                        {
                            // When X condition Happens Do X
                            if (Appearence.RandomizeConditions == "OnRuntime")
                            {
                                var RandomizedImageList = Imagelist.Randomize();
                            }
                        }

                        if (Appearence.Layer == "CardBacking")
                        {
                            new AppearenceBackgroundInfo
                            {
                                CardBack = Imagelist[0]
                            };

                        }
                    }
                }
                catch (Exception e)
                {
                    Plugin.Log.LogError($"Error loading appearence from file {file}");
                    Plugin.Log.LogError(e);
                }
            }
        }
    }
}