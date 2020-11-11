using MalbersAnimations;
using MalbersAnimations.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    AnimalAIControl follow;
    Player player;
    ContentHandler content;
    Transform spawn;
    GameObject npc;
    GameSystem gameSystem;
    Animal dragon;
    public float AttackDelay;
    bool isAttacking;
    float direction;
    Image HPbar;
    float startHP;
    float currentHP;
    int difficulty;
    float range;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>() ;
        spawn = this.transform; 
        follow = this.GetComponent<AnimalAIControl>();
        dragon = this.GetComponent<Animal>();
        npc = GameObject.FindWithTag("NPC");
        isAttacking = false;
        currentHP = dragon.life;
        startHP = currentHP;
        startHP = dragon.life;
        HPbar = GetComponentInChildren<Image>();
        gameSystem = GameObject.FindWithTag("World").GetComponent<GameSystem>();
        difficulty = 1;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        HPbar.fillAmount = dragon.life / startHP;
        if (isDead())
        {
            player.monsterKills++;
            content.CheckProgress();
            dragon.Death = true;
            dragon.DisableAnimal();
            Destroy(this.gameObject);
            gameSystem.Save();
            //StartCoroutine(WaitandDestroy());
        }
        else
        {
            if(player != null)
            {
                DifficultyAdjustment();
                FollowAndAttack(player.gameObject);
                if (TargetDead(player.gameObject))
                {
                    StopAttack();
                    this.GetComponent<LookAt>().enabled = false;
                    if (spawn == null)
                        return;
                    follow.target = spawn;
                    Reposition(3);
                }
            }
            if(npc != null)
            {
                DifficultyAdjustment();
                FollowAndAttack(npc);
                if (TargetDead(npc))
                {
                    StopAttack();
                    this.GetComponent<LookAt>().enabled = false;
                    if (spawn == null)
                        return;
                    follow.target = spawn; 
                }
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Player"))
        {
            player.HP = player.HP - dragon.attackStrength;
            player.DamagedAnimation();
        }
        if (other.tag == ("NPC"))
        {
            //npc.GetComponent<NPCTraining>().npcHP = npc.GetComponent<NPCTraining>().npcHP - dragon.attackStrength;
            //npc.GetComponent<NPCTraining>().DamagedAnimation();
        }
    }

    private void DifficultyAdjustment()
    {
        switch(difficulty)
        {
            case 0:  //easy
                range = 5.0f;
                AttackDelay = 2.5f;
                dragon.attackStrength = 1;
                break;
            case 1:  //medium
                range = 2.0f;
                AttackDelay = 2.0f;
                dragon.attackStrength = 2;
                break;
            case 2:  //hard
                range = 1.0f;
                AttackDelay = 1.5f;
                dragon.attackStrength = 3;
                break;
        }
    }

    private void FollowAndAttack(GameObject player)
    {
        direction = Vector3.Angle(player.transform.position - dragon.transform.position, dragon.transform.forward);
        if (Vector3.Distance(transform.position, player.transform.position) < 15.0f)
        {
            follow.target = player.transform;
            this.GetComponent<LookAt>().Target = player.transform;
            this.GetComponent<LookAt>().enabled = true;
            if (Vector3.Distance(transform.position, player.transform.position) < 4.0f)
            {

                if (!isAttacking)
                {
                    MeeleAttack();
                    StartCoroutine(WaitandAttack());
                }
                else
                {
                    Reposition(0);
                }

            }
            else if (Vector3.Distance(transform.position, player.transform.position) < 3.0f + range && direction > 90f)
            {
                Reposition(0);
            }
            else if (Vector3.Distance(transform.position, player.transform.position) < 6.0f + range && direction < 90f)
            {
                RangeAttack();
                Reposition(5);
            }
            else if (Vector3.Distance(transform.position, player.transform.position) < 6.0f + range && direction > 90f)
            {
                StopAttack();
                Reposition(5);
            }
            else if (Vector3.Distance(transform.position, player.transform.position) > 10.0f + range)
            {
                StopAttack();
                Reposition(5);
            }

        }
        else
        {
            StopAttack();
            ReturnToSpawn();
            this.GetComponent<LookAt>().enabled = false;
        }
    }

    public bool isDead()
    {
        if (dragon.life <= 0)
        {
            return true;
        }
        else
            return false;
    }
    private bool TargetDead(GameObject player)
    {
        if (player == npc)
        {
            if (player.GetComponent<NPCController>().npcHP <= 0)
            {
                return true;
            }
            else
                return false;
        }
        else
        {
            if (player.GetComponent<Player>().HP <= 0)
            {
                return true;
            }
            else
                return false;
        }
    }

    private void ReturnToSpawn()
    {
        if (spawn == null)
            return;
        follow.target = spawn;
        Reposition(3);
    }

    private void Reposition(float distance)
    {
        follow.stoppingDistance = distance;
        this.GetComponent<NavMeshAgent>().stoppingDistance = distance;
    }

    private void RangeAttack()
    {
        dragon.Attack1 = false;
        dragon.Attack2 = true;
    }

    private void MeeleAttack()
    {
        
        dragon.Attack1 = true;
        dragon.Attack2 = false;
        isAttacking = true;
    }

    private void StopAttack()
    {
        dragon.Attack1 = false;
        dragon.Attack2 = false;
        isAttacking = false;
    }

    IEnumerator WaitandAttack()
    {
        yield return new WaitForSeconds(AttackDelay);
        isAttacking = false;
    }

    IEnumerator WaitandDestroy()
    {
        yield return new WaitForSeconds(2);
        Destroy(this.gameObject);
    }
}
