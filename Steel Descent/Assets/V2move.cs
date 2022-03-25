using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class V2move : MonoBehaviour
{
    public CharacterController controller;
    public float x;
    public float z;
    public float yRot = 0f;
    public float moveSpeed = 10f;
    public float mouseSens = 500f;
    public float gravity = -13f;
    public Transform groundCheck;
    public float groundDist = 0.7f;
    public LayerMask groundMask;
    public float jumpHeight = 2f;
    public bool tryJump;
    public Vector3 velocity;
    public Vector3 move;
    public Vector3 moveOffGround;
    public bool isGrounded;
    public bool isMoving;
    public float xMove = 0f;
    public float zMove = 0f;
    public float decel = 0.983f;
    public float currentSpeed = 0f;
    public float airControl = 0.5f;
    public int direction;
    public Vector3 airMove;
    public float smooth = 50f;
    public float runAddModifier = 0.4f;
    public Vector3 momentum;
    public float moveMag;
    public Vector3 moveDir;
    public Vector3 totalMove;
    public float friction = 10f;
    public float defaultGravity = -6f;
    public float crouchSlowModifier = 0.4f;
    public float crouchFrictionModifier = 0.3f;
    private float moveSpeedHolder;
    private float crouchSpeed;
    private float crouchFriction;
    private float frictionHolder;
    private float doubleShift = 0f;
    public float holdSpace = 0f;
    //private Coroutine dashCheck = speedSlide;
    public float timeInterval;
    public Vector3 dash;
    public Vector3 rotationAmount;
    public float mouseX;
    public float snappiness = 0.15f;
    public float posToRotateTo = 0f;
    public Health playerHealth;
    public float repairProgress;
    public float repairThreshold = 0.5f;
    public float dashingThreshold = 1f;
    public float dashingLeft;
    public bool isDashing;
    public Image stamBar;
    public float stamLeft;
    public float stamMax = 12;
    public float stamUsedOnMove = 4;
    public float dashJump;
    RaycastHit slopeHit;
    public Vector3 slopeMove;
    public float slopeAngle;
    public Vector3 slopeDirection;
    public bool onSlope;
    public float hitAngle;
    public Vector3 toMove;
    public bool doneMove;
    public bool fell;
    public float slideTime;
    public float slideLeft;
    public bool isSprinting;
    public bool isCrouching;
    
    //public Vector3 slopeMove;
    //public Vector3 slide;
    public bool isSliding = false;

    void Start()
    {
        dashingLeft = dashingThreshold;
        stamLeft = stamMax;
        //toMove = new Vector3(0f, 0f, 1f);
        slopeMove = new Vector3(0f, 0f, 1f);
    }

    void Update()
    {
        //------------------------------------PROTECTED------------------------------------//

        // if the player crouches, make them go small, otherwise, make them go normal size
        if(isCrouching){
            if(controller.height == 2f){
                isCrouching = false;
            }
            controller.height = 1.5f;
            if (!isCrouching){
                controller.Move(new Vector3(0f, -0.5f, 0f));
            }
            isCrouching = true;
        }
        else{
            controller.height = 2f;
        }
        
        // raises gravity while in the air to make player go splat
        if(!isGrounded){
            velocity.y += gravity * Time.deltaTime;
        }

        //If the player presses jump and lets go, normal jump. if they hold, they super jump.
        if(hitAngle < 45 && hitAngle > 0)
        {
            if (Input.GetButtonUp("Jump") && isGrounded && holdSpace < 1f)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            else if (Input.GetButtonUp("Jump") && isGrounded && holdSpace >= 1f)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * 3f * -2f * gravity);
            }
            if (Input.GetButton("Jump") && isGrounded)
            {
                holdSpace += 1.5f * Time.deltaTime;
            }
            else
            {
                holdSpace = 0f;
            }
        }

        //sets the fill amount of the charge bar to the amount of stamina left, and recharges stamina
        stamBar.fillAmount = stamLeft / stamMax;
        if (!isDashing){
            if(stamLeft < stamMax){
                stamLeft += Time.deltaTime;
            }
            if(stamLeft > stamMax){
                stamLeft = stamMax;
            }
        }

        //changes y rotation of the player body based on mouse movement in the y rotation
        mouseX = Input.GetAxisRaw("Mouse X") * mouseSens * Time.deltaTime;
        rotationAmount = new Vector3(0f, mouseX, 0f);
        transform.Rotate(rotationAmount);

        //grabs direction of the player WASD, including diagonals, and adds it to xMove/zMove
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        //checks if the player is trying to heal, if they are trying to heal, it disallows them from moving
        if (Input.GetKey(KeyCode.H)){
            repairProgress += Time.deltaTime;
            if(repairProgress >= repairThreshold && playerHealth.currentHealth < playerHealth.maxHealth)
            {  
                playerHealth.startHealthRegen();
                x = 0;
                z = 0;
            }
        }
        else{
            repairProgress = 0;
        }

        //realizes the x and z components the player is moving with into a vector3 direction
        moveDir = (transform.right * x + transform.forward * z);
        if (x != 0 || z != 0)
        {
            moveDir = moveDir.normalized;
        }

        //changes the rotation of the player to a specified point through linear interpolation
        if (Mathf.Abs(posToRotateTo) > 0.02f){
            rotationAmount = new Vector3(0f, Mathf.Lerp(0, posToRotateTo, snappiness), 0f);
            transform.Rotate(rotationAmount);
            posToRotateTo -= posToRotateTo * snappiness;
        }
        else{
            posToRotateTo = 0;
        }

        

        //on a doublepress of LShift, the player will be boosted in the direction they are facing for a short amount of time
        if (Input.GetKeyDown(KeyCode.LeftShift) && stamLeft >= stamUsedOnMove)
        {
            doubleShift += 1f;
        }
        if (doubleShift > 0f)
        {
            timeInterval += Time.deltaTime;
        }
        if (timeInterval > 0.5f)
        {
            doubleShift = 0f;
            timeInterval = 0f;
        }
        if (doubleShift == 2f)
        {
            doubleShift = 0f;
            timeInterval = 0f;
            
            if(!isDashing)
            {
                isDashing = true;
                if(moveDir.magnitude != 0){
                    dash = moveDir * moveSpeed * 2f;
                }
                else{
                    dash = transform.forward * moveSpeed * 2f;
                }
                    
                velocity.y = dashJump;
                gravity /= 2;
                stamLeft -= stamUsedOnMove;
            }
        }
        if(isDashing)
        {
            dashingLeft -= Time.deltaTime;
            if(dashingLeft <= 0)
            {
                dashingLeft = dashingThreshold;
                isDashing = false;
                move = dash;
                dash *= 0;
                gravity *= 2;
            }
        }

        //Improved movement system. Lerps character movement to different values based on certain movement states, and allows for dashing.
        if (!isSliding){
            if (isGrounded)
            {
                if (Input.GetButton("Crouch")){
                    move = Vector3.Lerp(move, moveDir * moveSpeed * (crouchSlowModifier), 0.02f);
                    isCrouching = true;
                }
                else if (Input.GetKey(KeyCode.LeftShift) && z > 0){
                    move = Vector3.Lerp(move, moveDir * moveSpeed * (runAddModifier + 1), 0.02f);
                    isSprinting = true;
                    isCrouching = false;
                }
                else{
                    move = Vector3.Lerp(move, moveDir * moveSpeed, 0.02f);
                    isSprinting = false;
                    isCrouching = false;
                }
            
            }
            else{
                if(moveDir.magnitude != 0)
                {
                    if (move.magnitude > moveSpeed){
                        move = Vector3.Lerp(move, moveDir * moveSpeed * (runAddModifier + 1), 0.02f / airControl);
                    }
                    else{
                        move = Vector3.Lerp(move, moveDir * moveSpeed, 0.02f / airControl);
                    }
                }
                else if (move.magnitude > moveSpeed)
                {
                    move = Vector3.Lerp(move, move.normalized * moveSpeed * (runAddModifier + 1), 0.02f / airControl);
                }
            }
        }
            
        
        //if the player moves forward and crouches while sprinting, then they will enter a slide. A slide moves them in at a sprint speed and prevents them from 
        if(isSprinting){
            if(Input.GetButton("Crouch") && !isSliding){
                move = transform.forward * moveSpeed * (runAddModifier + 1);
                isSliding = true;
                slideLeft = slideTime;
            }
        }
        if(isSliding || Input.GetButtonUp("Jump")){
            slideLeft -= Time.deltaTime;
            if(slideLeft <= 0){
                isSliding = false;
                isSprinting = false;
            }
        }

        //move the character controller by its move vector
        if (!isDashing)
        {
            controller.Move(move * Time.deltaTime);
        }else{
            controller.Move(dash * Time.deltaTime);
        }
        
        isGrounded = false;    
        controller.Move(velocity * Time.deltaTime);
        if (!isGrounded && !fell && velocity.y < 0){
            velocity *= 0;
            fell = true;
        }
        //------------------------------------PROTECTED------------------------------------//
        
    }

    public void rotateHorizontally(float amount)
    {
        posToRotateTo += amount;
    }

    void OnControllerColliderHit(ControllerColliderHit controllerHit){
        hitAngle = (Mathf.Atan(Mathf.Sqrt(controllerHit.normal.x * controllerHit.normal.x + controllerHit.normal.z * controllerHit.normal.z) / controllerHit.normal.y)) * 180 / Mathf.PI;
        
        if(controllerHit.normal.y > 0 && hitAngle < 45){
            isGrounded = true;
            fell = false;
            if(velocity.y < 0){
                velocity.y = -move.magnitude * Mathf.Tan(hitAngle / 180 * Mathf.PI) - 3;
                if(Mathf.Abs(velocity.x + velocity.z) > 0){
                    move = new Vector3(move.x + velocity.x * 2, 0, move.z + velocity.z * 2);
                    velocity = new Vector3(0, velocity.y, 0);
                }
            }
        }
        else if(hitAngle >= 45 && velocity.y < 0){
            velocity = new Vector3(-controllerHit.normal.normalized.x * (Mathf.Tan((90 - hitAngle) / 180 * Mathf.PI) * velocity.y), velocity.y, -controllerHit.normal.normalized.z * (Mathf.Tan((90 - hitAngle) / 180 * Mathf.PI) * velocity.y));
        }
    }
}