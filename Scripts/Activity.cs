using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Activity
{
    public bool isActive;
    public string activityTitle;
    public Type activityType;
    public Location activityLocation;
    public string activityDescription;
    public int activityDifficulty;

    public float goal;
    public float current;

    public enum Location
    {
        Mountain,
        Forest,
        Desert,
        River
    }

    public enum Type
    {
        Gathering,
        Fishing
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
        if (activityType == Type.Gathering)
        {
            current++;
        }
        if (activityType == Type.Fishing)
        {
            current++;
        }
    }

    public void Clear()
    {
        isActive = false;
        activityTitle = "";
        activityType = 0;
        activityLocation = 0;
        activityDescription = "";
        activityDifficulty = 0;

        goal = 0;
        current = 0;
    }
}
