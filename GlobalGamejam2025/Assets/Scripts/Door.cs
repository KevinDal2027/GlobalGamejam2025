using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Notify the ProceduralRoomGenerator to generate new rooms
            ProceduralRoomGenerator.Instance.OnPlayerEnterDoor();
        }
    }
}