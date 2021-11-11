using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCheckpoints : MonoBehaviour
{
    OpenableDoor[] openableDoors;

    // Start is called before the first frame update
    void Awake()
    {
        openableDoors = FindObjectsOfType<OpenableDoor>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCheckpoint()
    {
        foreach(var door in openableDoors)
        {
            door.SetCheckpoint();
        }
    }

    public void OnRespawn()
    {
        foreach (var door in openableDoors)
        {
            door.RespawnReset();
        }
    }
}
