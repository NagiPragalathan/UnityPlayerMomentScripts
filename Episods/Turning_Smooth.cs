using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Movement")]
    public float playerSpeed = 1.9f;

    [Header("Player Animator and Gravity")]
    public CharacterController cC;

    [Header("Player Jumping & Velocity")]
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;

    void Update() // Corrected method name to "Update" with capital "U"
    {
        PlayerMove(); // Corrected method name to "PlayerMove" with capital "P" and added missing parentheses
    }

    void PlayerMove() // Corrected method name to "PlayerMove" with capital "P" and added missing parentheses
    {
        // Getting input from user
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        //  Calculating the direction vector of the player's
        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Player Facing direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnCalmVelocity, turnCalmTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            // Player Movement
            cC.Move(direction * playerSpeed * Time.deltaTime);
        }
    }
}
