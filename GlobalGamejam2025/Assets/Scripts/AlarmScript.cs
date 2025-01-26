using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmScript : MonoBehaviour
{
    public Light pointLight; // Reference to the Unity Light component
    public float minIntensity = 1f; // Minimum intensity of the light
    public float maxIntensity = 3f; // Maximum intensity of the light
    public float pulseSpeed = 2f; // Speed of the pulsing effect

    void Start()
    {
        if (pointLight == null)
        {
            pointLight = GetComponent<Light>();
        }
    }

    void Update()
    {
        if (pointLight != null)
        {
            // Calculate the intensity using a sine wave
            float intensity = minIntensity + Mathf.PingPong(Time.time * pulseSpeed, maxIntensity - minIntensity);
            pointLight.intensity = intensity;
        }
    }
}