using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class SpawnerAgent : Agent
{
    float killer;
    float location;
    int treeID;
    EnemySpawner enemySpawner;
    TreeSpawner treeSpawner;

    public override void Initialize()
    {
        RandomParameter();
        enemySpawner = GetComponent<EnemySpawner>();
        treeSpawner = GetComponent<TreeSpawner>();
        treeID = -1;
        MaxStep = 1000;
    }
    private void Update()
    {
        EnemyRewardFunction();
        TreeRewardFunction();
        Debug.Log(GetCumulativeReward());
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(killer);
        sensor.AddObservation(enemySpawner.numObjects);
        sensor.AddObservation(location);
        sensor.AddObservation(treeID);
    }

    public override void OnEpisodeBegin()
    {
        treeID = -1;
        RandomParameter();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.FloorToInt(vectorAction[0]) == 0) 
        {
            enemySpawner.numObjects = 0;
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
            treeSpawner.prefab = GameObject.FindWithTag("Mountain Tree");
            treeID = 0;
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 1)
        {
            treeSpawner.prefab = GameObject.FindWithTag("Forest Tree");
            treeID = 1;
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 2)
        {
            treeSpawner.prefab = GameObject.FindWithTag("Desert Tree");
            treeID = 2;
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 3)
        {
            treeSpawner.prefab = GameObject.FindWithTag("River Tree");
            treeID = 3;
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
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

            actionsOut[1] = 0;
            actionsOut[1] = 1;
            actionsOut[1] = 2;
            actionsOut[1] = 3;

    }

    private void EnemyRewardFunction()
    {
        if(killer <= 3)
        {
            if(enemySpawner.numObjects > 3)
            {
                AddReward(-1.0f / MaxStep);
            }
            else
            {
                AddReward(1.0f / MaxStep);
            }
        }
        else if (killer <= 5)
        {
            if (enemySpawner.numObjects > 5)
            {
                AddReward(-1.0f / MaxStep);
            }
            else
            {
                if (enemySpawner.numObjects > 3)
                    AddReward(1.0f / MaxStep);
                else 
                    AddReward(0.5f / MaxStep);
            }
        }
        else if (killer <= 8)
        {
            if (enemySpawner.numObjects > 8)
            {
                AddReward(-1.0f / MaxStep);
            }
            else
            {
                if (enemySpawner.numObjects > 5)
                    AddReward(1.0f / MaxStep);
                else if (enemySpawner.numObjects > 3)
                    AddReward(0.5f / MaxStep);
                else
                {
                    AddReward(0.25f / MaxStep);
                }
            }
        }
        else
        {
            if (enemySpawner.numObjects > 10)
            {
                AddReward(-1.0f / MaxStep);
            }
            else
            {
                if (enemySpawner.numObjects > 8)
                    AddReward(1.0f / MaxStep);
                else if (enemySpawner.numObjects > 5)
                    AddReward(0.5f / MaxStep);
                else if (enemySpawner.numObjects > 3)
                    AddReward(0.25f / MaxStep);
                else
                {
                    AddReward(0.125f / MaxStep);
                }
            }
        }
    }

    private void TreeRewardFunction()
    {
        if (location == treeID)
        {
            AddReward(1.0f / MaxStep);
        }
        else
        {
            AddReward(-1.0f / MaxStep);
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
    }

}


