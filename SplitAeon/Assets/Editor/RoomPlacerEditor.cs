using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomPlacer))]
public class RoomPlacerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RoomPlacer roomPlacer = target as RoomPlacer;
        if (GUILayout.Button("Align pairs to Past"))
        {
            foreach(var roomPair in roomPlacer.roomPairs)
            {
                roomPair.futureRoom.transform.position = roomPair.pastRoom.transform.position + roomPlacer.offset;
            }
        }
        if (GUILayout.Button("Align pairs to Future"))
        {
            foreach (var roomPair in roomPlacer.roomPairs)
            {
                roomPair.pastRoom.transform.position = roomPair.futureRoom.transform.position - roomPlacer.offset;
            }
        }
    }
}
