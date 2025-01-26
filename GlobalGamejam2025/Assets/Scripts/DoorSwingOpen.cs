using UnityEngine;

public class DoorSwingOpen : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float openDistance = 5f; // Distance at which the door should open
    private Animator animator; // Reference to the Animator component
    private bool doorOpen = false;
    
    private void Start()
    {
        // Get the Animator component attached to the door
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Calculate the distance between the player and the door
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        print(distanceToPlayer);
        // Check if the player is within the specified distance
        if (distanceToPlayer <= openDistance && !doorOpen)
        {
            // Play the door open animation
            animator.SetTrigger("OpenDoor");
            doorOpen = true;
        }
    }
}