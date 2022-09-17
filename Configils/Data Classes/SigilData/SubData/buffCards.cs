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
        public List<string> removeAbilities;

        public static IEnumerator BuffCards(AbilityBehaviourData abilitydata)
        {
            foreach (buffCards buffcardsinfo in abilitydata.buffCards)
            {
                if (SigilData.ConvertArgument(buffcardsinfo.runOnCondition, abilitydata) == "false")
                {
                    continue;
                }

                yield return new WaitForSeconds(0.3f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);

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
                    if (buffcardsinfo.targetCard != null)
                    {
                        if (Regex.Matches(buffcardsinfo.targetCard, RegexStrings.Variable) is var variables
                        && variables.Cast<Match>().Any(variables => variables.Success))
                        {
                            card = (PlayableCard)Interpreter.ProcessGeneratedVariable(variables[0].Groups[1].Value, abilitydata);
                        }
                    }
                    else
                    {
                        card = abilitydata.self;
                    }
                }

                if (card != null)
                {
                    CardModificationInfo mod = new CardModificationInfo();
                    if (buffcardsinfo.heal != null)
                    {
                        if (card.Status.damageTaken > 0)
                        {
                            card.HealDamage(Math.Min(card.Status.damageTaken, int.Parse(SigilData.ConvertArgument(buffcardsinfo.heal, abilitydata))));
                        }
                    }
                    if (buffcardsinfo.addStats != null)
                    {
                        mod.attackAdjustment += int.Parse(SigilData.ConvertArgument(buffcardsinfo.addStats.Split('/')[0], abilitydata));
                        mod.healthAdjustment += int.Parse(SigilData.ConvertArgument(buffcardsinfo.addStats.Split('/')[1], abilitydata));
                    }
                    if (buffcardsinfo.setStats != null)
                    {
                        mod.attackAdjustment += int.Parse(SigilData.ConvertArgument(buffcardsinfo.setStats.Split('/')[0], abilitydata)) - card.Info.Attack;
                        mod.healthAdjustment += int.Parse(SigilData.ConvertArgument(buffcardsinfo.setStats.Split('/')[1], abilitydata)) - card.Info.Health;
                    }

                    if (buffcardsinfo.addAbilities != null || buffcardsinfo.removeAbilities != null)
                    {
                        yield return new WaitForSeconds(0.15f);
                        card.Anim.PlayTransformAnimation();
                        yield return new WaitForSeconds(0.15f);
                    }

                    if (buffcardsinfo.removeAbilities != null)
                    {
                        List<Ability> removeSigils = SigilData.ConvertArgument(buffcardsinfo.removeAbilities, abilitydata).Select(x => CardSerializeInfo.ParseEnum<Ability>(x)).ToList();

                        card.temporaryMods.ForEach(x => x.abilities = x.abilities.Except(removeSigils).ToList());
                        card.Status.hiddenAbilities.AddRange(removeSigils);
                        mod.negateAbilities.AddRange(removeSigils);
                    }
                    if (buffcardsinfo.addAbilities != null)
                    {
                        List<Ability> addSigils = SigilData.ConvertArgument(buffcardsinfo.addAbilities.Select(x => x.name).ToList(), abilitydata).Select(x => CardSerializeInfo.ParseEnum<Ability>(x)).ToList();

                        card.temporaryMods.ForEach(x => x.negateAbilities = x.negateAbilities.Except(addSigils).ToList());
                        card.Status.hiddenAbilities = card.Status.hiddenAbilities.Except(addSigils).ToList();
                        if (ConvertArgument(buffcardsinfo.addAbilities.Select(x => x.infused).ToList(), abilitydata).Any(x => x == "true"))
                        {
                            card.renderInfo.forceEmissivePortrait = true;
                            mod.fromCardMerge = true;
                        }
                        mod.abilities.AddRange(addSigils);
                    }
                    card.AddTemporaryMod(mod);
                    card.OnStatsChanged();
                    if (card.Health <= 0)
                    {
                        yield return card.Die(false);
                    }
                }
            }

            yield return new WaitForSeconds(0.3f);
            yield break;
        }
    }
}