using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomPlacer))]
public class RoomPlacerEditor : Editor
{
    Vector3 m_prevOffset = Vector3.zero;

    bool m_isEditing = false;

    static int _elementPixelSpace = 21;
    static int _beginPixelSpace = 10;
    static int _endPixelSpace = 5;

    public static bool showLayoutOptions = false;

    public override void OnInspectorGUI()
    {
        RoomPlacer roomPlacer = target as RoomPlacer;

        InspectorLayout(roomPlacer);

        ValidateEditor(roomPlacer);
    }

    public void ValidateEditor(RoomPlacer roomPlacer)
    {
        // check if the room placer that is being edited is the room placer that is currently being managed.
        if(!RoomPlacerSceneManager.CompareRoomPlacer(roomPlacer))
        {
            // if this room placer does not equal themanaged one, we simply set the manager to this one.
            RoomPlacerSceneManager.SetRoomPlacer(roomPlacer);
        }

        // room placer is now the same as the managed one
        // finish validations
        ValidateOffset(roomPlacer);
    }

    void ValidateOffset(RoomPlacer roomPlacer)
    {
        if (m_prevOffset != roomPlacer.offset)
        {
            roomPlacer.AlignAllRoomPairsToPast();
        }
        m_prevOffset = roomPlacer.offset;
    }

    void InspectorLayout(RoomPlacer roomPlacer)
    {
        int beginPixelSpace;
        if (m_isEditing)
        {
            beginPixelSpace = _beginPixelSpace - 2;
        }
        else
        {
            beginPixelSpace = _beginPixelSpace;
        }

        roomPlacer.offset = EditorGUILayout.Vector3Field("Offset", roomPlacer.offset);

        GUILayout.Space(beginPixelSpace);
        RoomPairListLayout(roomPlacer);

        ButtonsLayout(roomPlacer);

        if (showLayoutOptions)
        {
            GUILayout.Label("Layout Options", EditorStyles.boldLabel);
            ExtraOptionsLayout();
        }
    }

    void RoomPairLayout(RoomPlacer roomPlacer, RoomPlacer.RoomPair roomPair)
    {
        if(!m_isEditing)
        {
            GUILayout.Label(roomPair.roomName, EditorStyles.boldLabel);
        }
        else
        {
            EditorGUILayout.TextField(roomPair.roomName);
            GUILayout.Space(-1);
        }
        roomPair.pastRoom = EditorGUILayout.ObjectField("Past Room", roomPair.pastRoom, typeof(GameObject), true) as GameObject;
        roomPair.futureRoom = EditorGUILayout.ObjectField("Future Room", roomPair.futureRoom, typeof(GameObject), true) as GameObject;

        if(m_isEditing)
        {
            EditorGUILayout.BeginVertical();
            if(GUILayout.Button("Remove"))
            {
                RoomPlacerSceneManager.RemoveRoomPairData(roomPair);
                roomPlacer.roomPairs.Remove(roomPair);
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(_elementPixelSpace - 23);
        }
        else
        {
            GUILayout.Space(_elementPixelSpace);
        }
    }

    void RoomPairListLayout(RoomPlacer roomPlacer)
    {
        for (int i = 0; i < roomPlacer.roomPairs.Count; i++)
        {
            var roomPair = roomPlacer.roomPairs[i];
            RoomPairLayout(roomPlacer, roomPair);
        }
        if(m_isEditing)
        {
            GUILayout.Space(_endPixelSpace + 2);
        }
        else
        {
            GUILayout.Space(_endPixelSpace);
        }
    }

    void ButtonsLayout(RoomPlacer roomPlacer)
    {
        if (GUILayout.Button("Add New"))
        {
            var newRoom = new RoomPlacer.RoomPair();
            roomPlacer.roomPairs.Add(newRoom);
            RoomPlacerSceneManager.AddRoomPairData(newRoom);

        }
        if (GUILayout.Button("Edit Room Pair"))
        {
            m_isEditing = !m_isEditing;
        }
    }

    public static void ExtraOptionsLayout()
    {
        GUILayout.Label("Pixel Space");
        SliderOptionLayout("Element", ref _elementPixelSpace, 21, 100);
        SliderOptionLayout("Begin", ref _beginPixelSpace, 5, 100);
        SliderOptionLayout("End", ref _endPixelSpace, 5, 100);
    }

    static void SliderOptionLayout(string label, ref int target, int min, int max)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        target = EditorGUILayout.IntSlider(label, target, min, max);
        EditorGUILayout.EndHorizontal();
    }
}