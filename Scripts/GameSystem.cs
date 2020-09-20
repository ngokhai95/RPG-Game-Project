using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera mainCam;
    public Camera frontCam;
    public GameObject player;
    public GameObject npc;
    public bool frontMode;

    void Start()
    {
        frontMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!frontMode)
            {
                frontMode = true;
                frontCam.fieldOfView = mainCam.fieldOfView;
                frontCam.enabled = true;
                mainCam.enabled = false;
            }
            else
            {
                frontMode = false;
                mainCam.fieldOfView = frontCam.fieldOfView;
                mainCam.enabled = true;
                frontCam.enabled = false;          
            }
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
       
        if (frontMode)
        {
            frontCam.GetComponent<CameraController>().enabled = false;
        }
        else
        {
            mainCam.GetComponent<CameraController>().enabled = false;
        }
        player.GetComponent<PlayerController>().enabled = false;
    }
    
    public void UnPauseCharacter()
    {
        if (frontMode)
        {
            frontCam.GetComponent<CameraController>().enabled = true;
        }
        else
        {
            mainCam.GetComponent<CameraController>().enabled = true;
        } 
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
