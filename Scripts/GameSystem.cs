using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    public GameObject player;
    public GameObject npc;
    public GameObject enemy;
    public GameObject alertHUD;

    public string location;
    private GameObject goal;

    public FuzzyLogic logic = new FuzzyLogic();
    public double kill;
    public double killquest;
    public double numquest;
    public double difficulty;
    public double travel;
    public double explorequest;
    public double activity;
    public double interact;

    private void Start()
    {
        Load();
        location = SceneManager.GetActiveScene().name;
        goal = GameObject.FindWithTag("Goal");
    }
    private void Update()
    {
        TalkToNPC();
        logic.Characterize(logic.KillerFuzzy(kill, killquest), logic.AchieverFuzzy(numquest, difficulty), logic.ExplorerFuzzy(travel, explorequest), logic.SocializerFuzzy(activity, interact));
    }

    public void Save()
    {
        SaveLoadSystem.Save(player.GetComponent<Player>(), npc.GetComponent<ContentHandler>());
    }

    public void Load()
    {
        SaveData data = SaveLoadSystem.Load();

        player.GetComponent<Player>().HP = data.hp;
        player.GetComponent<Player>().acceptedQuest = data.acceptedQuest;
        player.GetComponent<Player>().finishedQuests = data.finishedQuests;
        player.GetComponent<Player>().acceptedActivity = data.acceptedActivity;
        player.GetComponent<Player>().finishedActivities = data.finishedActivities;
        player.GetComponent<Player>().monsterKills = data.monsterKills;

        npc.GetComponent<ContentHandler>().availableQuests = data.availableQuests;
        npc.GetComponent<ContentHandler>().availableActivities = data.availableActivities;

    }

    public void loadScene(string name)
    {
        if (SceneManager.GetActiveScene().name == name)
        {
            Alert("You are currently at " + name + "!");
        }
        else
        {
            Save();
            Alert("Moving to " + name + "...");
            SceneManager.LoadScene(name);
        }
    }


    public void TalkToNPC()
    {
        if (Vector3.Distance(player.transform.position, npc.transform.position) < 3.0f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                npc.GetComponent<ContentHandler>().dialogHUD.SetActive(true);
                player.GetComponent<Player>().IdleAnimation();
                PauseCharacter();
            }
        }
        else if (Vector3.Distance(player.transform.position, goal.transform.position) < 3.0f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                npc.GetComponent<ContentHandler>().dialogHUD.SetActive(true);
                player.GetComponent<Player>().IdleAnimation();
                PauseCharacter();
            }
        }
    }

    public void PauseCharacter()
    {
        player.GetComponent<PlayerController>().enabled = false;
    }

    public void UnPauseCharacter()
    {
        player.GetComponent<PlayerController>().enabled = true;
    }


    public void Alert(string message)
    {
        alertHUD.SetActive(true);
        alertHUD.GetComponent<Text>().text = message;
        StartCoroutine(WaitandClear());
    }
    IEnumerator WaitandClear()
    {
        yield return new WaitForSeconds(5);
        alertHUD.SetActive(false);
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StopGame()
    {
        Application.Quit();
    }
}
