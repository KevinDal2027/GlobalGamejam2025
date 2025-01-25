using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    CharacterController characterController;
    public float speed = 12f;
    public float gravity = -12f;
    public float jumpHeight = 4f;
    public float slideSpeed = 12f; // Sliding speed, should be less than or equal to running speed
    public float initialSlideSpeed = 30f;
    public float slideDeceleration = 5f; // Rate at which speed decreases during sliding
    public float fastDescentGravity = -20f; // Gravity when sliding in the air
    public float bubbleGravity = -2f; // Gravity when floating in the bubble
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    Vector3 slideDirection;
    bool isGrounded;
    bool isSliding = false;
    bool isInBubble = false;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            isInBubble = false; // Reset bubble state when grounded
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!isSliding)
            {
                // Capture the direction when sliding starts
                slideDirection = move.normalized;
                isSliding = true;
            }

            // Move in the captured slide direction
            characterController.Move(slideDirection * slideSpeed * Time.deltaTime);
            slideSpeed = Mathf.Max(0, slideSpeed - slideDeceleration * Time.deltaTime); // Gradually reduce speed

            if (!isGrounded)
            {
                isInBubble = false; // Pop the bubble if in the air
                velocity.y = fastDescentGravity; // Fast descent when sliding in the air
            }
        }
        else
        {
            isSliding = false;
            slideSpeed = initialSlideSpeed; // Reset slide speed when not sliding
            characterController.Move(move * speed * Time.deltaTime); // Allow normal movement
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
            else if (!isInBubble)
            {
                isInBubble = true; // Activate bubble
            }
        }

        if (isInBubble)
        {
            velocity.y += bubbleGravity * Time.deltaTime; // Slow descent in bubble
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);
    }
}