using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MomentumController : MonoBehaviour
{
    public CharacterController controller;
    public float x;
    public float z;
    public float yRot = 0f;
    public float moveSpeed = 10f;
    public float mouseSens = 1000f;
    public float gravity = -13f;
    public Transform groundCheck;
    public float groundDist = 10f;
    public LayerMask groundMask;
    public float jumpHeight = 1000f;
    public bool tryJump;
    public Vector3 velocity;
    public Vector3 move;
    public Vector3 moveOffGround;
    public bool isGrounded;
    public bool isMoving;
    public float xMove = 0f;
    public float zMove = 0f;
    public float decel = 0.1f;
    public float currentSpeed = 0f;
    public float airControl = 0.5f;
    public int direction;
    public Vector3 airMove;
    public float smooth = 1000f;
    public float runAddModifier = 0.5f;
    public Vector3 momentum;
    public float moveMag;
    public Vector3 moveDir;
    public Vector3 totalMove;
    public float friction = 50f;
    public float defaultGravity = -20f;
    public float crouchSlowModifier = 0.4f;
    public float crouchFrictionModifier = 0.5f;
    private float moveSpeedHolder;
    private float crouchSpeed;
    private float crouchFriction;
    private float frictionHolder;

    void Start()
    {
        crouchSpeed = moveSpeed * crouchSlowModifier;
        moveSpeedHolder = moveSpeed;
        crouchFriction = friction * crouchFrictionModifier;
        frictionHolder = friction;
    }

    void Update()
    {
        if (Input.GetButton("Crouch") == true)
        {
            moveSpeed = crouchSpeed;
            friction = crouchFriction;
        }
        else
        {
            moveSpeed = moveSpeedHolder;
            friction = frictionHolder;
        }
       

        //prevents gravity from increasing on the ground, checks if the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = defaultGravity;
        }
        
        //jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //changes y rotation of the player body based on mouse movement in the y rotation
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSens * Time.deltaTime;
        yRot += mouseX;
        transform.localRotation = Quaternion.Euler(0f, yRot, 0f);

        //grabs direction of the player WASD, including diagonals, and adds it to xMove/zMove
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        moveDir = (transform.right * x + transform.forward * z);
        moveDir = moveDir.normalized * moveSpeed;


        if (isGrounded)
        {
            move = move + moveDir;
            move = Vector3.ClampMagnitude(move, moveSpeed);
            momentum = momentum - momentum.normalized * Time.deltaTime * friction;
            totalMove = totalMove.magnitude * moveDir.normalized;
            if (Input.GetButton("Crouch") == true)
            {
                totalMove += move;
                totalMove = Vector3.ClampMagnitude(move, moveSpeed);
            }
            else
            {
                totalMove = Vector3.ClampMagnitude(move + momentum, moveSpeed);
            }
            if (z > 0 && Input.GetKey(KeyCode.LeftShift) == true && Input.GetButton("Crouch") == false)
            {
                totalMove += transform.forward * moveSpeed * runAddModifier;
            }
            if (Input.GetButton("Crouch") == true)
            {
                controller.Move((totalMove + momentum) * Time.deltaTime);
            }
            else
            {
                controller.Move(totalMove * Time.deltaTime);
            }
            /*if (x == 0 && z == 0)
            {
                move -= move.normalized / friction;
            }*/
        }
        else
        {
            momentum = totalMove;
            controller.Move(momentum * Time.deltaTime);
        }

        move -= move.normalized / friction;
        moveMag = move.magnitude;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("I am colliding");
    }
}
