using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Import the SceneManager namespace

public class Obstacle : MonoBehaviour
{
    // This method is called when another collider enters the trigger collider attached to the object where this script is attached
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that collided with the obstacle is the player
        if (other.CompareTag("Player"))
        {
            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}