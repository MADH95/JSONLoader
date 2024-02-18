using DiskCardGame;
using System.Collections;
using UnityEngine;
using static JLPlugin.V2.Data.CardSerializeInfo;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class strafeData
    {
        public string direction;
        public string flipSigil;
        public bool movingLeft;

        public enum StrafeType
        {
            normal,
            left,
            right
        }

        public IEnumerator Strafe(AbilityBehaviourData abilityData, moveCards movecardinfo, CardSlot SlotToMove)
        {
            StrafeType strafeType;
            if (string.IsNullOrWhiteSpace(movecardinfo.strafe.direction))
            {
                strafeType = StrafeType.right;
            }
            else
            {
                strafeType = ImportExportUtils.ParseEnum<StrafeType>(AConfigilData.ConvertArgument(movecardinfo.strafe.direction, abilityData));
            }

            switch (strafeType)
            {
                case StrafeType.normal:
                    break;
                case StrafeType.left:
                    movingLeft = true;
                    break;
                case StrafeType.right:
                    movingLeft = false;
                    break;
                default:
                    yield break;
            }

            CardSlot CardSlotLeft = Singleton<BoardManager>.Instance.GetAdjacent(SlotToMove, true);
            CardSlot CardSlotRight = Singleton<BoardManager>.Instance.GetAdjacent(SlotToMove, false);
            bool LeftSlotEmpty = CardSlotLeft != null && CardSlotLeft.Card == null;
            bool RightSlotEmpty = CardSlotRight != null && CardSlotRight.Card == null;
            if (this.movingLeft && !LeftSlotEmpty)
            {
                this.movingLeft = false;
            }
            if (!this.movingLeft && !RightSlotEmpty)
            {
                this.movingLeft = true;
            }
            CardSlot destination = this.movingLeft ? CardSlotLeft : CardSlotRight;
            bool destinationValid = this.movingLeft ? LeftSlotEmpty : RightSlotEmpty;
            yield return this.MoveToSlot(abilityData, movecardinfo, destination, destinationValid, SlotToMove);
            yield break;
        }

        // Token: 0x0600159E RID: 5534 RVA: 0x00049972 File Offset: 0x00047B72
        public IEnumerator MoveToSlot(AbilityBehaviourData abilityData, moveCards movecardinfo, CardSlot destination, bool destinationValid, CardSlot SlotToMove)
        {
            if ((AConfigilData.ConvertArgument(movecardinfo.strafe.flipSigil, abilityData) ?? "true") == "true" && abilityData.ability != null)
            {
                SlotToMove.Card.RenderInfo.SetAbilityFlipped((Ability)abilityData.ability, this.movingLeft);
            }
            SlotToMove.Card.RenderInfo.flippedPortrait = (this.movingLeft && SlotToMove.Card.Info.flipPortraitForStrafe);
            SlotToMove.Card.RenderCard();
            if (destination != null && destinationValid)
            {
                //If issues arise with the OnMove trigger then we could always add a variable here
                //to make sigils like squirrel strafe possible
                //CardSlot oldSlot = SlotToMove.Card.Slot;

                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(SlotToMove.Card, destination, 0.1f, null, true);
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                SlotToMove.Card.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.15f);
            }
            yield break;
        }
    }
}