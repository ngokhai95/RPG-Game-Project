using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.IO.Pipes;
using System.ComponentModel;

public class ObjectAgent : Agent
{
    float killer;
    float location;
    int objectID;
    EnemySpawner enemySpawner;
    ObjectSpawner objectSpawner;
    NPCAgent npcFighter;
    bool playerAttacking;
    bool npcSwitch;

    public override void Initialize()
    {
        RandomParameter();
        enemySpawner = GetComponent<EnemySpawner>();
        objectSpawner = GetComponent<ObjectSpawner>();
        objectID = -1;
        MaxStep = 1000;
    }
    private void Update()
    {
        NumberRewardFunction();
        TypeRewardFunction();
        Debug.Log(GetCumulativeReward());
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(killer);
        sensor.AddObservation(enemySpawner.numObjects);
        sensor.AddObservation(location);
        sensor.AddObservation(objectID);
        sensor.AddObservation(playerAttacking);
        sensor.AddObservation(npcSwitch);
    }

    public override void OnEpisodeBegin()
    {
        objectID = -1;
        RandomParameter();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.FloorToInt(vectorAction[0]) == 0) 
        {
            enemySpawner.numObjects = 1;
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 1) 
        {
            enemySpawner.numObjects = 1;
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 2) 
        {
            enemySpawner.numObjects = 2;
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 3) 
        {
            enemySpawner.numObjects = 3;
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 4) 
        {
            enemySpawner.numObjects = 4;
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 5)
        {
            enemySpawner.numObjects = 5;
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 6)
        {
            enemySpawner.numObjects = 6;
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 7)
        {
            enemySpawner.numObjects = 7;
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 8)
        {
            enemySpawner.numObjects = 8;
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 9)
        {
            enemySpawner.numObjects = 9;
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 10)
        {
            enemySpawner.numObjects = 10;
        }

        if (Mathf.FloorToInt(vectorAction[1]) == 0)
        {
            objectSpawner.prefab = GameObject.FindWithTag("Mountain Tree");
            objectID = 0;
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 1)
        {
            objectSpawner.prefab = GameObject.FindWithTag("Forest Tree");
            objectID = 1;
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 2)
        {
            objectSpawner.prefab = GameObject.FindWithTag("Desert Tree");
            objectID = 2;
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 3)
        {
            objectSpawner.prefab = GameObject.FindWithTag("River Tree");
            objectID = 3;
        }

        if (Mathf.FloorToInt(vectorAction[2]) == 0)
        {
            npcFighter.enabled = false;
            npcSwitch = false;
        }
        if (Mathf.FloorToInt(vectorAction[2]) == 1)
        {
            npcFighter.enabled = true;
            npcSwitch = true;
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        //number of objects to spawn
            actionsOut[0] = 0;
            actionsOut[0] = 1;
            actionsOut[0] = 2;
            actionsOut[0] = 3;
            actionsOut[0] = 4;
            actionsOut[0] = 5;
            actionsOut[0] = 6;
            actionsOut[0] = 7;
            actionsOut[0] = 8;
            actionsOut[0] = 9;
            actionsOut[0] = 10;
        //which object to spawn
            actionsOut[1] = 0;
            actionsOut[1] = 1;
            actionsOut[1] = 2;
            actionsOut[1] = 3;
        //whether or not to enable NPC Compaion to fight
            actionsOut[2] = 0;
            actionsOut[2] = 1;

    }

    private void NumberRewardFunction()
    {
        if(killer <= 3)
        {
            if(enemySpawner.numObjects > 3)
            {
                AddReward(-10.0f / MaxStep);
            }
            else
            {
                AddReward(10.0f / MaxStep);
            }
        }
        else if (killer <= 5)
        {
            if (enemySpawner.numObjects > 5)
            {
                AddReward(-10.0f / MaxStep);
            }
            else
            {
                if (enemySpawner.numObjects > 3)
                    AddReward(10.0f / MaxStep);
                else 
                    AddReward(5.0f / MaxStep);
            }
        }
        else if (killer <= 8)
        {
            if (enemySpawner.numObjects > 8)
            {
                AddReward(-10.0f / MaxStep);
            }
            else
            {
                if (enemySpawner.numObjects > 5)
                    AddReward(10.0f / MaxStep);
                else if (enemySpawner.numObjects > 3)
                    AddReward(5.0f / MaxStep);
                else
                {
                    AddReward(2.5f / MaxStep);
                }
            }
        }
        else
        {
            if (enemySpawner.numObjects > 10)
            {
                AddReward(-10.0f / MaxStep);
            }
            else
            {
                if (enemySpawner.numObjects > 8)
                    AddReward(10.0f / MaxStep);
                else if (enemySpawner.numObjects > 5)
                    AddReward(5.0f / MaxStep);
                else if (enemySpawner.numObjects > 3)
                    AddReward(2.5f / MaxStep);
                else
                {
                    AddReward(1.25f / MaxStep);
                }
            }
        }
    }

    private void TypeRewardFunction()
    {
        if (location == objectID)
        {
            AddReward(10.0f / MaxStep);
        }
        else
        {
            AddReward(-10.0f / MaxStep);
        }
    }

    private void NPCRewardFunction()
    {
        if (killer < 3.0f && npcSwitch == false)
        {
            AddReward(-5.0f / MaxStep);
        }
        else
        {
            AddReward(1.0f / MaxStep);
        }
        if (killer > 7.0f && npcSwitch == true)
        {
            AddReward(-5.0f / MaxStep);
        }
        else
        {
            AddReward(1.0f / MaxStep);
        }
        if (!playerAttacking && npcSwitch == true)
        {
            AddReward(-10.0f / MaxStep);
        }    

    }

    private void RandomParameter()
    {
        killer = Random.Range(0.0f, 10.0f);
        Debug.Log(killer);
        location = Random.Range(0, 3);
        switch (location)
        {
            case 0:
                Debug.Log("Mountain");
                break;
            case 1:
                Debug.Log("Forest");
                break;
            case 2:
                Debug.Log("Desert");
                break;
            case 3:
                Debug.Log("River");
                break;
        }
        StartCoroutine(WaitandChange());
    }

    IEnumerator WaitandChange()
    {
        playerAttacking = true;
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        playerAttacking = false;
    }

}


