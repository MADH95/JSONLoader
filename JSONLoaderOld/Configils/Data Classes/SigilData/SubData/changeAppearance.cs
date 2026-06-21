using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using static JLPlugin.Interpreter;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class changeAppearance
    {
        public string runOnCondition;
        public slotData slot;
        public string targetCard;
        public string changePortrait;
        public string changeName;
        public List<string> addDecals;
        public List<string> removeDecals;

        public static IEnumerator ChangeAppearance(AbilityBehaviourData abilitydata)
        {
            foreach (changeAppearance changeAppearanceInfo in abilitydata.changeAppearance)
            {
                if (AConfigilData.ConvertArgument(changeAppearanceInfo.runOnCondition, abilitydata) == "false")
                {
                    continue;
                }

                // yield return new WaitForSeconds(0.3f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);

                PlayableCard CardToModify = null;
                if (changeAppearanceInfo.slot != null)
                {
                    CardSlot slot = slotData.GetSlot(changeAppearanceInfo.slot, abilitydata);
                    if (slot != null)
                    {
                        if (slot.Card != null)
                        {
                            CardToModify = slot.Card;
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(changeAppearanceInfo.targetCard))
                    {
                        CardToModify = (PlayableCard)AConfigilData.ConvertArgumentToType(changeAppearanceInfo.targetCard, abilitydata, typeof(PlayableCard));
                    }
                    else
                    {
                        CardToModify = abilitydata.self;
                    }
                }

                if (CardToModify != null)
                {
                    //AConfigilData.ConvertArgument(buffcardsinfo.addStats.Split('/')[0], abilitydata);
                    if (!string.IsNullOrWhiteSpace(changeAppearanceInfo.changePortrait))
                    {
                        try
                        {
                            Texture2D PortraitTexture = null;
                            ImportExportUtils.ApplyValue(ref PortraitTexture, ref changeAppearanceInfo.changePortrait, true, "Configils", "changePortrait");
                            UnityEngine.Sprite PortraitSprite = TextureHelper.ConvertTexture(PortraitTexture, TextureHelper.SpriteType.CardPortrait, FilterMode.Point);
                            CardToModify.SwitchToPortrait(PortraitSprite);
                        }
                        catch (FileNotFoundException innerException)
                        {
                            throw new ArgumentException("Image file not found for card \"" + abilitydata.self.name + "\"!", innerException);
                        }
                    }


                    if (!string.IsNullOrWhiteSpace(changeAppearanceInfo.changeName))
                    {
                        CardToModify.RenderInfo.nameOverride = changeAppearanceInfo.changeName;

                    }

                    if (changeAppearanceInfo.removeDecals != null)
                    {
                        foreach (string removeDecal in changeAppearanceInfo.removeDecals)
                        {
                            object obj;
                            if (abilitydata.ability == null)
                                obj = abilitydata.specialAbility == null ? abilitydata.specialStatIcon : abilitydata.specialAbility;
                            else
                                obj = abilitydata.ability;
                            
                            string name = $"{obj}_{removeDecal}";
                            CardToModify.Info.temporaryDecals.RemoveAll(x => x.name == name);
                        }
                    }

                    if (changeAppearanceInfo.addDecals != null)
                    {
                        for (var i = 0; i < changeAppearanceInfo.addDecals.Count; i++)
                        {
                            var addDecal = changeAppearanceInfo.addDecals[i];
                            Texture2D texture = null;
                            ImportExportUtils.ApplyValue(ref texture, ref addDecal, true, "Configils", "addDecals");
                            object obj;
                            if (abilitydata.ability == null)
                                obj = abilitydata.specialAbility == null ? abilitydata.specialStatIcon : abilitydata.specialAbility;
                            else
                                obj = abilitydata.ability;
                            texture.name = $"{obj}_{addDecal}";
                            CardToModify.Info.temporaryDecals.Add(texture);
                        }
                    }

                    CardToModify.RenderCard();
                }
            }
            yield break;
        }
    }
}
