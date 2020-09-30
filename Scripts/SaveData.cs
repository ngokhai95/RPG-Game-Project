using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float hp;
    public Quest acceptedQuest;
    public List<Quest> finishedQuests;
    public float monsterKills;

    public SaveData(Player player)
    {
        hp = player.HP;
        acceptedQuest = player.acceptedQuest;
        finishedQuests = player.finishedQuests;
        monsterKills = player.monsterKills;
    }
}
