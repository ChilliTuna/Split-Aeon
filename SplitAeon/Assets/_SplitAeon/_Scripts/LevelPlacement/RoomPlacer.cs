using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlacer : MonoBehaviour
{
    public Vector3 offset = Vector3.zero;
    public List<GameObjectPairList> roomPairsList;
    //public List<GameObjectPair> roomItemPairs;

    [System.Serializable]
    public class GameObjectPairList
    {
        public string listName = "New Room Pair List";
        public List<GameObjectPair> roomPairs;

        public RoomPlacer owner;
        public bool shouldShow = true; // I don't like putting this here, but it would be a massive headache to figure out where it goes in editor.
        public bool isRoomItemList = false;

        public GameObjectPairList(RoomPlacer roomPlacer)
        {
            owner = roomPlacer;
            roomPairs = new List<GameObjectPair>();
        }

        public void SmartAlignAllRooms()
        {
            if(!isRoomItemList)
            {
                foreach (GameObjectPair roomPair in roomPairs)
                {
                    AlignRoomPairToPast(roomPair);
                }
            }
        }

        public void AlignAllRoomPairsToPast()
        {
            foreach (GameObjectPair roomPair in roomPairs)
            {
                AlignRoomPairToPast(roomPair);
            }
        }

        public void AlignAllRoomPairsToFuture()
        {
            foreach (GameObjectPair roomPair in roomPairs)
            {
                AlignRoomPairToFuture(roomPair);
            }
        }

        public void AlignRoomPairToPast(GameObjectPair roomPair)
        {
            roomPair.AlignToPast(owner.offset);
        }

        public void AlignRoomPairToFuture(GameObjectPair roomPair)
        {
            roomPair.AlignToFuture(owner.offset);
        }
    }

    [System.Serializable]
    public class GameObjectPair
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

        public void AlignItemToRoom(Vector3 roomOffset)
        {
            if(IsFullyAssigned())
            {
                Transform item = futureRoom.transform;
                Transform room = pastRoom.transform;

                item.transform.position = room.transform.position + roomOffset;
                item.transform.rotation = room.transform.rotation;
            }
        }

        public void AlignToFuture(Vector3 offset)
        {
            if (IsFullyAssigned())
            {
                AlignTransforms(futureRoom.transform, pastRoom.transform, -offset);
            }
        }

        static void AlignTransforms(Transform origin, Transform target, Vector3 offset)
        {
            target.transform.position = origin.transform.position + offset;
            target.transform.rotation = origin.transform.rotation;
            target.transform.localScale = origin.transform.localScale;
        }

        public bool IsFullyAssigned()
        {
            return pastRoom != null && futureRoom != null;
        }
    }

    public void CreateNewList()
    {
        roomPairsList.Add(new GameObjectPairList(this));
    }

    public void RemoveLastList()
    {
        roomPairsList.RemoveAt(roomPairsList.Count - 1);
    }

    private void Start()
    {
        if(!Application.isEditor)
        {
            Destroy(this);
        }
    }
}
