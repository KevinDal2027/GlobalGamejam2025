using System.Collections.Generic;
using UnityEngine;

public class ProceduralRoomGenerator : MonoBehaviour
{
    public static ProceduralRoomGenerator Instance { get; private set; }

    public GameObject[] roomPrefabs;  // Array of room prefabs to choose from
    public int roomsAhead = 3;        // Number of rooms to generate ahead of the player
    public Transform player;          // Reference to the player transform

    private Queue<GameObject> activeRooms = new Queue<GameObject>();  // Queue to track active rooms
    private float cumulativeRotation = 0f;  // Track the total rotation applied to the rooms

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Generate initial rooms
        for (int i = 0; i < roomsAhead; i++)
        {
            GenerateRoom();
        }
    }

    public void OnPlayerEnterDoor()
    {
        // Generate a new room and remove the oldest one
        GenerateRoom();
    }

    private void GenerateRoom()
    {
        if (activeRooms.Count > 0)
        {
            GameObject lastRoom = activeRooms.ToArray()[activeRooms.Count - 1];
            ProceduralRoom lastRoomScript = lastRoom.GetComponent<ProceduralRoom>();

            // Get the exit point of the last room
            Transform lastExitPoint = lastRoomScript.exitPoint;

            // Choose a random room prefab and instantiate it
            int roomIndex = Random.Range(0, roomPrefabs.Length);
            GameObject newRoom = Instantiate(roomPrefabs[roomIndex], Vector3.zero, Quaternion.identity);
            ProceduralRoom newRoomScript = newRoom.GetComponent<ProceduralRoom>();

            // Calculate the position offset to align the new room's entry point with the last room's exit point
            Vector3 positionOffset = lastExitPoint.position - newRoom.transform.position;
            newRoom.transform.position += positionOffset;

            // Determine the rotation based on the type of room
            float rotationOffset = 0f;
            if (roomIndex == 1)
            {
                // Turn left (90 degrees counterclockwise)
                rotationOffset = -90f;
            }
            else if (roomIndex == 2)
            {
                // Turn right (90 degrees clockwise)
                rotationOffset = 90f;
            }

            // Update the cumulative rotation
            cumulativeRotation += rotationOffset;

            // Apply the cumulative rotation to the new room
            newRoom.transform.rotation = Quaternion.Euler(0, cumulativeRotation, 0);

            newRoomScript.InitializeRoom();

            // Add the new room to the queue
            activeRooms.Enqueue(newRoom);

            // Remove the oldest room if we have more than the required number of rooms
            if (activeRooms.Count > roomsAhead)
            {
                GameObject oldRoom = activeRooms.Dequeue();
                Destroy(oldRoom);
            }
        }
        else
        {
            // If no rooms are active, just instantiate the first room at the origin
            GameObject firstRoom = Instantiate(roomPrefabs[0], Vector3.zero, Quaternion.identity);
            firstRoom.GetComponent<ProceduralRoom>().InitializeRoom();
            activeRooms.Enqueue(firstRoom);
        }
    }
}