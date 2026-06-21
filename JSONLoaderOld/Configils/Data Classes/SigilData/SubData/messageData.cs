using DiskCardGame;
using System.Collections;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class messageData
    {
        public string runOnCondition;
        public string message;
        public string length;
        public string emotion;
        public string letterAnimation;
        public string speaker;

        public static IEnumerator showMessage(AbilityBehaviourData abilitydata)
        {
            messageData data = abilitydata.showMessage;

            if (AConfigilData.ConvertArgument(data.runOnCondition, abilitydata) == "false")
            {
                yield break;
            }

            yield return Singleton<TextDisplayer>.Instance.ShowThenClear(
                AConfigilData.ConvertArgument(data.message, abilitydata) ?? "",
                float.Parse(AConfigilData.ConvertArgument(data.length, abilitydata) ?? "2"),
                0,
                SigilDicts.Emotion[data.emotion ?? "Neutral"],
                SigilDicts.LetterAnimation[data.letterAnimation ?? "Jitter"],
                SigilDicts.Speaker[data.speaker ?? "Single"]
                );
            yield break;
        }
    }
}