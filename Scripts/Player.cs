using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player: MonoBehaviour
{
    public float HP;
    public float Attackk;
    public float Speed;
    public Quest acceptedQuest;
    public List<Quest> finishedQuests;
    private Image HPbar;
    private float startHP;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Attackk = 10f;
        HP = 100f;
        Speed = 0.6f;
        HPbar = GetComponentInChildren<Image>();
        startHP = HP;
    }

    private void Update()
    {
        HPbar.fillAmount = HP / startHP;
    }

    public bool isIdle()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            return true;
        }
        else
            return false;
    }

    public bool isAttacking()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Light Attack") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Strong Attack") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Strong Attack") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Light Attack"))
        {
            return true;
        }
        else
            return false;
    }

    public bool isDamaged()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))
        {
            return true;
        }
        else
            return false;
    }

    public bool isRunning()
    {
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Forward") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Left") || this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run Right"))
        {
            return true;
        }
        else
            return false;
    }

    public void ClearQuest()
    {
        acceptedQuest = new Quest();
    }


    //animation
    public void MoveAnimation(int direction)
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
    }
    public void RunAnimation(int direction)
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

    public void AttackAnimation(int type)
    {
        switch (type)
        {
            case 1:
                animator.SetTrigger("attackL");
                break;
            case 2:
                animator.SetTrigger("attackS");
                break;
            case 3:
                animator.SetTrigger("runattackL");
                break;
            case 4:
                animator.SetTrigger("runattackS");
                break;
        }
    }
    public void IdleAnimation()
    {
        animator.SetInteger("state", 0);
    }
    public void DeadAnimation()
    {
        animator.SetBool("Dead", true);
        animator.SetTrigger("Ded");
    }
}
