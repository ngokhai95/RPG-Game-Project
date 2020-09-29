using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject npc;
    public GameObject alertHUD;

    public float monsterKills;

    private void Start()
    {
        monsterKills = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            RestartGame();
        }
        TalkToNPC();

    }

    public void loadScene(string name)
    {
        if (SceneManager.GetActiveScene().name == name)
        {
            Alert("You are currently at " + name + "!");
        }
        else
        {
            SceneManager.LoadScene(name);
            Alert("Moving to " + name + "...");
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

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
