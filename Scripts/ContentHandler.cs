using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using UnityEngine;
using UnityEngine.UI;

public class ContentHandler : MonoBehaviour
{
    public List<Quest> availableQuests = new List<Quest>();
    public List<Activity> availableActivities = new List<Activity>();
    public Player player;
    public GameObject contentHUD;
    public GameObject dialogHUD;
    public int numQuest, numActivity;
    public int questType, activityType;
    public int questDiff, activityDiff;

    GameSystem gameSystem;

    public Text title;
    public Text desc;
    public Text difficulty;

    public bool isComplete;

    private Quest quest = new Quest();
    private Activity activity = new Activity();
    private int choice;
    private int count;
    private List<string> titleList = new List<string>();

    void Start()
    {
        gameSystem = GameObject.FindWithTag("World").GetComponent<GameSystem>();
        count = 0;
        Initialize(); 
    }

    void Update()
    {
        CheckComplete();
    }

    private void Initialize()
    {
        titleList.Add("Let's Kill Some Dragons");
        titleList.Add("Let's Go Explore");
        titleList.Add("Let's Go Fishing");
        titleList.Add("Gathering Some Woods");
        if (availableQuests.Count == 0)
        {
            numQuest = Random.Range(1, 3);
            for (int i = 0; i < numQuest; i++)
            {
                AddQuest(0);
                AddQuest(1);
            }
        }
        if (availableActivities.Count == 0)
        {
            numActivity = Random.Range(1, 3);
            for (int i = 0; i < numActivity; i++)
            {
                AddActivity(0);
                AddActivity(1);
            }
        }
    }

    public void ChooseQuest()
    {
        choice = 0;
    }

    public void ChooseActivity()
    {
        choice = 1;
    }

    public void CheckProgress()
    {
        if (player.acceptedQuest.isActive && player.acceptedQuest.questLocation.ToString() == gameSystem.location)
        {
            player.acceptedQuest.Progress();
            gameSystem.Alert(player.acceptedQuest.questTitle + ": " + player.acceptedQuest.current + "/" + player.acceptedQuest.goal + "!");
        }
        
        if (player.acceptedActivity.isActive && player.acceptedActivity.activityLocation.ToString() == gameSystem.location)
        {
            player.acceptedActivity.Progress();
            gameSystem.Alert(player.acceptedActivity.activityTitle + ": " + player.acceptedActivity.current + "/" + player.acceptedActivity.goal + "!");
        }   
    }

    private void CheckComplete()
    {
        if (player.acceptedQuest.isActive && player.acceptedQuest.IsReached())
        {
            CompleteQuest();
        }

        if (player.acceptedActivity.isActive && player.acceptedActivity.IsReached())
        {
            CompleteActivity();
        }
    }

    public void CycleContent()
    {
        if (choice == 0)
        {
            if (count >= availableQuests.Count)
            {
                count = 0;
            }
            quest = availableQuests[count];
            count++;
        }
        if (choice == 1)
        {
            if (count >= availableActivities.Count)
            {
                count = 0;
            }
            activity = availableActivities[count];
            count++;
        }
    }

    public void UpdateContent()
    {
        if (choice == 0)
        {
            title.text = quest.questTitle;
            desc.text = quest.questDescription;
            difficulty.text = "Difficulty level: " + quest.questDifficulty.ToString();
        }
        if (choice == 1)
        {
            title.text = activity.activityTitle;
            desc.text = activity.activityDescription;
            difficulty.text = "Difficulty level: " + activity.activityDifficulty.ToString();
        }
    }

    public void CloseContent()
    {
        contentHUD.SetActive(false);
    }

    public void AcceptContent()
    {
        if (choice == 0)
        {
            contentHUD.SetActive(false);
            if (!player.acceptedQuest.isActive)
            {
                quest.isActive = true;
                player.acceptedQuest = quest;
                availableQuests.Remove(quest);
                gameSystem.Save();
            }
            else
            {
                gameSystem.Alert("Please finish your current quest!");
            }
        }
        if (choice == 1)
        {
            contentHUD.SetActive(false);
            if (!player.acceptedActivity.isActive)
            {
                activity.isActive = true;
                player.acceptedActivity = activity;
                availableActivities.Remove(activity);
                gameSystem.Save();
            }
            else
            {
                gameSystem.Alert("Please finish your current activity!");
            }
        }

    }

    //Quest

    private void AddQuest(int type)
    {
        quest = new Quest();
        switch (type)
        {
            case 0:
                GenerateKillingQuest(quest);
                break;
            case 1:
                GenerateExploringQuest(quest);
                break;
        }
    }
    private void GenerateKillingQuest(Quest quest)
    {
        quest.questTitle = titleList[0];
        quest.questType = Quest.Type.Killing;
        quest.questLocation = (Quest.Location)Random.Range(0, 3);
        quest.goal = Random.Range(2, 5);
        quest.questDescription = "We need you to go slay " + quest.goal + " Dragons at the " + quest.questLocation + "!";
        quest.questDifficulty = questDiff;
        availableQuests.Add(quest);
    }
    private void GenerateExploringQuest(Quest quest)
    {
        quest.questLocation = (Quest.Location)Random.Range(0, 3);
        quest.questTitle = titleList[1] + " " + quest.questLocation;
        quest.questType = Quest.Type.Exploring;
        quest.goal = 1;
        quest.questDescription = "There is something going on at the " + quest.questLocation + ", go explore it!";
        quest.questDifficulty = questDiff;
        availableQuests.Add(quest);
    }

    public void OpenQuest()
    {
        contentHUD.SetActive(true);
        count = 0;
        quest = availableQuests[count];
    }



    private void CompleteQuest()
    {
        player.acceptedQuest.isActive = false;
        gameSystem.Alert(player.acceptedQuest.questTitle + " has been completed!");
        player.finishedQuests.Add(player.acceptedQuest);
        player.ClearQuest();
        for (int i = 0; i < numQuest; i++)
        {
            AddQuest(questType);
        }
        gameSystem.Save();

    }
    
    //Activity

    private void AddActivity(int type)
    {
        activity = new Activity();
        switch (type)
        {
            case 0:
                GenerateAcivityFishing(activity);
                break;
            case 1:
                GenerateAcivityGathering(activity);
                break;
        }
    }

    private void GenerateAcivityFishing(Activity activity)
    {
        activity.activityTitle = titleList[2];
        activity.activityType = Activity.Type.Fishing;
        activity.activityLocation = Activity.Location.River;
        activity.goal = Random.Range(2, 5);
        activity.activityDescription = "We need some fishes to cook. Go catch " + activity.goal + " fishes at the " + activity.activityLocation + "!";
        activity.activityDifficulty = questDiff;
        availableActivities.Add(activity);
    }

    private void GenerateAcivityGathering(Activity activity)
    {
        activity.activityTitle = titleList[3];
        activity.activityType = Activity.Type.Gathering;
        activity.activityLocation = Activity.Location.Forest;
        activity.goal = Random.Range(2, 5);
        activity.activityDescription = "We need some woods to build. Go gather " + activity.goal + " woods at the " + activity.activityLocation + "!";
        activity.activityDifficulty = questDiff;
        availableActivities.Add(activity);
    }

    public void OpenActivity()
    {
        contentHUD.SetActive(true);
        count = 0;
        activity = availableActivities[count];
    }

    public void CompleteActivity()
    {
        player.acceptedActivity.isActive = false;
        gameSystem.Alert(player.acceptedActivity.activityTitle + " has been completed!");
        player.finishedActivities.Add(player.acceptedActivity);
        player.ClearActivity();
        for (int i = 0; i < numQuest; i++)
        {
            AddActivity(activityType);
        }
        gameSystem.Save();
    }


    public void CloseDialog()
    {
        dialogHUD.SetActive(false);
    }
}
