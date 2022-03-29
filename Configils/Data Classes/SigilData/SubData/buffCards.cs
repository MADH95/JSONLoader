using System.Collections.Generic;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class buffCards
    {
        public SlotData slot;
        public string stats;
        public List<string> abilities;
        public List<string> removeAbilities;
        public string self;
    }
}