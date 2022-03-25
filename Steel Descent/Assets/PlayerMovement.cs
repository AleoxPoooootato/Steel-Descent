using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
    public float xMove;
    public float zMove;
    public float decel = 0.1f;
    public float currentSpeed = 0f;
    public float airControl = 0.5f;



    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSens * Time.deltaTime;

        yRot += mouseX;

        transform.localRotation = Quaternion.Euler(0f, yRot, 0f);

        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(x) == Mathf.Abs(z) && Mathf.Abs(x) == 1)
        {
            x /= Mathf.Sqrt(2);
            z /= Mathf.Sqrt(2);
        }

        if (x != 0 | z != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        
        if (isMoving && currentSpeed * Time.deltaTime < moveSpeed * Time.deltaTime && isGrounded == true)
        {
            xMove += x / 80;
            zMove += z / 80;
        }
        else if (isGrounded == true)
        {
            xMove *= decel;
            zMove *= decel;
        }
        else if (isGrounded == false)
        {
            xMove += x / 80;
            zMove += z / 80;
        }
        if (isGrounded == true)
        {
            move = (transform.right * xMove + transform.forward * zMove) * moveSpeed * Time.deltaTime;
        }
        else
        {
            Debug.Log("fuckin workin now lmao");
            Vector3 moveOffGround = new Vector3(move.x * xMove * moveSpeed * Time.deltaTime, transform.position.y * moveSpeed * Time.deltaTime, move.z * zMove * moveSpeed * Time.deltaTime);
            move = moveOffGround;
        }
        
        currentSpeed = move.magnitude / Time.deltaTime;

        controller.Move(move);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
