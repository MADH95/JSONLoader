using DiskCardGame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static JSONLoader.API.JSONLoaderAPI;
using ABAction = System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>>>;

namespace JLPlugin.Data
{
    public class customActions
    {
        public static IEnumerator runCustomActions(AbilityBehaviourData abilitydata)
        {
            if (abilitydata.customActions == null)
            {
                yield break;
            }

            foreach (ConfigilAction action in customActionList)
            {
                ABAction abaction = abilitydata.customActions.FirstOrDefault(x => x.Key == action.actionName);

                if (!abaction.Equals(default(ABAction)))
                {
                    foreach (Dictionary<string, string> abfields in abaction.Value)
                    {
                        if (!action.fields.Contains("runOnCondition") &&
                            abfields.Keys.Contains("runOnCondition") &&
                            SigilData.ConvertArgument(abfields["runOnCondition"], abilitydata) == "false")
                        {
                            continue;
                        }

                        Dictionary<string, string> SetFields = new Dictionary<string, string>();
                        foreach (string field in action.fields)
                        {
                            if (abfields.Keys.Contains(field))
                            {
                                SetFields[field] = SigilData.ConvertArgument(abfields[field], abilitydata);
                            }
                            else
                            {
                                SetFields[field] = null;
                            }
                        }

                        action.functionToCall(SetFields);
                    }
                }
            }
            yield break;
        }
    }
}