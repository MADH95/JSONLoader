namespace JLPlugin.Data
{
    [System.Serializable]
    public class trigger
    {
        public string triggerType;
        public bool activatesForAlliedCards;
        public bool activatesForOpponentCards;
        public bool excludeCardBearingSigil;
    }
}