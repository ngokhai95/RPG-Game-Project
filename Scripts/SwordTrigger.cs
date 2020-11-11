using MalbersAnimations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
    NPCController npc;
    float direction;
    GameObject dragon;
    // Start is called before the first frame update
    void Start()
    {
        npc = transform.GetComponentInParent<NPCController>();
        dragon = npc.dragon;
    }

    // Update is called once per frame
    void Update()
    {
        direction = Vector3.Angle(-npc.gameObject.transform.position + dragon.transform.position, npc.gameObject.transform.forward);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Dragon") || other.tag == ("DragonParts"))
        {
            switch (npc.isAttacking())
            {
                case 0:
                    npc.gameObject.GetComponent<NPCAgent>().isHit = 0;
                    break;
                case 1:
                    if (direction < 50f)
                    {
                        other.GetComponentInParent<Animal>().life = other.GetComponentInParent<Animal>().life - npc.npcDamage;
                        other.GetComponentInParent<Animal>().Damaged = true;
                        npc.gameObject.GetComponent<NPCAgent>().isHit = 1;
                    }
                    break;
                case 2:
                    if (direction < 50f)
                    {
                        other.GetComponentInParent<Animal>().life = other.GetComponentInParent<Animal>().life - npc.npcDamage * 2f;
                        other.GetComponentInParent<Animal>().Damaged = true;
                        npc.gameObject.GetComponent<NPCAgent>().isHit = 2;
                    }
                    break;
                case 3:
                    if (direction < 90f)
                    {
                        other.GetComponentInParent<Animal>().life = other.GetComponentInParent<Animal>().life - npc.npcDamage * 1.5f;
                        other.GetComponentInParent<Animal>().Damaged = true;
                        npc.gameObject.GetComponent<NPCAgent>().isHit = 3;
                    }
                    break;
                case 4:
                    other.GetComponentInParent<Animal>().life = other.GetComponentInParent<Animal>().life - npc.npcDamage * 3f;
                    other.GetComponentInParent<Animal>().Damaged = true;
                    npc.gameObject.GetComponent<NPCAgent>().isHit = 4;
                    break;
            }
        }
    }
}
