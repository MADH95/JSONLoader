using DiskCardGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TinyJson;
using UnityEngine;

namespace JLPlugin.Data
{
    using InscryptionAPI.Card;
    using SigilCode;
    using static InscryptionAPI.Card.SpecialTriggeredAbilityManager;
    using SigilTuple = Tuple<Type, SigilData>;
    using StatTuple = Tuple<Type, SigilData, SpecialStatIcon>;

    public partial class SigilData
    {
        public void GenerateNew()
        {
            //It might be a good idea to add a check here to see if the trigger is valid
            //and then send an error message if it isn't?

            //Type SigilType = GetType("JLPlugin.SigilCode", this.sigilBase);
            Type SigilType = typeof(ConfigurableMain);

            if (this.isSpecialAbility == true)
            {
                SigilType = typeof(ConfigurableSpecial);

                string abilityName = "";
                ImportExportUtils.ApplyLocaleField("name", ref name, ref abilityName, true);
                FullSpecialTriggeredAbility specialAbility = SpecialTriggeredAbilityManager.Add(
                    this.GUID ?? Plugin.PluginGuid,
                    abilityName,
                    SigilType
                    );

                SigilDicts.SpecialArgumentList[specialAbility.Id] = new SigilTuple(SigilType, this);

                return;
            }
            else if (this.isPowerStat == true)
            {
                SigilType = typeof(ConfigurablePowerStat);

                StatIconInfo val = ScriptableObject.CreateInstance<StatIconInfo>();
                ImportExportUtils.ApplyValue(ref val.metaCategories, ref metaCategories, true, "PowerStat", "metaCategories");
                ImportExportUtils.ApplyValue(ref val.appliesToAttack, ref appliesToAttack, true, "appliesToAttack", "appliesToAttack");
                ImportExportUtils.ApplyValue(ref val.appliesToHealth, ref appliesToHealth, true, "appliesToHealth", "appliesToHealth");
                ImportExportUtils.ApplyLocaleField("name", ref name, ref val.rulebookName, true);
                ImportExportUtils.ApplyLocaleField("description", ref description, ref val.rulebookDescription, true);
                ImportExportUtils.ApplyValue(ref val.iconGraphic, ref texture, true, "PowerStat", "texture");

                var FullStatIcon = StatIconManager.Add(this.GUID ?? Plugin.PluginGuid, val, typeof(ConfigurablePowerStat));

                SigilDicts.PowerStatArgumentList[FullStatIcon.AbilityId] = new StatTuple(SigilType, this, FullStatIcon.Id);

                return;
            }

            // This is for debugging it should be removed before release
            //var fields = this.GetType()
            //         .GetFields();
            //
            //var values = fields.Select(field => field.GetValue(this)).ToList();
            //
            //List<string> fieldsinfo = new();
            //
            //for (int i = 0; i < fields.Length; ++i)
            //{
            //    Plugin.Log.LogWarning($"{fields[i].Name}: {values[i]}\n");
            //}

            Texture2D convertedTexture = null;
            ImportExportUtils.ApplyValue(ref convertedTexture, ref texture, true, "Configils", "texture");
            
            AbilityInfo info = AbilityManager.New(
                    guid: this.GUID ?? Plugin.PluginGuid,
                    rulebookName: this.name.EnglishValue ?? "",
                    rulebookDescription: this.description.EnglishValue ?? "",
                    behavior: SigilType,
                    tex: convertedTexture == null ? new Texture2D(49, 49) : convertedTexture
                );
            
            string temp = "_";
            ImportExportUtils.ApplyLocaleField("name", ref this.name, ref temp, true);
            ImportExportUtils.ApplyLocaleField("description", ref this.description, ref temp, true);
            ImportExportUtils.ApplyValue(ref info.powerLevel, ref this.powerLevel, true, "Configils", "powerLevel");
            ImportExportUtils.ApplyValue(ref info.canStack, ref this.canStack, true, "Configils", "canStack");
            ImportExportUtils.ApplyValue(ref info.opponentUsable, ref opponentUsable, true, "Configils", "opponentUsable");
            ImportExportUtils.ApplyValue(ref info.abilityLearnedDialogue, ref abilityLearnedDialogue, true, "Configils", "abilityLearnedDialogue");

            ImportExportUtils.ApplyValue(ref info.metaCategories, ref this.metaCategories, true, "Configils", "metaCategories");
            if (metaCategories == null || metaCategories.Count == 0)
            {
                info.SetDefaultPart1Ability();
            }
            
            Texture2D convertedPixelTexture = null;
            ImportExportUtils.ApplyValue(ref convertedPixelTexture, ref pixelTexture, true, "Configils", "pixelTexture");
            info.SetPixelAbilityIcon(convertedPixelTexture == null ? new Texture2D(17, 17) : convertedPixelTexture);

            if (abilityBehaviour != null)
            {
                info.activated = this.abilityBehaviour.Any(x => x.trigger?.triggerType == "OnActivate");
            }

            // Below is an example of the TriggerType enum being used. This current implementation doesn't make good use of it as the enum
            // would need converted from the string each time. Perhaps there's a method where the list of strings are converted and stored
            // but this is something for down the line. None the less, the TriggerType enum exists in the same file as the Trigger class.

            //info.activated = this.abilityBehaviour.Any( x => ParseEnum<TriggerType>( x.trigger?.triggerType ) == TriggerType.OnActivate );

            SigilDicts.ArgumentList[info.ability] = new SigilTuple(SigilType, this);
        }

        public static SigilData GetAbilityArguments(Ability ability)
        {
            SigilTuple data;
            return SigilDicts.ArgumentList.TryGetValue(ability, out data) ? data.Item2 : null;
        }

        public static SigilData GetAbilityArguments(SpecialTriggeredAbility ability)
        {
            SigilTuple data;
            return SigilDicts.SpecialArgumentList.TryGetValue(ability, out data) ? data.Item2 : null;
        }

        public static void LoadAllSigils(List<string> files)
        {
            for (int index = 0; index < files.Count; index++)
            {
                string file = files[index];
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (!filename.EndsWith("_sigil.jldr2"))
                    continue;

                files.RemoveAt(index--);

                Plugin.VerboseLog($"Loading JLDR2 (sigil) {filename}");
                ImportExportUtils.SetDebugPath(file);
                try
                {
                    SigilData sigilInfo = JSONParser.FromFilePath<SigilData>(file);

                    ImportExportUtils.SetID(sigilInfo.GUID + "_" + sigilInfo.name.EnglishValue);
                    sigilInfo.GenerateNew();
                    Plugin.VerboseLog($"Loaded JSON sigil {sigilInfo.name}");
                }
                catch (Exception e)
                {
                    Plugin.Log.LogError($"Failed to load {filename}: {e.Message}");
                    Plugin.Log.LogError(e);
                }
            }
        }

        
    }
}
