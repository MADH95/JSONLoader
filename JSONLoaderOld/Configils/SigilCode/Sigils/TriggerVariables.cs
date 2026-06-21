using System;
using System.Collections.Generic;

public class TriggerVariables : Dictionary<string, object>
{
    public TriggerVariables(string key, object value)
    {
        Add(key, value);
    }
    
    public TriggerVariables(string key, object value, string key2, object value2)
    {
        Add(key, value);
        Add(key2, value2);
    }
    
    public TriggerVariables(string key, object value, string key2, object value2, string key3, object value3)
    {
        Add(key, value);
        Add(key2, value2);
        Add(key3, value3);
    }

    public static implicit operator TriggerVariables((string, object) a)
    {
        return new TriggerVariables(a.Item1, a.Item2);
    }

    public static implicit operator TriggerVariables((string, object, string, object) a)
    {
        return new TriggerVariables(a.Item1, a.Item2, a.Item3, a.Item4);
    }

    public static implicit operator TriggerVariables((string, object, string, object, string, object) a)
    {
        return new TriggerVariables(a.Item1, a.Item2, a.Item3, a.Item4, a.Item5, a.Item6);
    }
}