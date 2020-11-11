using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;
using MalbersAnimations;

public class NPCAgent : Agent
{
    NPCController controller;
    GameObject enemy;
    Rigidbody rb;

    private Vector3 startPos;
    private Vector3 DstartPos;

    public bool isDamaged;
    public int isHit;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<NPCController>();
        enemy = controller.dragon;
        startPos = transform.position;
        DstartPos = enemy.transform.position;
        isDamaged = false;
        isHit = 0;
        MaxStep = 5000;
    }

    void Update()
    {
        DistanceRewardFunction();
        DamagedRewardFunction();
        AttackRewardFunction();
        FinishRewardFunction();
        Debug.Log(GetCumulativeReward());
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(transform.rotation);
        sensor.AddObservation(rb.velocity);
        sensor.AddObservation(GetComponent<NPCController>().npcHP);
        sensor.AddObservation(enemy.transform.position);
        sensor.AddObservation(Vector3.Distance(transform.position, enemy.transform.position));
        sensor.AddObservation(enemy.GetComponent<Animal>().life);
    }

    public override void OnEpisodeBegin()
    {
        enemy = controller.dragon;
        transform.position = startPos;
        transform.rotation = Quaternion.identity;
        enemy.transform.position = DstartPos + new Vector3(UnityEngine.Random.Range(-10, 10), 0, UnityEngine.Random.Range(-10, 10));
        enemy.GetComponent<Animal>().life = 100;
        GetComponent<NPCController>().npcHP = 100;
        isDamaged = false;
    }

    public override void Heuristic(float[] actionsOut)
    {
        //move
        actionsOut[0] = 0;
        if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            actionsOut[0] = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            actionsOut[0] = 3;
        }
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[0] = 4;
        }

        //attack
        actionsOut[1] = 0;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            actionsOut[1] = 1;
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            actionsOut[1] = 2;
        }

    }

    public override void OnActionReceived(float[] vectorAction)
    {
        controller.Movement(vectorAction);
        controller.Attack(enemy, vectorAction);
    }

    private void DistanceRewardFunction()
    {
        if (Vector3.Distance(transform.position, enemy.transform.position) <= 4f)
        {
            AddReward(2f / MaxStep);
        }
        else if (Vector3.Distance(transform.position, enemy.transform.position) <= 6f)
        {
            AddReward(1f / MaxStep);
        }
        else if (Vector3.Distance(transform.position, enemy.transform.position) <= 8f)
        {
            AddReward(0.2f / MaxStep);
        }
        else if (Vector3.Distance(transform.position, enemy.transform.position) > 8f)
        {
            AddReward(-2.4f / MaxStep);
        }
        else if (Vector3.Distance(transform.position, enemy.transform.position) > 10f)
        {
            AddReward(-4.9f / MaxStep);
        }
    }

    private void DamagedRewardFunction()
    {
        if (isDamaged)
        {
            AddReward(-enemy.GetComponent<Animal>().attackStrength / MaxStep);
            isDamaged = false;
        }
        else
        {
            AddReward(0.001f / MaxStep);
        }    
    }

    private void AttackRewardFunction()
    {
        if (isHit != 0)
        {
            switch (isHit)
            {
                case 1:
                    AddReward((controller.npcDamage * 100f) / MaxStep);
                    break;
                case 2:
                    AddReward((controller.npcDamage * 200f) / MaxStep);
                    break;
                case 3:
                    AddReward((controller.npcDamage * 150f) / MaxStep);
                    break;
                case 4:
                    AddReward((controller.npcDamage * 300f) / MaxStep);
                    break;
            }
            isHit = 0;
        }
        else
        {
            if (controller.isAttacking() != 0)
                AddReward(-controller.npcDamage / MaxStep);
        }
    }

    private void FinishRewardFunction()
    {
        AddReward(-0.1f / MaxStep);
        //win
        if (enemy.GetComponent<Animal>().life <= 0)
        {
            AddReward(0.5f);
            EndEpisode();
        }
        //lose
        if (GetComponent<NPCController>().npcHP <= 0)
        {
            AddReward(-0.5f);
            EndEpisode();
        }
    }
}
