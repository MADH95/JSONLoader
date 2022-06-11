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

        public IEnumerator Strafe(AbilityBehaviourData abilityData, moveCards movecardinfo)
        {
            StrafeType strafeType = ParseEnum<StrafeType>(movecardinfo.strafe.direction);
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

            CardSlot CardSlotLeft = Singleton<BoardManager>.Instance.GetAdjacent(abilityData.self.slot, true);
            CardSlot CardSlotRight = Singleton<BoardManager>.Instance.GetAdjacent(abilityData.self.Slot, false);
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
            yield return this.MoveToSlot(abilityData, movecardinfo, destination, destinationValid);
            yield break;
        }

        // Token: 0x0600159E RID: 5534 RVA: 0x00049972 File Offset: 0x00047B72
        protected IEnumerator MoveToSlot(AbilityBehaviourData abilityData, moveCards movecardinfo, CardSlot destination, bool destinationValid)
        {
            if ((SigilData.ConvertArgument(movecardinfo.strafe.flipSigil, abilityData) ?? "true") == "true")
            {
                abilityData.self.RenderInfo.SetAbilityFlipped(abilityData.ability, this.movingLeft);
            }
            abilityData.self.RenderInfo.flippedPortrait = (this.movingLeft && abilityData.self.Info.flipPortraitForStrafe);
            abilityData.self.RenderCard();
            if (destination != null && destinationValid)
            {
                //If issues arise with the OnMove trigger then we could always add a variable here
                //to make sigils like squirrel strafe possible
                //CardSlot oldSlot = abilityData.self.Slot;

                yield return Singleton<BoardManager>.Instance.AssignCardToSlot(abilityData.self, destination, 0.1f, null, true);
                yield return new WaitForSeconds(0.25f);
            }
            else
            {
                abilityData.self.Anim.StrongNegationEffect();
                yield return new WaitForSeconds(0.15f);
            }
            yield break;
        }
    }
}