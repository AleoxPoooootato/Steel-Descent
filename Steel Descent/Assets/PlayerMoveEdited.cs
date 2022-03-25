using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveEdited : MonoBehaviour
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
    public float runModifier = 1.5f;



    void Update()
    {
        //prevents gravity from increasing on the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -20f;
        }

        //changes y rotation of the player body based on mouse movement in the y rotation
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSens * Time.deltaTime;
        yRot += mouseX;
        transform.localRotation = Quaternion.Euler(0f, yRot, 0f);

        //grabs direction of the player WASD, including diagonals, and adds it to xMove/zMove
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(x) == Mathf.Abs(z) && Mathf.Abs(x) == 1)
        {
            x /= Mathf.Sqrt(2);
            z /= Mathf.Sqrt(2);
        }
        
        xMove += x;
        zMove += z;
       
        //smooths out the movement. larger the smooth value, the slower it will add
        if (Mathf.Abs(xMove) > smooth)
        {
            xMove = smooth * xMove / Mathf.Abs(xMove);
        }

        if (Mathf.Abs(zMove) > smooth)
        {
            zMove = smooth * zMove / Mathf.Abs(zMove);
        }

        //slows down the character when moving and no directional key is pressed
        if (isGrounded)
        {
            if (xMove != 0 && x == 0)
            {
                xMove -= xMove / Mathf.Abs(xMove);
            }
            if (zMove != 0 && z == 0)
            {
                zMove -= zMove / Mathf.Abs(zMove);
            }
        }

        //sets move based on x and z coords wanted
        if (isGrounded)
        {
            move = (transform.right * xMove / smooth + transform.forward * zMove / smooth) * moveSpeed * Time.deltaTime;
        }
        
        //checks current speed
        currentSpeed = move.magnitude;

        //moves
        controller.Move(move);

        //jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}