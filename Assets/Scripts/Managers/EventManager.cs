using System;
using System.Collections.Generic;

public static class EventManager
{
    private static readonly Dictionary<string, Action> events = new();

    public static void StartListening(string name, Action listener)
    {
        if (!events.ContainsKey(name)) events[name] = null;
        events[name] += listener;
    }

    public static void StopListening(string name, Action listener)
    {
        if (events.ContainsKey(name)) events[name] -= listener;
    }

    public static void TriggerEvent(string name)
    {
        if (events.ContainsKey(name)) events[name]?.Invoke();
    }

}
