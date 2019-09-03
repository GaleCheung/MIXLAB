using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent
{
    private ushort id;
    private Dictionary<string, object> parm = new Dictionary<string, object>();

    public GameEvent(EventDefine.EventId eventId)
    {
        id = (ushort) eventId;
    }

    public ushort GetId()
    {
        return id;
    }

    public void SetEventParm(string key, object value)
    {
        parm.Add(key, value);
    }

    public T GetEventParm<T>(string key)
    {
        return parm.GetValueWithType<T>(key);
    }
}

public class EventManager : Singleton<EventManager>
{
    private Dictionary<ushort, List<Action<GameEvent>>> eventDictionary =
        new Dictionary<ushort, List<Action<GameEvent>>>();

    public void AddEventListener(EventDefine.EventId eventId, Action<GameEvent> act)
    {
        ushort id = (ushort) eventId;
        if (!eventDictionary.ContainsKey(id))
        {
            var list = new List<Action<GameEvent>>();
            eventDictionary.Add(id, list);
        }

        var actList = eventDictionary[id];
        if (!actList.Contains(act))
        {
            actList.Add(act);
        }
        else
        {
            Debug.LogWarning("重复添加监听");
        }
    }

    public void RemoveEventListener(EventDefine.EventId eventId, Action<GameEvent> act)
    {
        ushort id = (ushort) eventId;
        if (!eventDictionary.ContainsKey(id))
        {
            Debug.LogWarning("尝试移除不存在的监听");
            return;
        }

        var list = eventDictionary[id];
        if (!list.Contains(act))
        {
            Debug.LogWarning("尝试移除不存在的监听");
        }
        else
        {
            list.Remove(act);
            if (list.Count == 0)
            {
                eventDictionary.Remove(id);
            }
        }
    }

    public void SendEvent(EventDefine.EventId eventId, GameEvent gameEvent)
    {
        ushort id = (ushort) eventId;
        if (!eventDictionary.ContainsKey(id))
        {
            Debug.LogWarning($"没有监听 id:{id.ToString()}");
            return;
        }

        var actList = eventDictionary[id];

        for (int i = 0; i < actList.Count; i++)
        {
            actList[i](gameEvent);
        }
    }
}