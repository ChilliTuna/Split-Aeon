using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public OpenableDoor[] targetDoors;

    public ToggleDoorPair[] targetDoorPairs;

    [System.Serializable]
    public class ToggleDoorPair
    {
        public GameObject before;
        public GameObject after;

        public void Open()
        {
            before.SetActive(false);
            after.SetActive(true);
        }

        public void Close()
        {
            before.SetActive(true);
            after.SetActive(false);
        }
    }

    public void OpenDoors()
    {
        foreach(var door in targetDoors)
        {
            door.Open();
        }

        foreach (var pair in targetDoorPairs)
        {
            pair.Open();
        }
    }

    public void CloseDoors()
    {
        foreach (var door in targetDoors)
        {
            door.Close();
        }

        foreach (var pair in targetDoorPairs)
        {
            pair.Close();
        }
    }
}
