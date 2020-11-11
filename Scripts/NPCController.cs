using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations;
using Unity.Mathematics;
using System;

public class NPCController : MonoBehaviour
{
    GameObject[] dragons;
    public GameObject dragon;
    private Rigidbody rb;
    Animator animator;

    public float npcHP;
    public float npcSpeed;
    private float rotationSpeed;
    public float npcDamage;
    private bool isPathFinding;

    private void Awake()
    {
        dragons = GameObject.FindGameObjectsWithTag("Dragon");
        dragon = GetClosestEnemy(dragons);
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }
    void Start()
    { 
        npcHP = 100;
        npcDamage = 5;
        npcSpeed = 0.4f;
        rotationSpeed = 1f;
        isPathFinding = false;
    }

    public void Attack(GameObject enemy, float[] actions)
    {
        var action = Mathf.FloorToInt(actions[1]);
        switch (action)
        {
            case 1:
                if (isAttacking() != 0)
                    return;
                if (isRunning())
                {
                    AttackAnimation(3);
                }
                else
                {
                    AttackAnimation(1);
                }
                break;
            case 2:
                if (isAttacking() != 0)
                    return;
                if (isRunning())
                {
                    AttackAnimation(4);
                }
                else
                {
                    AttackAnimation(2);
                }
                break;
        }
    }

    public void Movement(float[] actions)
    {
        var action = Mathf.FloorToInt(actions[0]);
        var direction = Vector3.zero; ;
        float rotation = 0f; 
        switch (action)
        {
            case 0:
                IdleAnimation();
                break;
            case 1: //forward
                if (rb.velocity.magnitude < npcSpeed * 8)
                {
                    MoveAnimation(0);
                    direction = transform.forward;
                }
                else if (rb.velocity.magnitude < npcSpeed * 12)
                {
                    RunAnimation(0);
                    direction = transform.forward;
                }
                else
                {
                    RunAnimation(0);
                }
                
                break;
            case 2: //backward
                if (rb.velocity.magnitude < npcSpeed * 8)
                {
                    MoveAnimation(1);
                    direction = -transform.forward;
                }
                else
                {
                    rb.velocity = rb.velocity * 0.9f;
                }    
                break;
            case 3: //left
                if (rb.velocity.magnitude < npcSpeed * 8)
                {
                    MoveAnimation(2);
                    direction = -transform.right;
                    rotation = -1f;
                }
                else if (rb.velocity.magnitude < npcSpeed * 12)
                {
                    RunAnimation(2);
                    direction = -transform.right;
                    rotation = -1f;
                }
                else
                {
                    RunAnimation(2);
                    rotation = -1f;
                }
                break;
            case 4: //right
                if (rb.velocity.magnitude < npcSpeed * 8)
                {
                    MoveAnimation(3);
                    direction = transform.right;
                    rotation = 1f;
                }
                else if (rb.velocity.magnitude < npcSpeed * 12)
                {
                    RunAnimation(3);
                    direction = transform.right;
                    rotation = 1f;
                }
                else
                {
                    RunAnimation(3);
                    rotation = 1f;
                }
                break;
        }
        transform.rotation *= Quaternion.AngleAxis(rotation * rotationSpeed, Vector3.up);
        rb.AddForce(direction * npcSpeed,
           ForceMode.VelocityChange);
    }

    bool isRunning()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Forward") || 
            this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Left") || 
            this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Right"))
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

    private void AttackAnimation(int type)
    {
        switch (type)
        {
            case 1:
                animator.SetTrigger("Attack");
                animator.SetInteger("AttackMove", 1);
                rb.velocity = new Vector3(0, 0, 0);
                break;
            case 2:
                animator.SetTrigger("Attack");
                animator.SetInteger("AttackMove", 2);      
                rb.velocity = new Vector3(0, 0, 0);
                break;
            case 3:
                animator.SetTrigger("Attack");
                animator.SetInteger("AttackMove", 3);
                rb.velocity = new Vector3(0, 0, 0);
                break;
            case 4:
                animator.SetTrigger("Attack");
                animator.SetInteger("AttackMove", 4);
                rb.velocity = new Vector3(0, 0, 0);
                break;
        }
    }

    public int isAttacking()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Light Attack"))
            return 1;
        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Strong Attack"))
            return 2;
        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Strong Attack"))
            return 3;
        else if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Light Attack"))
            return 4;
        else
            return 0;
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
 
    public void DamagedAnimation()
    {
        animator.SetInteger("state", 3);
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
