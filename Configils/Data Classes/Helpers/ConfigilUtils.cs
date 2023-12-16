using BepInEx.Bootstrap;
using BepInEx.Configuration;
using DiskCardGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLPlugin.Data
{
    public class ConfigilUtils
    {
        public static CardModificationInfo GetModById(PlayableCard card, string id, bool isPermanent = false)
        {
            CardModificationInfo mod;
            if ((isPermanent ? card.Info.Mods : card.TemporaryMods).Where(x => x.singletonId == id).ToList().Count > 0)
            {
                mod = (isPermanent ? card.Info.Mods : card.TemporaryMods).Where(x => x.singletonId == id).ToList()[0];
            }
            else
            {
                mod = new CardModificationInfo() { singletonId = id };
                if (isPermanent)
                {
                    card.Info.Mods.Add(mod);
                }
                else
                {
                    card.AddTemporaryMod(mod);
                }
            }
            return mod;
        }

        public static object GetConfigByGuid(string guid, string configSection, string configName)
        {
            KeyValuePair<string, BepInEx.PluginInfo>? PluginInfo = Chainloader.PluginInfos.First(x => x.Key == guid);
            if (PluginInfo == null)
            {
                return null;
            }

            ConfigFile config = PluginInfo.Value.Value.Instance.Config;
            return config[new ConfigDefinition(configSection, configName)].BoxedValue;
        }
    }
}
