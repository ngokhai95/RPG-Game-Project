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
    public Location questLocation;
    public string questDescription;
    public int questDifficulty;

    public float goal;
    public float current;

    public enum Type
    {
        Killing,
        Exploring
    }

    public enum Location
    {
        Mountain,
        Forest,
        Desert,
        River
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
        if (questType == Type.Exploring)
        {
            current++;
        }
    }

    public void Clear()
    {
        isActive = false;
        questTitle = "";
        questType = 0;
        questLocation = 0;
        questDescription = "";
        questDifficulty = 0;

        goal = 0;
        current = 0;
    }
}
