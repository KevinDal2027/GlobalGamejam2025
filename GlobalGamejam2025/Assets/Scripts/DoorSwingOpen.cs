using UnityEngine;

public class DoorSwingOpen : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float openDistance = 5f; // Distance at which the door should open
    private Animation animation; // Reference to the Animator component

    private void Start()
    {
        // Get the Animator component attached to the door
        animation = GetComponent<Animation>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Calculate the distance between the player and the door
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        // Check if the player is within the specified distance
        if (distanceToPlayer <= openDistance)
        {
            // Play the door open animation
            animation.Play();
        }
    }
}