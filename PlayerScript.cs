using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 1.9f;

    // Taking the reference or main camera 
    [Header("Player Camera")]
    public Transform playerCamera;

    [Header("Player Animator and Gravity")]
    public CharacterController cC;

    [Header("Player Jumping & Velocity")]
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;

    void Update()
    {
        PlayerMove();
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
            // Player facing direction, considering the camera's rotation
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // Apply the rotation to the player

            // Calculate movement direction based on the target angle
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            // Player Movement
            cC.Move(moveDirection * playerSpeed * Time.deltaTime);
        }
    }
}
