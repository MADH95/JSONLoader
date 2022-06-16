using DiskCardGame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLPlugin.Data
{
    public static class SacrificeHelper
    {
        public static IEnumerator ChooseSacrificesForCard(this BoardManager self, List<CardSlot> validSlots, PlayableCard card, int requiredSacrifices, List<CardSlot> sacrificedSlots = null, List<CardInfo> sacrificedCards = null, bool killVictims = true)
        {
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
            Singleton<ViewManager>.Instance.SwitchToView(self.BoardView, false, false);
            Singleton<InteractionCursor>.Instance.ForceCursorType(CursorType.Sacrifice);
            self.cancelledPlacementWithInput = false;
            self.currentValidSlots = validSlots;
            self.currentSacrificeDemandingCard = card;
            self.CancelledSacrifice = false;
            self.LastSacrificesInfo.Clear();
            self.SetQueueSlotsEnabled(false);
            foreach (CardSlot slot in self.AllSlots)
            {
                bool flag = !slot.IsPlayerSlot || slot.Card == null;
                if (flag)
                {
                    slot.SetEnabled(false);
                    slot.ShowState(HighlightedInteractable.State.NonInteractable, false, 0.15f);
                }
                bool flag2 = slot.IsPlayerSlot && slot.Card != null && slot.Card.CanBeSacrificed && validSlots.Contains(slot);
                if (flag2)
                {
                    slot.Card.Anim.SetShaking(true);
                }
            }
            yield return self.SetSacrificeMarkersShown(requiredSacrifices);
            while (self.GetValueOfSacrifices(self.currentSacrifices) < requiredSacrifices && !self.cancelledPlacementWithInput)
            {
                self.SetSacrificeMarkersValue(self.currentSacrifices.Count);
                yield return new WaitForEndOfFrame();
            }
            foreach (CardSlot slot2 in self.AllSlots)
            {
                slot2.SetEnabled(false);
                bool flag3 = slot2.IsPlayerSlot && slot2.Card != null;
                if (flag3)
                {
                    slot2.Card.Anim.SetShaking(false);
                }
            }
            foreach (CardSlot s in self.currentSacrifices)
            {
                self.LastSacrificesInfo.Add(s.Card.Info);
            }
            bool cancelledDueToNoSpaceForCard = !self.SacrificesCreateRoomForCard(card, self.currentSacrifices) && !card.OnBoard;
            bool flag4 = self.cancelledPlacementWithInput || cancelledDueToNoSpaceForCard;
            if (flag4)
            {
                self.HideSacrificeMarkers();
                bool flag5 = cancelledDueToNoSpaceForCard;
                if (flag5)
                {
                    yield return new WaitForSeconds(0.25f);
                }
                foreach (CardSlot s2 in self.GetSlots(true))
                {
                    bool flag6 = s2.Card != null;
                    if (flag6)
                    {
                        s2.Card.Anim.SetSacrificeHoverMarkerShown(false);
                        bool flag7 = self.currentSacrifices.Contains(s2);
                        if (flag7)
                        {
                            s2.Card.Anim.SetMarkedForSacrifice(false);
                        }
                    }
                }
                Singleton<ViewManager>.Instance.SwitchToView(self.defaultView, false, false);
                Singleton<InteractionCursor>.Instance.ClearForcedCursorType();
                self.CancelledSacrifice = true;
            }
            else
            {
                self.SetSacrificeMarkersValue(self.GetValueOfSacrifices(self.currentSacrifices));
                yield return new WaitForSeconds(0.2f);
                self.HideSacrificeMarkers();
                foreach (CardSlot s3 in self.currentSacrifices)
                {
                    bool flag8 = s3.Card != null && !s3.Card.Dead;
                    if (flag8)
                    {
                        if (killVictims)
                        {
                            int sacrificesMadeThisTurn = self.SacrificesMadeThisTurn;
                            self.SacrificesMadeThisTurn = sacrificesMadeThisTurn + 1;
                            yield return s3.Card.Sacrifice();
                            Singleton<ViewManager>.Instance.SwitchToView(self.BoardView, false, false);
                        }
                        else
                        {
                            s3.Card.FakeOutSacrifice();
                            Singleton<ViewManager>.Instance.SwitchToView(self.BoardView, false, false);
                        }
                    }
                }
            }
            self.SetQueueSlotsEnabled(true);
            foreach (CardSlot slot3 in self.AllSlots)
            {
                slot3.SetEnabled(true);
                slot3.ShowState(HighlightedInteractable.State.Interactable, false, 0.15f);
            }
            self.currentSacrificeDemandingCard = null;
            if (sacrificedSlots != null && !self.CancelledSacrifice)
            {
                sacrificedSlots.AddRange(self.currentSacrifices);
            }
            if (sacrificedCards != null && !self.CancelledSacrifice)
            {
                sacrificedCards.AddRange(self.LastSacrificesInfo);
            }
            self.currentSacrifices.Clear();
            Singleton<InteractionCursor>.Instance.ClearForcedCursorType();
            yield break;
        }

        public static void FakeOutSacrifice(this PlayableCard card)
        {
            card.Anim.PlaySacrificeSound();
            card.Anim.SetSacrificeHoverMarkerShown(false);
            card.Anim.SetMarkedForSacrifice(false);
            card.Anim.PlaySacrificeParticles();
        }

        public static int AvailableSacrificeValueInSlots(this BoardManager self, List<CardSlot> slots)
        {
            return self.GetValueOfSacrifices(slots.FindAll((CardSlot x) => x.Card != null && x.Card.CanBeSacrificed));
        }
    }
}
