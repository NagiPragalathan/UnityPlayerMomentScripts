using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 1.9f;
    public float  currentPlayerSpeed = 0f;
    public float playerSprint = 3f;
    public float currentPlayerSprint = 0f;

    // Taking the reference of the main camera 
    [Header("Player Camera")]
    public Transform playerCamera;

    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;
    public Animator animator;


    [Header("Player Jumping & Velocity")]
    public float JumpRange;
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    Vector3 velocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;

    void Update()
    {
        // Check if the player is on the surface
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        // If the player is on the surface and moving downwards, reset the vertical velocity
        if(onSurface && velocity.y < 0){
            velocity.y = -2f;
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;

        // Move the player
        cC.Move(velocity * Time.deltaTime);

        // Move the player according to input
        PlayerMove();

        // Adjust the camera's position to avoid going into the ground
        AdjustCameraPosition();

        //Jump
        Jump();

        //Sprint
        Sprint();
    }

    void PlayerMove()
    {
        // Getting input from user
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        //  Calculating the direction vector of the player's
        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("Walk",true);
            animator.SetBool("Running", false);
            animator.SetBool("Idle",false);
            animator.SetTrigger("Jump");
            animator.SetBool("AimWalk", false);
            animator.SetBool("IdleAim", false);


            // Player facing direction, considering the camera's rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // Apply the rotation to the player

            // Calculate movement direction based on the target angle
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            // Player Movement
            cC.Move(moveDirection * playerSpeed * Time.deltaTime);

            currentPlayerSpeed = playerSpeed;
        }else{
            animator.SetBool("Idle",true);
            animator.SetTrigger("Jump");
            animator.SetBool("Walk",false);
            animator.SetBool("Running", false);
            animator.SetBool("AimWalk", false);
            animator.SetBool("IdleAim", false);
            currentPlayerSpeed = 0f;
        }
    }

    void AdjustCameraPosition()
    {
        // Get the current position of the camera
        Vector3 cameraPosition = playerCamera.position;

        // Set the camera's y-position to be above the ground based on the player's position and the ground's position
        cameraPosition.y = transform.position.y + 1.5f; // Adjust this value as needed

        // Set the camera's position
        playerCamera.position = cameraPosition;
    }
    void Jump(){
        if(Input.GetButtonDown("Jump") && onSurface)
        {
            // Animation
            animator.SetBool("Walk",false);
            animator.SetTrigger("Jump");
            // Function of event...
            velocity.y = Mathf.Sqrt(JumpRange * -2 * gravity);
        }else{
            animator.ResetTrigger("Jump");
        }
    }
    void Sprint()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && onSurface)
        {
            // Getting input from user
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");

            //  Calculating the direction vector of the player's
            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk",false);
                animator.SetBool("Running", true);
                animator.SetBool("Idle",false);
                animator.SetBool("IdleAim", false);
                // Player facing direction, considering the camera's rotation
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f); // Apply the rotation to the player

                // Calculate movement direction based on the target angle
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                // Player Movement
                cC.Move(moveDirection * playerSprint * Time.deltaTime);

                currentPlayerSprint = playerSprint;
            }else{
                animator.SetBool("Idle",false);
                animator.SetBool("Walk",false);
                currentPlayerSprint = 0f;
            }
        }
    }
}


// playerHitDamage
