using UnityEngine;
using UnityEngine.SceneManagement; // For reloading the scene

public class Buble : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float initialSpeed = 2f; // Initial speed of the enemy
    public float speedIncreaseRate = 0.1f; // Rate at which the enemy's speed increases per second
    public float catchDistance = 1f; // Distance at which the enemy catches the player

    private float currentSpeed; // Current speed of the enemy

    private void Start()
    {
        // Initialize the current speed
        currentSpeed = initialSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        transform.LookAt(player.transform);
        // Calculate the direction towards the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Move the enemy towards the player
        transform.position += direction * currentSpeed * Time.deltaTime;

        // Gradually increase the enemy's speed over time
        currentSpeed += speedIncreaseRate * Time.deltaTime;

        // Check if the enemy has reached the player
        if (Vector3.Distance(transform.position, player.position) <= catchDistance)
        {
            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}