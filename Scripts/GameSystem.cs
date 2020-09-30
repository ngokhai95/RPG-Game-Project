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
    public GameObject alertHUD;

    public string location;
    private GameObject goal;

    private void Start()
    {
        Load();
        location = SceneManager.GetActiveScene().name;
        goal = GameObject.FindWithTag("Goal");
    }
    private void Update()
    {
        TalkToNPC();
    }

    public void Save()
    {
        SaveLoadSystem.Save(player.GetComponent<Player>());
    }

    public void Load()
    {
        SaveData data = SaveLoadSystem.Load();

        player.GetComponent<Player>().HP = data.hp;
        player.GetComponent<Player>().acceptedQuest = data.acceptedQuest;
        player.GetComponent<Player>().finishedQuests = data.finishedQuests;
        player.GetComponent<Player>().monsterKills = data.monsterKills;
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

    public void Alert(string message)
    {
        alertHUD.SetActive(true);
        alertHUD.GetComponent<Text>().text = message;
        StartCoroutine(WaitandClear());
    }
    public void TalkToNPC()
    {
        if (Vector3.Distance(player.transform.position, npc.transform.position) < 3.0f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                npc.GetComponent<QuestHandler>().dialogHUD.SetActive(true);
                player.GetComponent<Player>().IdleAnimation();
                PauseCharacter();
            }
        }
        else if (Vector3.Distance(player.transform.position, goal.transform.position) < 3.0f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                npc.GetComponent<QuestHandler>().dialogHUD.SetActive(true);
                player.GetComponent<Player>().IdleAnimation();
                PauseCharacter();
            }
        }
    }
    IEnumerator WaitandClear()
    {
        yield return new WaitForSeconds(5);
        alertHUD.SetActive(false);
    }

    public void PauseCharacter()
    {
        player.GetComponent<PlayerController>().enabled = false;
    }
    
    public void UnPauseCharacter()
    {
        player.GetComponent<PlayerController>().enabled = true;
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
}
