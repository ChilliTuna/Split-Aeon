using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomTrackerStateBucket
{
    public static void SetUpStateMachine(StateMachine<RoomTracker> target)
    {
        target.AddState(new FindStateDirection()); // 0
        target.AddState(new InRoom()); // 1
        target.AddState(new TryFindNeighbour()); // 2
        target.AddState(new TryFindEnter()); // 3
    }
}

// 0
public class FindStateDirection : IState<RoomTracker>
{
    void IState<RoomTracker>.Enter(RoomTracker tracker)
    {
        // Try to find neighbour room of previous first
        if (tracker.FindCurrentRoomFromPrevNeighbours())
        {
            Debug.Log("Found Neighbour: instantly");
            tracker.SetState(1);
            return;
        }
        else
        {
            // If we get here we did not enter another room
            // now we should try to find the neighbour room repeatedly for a small amount of time
            tracker.SetState(2);
        }
    }

    void IState<RoomTracker>.Update(RoomTracker tracker)
    {
        // We should never have reached here, but just in case set the state to search for rooms
        tracker.SetState(2);
    }

    void IState<RoomTracker>.Exit(RoomTracker tracker)
    {

    }
}

// 1
public class InRoom : IState<RoomTracker>
{
    void IState<RoomTracker>.Enter(RoomTracker tracker)
    {
        tracker.currentRoom.enterRoom.Invoke();
    }

    void IState<RoomTracker>.Update(RoomTracker tracker)
    {
        Vector3 position = tracker.transform.position;
        // check if the player has exited the current room
        if (!tracker.currentRoom.ExitContainsPoint(position))
        {
            Debug.Log("Exit room");
            tracker.SetState(0);// Change to Find State
        }
    }

    void IState<RoomTracker>.Exit(RoomTracker tracker)
    {
        tracker.currentRoom.exitRoom.Invoke();

        tracker.previousRoom = tracker.currentRoom;
        tracker.currentRoom = null;
    }
}

// 2
public class TryFindNeighbour : IState<RoomTracker>
{
    int tryCount = 0;

    void IState<RoomTracker>.Enter(RoomTracker tracker)
    {
        tryCount = 0;
    }

    void IState<RoomTracker>.Update(RoomTracker tracker)
    {
        if (tryCount > 60)
        {
            Debug.Log("Could not find neighbour");
            tracker.SetState(3);
        }

        // Try to find neighbour room of previous
        if (tracker.FindCurrentRoomFromPrevNeighbours())
        {
            Debug.Log("Found current from prev neighbours");
            tracker.SetState(1);
        }
        tryCount++;
    }

    void IState<RoomTracker>.Exit(RoomTracker tracker)
    {

    }
}

// 3
public class TryFindEnter : IState<RoomTracker>
{
    RoomBounds enterRoom = null;

    void IState<RoomTracker>.Enter(RoomTracker tracker)
    {
        enterRoom = null;
    }

    void IState<RoomTracker>.Update(RoomTracker tracker)
    {
        foreach(var room in RoomTracker.allRooms)
        {
            if(room.EntryContainsPoint(tracker.transform.position))
            {
                Debug.Log("Found new room from all");
                enterRoom = room;
                tracker.SetState(1);
            }
        }
    }

    void IState<RoomTracker>.Exit(RoomTracker tracker)
    {
        tracker.currentRoom = enterRoom;
    }
}
