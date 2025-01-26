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
    public Camera mainCamera;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float normalHeight = 2f; // Normal height of the character
    public float slideHeight = 1f;  // Height of the character when sliding
    public float slideCameraOffsetY = -0.5f; // Camera offset when sliding

    public float bubbleSolutionAmount = 10f; // Total amount of bubble solution available
    public float bubbleConsumptionRate = 1f; // Rate at which bubble solution is consumed per second
	public GameObject bubble;

    Vector3 velocity;
    Vector3 slideDirection;
    bool isGrounded;
    private bool isRunning;
    bool isSliding = false;
    bool isInBubble = false;
    private AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioClip slidingClip;
    public AudioClip runningClip;
    public AudioClip bubbleBlowClip;
    public AudioClip bubblePopClip;
    
    private Vector3 originalCameraPosition;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        originalCameraPosition = mainCamera.transform.localPosition;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0 && isInBubble)
        {
            velocity.y = -2f;
            isInBubble = false; // Reset bubble state when grounded
			bubble.SetActive(false);
            audioSource2.PlayOneShot(bubblePopClip);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (move.magnitude >= 0 && !(isInBubble) && !(isSliding) && (isGrounded))
        {
            print("Something");
            isRunning = true;
            isSliding = false;
            PlaySound(runningClip);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!isSliding)
            {
                // Capture the direction when sliding starts
                slideDirection = move.normalized;
                isSliding = true;
                PlaySound(slidingClip);
                isRunning = false;
                // Adjust the character controller for sliding
                characterController.height = slideHeight;

                // Adjust camera position
                mainCamera.transform.localPosition = new Vector3(originalCameraPosition.x, originalCameraPosition.y + slideCameraOffsetY, originalCameraPosition.z);
            }

            // Move in the captured slide direction
            characterController.Move(slideDirection * slideSpeed * Time.deltaTime);
            slideSpeed = Mathf.Max(0, slideSpeed - slideDeceleration * Time.deltaTime); // Gradually reduce speed

            if (!isGrounded && isInBubble)
            {
                isInBubble = false; // Pop the bubble if in the air
				bubble.SetActive(false);
                audioSource2.PlayOneShot(bubblePopClip);
                velocity.y = fastDescentGravity; // Fast descent when sliding in the air
            }
        }
        else
        {
            if (isSliding)
            {
                isRunning = true;
                // Reset the character controller to normal height
                characterController.height = normalHeight;

                // Reset camera position
                mainCamera.transform.localPosition = originalCameraPosition;
            }

            isSliding = false;
            slideSpeed = initialSlideSpeed; // Reset slide speed when not sliding
            characterController.Move(move * speed * Time.deltaTime); // Allow normal movement
        }

        if (!(isRunning) && !(isSliding))
        {
            audioSource.clip = null;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
            else if (!isInBubble && bubbleSolutionAmount > 0)
            {
                isInBubble = true; // Activate bubble
                audioSource2.PlayOneShot(bubbleBlowClip);
				bubble.SetActive(true);
            }
            audioSource.clip = null;
        }

        if (isInBubble)
        {
            velocity.y += bubbleGravity * Time.deltaTime; // Slow descent in bubble
            bubbleSolutionAmount -= bubbleConsumptionRate * Time.deltaTime; // Consume bubble solution

            if (bubbleSolutionAmount <= 0)
            {
                isInBubble = false; // Deactivate bubble if solution runs out
                audioSource2.PlayOneShot(bubblePopClip);
				bubble.SetActive(false);
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        characterController.Move(velocity * Time.deltaTime);
    }

    public float GetCurrentPlayerSpeed()
    {
        if (isSliding)
        {
            return slideSpeed;
        }
        else
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 move = transform.right * x + transform.forward * z;
            return move.magnitude * speed;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource.clip != clip || !audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.loop = clip == runningClip || clip == slidingClip;
            audioSource.Play();
        }
    }
}