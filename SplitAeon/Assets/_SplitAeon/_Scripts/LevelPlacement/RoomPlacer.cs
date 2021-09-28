using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlacer : MonoBehaviour
{
    public Vector3 offset = Vector3.zero;
    public List<RoomPair> roomPairs;

    [System.Serializable]
    public class RoomPair
    {
        public string roomName = "New Room Pair";
        public GameObject pastRoom;
        public GameObject futureRoom;

        public void AlignToPast(Vector3 offset)
        {
            if(IsFullyAssigned())
            {
                AlignTransforms(pastRoom.transform, futureRoom.transform, offset);
            }
        }

        public void AlignToFuture(Vector3 offset)
        {
            if (IsFullyAssigned())
            {
                AlignTransforms(futureRoom.transform, pastRoom.transform, -offset);
            }
        }

        void AlignTransforms(Transform origin, Transform target, Vector3 offset)
        {
            target.transform.position = origin.transform.position + offset;
            target.transform.rotation = origin.transform.rotation;
            target.transform.localScale = origin.transform.localScale;
        }

        bool IsFullyAssigned()
        {
            return pastRoom != null && futureRoom != null;
        }
    }

    public void AlignRoomPairToPast(RoomPair roomPair)
    {
        roomPair.AlignToPast(offset);
    }

    public void AlignRoomPairToFuture(RoomPair roomPair)
    {
        roomPair.AlignToFuture(offset);
    }

    public void AlignAllRoomPairsToPast()
    {
        foreach(RoomPair roomPair in roomPairs)
        {
            AlignRoomPairToPast(roomPair);
        }
    }

    public void AlignAllRoomPairsToFuture()
    {
        foreach (RoomPair roomPair in roomPairs)
        {
            AlignRoomPairToFuture(roomPair);
        }
    }
}
