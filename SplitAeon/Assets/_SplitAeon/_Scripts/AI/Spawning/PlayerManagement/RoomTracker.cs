using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTracker : MonoBehaviour
{
    [HideInInspector] public RoomBounds currentRoom;
    RoomBounds m_previousRoom;
    [HideInInspector] public RoomBounds previousRoom { get { return m_previousRoom; } set { m_previousRoom = value; if (value) { Debug.Log(value.name); } else { Debug.Log("null"); } } }

    public int index = 0;
    public string roomName = "";
    public string prevName = "";

    StateMachine<RoomTracker> stateMachine;

    static RoomBounds[] _allRooms;
    public static RoomBounds[] allRooms { get { return _allRooms; } }

    // Start is called before the first frame update
    void Start()
    {
        _allRooms = FindObjectsOfType<RoomBounds>();

        foreach(var room in _allRooms)
        {
            if(room.EntryContainsPoint(transform.position))
            {
                currentRoom = room;
            }
        }

        if(currentRoom == null)
        {
            // Player did not start in a room, should find closest
            float lowestDistance = _allRooms[0].Distance(transform.position);
            currentRoom = _allRooms[0];

            for(int  i = 1; i < _allRooms.Length; i++)
            {
                float dist = _allRooms[i].Distance(transform.position);
                if(dist < lowestDistance)
                {
                    lowestDistance = dist;
                    currentRoom = _allRooms[i];
                }
            }
        }

        previousRoom = currentRoom;

        stateMachine = new StateMachine<RoomTracker>(this);
        RoomTrackerStateBucket.SetUpStateMachine(stateMachine);
        stateMachine.Init();
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        index = stateMachine.currentIndex;
        if(currentRoom != null)
        {
            roomName = currentRoom.name;
        }
        else
        {
            roomName = "null";
        }
        prevName = previousRoom.name;
    }

    public void SetState(int stateIndex)
    {
        stateMachine.ChangeState(stateIndex);
    }

    public bool FindCurrentRoomFromPrevNeighbours()
    {
        Vector3 position = transform.position;
        // Try to find neighbour room of previous
        foreach (RoomBounds neighbour in previousRoom.neighbours)
        {
            if (neighbour.EntryContainsPoint(position))
            {
                currentRoom = neighbour;
                return true;
            }
        }

        // did not find a relevant neighbour
        return false;
    }

    private void OnDestroy()
    {
        _allRooms = null;
    }
}
