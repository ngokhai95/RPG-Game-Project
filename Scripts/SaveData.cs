using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float hp;
    public Quest acceptedQuest;
    public Activity acceptedActivity;
    public List<Quest> availableQuests;
    public List<Activity> availableActivities;
    public List<Quest> finishedQuests;
    public List<Activity> finishedActivities;
    public float monsterKills;

    public SaveData(Player player, ContentHandler content)
    {
        hp = player.HP;
        acceptedQuest = player.acceptedQuest;
        acceptedActivity = player.acceptedActivity;
        finishedQuests = player.finishedQuests;
        finishedActivities = player.finishedActivities;
        monsterKills = player.monsterKills;
        availableQuests = content.availableQuests;
        availableActivities = content.availableActivities;
    }
}
