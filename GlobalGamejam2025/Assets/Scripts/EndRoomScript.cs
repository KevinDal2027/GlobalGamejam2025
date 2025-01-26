using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRoomScript : MonoBehaviour
{
    SceneLoader sceneLoader;

    private void Start()
    {
        sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Cursor.lockState = CursorLockMode.None;
        if (other.CompareTag("Player"))
        {
            sceneLoader.LoadScene(2);
        }
    }
}
