using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class ContentAgent : Agent
{
    ContentHandler content;
    NPCHelper npcHelper;
    float killer, achiever, explorer, socializer;
    int questCompleted; 
    float avgDifficulty;
    float time;
    bool npcSwitch;
    bool playerAccpetedstatus;

    public override void Initialize()
    {
        content = GetComponent<ContentHandler>();
        MaxStep = 1000;
        RandomParameter();
    }
    private void Update()
    {
        QuestDiffRewardFunction();
        QuestTypeRewardFunction();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(content.questType);
        sensor.AddObservation(content.questDiff);
        sensor.AddObservation(killer);
        sensor.AddObservation(achiever);
        sensor.AddObservation(explorer);
        sensor.AddObservation(socializer);
        sensor.AddObservation(questCompleted);
        sensor.AddObservation(avgDifficulty);
        sensor.AddObservation(npcSwitch);
        sensor.AddObservation(time);
        sensor.AddObservation(playerAccpetedstatus);
    }

    public override void OnEpisodeBegin()
    {
        RandomParameter();
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.FloorToInt(vectorAction[0]) == 0)
        {
            content.questType = 0;
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 1)
        {
            content.questType = 1;
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 2)
        {
            content.questType = 2;
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 3)
        {
            content.questType = 3;
        }

        if (Mathf.FloorToInt(vectorAction[1]) == 0)
        {
            content.questDiff = 1;
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 1)
        {
            content.questDiff = 2;
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 2)
        {
            content.questDiff = 3;
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 3)
        {
            content.questDiff = 4;
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 4)
        {
            content.questDiff = 5;
        }

        if (Mathf.FloorToInt(vectorAction[2]) == 0)
        {
            npcHelper.enabled = false;
            npcSwitch = false;
        }
        if (Mathf.FloorToInt(vectorAction[2]) == 1)
        {
            npcHelper.enabled = true;
            npcSwitch = true;
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        //type of quest to spawn
        actionsOut[0] = 0;
        actionsOut[0] = 1;
        actionsOut[0] = 2;
        actionsOut[0] = 3;
        //difficulty of quest to spawn
        actionsOut[1] = 0;
        actionsOut[1] = 1;
        actionsOut[1] = 2;
        actionsOut[1] = 3;
        actionsOut[1] = 4;
        ////whether or not to enable NPC Compaion to help
        actionsOut[2] = 0;
        actionsOut[2] = 1;
    }

    private void QuestDiffRewardFunction()
    {
        if (avgDifficulty < 1.5f)
        {
            if (content.questDiff > 2)
            {
                AddReward(-10.0f / MaxStep);
            }
            else
            {
                AddReward(10.0f / MaxStep);
            }
        }
        else if (avgDifficulty < 3.5f)
        {
            if (content.questDiff > 4)
            {
                AddReward(-10.0f / MaxStep);
            }
            else
            {
                AddReward(10.0f / MaxStep);
            }
        }
        else
        {
            if (content.questDiff < 3)
            {
                AddReward(-10.0f / MaxStep);
            }
            else
            {
                AddReward(10.0f / MaxStep);
            }
        }

        if (avgDifficulty / content.questDiff >= 0.75f && avgDifficulty / content.questDiff <= 1.5f)
        {
            AddReward(5.0f / MaxStep);
        }
    }

    private void QuestTypeRewardFunction()
    {
        if (killer == 0.0f)
        {
            if (content.questType == 0)
            {
                AddReward(10.0f / MaxStep);
            }
        }
        else if (killer < 1.0f)
        {
            if (content.questType == 0)
            {
                AddReward(-10.0f / MaxStep);
            }
        }
        else if (killer < 3.0f)
        {
            if (content.questType == 0)
            {
                AddReward(-5.0f / MaxStep);
            }
        }
        else if (killer < 5.0f)
        {
            if (content.questType == 0)
            {
                AddReward(5.0f / MaxStep);
            }
        }
        else
        {
            if (content.questType == 0)
            {
                AddReward(10.0f / MaxStep);
            }
        }

        if (achiever == 0.0f)
        {
            if (content.questType == 3)
            {
                AddReward(10.0f / MaxStep);
            }
        }
        else if (achiever < 1.0f)
        {
            if (content.questType == 3)
            {
                AddReward(-10.0f / MaxStep);
            }
        }
        else if (achiever < 3.0f)
        {
            if (content.questType == 3)
            {
                AddReward(-5.0f / MaxStep);
            }
        }
        else if (achiever < 5.0f)
        {
            if (content.questType == 3)
            {
                AddReward(5.0f / MaxStep);
            }
        }
        else
        {
            if (content.questType == 3)
            {
                AddReward(10.0f / MaxStep);
            }
        }

        if (explorer == 0.0f)
        {
            if (content.questType == 1)
            {
                AddReward(10.0f / MaxStep);
            }
        }
        else if (explorer < 1.0f)
        {
            if (content.questType == 1)
            {
                AddReward(-10.0f / MaxStep);
            }
        }
        else if (explorer < 3.0f)
        {
            if (content.questType == 1)
            {
                AddReward(-5.0f / MaxStep);
            }
        }
        else if (explorer < 5.0f)
        {
            if (content.questType == 1)
            {
                AddReward(5.0f / MaxStep);
            }
        }
        else
        {
            if (content.questType == 1)
            {
                AddReward(10.0f / MaxStep);
            }
        }

        if (socializer == 0.0f)
        {
            if (content.questType == 2)
            {
                AddReward(10.0f / MaxStep);
            }
        }
        else if (socializer < 1.0f)
        {
            if (content.questType == 2)
            {
                AddReward(-10.0f / MaxStep);
            }
        }
        else if (socializer < 3.0f)
        {
            if (content.questType == 2)
            {
                AddReward(-5.0f / MaxStep);
            }
        }
        else if (socializer < 5.0f)
        {
            if (content.questType == 2)
            {
                AddReward(5.0f / MaxStep);
            }
        }
        else
        {
            if (content.questType == 2)
            {
                AddReward(10.0f / MaxStep);
            }
        }
    }

    private void NPCRewardFunction()
    {
        if (time > 5.0f && npcSwitch == false)
        {
            AddReward(-5.0f / MaxStep);
        }
        else
        {
            AddReward(1.0f / MaxStep);
        }
        if (time < 2.0f && npcSwitch == true)
        {
            AddReward(-5.0f / MaxStep);
        }
        else
        {
            AddReward(1.0f / MaxStep);
        }
        if (!playerAccpetedstatus && npcSwitch == true)
        {
            AddReward(-10.0f / MaxStep);
        }

    }


    private void RandomParameter()
    {
        killer = Random.Range(0.0f, 10.0f);
        achiever = Random.Range(0.0f, 10.0f);
        explorer = Random.Range(0.0f, 10.0f);
        socializer = Random.Range(0.0f, 10.0f);
        questCompleted = Random.Range(0, 100);
        avgDifficulty = Random.Range(0.0f, 5.0f);
        time = Random.Range(0.0f, 10.0f);
    }
}
