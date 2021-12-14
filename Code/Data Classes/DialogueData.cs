using System.Collections.Generic;

namespace JLPlugin.Data
{
    public class DialogueData
    {
        public string id;
        public List<DialogueLineData> mainLines;
        public List<List<DialogueLineData>> repeatLines;
    }
}