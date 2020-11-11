using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NPCHelper : MonoBehaviour
{
    // Start is called before the first frame update
    Player player;
    GameSystem gameSystem;
    bool isPathFinding;
    GameObject goal;
    NavMeshAgent agent;

    void Start()
    {
        goal = GameObject.FindWithTag("Goal");
    }

    // Update is called once per frame
    void Update()
    {
        NPCPathFinding();
    }

    private void NPCPathFinding()
    {
        if (player.acceptedQuest.isActive
        && player.acceptedQuest.questType == Quest.Type.Exploring
        && gameSystem.location == player.acceptedQuest.questLocation.ToString())
        {
            UnityEngine.Debug.Log("Follow the NPC to the Destination!");
            if (!isPathFinding)
            {
                PathFinding();
                gameSystem.Alert("Going to destination...");
            }

        }
        else
        {
            gameSystem.Alert("You are on the wrong map!");
        }
        if (player.acceptedQuest.isActive
            && player.acceptedQuest.questType == Quest.Type.Exploring
            && gameSystem.location == player.acceptedQuest.questLocation.ToString())
        {
            if (Vector3.Distance(transform.position, goal.transform.position) < 3.0f)
            {
                player.acceptedQuest.Progress();
                isPathFinding = false;
                agent.enabled = false;
            }
        }
    }

    private void PathFinding()
    {
        agent.enabled = true;
        agent.destination = goal.transform.position;
        isPathFinding = true;
        agent.stoppingDistance = 5;
    }
}
