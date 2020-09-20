using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Quest
{
    public bool isActive;
    public string questTitle;
    public Type questType;
    public string questDescription;
    public int questReward;

    public float goal;
    public float current;

    public enum Type
    {
        Killing,
        Activity,
        Exploring,
        Gathering
    }

    public bool IsReached()
    {
        if (current >= goal)
        {
            current = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Progress()
    {
        if (questType == Type.Killing)
        {
            current++;
        }
        if (questType == Type.Gathering)
        {
            current++;
        }
        if (questType == Type.Exploring)
        {
            current++;
        }
        if (questType == Type.Activity)
        {
            current++;
        }
    }

 


}
