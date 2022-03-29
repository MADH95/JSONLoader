// Using Inscryption
using DiskCardGame;
using JLPlugin.Data;
// Modding Inscryption
using System.Collections;


namespace JLPlugin.SigilCode
{
    public class ConfigurableMain : ConfigurableBase
    {
        public Ability sigilOverride;

        public override Ability Ability
        {
            get
            {
                return sigilOverride;
            }
        }

        public override bool RespondsToResolveOnBoard()
        {
            return SigilData.GetAbilityArguments(base.ability).trigger.triggerType == "OnResolveOnBoard";
        }

        // Token: 0x06001301 RID: 4865 RVA: 0x000433D2 File Offset: 0x000415D2
        public override IEnumerator OnResolveOnBoard()
        {
            SigilData.RunActions(ability, base.Card.Slot, base.Card);
            yield break;
        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return SigilData.GetAbilityArguments(base.ability).trigger.triggerType == "OnStartOfTurn" || SigilData.GetAbilityArguments(base.ability).trigger.triggerType == "OnEndOfTurn";
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            if (base.Card.OpponentCard != playerUpkeep && SigilData.GetAbilityArguments(base.ability).trigger.triggerType == "OnStartOfTurn")
            {
                SigilData.RunActions(ability, base.Card.Slot, base.Card);
            }
            if (base.Card.OpponentCard == playerUpkeep && SigilData.GetAbilityArguments(base.ability).trigger.triggerType == "OnEndOfTurn")
            {
                SigilData.RunActions(ability, base.Card.Slot, base.Card);
            }
            yield break;
        }

        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            return target == base.Card && SigilData.GetAbilityArguments(base.ability).trigger.triggerType == "OnStruck";
        }

        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            SigilData.RunActions(ability, base.Card.Slot, base.Card);
            yield break;
        }

        public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
        {
            return SigilData.GetAbilityArguments(base.ability).trigger.triggerType == "OnDie";
        }

        public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
        {
            SigilData.RunActions(ability, base.Card.Slot, base.Card);
            yield break;
        }

        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            return SigilData.GetAbilityArguments(base.ability).trigger.triggerType == "OnAttack";
        }

        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            SigilData.RunActions(ability, base.Card.Slot, base.Card);
            yield break;
        }

        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return fromCombat && base.Card == killer && base.Card.slot.IsPlayerSlot && SigilData.GetAbilityArguments(base.ability).trigger.triggerType == "OnKill";
        }


        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            SigilData.RunActions(ability, base.Card.Slot, base.Card);
            yield break;
        }
    }
}