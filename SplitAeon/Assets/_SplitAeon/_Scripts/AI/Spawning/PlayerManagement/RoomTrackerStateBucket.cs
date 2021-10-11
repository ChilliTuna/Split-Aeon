using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomTrackerStateBucket
{
    public static void SetUpStateMachine(StateMachine<RoomTracker> target)
    {
        target.AddState(new FindStateDirection());
        target.AddState(new InRoom());
        target.AddState(new TryFindNeighbour());
        target.AddState(new TryFindEnter());
    }
}

public class FindStateDirection : IState<RoomTracker>
{
    void IState<RoomTracker>.Enter(RoomTracker tracker)
    {

    }

    void IState<RoomTracker>.Update(RoomTracker tracker)
    {
        // Try to find neighbour room of previous first
        if (tracker.FindCurrentRoomFromPrevNeighbours())
        {
            tracker.SetState(1);
        }

        // If we get here we did not enter another room
        // now we should try to find the neighbour room repeatedly for a small amount of time
        tracker.SetState(2);
    }

    void IState<RoomTracker>.Exit(RoomTracker tracker)
    {

    }
}

public class InRoom : IState<RoomTracker>
{
    void IState<RoomTracker>.Enter(RoomTracker tracker)
    {
        
    }

    void IState<RoomTracker>.Update(RoomTracker tracker)
    {
        Vector3 position = tracker.transform.position;
        // check if the player has exited the current room
        if (!tracker.currentRoom.ExitContainsPoint(position))
        {
            tracker.SetState(0);// Change to Find State
        }
    }

    void IState<RoomTracker>.Exit(RoomTracker tracker)
    {
        tracker.previousRoom = tracker.currentRoom;
        tracker.currentRoom = null;
    }
}

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
            tracker.SetState(3);
        }

        // Try to find neighbour room of previous
        if (tracker.FindCurrentRoomFromPrevNeighbours())
        {
            tracker.SetState(1);
        }
        tryCount++;
    }

    void IState<RoomTracker>.Exit(RoomTracker tracker)
    {

    }
}

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
