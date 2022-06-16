
﻿using System.Collections.Generic;

namespace JLPlugin.Data
{
    [System.Serializable]
    public class activationCost
    {
        public int? bonesCost;
        public int? energyCost;

        public int? bloodCost;
        public List<string> gemCost;
    }
}