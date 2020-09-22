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
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    public Player player;
    public float rotationSpeed;
    public float walkSpd;
    public float runSpd;
    public Transform followTarget;
    public float sensitivity;
    public CinemachineVirtualCamera playerCam;
    public float zoomSpd;

    private float targetZoom;
    private float smoothValue;
    private Rigidbody Rigid;
    Vector3 verticalSpeed;
    Vector3 horizontalSpeed;
    GameObject world;
    private bool isDead;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Rigid = GetComponent<Rigidbody>();
        world = GameObject.FindWithTag("World");
        isDead = false;
        smoothValue = 5.0f;
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
                //camera
                CameraRotation();
                Zoom();
                //character
                Rotation();
                Movement();
                MovementAnimation();
                Attack();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == ("Dragon") || other.tag == ("DragonParts"))
        {
            if (player.isAttacking())
            {
                other.GetComponentInParent<Animal>().life = other.GetComponentInParent<Animal>().life - 5;
                other.GetComponentInParent<Animal>().Damaged = true;
            }
        }


    }

    //camera inputs
    private void CameraRotation()
    {
        followTarget.rotation *= Quaternion.AngleAxis(Input.GetAxis("Mouse X") * sensitivity, Vector3.up);
        followTarget.rotation *= Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * sensitivity, Vector3.right);

        var angles = followTarget.localEulerAngles;
        angles.z = 0;

        var angle = followTarget.localEulerAngles.x;

        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }
        followTarget.transform.localEulerAngles = angles;
        if (Rigid.velocity.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Euler(0, followTarget.rotation.eulerAngles.y, 0);
            followTarget.localEulerAngles = new Vector3(angles.x, 0, 0);
        }
    }


    private void Zoom()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");
        targetZoom -= scrollData * smoothValue;
        targetZoom = Mathf.Clamp(targetZoom, 20f, 90f);
        playerCam.m_Lens.FieldOfView = Mathf.Lerp(playerCam.m_Lens.FieldOfView, targetZoom, Time.deltaTime * zoomSpd);

    }

    //player inputs
    private void Rotation()
    {
        transform.rotation *= Quaternion.AngleAxis(Input.GetAxis("Horizontal") * rotationSpeed, Vector3.up);
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
            if (Rigid.velocity.magnitude <= player.Speed * walkSpd)
            {   
                Rigid.AddForce(verticalSpeed + horizontalSpeed, ForceMode.VelocityChange);
            }
            else
            {
                Rigid.velocity = Rigid.velocity * 0.9f;
            }
        }
        else if (Input.GetAxis("Horizontal") > 0)
        {
            if (Rigid.velocity.magnitude <= player.Speed * runSpd)
            {
                Rigid.AddForce(verticalSpeed + horizontalSpeed/2, ForceMode.VelocityChange);
            }

        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            if (Rigid.velocity.magnitude <= player.Speed * runSpd)
            {
                Rigid.AddForce(verticalSpeed + horizontalSpeed/2, ForceMode.VelocityChange);
            }
        }
        else if (Input.GetAxis("Vertical") > 0)
        {
            if (Rigid.velocity.magnitude <= player.Speed * runSpd)
            {
                Rigid.AddForce(verticalSpeed + horizontalSpeed, ForceMode.VelocityChange);
            }
        }
    }

    private void MovementAnimation()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Rigid.velocity.magnitude <= player.Speed * walkSpd)
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
            if (Rigid.velocity.magnitude <= player.Speed * walkSpd)
                player.MoveAnimation(2);
            else
                player.RunAnimation(2);
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (Rigid.velocity.magnitude <= player.Speed * walkSpd)
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
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && !player.isAttacking())
        {
            player.AttackAnimation(2);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !player.isAttacking() && player.isRunning())
        {
            player.AttackAnimation(3);
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && !player.isAttacking() && player.isRunning())
        {
            player.AttackAnimation(4);
        }
    }
}
