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

    public Text title;
    public Text desc;
    public Text reward;
    public GameObject alert;

    public bool isComplete;
    private int count;

    void Start()
    {
        count = 0;
    }

    void Update()
    {
        if(isComplete)
        {
            Complete();
            isComplete = false;
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
            Alert(quest.questTitle + " has been accepted");
        }
        else
        {
            Alert("Please finish your current quest!");
        }
    }

    private void Alert(string message)
    {
        alert.SetActive(true);
        alert.GetComponent<Text>().text = message;
        StartCoroutine(WaitandClear());
    }

    private void Complete()
    {
        player.acceptedQuest.isActive = false;
        alert.SetActive(true);
        alert.GetComponent<Text>().text = player.acceptedQuest.questTitle + " has been completed!";
        availableQuests.Add(player.acceptedQuest);
        player.finishedQuests.Add(player.acceptedQuest);
        player.ClearQuest();
       
        StartCoroutine(WaitandClear());
    }

    IEnumerator WaitandClear()
    {
        yield return new WaitForSeconds(5);
        alert.SetActive(false);
    }
}
