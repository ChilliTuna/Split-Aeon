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

        public bool AlignAllRoomPairsToPast()
        {
            bool hasChanged = false;
            foreach (GameObjectPair roomPair in roomPairs)
            {
                if(AlignRoomPairToPast(roomPair))
                {
                    hasChanged = true;
                }
            }
            return hasChanged;
        }

        public bool AlignRoomPairToPast(GameObjectPair roomPair)
        {
            return roomPair.AlignToPast(owner.offset);
        }
    }

    [System.Serializable]
    public class GameObjectPair
    {
        public string roomName = "New Room Pair";
        public GameObject pastRoom;
        public GameObject futureRoom;

        public bool AlignToPast(Vector3 offset)
        {
            if(IsFullyAssigned())
            {
                return AlignTransforms(pastRoom.transform, futureRoom.transform, offset);
            }
            else
            {
                return false;
            }
        }

        public bool AlignItemToRoom(Vector3 roomOffset)
        {
            if(IsFullyAssigned())
            {
                Transform item = futureRoom.transform;
                Transform room = pastRoom.transform;

                var prevItem = TransformValues.CreateNew(item);

                item.transform.position = room.transform.position + roomOffset;
                item.transform.rotation = room.transform.rotation;

                return prevItem.DetectChange(item);
            }
            else
            {
                return false;
            }
        }

        public void AlignToFuture(Vector3 offset)
        {
            if (IsFullyAssigned())
            {
                AlignTransforms(futureRoom.transform, pastRoom.transform, -offset);
            }
        }

        static bool AlignTransforms(Transform origin, Transform target, Vector3 offset)
        {
            var prevTarget = TransformValues.CreateNew(target);

            target.transform.position = origin.transform.position + offset;
            target.transform.rotation = origin.transform.rotation;
            target.transform.localScale = origin.transform.localScale;

            return prevTarget.DetectChange(target);
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

public struct TransformValues
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    static TransformValues _identity;
    public static TransformValues identity { get { return _identity; } }

    public TransformValues(Transform transform)
    {
        if (transform != null)
        {
            position = transform.position;
            rotation = transform.rotation;
            scale = transform.localScale;
        }
        else
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
            scale = Vector3.zero;
        }
    }

    static TransformValues()
    {
        _identity = new TransformValues();
    }

    public bool DetectChange(TransformValues target)
    {
        return DetectChange(this, target);
    }

    public bool DetectChange(Transform target)
    {
        return DetectChange(this, CreateNew(target));
    }

    public static TransformValues CreateNew(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        TransformValues result = TransformValues._identity;
        result.position = position;
        result.rotation = rotation;
        result.scale = scale;
        return result;
    }

    public static TransformValues CreateNew(Transform target)
    {
        return CreateNew(target.position, target.rotation, target.lossyScale);
    }

    public static bool DetectChange(TransformValues lhs, TransformValues rhs)
    {
        return lhs.position != rhs.position || lhs.rotation != rhs.rotation || lhs.scale != rhs.scale;
    }
}
