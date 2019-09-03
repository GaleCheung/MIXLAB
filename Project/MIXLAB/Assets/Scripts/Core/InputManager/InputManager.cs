using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputAction
{
    public string id;
    public string tag;
    public Action action;

    public InputAction(string id, Action action, string tag = "default")
    {
        this.id = id;
        this.action = action;
        this.tag = tag;
    }
}

public class InputManager : SingletonMB<InputManager>
{
    private Dictionary<KeyCode, Dictionary<string, InputAction>> clickDict =
        new Dictionary<KeyCode, Dictionary<string, InputAction>>();

    private Dictionary<KeyCode, Dictionary<string, InputAction>> pressDict =
        new Dictionary<KeyCode, Dictionary<string, InputAction>>();

    private HashSet<string> ignoreTagHashSet = new HashSet<string>();
    private HashSet<string> ignoreIdHashSet = new HashSet<string>();

    private bool ignoreInput = false;

    void Update()
    {
        if (ignoreInput)
        {
            return;
        }
        //点击
        foreach (KeyValuePair<KeyCode, Dictionary<string, InputAction>> kvp in clickDict)
        {
            KeyCode keyStr = kvp.Key;
            Dictionary<string, InputAction> inputActions = kvp.Value;
            if (Input.GetKeyDown(keyStr))
            {
                foreach (InputAction value in inputActions.Values)
                {
                    string id = value.id;
                    string tag = value.tag;
                    Action action = value.action;
                    if (!ignoreTagHashSet.Contains(tag) && !ignoreIdHashSet.Contains(id))
                    {
                        action();
                    }
                }
            }
        }
        //长按
        foreach (KeyValuePair<KeyCode, Dictionary<string, InputAction>> kvp in pressDict)
        {
            KeyCode keyStr = kvp.Key;
            Dictionary<string, InputAction> inputActions = kvp.Value;
            if (Input.GetKey(keyStr))
            {
                foreach (InputAction value in inputActions.Values)
                {
                    string id = value.id;
                    string tag = value.tag;
                    Action action = value.action;
                    if (!ignoreTagHashSet.Contains(tag) && !ignoreIdHashSet.Contains(id))
                    {
                        action();
                    }
                }
            }
        }
    }

    public void SetIgnoreInput(bool state)
    {
        ignoreInput = state;
    }

    public void AddIgnoreId(string id)
    {
        ignoreIdHashSet.Add(id);
    }

    public void RemoveIgnoreId(string id)
    {
        ignoreIdHashSet.Remove(id);
    }

    public void AddClickAction(KeyCode keyStr, string id, string tag, Action action)
    {
        InputAction inputAction = new InputAction(id, action, tag);
        if (!clickDict.ContainsKey(keyStr))
        {
            Dictionary<string, InputAction> actionDict = new Dictionary<string, InputAction>();
            clickDict.Add(keyStr, actionDict);
        }

        clickDict[keyStr].Add(id, inputAction);
    }

    public void RemoveClickAction(string id)
    {
        foreach (KeyValuePair<KeyCode, Dictionary<string, InputAction>> kvp in clickDict)
        {
            Dictionary<string, InputAction> inputActions = kvp.Value;
            foreach (string k in inputActions.Keys)
            {
                if (k == id)
                    clickDict[kvp.Key].Remove(k);
            }
        }
    }
    
    public void RemoveClickAction(KeyCode keyStr)
    {
        clickDict.Remove(keyStr);
    }
    
    public void AddPressAction(KeyCode keyStr, string id, string tag, Action action)
    {
        InputAction inputAction = new InputAction(id, action, tag);
        if (!pressDict.ContainsKey(keyStr))
        {
            Dictionary<string, InputAction> actionDict = new Dictionary<string, InputAction>();
            pressDict.Add(keyStr, actionDict);
        }

        pressDict[keyStr].Add(id, inputAction);
    }

    public void RemovePressAction(string id)
    {
        foreach (KeyValuePair<KeyCode, Dictionary<string, InputAction>> kvp in pressDict)
        {
            Dictionary<string, InputAction> inputActions = kvp.Value;
            foreach (string k in inputActions.Keys)
            {
                if (k == id)
                    pressDict[kvp.Key].Remove(k);
            }
        }
    }
    
    public void RemovePressAction(KeyCode keyStr)
    {
        pressDict.Remove(keyStr);
    }
}