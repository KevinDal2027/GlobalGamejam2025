using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVController : MonoBehaviour
{
    public Camera playerCamera;
    public FirstPersonMovement playerMovement;
    public float baseFOV = 60f;  // Base field of view
    public float maxFOV = 90f;   // Maximum field of view
    public float speedForMaxFOV = 20f;  // Speed at which max FOV is reached

    private void Update()
    {
        if (playerMovement != null && playerCamera != null)
        {
            // Get the player's current speed
            float currentSpeed = playerMovement.GetCurrentPlayerSpeed();

            // Calculate the target FOV based on the player's speed
            float targetFOV = Mathf.Lerp(baseFOV, maxFOV, currentSpeed / speedForMaxFOV);

            // Apply the FOV to the camera
            playerCamera.fieldOfView = targetFOV;
        }
    }
}