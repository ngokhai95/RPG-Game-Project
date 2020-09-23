using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestHandler : MonoBehaviour
{
    public List<Quest> availableQuests;
    private Quest quest;
    public Player player;
    public GameObject questHUD;
    public GameObject dialogHUD;

    GameSystem gameSystem;

    public Text title;
    public Text desc;
    public Text reward;

    public bool isComplete;
    private int count;

    void Start()
    {
        gameSystem = GameObject.FindWithTag("World").GetComponent<GameSystem>();
        count = 0;
    }

    void Update()
    {
        if (player.acceptedQuest.isActive && player.acceptedQuest.IsReached())
        {
            Complete();
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
            gameSystem.Alert(quest.questTitle + " has been accepted");
        }
        else
        {
            gameSystem.Alert("Please finish your current quest!");
        }
    }

    

    private void Complete()
    {
        player.acceptedQuest.isActive = false;
        gameSystem.Alert(player.acceptedQuest.questTitle + " has been completed!");
        availableQuests.Add(player.acceptedQuest);
        player.finishedQuests.Add(player.acceptedQuest);
        player.ClearQuest();
    }

    
}
