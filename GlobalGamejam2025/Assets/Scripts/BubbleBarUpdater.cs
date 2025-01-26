using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubbleBarUpdater : MonoBehaviour
{
    private Image image;
    [SerializeField] FirstPersonMovement firstPersonMovement;
    float initialBubbleSolutionAmount;
    void Start()
    {
        image = GetComponent<Image>();
        initialBubbleSolutionAmount = firstPersonMovement.bubbleSolutionAmount;
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = firstPersonMovement.bubbleSolutionAmount / initialBubbleSolutionAmount;
    }
}
