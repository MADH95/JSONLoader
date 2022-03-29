using BepInEx;
using DiskCardGame;
using InscryptionAPI.Card;
using JLPlugin.V2.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TinyJson;
using UnityEngine;
using Random = System.Random;

namespace JLPlugin.Data
{
    using JLPlugin.SigilCode;
    using Utils;

    public partial class SigilData
    {
        public void GenerateNew()
        {
            //Type SigilType = GetType("JLPlugin.SigilCode", this.sigilBase);
            Type SigilType = typeof(ConfigurableMain);

            var fields = this.GetType()
                     .GetFields()
                     .Select(field => field.GetValue(this))
                     .ToList();

            Plugin.Log.LogInfo(string.Join(",", fields));

            Texture2D sigilTexture = new Texture2D(49, 49);
            if (this.texture != null)
            {
                sigilTexture = CDUtils.Assign(this.texture, nameof(this.texture));
            }
            sigilTexture.filterMode = FilterMode.Point;

            Texture2D sigilPixelTexture = new Texture2D(17, 17);
            if (this.pixelTexture != null)
            {
                sigilPixelTexture = CDUtils.Assign(this.pixelTexture, nameof(this.pixelTexture));
            }
            sigilPixelTexture.filterMode = FilterMode.Point;

            AbilityInfo info = AbilityManager.New(
                    this.GUID ?? "MADH.inscryption.JSONLoader",
                    this.name ?? "",
                    this.description ?? "",
                    SigilType,
                    sigilTexture

                );
            info.SetPixelAbilityIcon(sigilPixelTexture);
            info.powerLevel = this.powerLevel ?? 3;
            info.metaCategories = CDUtils.Assign(this.metaCategories, nameof(this.metaCategories), SigilDicts.AbilityMetaCategory) ?? new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };
            info.canStack = true;
            info.opponentUsable = true;

            SigilDicts.ArgumentList.Add(info.ability, new Tuple<Type, SigilData>(SigilType, this));
        }

        public static SigilData GetAbilityArguments(Ability ability)
        {
            return SigilDicts.ArgumentList[ability].Item2;
        }

        public static void LoadAllSigils()
        {
            foreach (string file in Directory.EnumerateFiles(Paths.PluginPath, "*.jldr2", SearchOption.AllDirectories))
            {
                string filename = file.Substring(file.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                if (filename.EndsWith("_sigilexample.jldr2"))
                {
                    Plugin.Log.LogDebug($"Skipping {filename}");
                    continue;
                }

                if (filename.EndsWith("_sigil.jldr2"))
                {
                    Plugin.Log.LogDebug($"Loading JLDR2 (sigil) {filename}");
                    SigilData sigilInfo = JSONParser.FromJson<SigilData>(File.ReadAllText(file));
                    Plugin.Log.LogInfo(sigilInfo.name);
                    sigilInfo.GenerateNew();
                    Plugin.Log.LogDebug($"Loaded JSON sigil {sigilInfo.name}");
                }
            }
        }

        public static T ConvertArgument<T>(T value)
        {
            if (value == null)
            {
                return value;
            }

            var random = new Random();

            if (value.GetType() == typeof(String))
            {
                if ((value as String).Contains("|"))
                {
                    List<String> StringList = (value as String).Split('|').ToList();
                    return (T)(object)StringList[random.Next(StringList.Count)];
                }
            }
            if (value.GetType() == typeof(List<String>))
            {
                List<String> newargumentlist = new List<String>();
                foreach (String argumentfromlist in (value as List<String>))
                {
                    if (argumentfromlist.Contains("|"))
                    {
                        List<String> StringList = argumentfromlist.Split('|').ToList();
                        newargumentlist.Add(StringList[random.Next(StringList.Count)]);
                    }
                    else
                    {
                        newargumentlist.Add(argumentfromlist);
                    }
                }
                return (T)(object)newargumentlist;
            }
            return value;
        }

        public static Type GetType(string nameSpace, string typeName)
        {
            string text = nameSpace + "." + typeName;
            Type type = Type.GetType(text);
            if (type != null)
            {
                return type;
            }
            if (text.Contains("."))
            {
                Assembly assembly = Assembly.Load(text.Substring(0, text.IndexOf('.')));
                if (assembly == null)
                {
                    return null;
                }
                type = assembly.GetType(text);
                if (type != null)
                {
                    return type;
                }
            }
            AssemblyName[] referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            for (int i = 0; i < referencedAssemblies.Length; i++)
            {
                Assembly assembly2 = Assembly.Load(referencedAssemblies[i]);
                if (assembly2 != null)
                {
                    type = assembly2.GetType(text);
                    if (type != null)
                    {
                        return type;
                    }
                }
            }
            return null;
        }

        public static void RunActions(Ability ability, CardSlot cardSlot, PlayableCard self)
        {
            CardSlot relativeSlot = cardSlot;
            SigilData abilitydata = GetAbilityArguments(ability);
            if (abilitydata.placeCards != null)
            {
                BoardManager.Instance.StartCoroutine(PlaceCards(abilitydata.placeCards, relativeSlot));
            }
            if (abilitydata.buffCards != null)
            {
                BoardManager.Instance.StartCoroutine(BuffCards(abilitydata.buffCards, relativeSlot, self));
            }
            if (abilitydata.drawCards != null)
            {
                BoardManager.Instance.StartCoroutine(DrawCards(abilitydata.drawCards));
            }
        }

        public static IEnumerator DrawCards(List<string> cards)
        {
            foreach (string card in cards)
            {
                if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
                {
                    yield return new WaitForSeconds(0.2f);
                    Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
                    yield return new WaitForSeconds(0.2f);
                }
                yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(CardLoader.GetCardByName(card), new List<CardModificationInfo>(), new Vector3(0f, 0f, 0f), 0, null);
                yield return new WaitForSeconds(0.45f);
            }
            yield break;
        }

        public static IEnumerator BuffCards(List<buffCards> buffCardsData, CardSlot relativeSlot, PlayableCard self)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.3f);

            foreach (buffCards buffcardsinfo in buffCardsData)
            {
                CardSlot slot = GetSlot(buffcardsinfo.slot, relativeSlot);
                PlayableCard card = null;
                if (slot != null)
                {
                    if (slot.Card != null)
                    {
                        card = slot.Card;
                    }
                }
                else if (buffcardsinfo.self != null)
                {
                    card = self;
                }

                if (card != null)
                {
                    CardModificationInfo mod = new CardModificationInfo();
                    if (buffcardsinfo.stats != null)
                    {
                        mod.attackAdjustment = int.Parse(buffcardsinfo.stats.Split('/')[0]);
                        mod.healthAdjustment = int.Parse(buffcardsinfo.stats.Split('/')[1]);
                    }
                    if (buffcardsinfo.abilities != null)
                    {
                        mod.abilities = buffcardsinfo.abilities.Select(s => CardSerializeInfo.ParseEnum<Ability>(s)).ToList();
                    }
                    if (buffcardsinfo.removeAbilities != null)
                    {
                        mod.negateAbilities = buffcardsinfo.removeAbilities.Select(s => CardSerializeInfo.ParseEnum<Ability>(s)).ToList();
                        card.Status.hiddenAbilities.AddRange(buffcardsinfo.removeAbilities.Select(s => CardSerializeInfo.ParseEnum<Ability>(s)));
                    }
                    card.AddTemporaryMod(mod);
                    card.RenderCard();
                    if (card.Health <= 0)
                    {
                        yield return card.Die(false);
                    }
                }
            }

            yield return new WaitForSeconds(0.3f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            yield break;
        }

        public static IEnumerator PlaceCards(List<placeCards> placeCardsData, CardSlot relativeSlot)
        {
            yield return new WaitForSeconds(0.3f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
            yield return new WaitForSeconds(0.3f);

            foreach (placeCards placecardinfo in placeCardsData)
            {
                CardSlot slot = GetSlot(placecardinfo.slot, relativeSlot);
                if (slot != null)
                {
                    if (placecardinfo.replace == "true" && slot.Card != null)
                    {
                        slot.Card.ExitBoard(0, new Vector3(0, 0, 0));
                    }
                    if (slot.Card == null)
                    {
                        Plugin.Log.LogInfo(CardLoader.GetCardByName(placecardinfo.card).name + " " + slot.Index);
                        yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName(placecardinfo.card), slot);
                    }
                }
            }

            yield return new WaitForSeconds(0.3f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            yield break;
        }

        public static CardSlot GetSlot(SlotData slotdata, CardSlot relativeSlot)
        {
            if (slotdata == null)
            {
                return null;
            }

            if (slotdata.index.Contains('|'))
            {
                List<CardSlot> randomiseableslots = new List<CardSlot>();
                foreach (int index in slotdata.index.Split('|').ToList().Select(int.Parse).ToList())
                {
                    int slotindex = index;
                    if (ConvertArgument(slotdata.isRelative) == "true")
                    {
                        slotindex += relativeSlot.Index;
                    }

                    if (slotindex < 0 || slotindex > 3)
                    {
                        continue;
                    }

                    CardSlot slot = null;
                    if (ConvertArgument(slotdata.isOpponentSlot) == "false")
                    {
                        slot = Singleton<BoardManager>.Instance.opponentSlots[slotindex];
                    }
                    else
                    {
                        slot = Singleton<BoardManager>.Instance.playerSlots[slotindex];
                    }
                    if (slot != null)
                    {
                        randomiseableslots.Add(slot);
                    }
                }
                if (randomiseableslots.Count > 0)
                {
                    if (slotdata.removeEmptySlots == "true")
                    {
                        randomiseableslots.RemoveAll(item => item.Card == null);
                    }
                    if (slotdata.removeOccupiedSlots == "true")
                    {
                        randomiseableslots.RemoveAll(item => item.Card != null);
                    }
                    Random random = new Random();
                    return randomiseableslots[random.Next(randomiseableslots.Count)];
                }
                else
                {
                    return null;
                }
            }
            else
            {
                int slotindex = int.Parse(slotdata.index);
                if (ConvertArgument(slotdata.isRelative) == "true")
                {
                    slotindex += relativeSlot.Index;
                }

                if (slotindex < 0 || slotindex > 3)
                {
                    return null;
                }

                CardSlot slot = null;
                if (ConvertArgument(slotdata.isOpponentSlot) == "false")
                {
                    slot = Singleton<BoardManager>.Instance.opponentSlots[slotindex];
                }
                else
                {
                    slot = Singleton<BoardManager>.Instance.playerSlots[slotindex];
                }
                return slot;
            }
        }
    }
}
