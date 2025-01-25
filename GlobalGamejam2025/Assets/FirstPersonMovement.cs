using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    CharacterController characterController;
    public float speed = 12f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    public float slideSpeed = 6f;
    public float slideDeceleration = 0.5f; // Rate at which speed decreases during sliding
    public float fastDescentGravity = -20f; // Gravity when sliding in the air
    public float bubbleGravity = -2f; // Gravity when floating in the bubble
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
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
        characterController.Move(move * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (isGrounded)
            {
                isSliding = true;
                characterController.Move(move * slideSpeed * Time.deltaTime);
                slideSpeed = Mathf.Max(0, slideSpeed - slideDeceleration * Time.deltaTime); // Gradually reduce speed
            }
            else
            {
                isInBubble = false; // Pop the bubble if in the air
                velocity.y = fastDescentGravity; // Fast descent when sliding in the air
            }
        }
        else
        {
            isSliding = false;
            slideSpeed = 6f; // Reset slide speed when not sliding
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