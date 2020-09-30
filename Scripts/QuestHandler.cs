using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestHandler : MonoBehaviour
{
    public List<Quest> availableQuests = new List<Quest>();
    public Player player;
    public GameObject questHUD;
    public GameObject dialogHUD;

    GameSystem gameSystem;

    public Text title;
    public Text desc;
    public Text reward;

    public bool isComplete;

    private Quest quest = new Quest();
    private int count;
    private List<string> titleList = new List<string>();

    void Start()
    {
        gameSystem = GameObject.FindWithTag("World").GetComponent<GameSystem>();
        count = 0;
        titleList.Add("Kill Dragons");
        titleList.Add("Explore");
        titleList.Add("Go Fishing");
        titleList.Add("Go Gathering Wood");
        for (int i = 0; i < 5; i++)
        {
            AddQuest();
        }
    }

    void Update()
    {
        if (player.acceptedQuest.isActive && player.acceptedQuest.IsReached())
        {
            Complete();
        }
    }

    public void AddQuest()
    {
        quest = new Quest();
        int randomType = Random.Range(0, 3);
        switch (randomType)
        {
            case 0:
                quest.questTitle = titleList[0];
                quest.questType = Quest.Type.Killing;
                quest.questLocation = (Quest.Location)Random.Range(0, 3);
                quest.goal = Random.Range(2, 5);
                quest.questDescription = "Go kill " + quest.goal + " Dragons at the " + quest.questLocation +"!";
                quest.questReward = 10;
                availableQuests.Add(quest);                  
                break;
            case 1:
                quest.questLocation = (Quest.Location)Random.Range(0, 3);
                quest.questTitle = titleList[1] + " " + quest.questLocation;
                quest.questType = Quest.Type.Exploring;          
                quest.goal = 1;
                quest.questDescription = "Go to this location at the " + quest.questLocation + "!";
                quest.questReward = 10;
                availableQuests.Add(quest);
                break;
            case 2:
                quest.questTitle = titleList[2];
                quest.questType = Quest.Type.Activity;
                quest.questLocation = Quest.Location.River;
                quest.goal = Random.Range(2, 5);
                quest.questDescription = "Go catch " + quest.goal + " Fishes at the " + quest.questLocation + "!";
                quest.questReward = 10;
                availableQuests.Add(quest);
                break;
            case 3:
                quest.questTitle = titleList[3];
                quest.questType = Quest.Type.Gathering;
                quest.questLocation = Quest.Location.Forest;
                quest.goal = Random.Range(2, 5);
                quest.questDescription = "Go gather " + quest.goal + " Woods at the " + quest.questLocation + "!";
                quest.questReward = 10;
                availableQuests.Add(quest);
                break;
        }
    }

    public void OpenQuest()
    {
        questHUD.SetActive(true);
        count = 0;
        quest = availableQuests[count];
    }

    public void UpdateQuest()
    {
        title.text = quest.questTitle;
        desc.text = quest.questDescription;
        reward.text = "Reward: " + quest.questReward.ToString() + " golds";
    }


    public void CycleQuest()
    {
        if(count >= availableQuests.Count)
        {
            count = 0;
        }
        quest = availableQuests[count];
        count++;
    }

    public void CloseDialog()
    {
        dialogHUD.SetActive(false);
    }

    public void CloseQuest()
    {
        questHUD.SetActive(false);
    }

    public void AcceptQuest()
    {
        questHUD.SetActive(false);
        if (!player.acceptedQuest.isActive)
        {
            quest.isActive = true;
            player.acceptedQuest = quest;
            availableQuests.Remove(quest);
            if (player.acceptedQuest.questType == Quest.Type.Exploring)
            {
                gameSystem.Alert("Quest has been accepted! Go to " + player.acceptedQuest.questLocation + " and press F1 to navigate!");
            }
            else
            {
                gameSystem.Alert(quest.questTitle + " has been accepted Go to " + player.acceptedQuest.questLocation + "to do it!");
            }
        }
        else
        {
            gameSystem.Alert("Please finish your current quest!");
        }
        gameSystem.Save();
    }

    

    private void Complete()
    {
        player.acceptedQuest.isActive = false;
        gameSystem.Alert(player.acceptedQuest.questTitle + " has been completed!");
        player.finishedQuests.Add(player.acceptedQuest);
        player.ClearQuest();
        AddQuest();
        gameSystem.Save();
    }

    
}
