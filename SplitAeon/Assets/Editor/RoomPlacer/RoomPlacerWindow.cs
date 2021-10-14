using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomPlacerWindow : EditorWindow
{
    RoomPlacer m_roomPlacer;

    RoomPlacer m_previousSelection = null;

    bool m_manualChange = false;

    [MenuItem("Window/Room Placer")]
    static void CreateWindow()
    {
        EditorWindow.GetWindow<RoomPlacerWindow>();
    }

    private void OnGUI()
    {
        m_roomPlacer = EditorGUILayout.ObjectField(m_roomPlacer, typeof(RoomPlacer), true) as RoomPlacer;
        ValidateRoomPlacer();

        if (GUILayout.Button("Find Room Placer"))
        {
            m_roomPlacer = Object.FindObjectOfType<RoomPlacer>();
            if(m_roomPlacer != null)
            {
                RoomPlacerSceneManager.SetRoomPlacer(m_roomPlacer);
                m_manualChange = true;
            }
            else
            {
                Debug.LogWarning("No Active Room Placer was found in the Scene.");
            }
        }
        if (GUILayout.Button("Clear Room Placer"))
        {
            RoomPlacerSceneManager.ClearRoomPlacer();
            m_roomPlacer = null;
            m_manualChange = true;
        }

        GUILayout.Label("Layout Options", EditorStyles.boldLabel);
        RoomPlacerEditor.showLayoutOptions = GUILayout.Toggle(RoomPlacerEditor.showLayoutOptions, "Show options in inspector");
        RoomPlacerEditor.ExtraOptionsLayout();
    }

    private void ValidateRoomPlacer()
    {
        if (m_manualChange)
        {
            if (m_roomPlacer != m_previousSelection)
            {
                // Field was changed
                RoomPlacerSceneManager.SetRoomPlacer(m_roomPlacer);
            }
            m_previousSelection = m_roomPlacer;
        }
        m_manualChange = false;
    }
}
