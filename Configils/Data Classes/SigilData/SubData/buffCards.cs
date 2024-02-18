using DiskCardGame;
using JLPlugin.V2.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using static JLPlugin.Data.SigilData;
using static JLPlugin.Interpreter;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class buffCards
    {
        public string runOnCondition;
        public string targetCard;
        public slotData slot;
        public string addStats;
        public string setStats;
        public string heal;
        public List<addAbilityData> addAbilities;
        public List<removeAbilityData> removeAbilities;
        public string isPermanent;

        public static IEnumerator BuffCards(AbilityBehaviourData abilitydata)
        {
            foreach (buffCards buffcardsinfo in abilitydata.buffCards)
            {
                if (AConfigilData.ConvertArgument(buffcardsinfo.runOnCondition, abilitydata) == "false")
                {
                    continue;
                }

                PlayableCard card = GetCard(abilitydata, buffcardsinfo);
                if (card == null)
                    continue;

                bool CardIsInHand = Singleton<PlayerHand>.Instance.CardsInHand.Contains(card);
                Singleton<ViewManager>.Instance.SwitchToView(CardIsInHand ? View.Hand : View.Board, false, false);

                bool isPermanent = AConfigilData.ConvertArgument(buffcardsinfo.isPermanent, abilitydata) == "true";
                CardModificationInfo mod = ConfigilUtils.GetModById(card, "ConfigilMod", isPermanent);

                Heal(abilitydata, buffcardsinfo, card);
                AddStats(abilitydata, buffcardsinfo, mod);
                SetStats(abilitydata, buffcardsinfo, mod, card);

                if (buffcardsinfo.addAbilities != null || buffcardsinfo.removeAbilities != null)
                    yield return PlayTransformAnimation(card);

                RemoveAbilities(abilitydata, buffcardsinfo, card, mod);
                AddAbilities(abilitydata, buffcardsinfo, card, mod, isPermanent);

                card.OnStatsChanged();
                if (card.Health <= 0)
                {
                    yield return card.Die(false);
                }
            }


            // yield return new WaitForSeconds(0.3f);
            yield break;
        }

        private static void AddAbilities(AbilityBehaviourData abilitydata, buffCards buffcardsinfo, PlayableCard card, CardModificationInfo mod, bool isPermanent)
        {
            if (buffcardsinfo.addAbilities == null)
                return;

            List<Ability> addSigils = new List<Ability>();
            List<Ability> addMergedSigils = new List<Ability>();
            foreach (addAbilityData sigilData in buffcardsinfo.addAbilities)
            {
                List<Ability> sigils = new List<Ability>();
                if (!string.IsNullOrWhiteSpace(sigilData.name))
                {
                    sigils.Add(ImportExportUtils.ParseEnum<Ability>(AConfigilData.ConvertArgument(sigilData.name, abilitydata)));
                }
                if (!string.IsNullOrWhiteSpace(sigilData.list))
                {
                    sigils.AddRange((List<Ability>)AConfigilData.ConvertArgumentToType(sigilData.list, abilitydata, typeof(List<Ability>)));
                }

                foreach (Ability sigil in sigils)
                {
                    if (mod.negateAbilities.Contains(sigil))
                    {
                        mod.negateAbilities.Remove(sigil);
                        card.Status.hiddenAbilities.Remove(sigil);
                    }
                    else
                    {
                        if (AConfigilData.ConvertArgument(sigilData.infused, abilitydata) == "true")
                        {
                            addMergedSigils.Add(sigil);
                        }
                        else
                        {
                            addSigils.Add(sigil);
                        }
                    }
                }
            }

            if (addSigils.Count > 0)
            {
                mod.abilities.AddRange(addSigils);
            }

            if (addMergedSigils.Count > 0)
            {
                CardModificationInfo mergedMod = ConfigilUtils.GetModById(card, "ConfigilMergedMod", isPermanent);

                card.renderInfo.forceEmissivePortrait = true;
                mergedMod.abilities.AddRange(addMergedSigils);
            }
        }

        private static void RemoveAbilities(AbilityBehaviourData abilitydata, buffCards buffcardsinfo, PlayableCard card,
            CardModificationInfo mod)
        {
            if (buffcardsinfo.removeAbilities == null)
                return;

            foreach (removeAbilityData sigilData in buffcardsinfo.removeAbilities)
            {
                List<Ability> sigils = new List<Ability>();
                if (!string.IsNullOrWhiteSpace(sigilData.name))
                {
                    sigils.Add(ImportExportUtils.ParseEnum<Ability>(AConfigilData.ConvertArgument(sigilData.name, abilitydata)));
                }
                if (!string.IsNullOrWhiteSpace(sigilData.list))
                {
                    sigils.AddRange((List<Ability>)AConfigilData.ConvertArgumentToType(sigilData.list, abilitydata, typeof(List<Ability>)));
                }

                if (AConfigilData.ConvertArgument(sigilData.all, abilitydata) == "true")
                {
                    //CardMods.ForEach(x => x.abilities = x.abilities.Except(sigils).ToList());
                    mod.abilities.RemoveAll(x => sigils.Contains(x));
                    card.Status.hiddenAbilities.AddRange(sigils);
                    mod.negateAbilities.AddRange(sigils);
                }
                else
                {
                    foreach (Ability sigil in sigils)
                    {
                        if (mod.abilities.Contains(sigil))
                        {
                            mod.abilities.Remove(sigil);
                        }
                        else
                        {
                            card.Status.hiddenAbilities.AddRange(sigils);
                            mod.negateAbilities.AddRange(sigils);
                        }
                    }
                }
            }
        }

        private static IEnumerator PlayTransformAnimation(PlayableCard card)
        {
            yield return new WaitForSeconds(0.15f);
            card.Anim.PlayTransformAnimation();
            yield return new WaitForSeconds(0.15f);
        }

        private static void SetStats(AbilityBehaviourData abilitydata, buffCards buffcardsinfo, CardModificationInfo mod,
            PlayableCard card)
        {
            if (buffcardsinfo.setStats == null)
                return;

            string attackAdjustment = AConfigilData.ConvertArgument(buffcardsinfo.setStats.Split('/')[0], abilitydata);
            if (attackAdjustment != "?" && attackAdjustment != null)
            {
                mod.attackAdjustment += int.Parse(attackAdjustment) - card.Info.Attack;
            }

            string healthAdjustment = AConfigilData.ConvertArgument(buffcardsinfo.setStats.Split('/')[1], abilitydata);
            if (healthAdjustment != "?" && healthAdjustment != null)
            {
                mod.healthAdjustment += int.Parse(healthAdjustment) - card.Info.Health;
            }
        }

        private static void AddStats(AbilityBehaviourData abilitydata, buffCards buffcardsinfo, CardModificationInfo mod)
        {
            if (buffcardsinfo.addStats == null)
                return;

            string attackAdjustment = AConfigilData.ConvertArgument(buffcardsinfo.addStats.Split('/')[0], abilitydata);
            if (attackAdjustment != "?" && attackAdjustment != null)
            {
                mod.attackAdjustment += int.Parse(attackAdjustment);
            }

            string healthAdjustment = AConfigilData.ConvertArgument(buffcardsinfo.addStats.Split('/')[1], abilitydata);
            if (healthAdjustment != "?" && healthAdjustment != null)
            {
                mod.healthAdjustment += int.Parse(healthAdjustment);
            }
        }

        private static void Heal(AbilityBehaviourData abilitydata, buffCards buffcardsinfo, PlayableCard card)
        {
            if (string.IsNullOrWhiteSpace(buffcardsinfo.heal)) return;
            if (card.Status.damageTaken <= 0) return;

            card.HealDamage(Math.Min(card.Status.damageTaken,
                int.Parse(AConfigilData.ConvertArgument(buffcardsinfo.heal, abilitydata))));
        }

        public static PlayableCard GetCard(AbilityBehaviourData abilitydata, buffCards buffcardsinfo)
        {
            PlayableCard card = null;
            if (buffcardsinfo.slot != null)
            {
                CardSlot slot = slotData.GetSlot(buffcardsinfo.slot, abilitydata);
                if (slot != null)
                {
                    if (slot.Card != null)
                    {
                        card = slot.Card;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(buffcardsinfo.targetCard))
                {
                    card = (PlayableCard)AConfigilData.ConvertArgumentToType(buffcardsinfo.targetCard, abilitydata, typeof(PlayableCard));
                }
                else
                {
                    card = abilitydata.self;
                }
            }
            return card;
        }
    }
}
