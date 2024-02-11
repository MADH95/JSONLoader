using DiskCardGame;
using InscryptionAPI.Triggers;
using JLPlugin.Data;
using System.Collections;

namespace JLPlugin.SigilCode
{
    public class ConfigurableSpecial : SpecialCardBehaviour, IOnBellRung, IOnOtherCardAddedToHand, IOnCardAssignedToSlotContext, IOnCardDealtDamageDirectly
    {
        private ABaseConfigilLogic _logic = null;
        
        public void Initialize(SigilData abilityData, SpecialTriggeredAbility ability)
        {
            _logic = new ConfigilSpecialAbilityLogic(this, abilityData, ability);
        }

        public IEnumerator Start()
        {
            yield return _logic.Start();
        }

        public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
        {
            return _logic.RespondsToOtherCardResolve(otherCard);
        }

        public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
        {
            yield return _logic.OnOtherCardResolve(otherCard);
        }

        public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            return _logic.RespondsToOtherCardAssignedToSlot(otherCard);
        }

        // Token: 0x0600157B RID: 5499 RVA: 0x000497BE File Offset: 0x000479BE
        public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
        {
            yield return _logic.OnOtherCardAssignedToSlot(otherCard);
        }

        public override bool RespondsToTurnEnd(bool playerTurnEnd)
        {
            return _logic.RespondsToTurnEnd(playerTurnEnd);
        }

        public override IEnumerator OnTurnEnd(bool playerTurnEnd)
        {
            yield return _logic.OnTurnEnd(playerTurnEnd);
        }

        public override bool RespondsToUpkeep(bool playerUpkeep)
        {
            return _logic.RespondsToUpkeep(playerUpkeep);
        }

        public override IEnumerator OnUpkeep(bool playerUpkeep)
        {
            yield return _logic.OnUpkeep(playerUpkeep);
        }

        public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            return _logic.RespondsToOtherCardDealtDamage(attacker, amount, target);
        }

        public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
        {
            yield return _logic.OnOtherCardDealtDamage(attacker, amount, target);
        }

        public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
            PlayableCard killer)
        {
            return _logic.RespondsToOtherCardDie(card, deathSlot, fromCombat, killer);
        }

        public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat,
            PlayableCard killer)
        {
            yield return _logic.OnOtherCardDie(card, deathSlot, fromCombat, killer);
        }

        public override bool RespondsToSacrifice()
        {
            return _logic.RespondsToSacrifice();
        }

        // Token: 0x06001553 RID: 5459 RVA: 0x000494CF File Offset: 0x000476CF
        public override IEnumerator OnSacrifice()
        {
            yield return _logic.OnSacrifice();
        }

        public override bool RespondsToOtherCardPreDeath(CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            return _logic.RespondsToOtherCardPreDeath(deathSlot, fromCombat, killer);
        }

        // Token: 0x06001B49 RID: 6985 RVA: 0x0005A16A File Offset: 0x0005836A
        public override IEnumerator OnOtherCardPreDeath(CardSlot deathSlot, bool fromCombat, PlayableCard killer)
        {
            yield return _logic.OnOtherCardPreDeath(deathSlot, fromCombat, killer);
        }

        public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            return _logic.RespondsToSlotTargetedForAttack(slot, attacker);
        }

        public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
        {
            yield return _logic.OnSlotTargetedForAttack(slot, attacker);
        }

        public bool RespondsToBellRung(bool playerCombatPhase)
        {
            return _logic.RespondsToBellRung(playerCombatPhase);
        }

        public IEnumerator OnBellRung(bool playerCombatPhase)
        {
            yield return _logic.OnBellRung(playerCombatPhase);
        }

        public bool RespondsToOtherCardAddedToHand(PlayableCard card)
        {
            return _logic.RespondsToOtherCardAddedToHand(card);
        }

        public IEnumerator OnOtherCardAddedToHand(PlayableCard card)
        {
            yield return _logic.OnOtherCardAddedToHand(card);
        }

        public bool RespondsToCardAssignedToSlotContext(PlayableCard card, CardSlot oldSlot, CardSlot newSlot)
        {
            return _logic.RespondsToCardAssignedToSlotContext(card, oldSlot, newSlot);
        }

        public IEnumerator OnCardAssignedToSlotContext(PlayableCard card, CardSlot oldSlot, CardSlot newSlot)
        {
            yield return _logic.OnCardAssignedToSlotContext(card, oldSlot, newSlot);
        }

        public bool RespondsToCardDealtDamageDirectly(PlayableCard attacker, CardSlot opposingSlot, int damage)
        {
            return _logic.RespondsToCardDealtDamageDirectly(attacker, opposingSlot, damage);
        }

        public IEnumerator OnCardDealtDamageDirectly(PlayableCard attacker, CardSlot opposingSlot, int damage)
        {
            yield return _logic.OnCardDealtDamageDirectly(attacker, opposingSlot, damage);
        }
    }
}
