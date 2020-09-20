using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Threading;
using UnityEngine.SceneManagement;
using MalbersAnimations;

public class PlayerController : MonoBehaviour
{
    public Player player;
    private Rigidbody Rigid;
    Vector3 verticalSpeed;
    Vector3 horizontalSpeed;
    private bool isHit;
    GameObject dragon;
    GameObject world;
    private bool isDead;
    public float rotationSpeed = 50f;
    public float deadZoneDegrees = 15f;

    private Transform mainCam;

    private Vector3 cameraDirection;
    private Vector3 playerDirection;
    private Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main.transform;
        Rigid = GetComponent<Rigidbody>();
        dragon = GameObject.FindWithTag("Dragon");
        world = GameObject.FindWithTag("World");
        isHit = false;
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {
            if (player.HP <= 0)
            {
                player.DeadAnimation();
                isDead = true;
                world.GetComponent<GameSystem>().PauseCharacter();
            }
            else
            {
                //panning
                Rotation();
                //moving
                Movement();
                MovementAnimation();
                Attack();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Dragon"))
        {
            isHit = true;
        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == ("Dragon"))
        {
            
        }
    }

    
    //player inputs
    private void Rotation()
    {
        if (world.GetComponent<GameSystem>().frontMode)
        {
            cameraDirection = new Vector3(mainCam.forward.x, 0f, mainCam.forward.z);
            playerDirection = new Vector3(transform.forward.x, 0f, transform.forward.z);

            if (Vector3.Angle(cameraDirection, playerDirection) > deadZoneDegrees)
            {
                targetRotation = Quaternion.LookRotation(cameraDirection, transform.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            cameraDirection = new Vector3(mainCam.forward.x, 0f, mainCam.forward.z);
            playerDirection = new Vector3(transform.forward.x, 0f, transform.forward.z);

            if (Vector3.Angle(cameraDirection, playerDirection) > deadZoneDegrees)
            {
                targetRotation = Quaternion.LookRotation(cameraDirection, transform.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private void Movement()
    {
        verticalSpeed = transform.forward * Input.GetAxis("Vertical") * player.Speed;
        horizontalSpeed = transform.right * Input.GetAxis("Horizontal") * player.Speed;

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            player.IdleAnimation();
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            if (Rigid.velocity.magnitude <= player.Speed * 10)
            {   
                Rigid.AddForce(verticalSpeed + horizontalSpeed, ForceMode.VelocityChange);
            }
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            if (Rigid.velocity.magnitude <= player.Speed * 20)
            {
                
                Rigid.AddForce(verticalSpeed + horizontalSpeed, ForceMode.VelocityChange);
            }

        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (Rigid.velocity.magnitude <= player.Speed * 20)
            {
                player.RunAnimation(2);
                Rigid.AddForce(verticalSpeed + horizontalSpeed, ForceMode.VelocityChange);
            }
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            if (Rigid.velocity.magnitude <= player.Speed * 20)
            {
                Rigid.AddForce(verticalSpeed + horizontalSpeed, ForceMode.VelocityChange);
            }
        }
    }

    private void MovementAnimation()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Rigid.velocity.magnitude <= player.Speed * 10)
                player.MoveAnimation(0);
            else
                player.RunAnimation(0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.MoveAnimation(1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (Rigid.velocity.magnitude <= player.Speed * 10)
                player.MoveAnimation(2);
            else
                player.RunAnimation(2);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (Rigid.velocity.magnitude <= player.Speed * 10)
                player.MoveAnimation(3);
            else
                player.RunAnimation(3);
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !player.isAttacking())
        {
            player.AttackAnimation(1);
            if (isHit)
            {
                if (dragon != null)
                {
                    dragon.GetComponent<Animal>().life = dragon.GetComponent<Animal>().life - 5;
                    dragon.GetComponent<Animal>().Damaged = true;
                    isHit = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && !player.isAttacking())
        {
            player.AttackAnimation(2);
            if (isHit)
            {
                if (dragon != null)
                {
                    dragon.GetComponent<Animal>().life = dragon.GetComponent<Animal>().life - 15;
                    dragon.GetComponent<Animal>().Damaged = true;
                    isHit = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !player.isAttacking() && player.isRunning())
        {
            player.AttackAnimation(3);
            if (isHit)
            {
                if (dragon != null)
                {
                    dragon.GetComponent<Animal>().life = dragon.GetComponent<Animal>().life - 10;
                    dragon.GetComponent<Animal>().Damaged = true;
                    isHit = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && !player.isAttacking() && player.isRunning())
        {
            player.AttackAnimation(4);
            if (isHit)
            {
                if (dragon != null)
                {
                    dragon.GetComponent<Animal>().life = dragon.GetComponent<Animal>().life - 20;
                    dragon.GetComponent<Animal>().Damaged = true;
                    isHit = false;
                }
            }
        }
    }
}
