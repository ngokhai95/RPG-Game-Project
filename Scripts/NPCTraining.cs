using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.Rendering.PostProcessing;
using MalbersAnimations;
using Unity.Mathematics;

public class NPCTraining : Agent
{
    GameObject[] dragons;
    GameObject dragon;
    private Rigidbody rb;
    Animator animator;
    private Vector3 startPos;
    private Vector3 DstartPos;
    public float npcHP;
    private float npcDamage;
    private bool isDead;
    private bool isHit;
    private float time;

    public override void Initialize()
    {
        dragons = GameObject.FindGameObjectsWithTag("Dragon");
        dragon = GetClosestEnemy(dragons);
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        startPos = transform.position;
        DstartPos = dragon.transform.position;
        npcHP = 100;
        npcDamage = 10;
        isDead = false;
        isHit = false;
        time = 0;
        
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;
        /*if (dragon.GetComponent<Animal>().life <= 0)
        {
            AddReward(100f);
            EndEpisode();
        }*/
        if(Vector3.Distance(transform.position, dragon.transform.position) <= 0f)
        {
            AddReward(0.04f);
        }
        else if (Vector3.Distance(transform.position, dragon.transform.position) <= 3f)
        {
            AddReward(0.03f);
        }
        else if (Vector3.Distance(transform.position, dragon.transform.position) <= 5f)
        {
            AddReward(0.02f);
        }
        else if (Vector3.Distance(transform.position, dragon.transform.position) <= 7f)
        {
            AddReward(0.01f);
        }
        else if(Vector3.Distance(transform.position, dragon.transform.position) <= 9f)
        {
            AddReward(0.005f);
        }
        else if (Vector3.Distance(transform.position, dragon.transform.position) > 10f)
        {
            AddReward(-0.01f);
        }
        if (time > 120)
        {
            AddReward(-120f);
            EndEpisode();
        }
        AddReward(-Time.deltaTime);
        if(isHit)
        {
            AddReward(100f);
            isHit = false;
            EndEpisode();
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(Vector3.Distance(transform.position, dragon.transform.position));
        sensor.AddObservation(transform.position);
        sensor.AddObservation(dragon.transform.position);
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        if (Mathf.FloorToInt(vectorAction[0]) == 0) //idle
        {
            Movement(0);
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 1) //move up
        {
            Movement(1);
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 2) //move down
        {
            Movement(2);
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 3) //move left
        {
            Movement(3);
        }
        if (Mathf.FloorToInt(vectorAction[0]) == 4) //move right
        {
            Movement(4);
        }
        /*if (Mathf.FloorToInt(vectorAction[1]) == 1) //attack 1
        {
            Attack(dragon,1);
        }
        if (Mathf.FloorToInt(vectorAction[1]) == 2) //attack 2
        {
            Attack(dragon,2);
        }*/
    }

    public override void OnEpisodeBegin()
    {
        transform.position = startPos;
        dragon.transform.position = DstartPos + new Vector3(UnityEngine.Random.Range(0, 10), 0, UnityEngine.Random.Range(0, 10));
        time = 0;
        //dragon.GetComponent<Animal>().life = 100;
    }

    public override void Heuristic(float[] actionsOut)
    {
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
        /*actionsOut[1] = 0;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            actionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            actionsOut[1] = 2;
        }*/

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Dragon"))
        {
            isHit = true;
        }
        if (other.tag == ("DragonParts"))
        {
            isHit = true;
        }
    }

    private void Attack(GameObject enemy, int type)
    {
        switch(type)
        {
            case 1:
                if (isAttacking())
                    return;
                if (isRunning())
                {
                    AttackAnimation(3);
                    if (dragon != null)
                    {
                        if (isHit)
                        {
                            dragon.GetComponent<Animal>().life = dragon.GetComponent<Animal>().life - 20;
                            dragon.GetComponent<Animal>().Damaged = true;
                            AddReward(20.0f);
                            isHit = false;
                        }
                    }
                }
                else
                {
                    AttackAnimation(1);
                    if (dragon != null)
                    {
                        if (isHit)
                        {
                            dragon.GetComponent<Animal>().life = dragon.GetComponent<Animal>().life - 10;
                            dragon.GetComponent<Animal>().Damaged = true;
                            AddReward(10.0f);
                            isHit = false;
                        }
                    }
                }
                break;
            case 2:
                if (isAttacking())
                    return;
                if (isRunning())
                {
                    AttackAnimation(4);
                    if (dragon != null)
                    {
                        if (isHit)
                        {
                            dragon.GetComponent<Animal>().life = dragon.GetComponent<Animal>().life - 30;
                            dragon.GetComponent<Animal>().Damaged = true;
                            AddReward(30.0f);
                            isHit = false;
                        }
                    }
                }
                else
                {
                    AttackAnimation(2);
                    if (dragon != null)
                    {
                        if (isHit)
                        {
                            dragon.GetComponent<Animal>().life = dragon.GetComponent<Animal>().life - 15;
                            dragon.GetComponent<Animal>().Damaged = true;
                            AddReward(15.0f);
                            isHit = false;
                        }
                    }
                }
                break;
        }
    }

    private void Movement(int type)
    {
        switch(type)
        {
            case 0:
                IdleAnimation();
                break;
            case 1:
                if (rb.velocity.z < 1.5f)
                {
                    MoveAnimation(0);
                    rb.AddForce(new Vector3(0, 0, 0.3f), ForceMode.VelocityChange);
                }
                else if (rb.velocity.z < 3.0f)
                {
                    RunAnimation(0);
                    rb.AddForce(new Vector3(0, 0, 0.3f), ForceMode.VelocityChange);
                }
                else
                {
                    RunAnimation(0);
                }
                
                break;
            case 2:
                if (rb.velocity.z > -1.5f)
                {
                    MoveAnimation(1);
                    rb.AddForce(new Vector3(0, 0, -0.3f), ForceMode.VelocityChange);
                }
                break;
            case 3:
                if (rb.velocity.x > -1.5f)
                {
                    MoveAnimation(2);
                    rb.AddForce(new Vector3(-0.3f, 0, 0), ForceMode.VelocityChange);
                }
                else if (rb.velocity.x > -3.0f)
                {
                    RunAnimation(2);
                    rb.AddForce(new Vector3(-0.3f, 0, 0), ForceMode.VelocityChange);
                }
                else
                {
                    RunAnimation(2);
                }
                break;
            case 4:
                if (rb.velocity.x < 1.5f)
                {
                    MoveAnimation(3);
                    rb.AddForce(new Vector3(0.3f, 0, 0), ForceMode.VelocityChange);
                }
                else if (rb.velocity.x < 3.0f)
                {
                    RunAnimation(3);
                    rb.AddForce(new Vector3(0.3f, 0, 0), ForceMode.VelocityChange);
                }
                else
                {
                    RunAnimation(3);
                }
                break;
        }    
    }

    bool isRunning()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Forward") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Left") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Right"))
        {
            return true;
        }
        else
            return false;
    }

    bool isAttacking()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Light Attack") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Strong Attack") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Strong Attack") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Light Attack"))
        {
            return true;
        }
        else
            return false;
    }

    private void RunAnimation(int direction)
    {
        animator.SetInteger("state", 2);
        switch (direction)
        {
            case 0: //forward
                animator.SetInteger("direction", 0);
                break;
            case 1: //backward
                animator.SetInteger("direction", 1);
                break;
            case 2: //left
                animator.SetInteger("direction", 2);
                break;
            case 3: //right
                animator.SetInteger("direction", 3);
                break;
        }
    }

    private void AttackAnimation(int type)
    {
        switch (type)
        {
            case 1:
                animator.SetTrigger("attackL");
                rb.velocity = new Vector3(0, 0, 0);
                break;
            case 2:
                animator.SetTrigger("attackS");
                rb.velocity = new Vector3(0, 0, 0);
                break;
            case 3:
                animator.SetTrigger("runattackL");
                rb.velocity = new Vector3(0, 0, 0);
                break;
            case 4:
                animator.SetTrigger("runattackS");
                rb.velocity = new Vector3(0, 0, 0);
                break;
        }
    }
    private void IdleAnimation()
    {
        animator.SetInteger("state", 0);
    }
    private void DeadAnimation()
    {
        animator.SetBool("Dead", true);
        animator.SetTrigger("Ded");
    }
    private void MoveAnimation(int direction)
    {
        animator.SetInteger("state", 1);
        switch (direction)
        {
            case 0: //forward
                animator.SetInteger("direction", 0);
                break;
            case 1: //backward
                animator.SetInteger("direction", 1);
                break;
            case 2: //left
                animator.SetInteger("direction", 2);
                break;
            case 3: //right
                animator.SetInteger("direction", 3);
                break;
        }

    }
    public void DamagedAnimation()
    {
        animator.SetTrigger("Damaged");
        rb.velocity = new Vector3(0, 0, 0);
    }

    private GameObject GetClosestEnemy(GameObject[] enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in enemies)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t.transform;
                minDist = dist;
            }
        }
        return tMin.gameObject;
    }
}
