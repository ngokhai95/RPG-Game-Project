using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public GameObject npc;

    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            RestartGame();
        }
        if (Vector3.Distance(player.transform.position, npc.transform.position) < 3.0f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                npc.GetComponent<QuestHandler>().dialogHUD.SetActive(true);
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
