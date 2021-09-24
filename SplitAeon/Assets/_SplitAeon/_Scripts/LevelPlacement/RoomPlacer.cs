using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlacer : MonoBehaviour
{
    [System.Serializable]
    public struct RoomPair
    {
        public GameObject pastRoom;
        public GameObject futureRoom;
    }

    public Vector3 offset = Vector3.zero;

    public List<RoomPair> roomPairs;
}
