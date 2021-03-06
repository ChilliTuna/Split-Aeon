using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class RoomNeighbourTool : EditorWindow
{
    RoomBounds m_first;
    RoomBounds m_second;


    [MenuItem("Window/Room Bounds")]
    static void CreateWindow()
    {
        EditorWindow.GetWindow<RoomNeighbourTool>();
    }

    private void OnGUI()
    {
        GetObjects();

        if (GUILayout.Button("Connect as Neighbours"))
        {
            m_first.neighbours.Add(m_second);
            m_second.neighbours.Add(m_first);

            MarkRoomDirty(m_first);
            MarkRoomDirty(m_second);
        }

        if (GUILayout.Button("Connect as TimePartners"))
        {
            m_first.timePartner = m_second;
            m_second.timePartner = m_first;

            MarkRoomDirty(m_first);
            MarkRoomDirty(m_second);
        }

        GUILayout.Label("");

        if (GUILayout.Button("Check Connections"))
        {
            var allRoomBounds = FindObjectsOfType<RoomBounds>(true);

            bool success = true;
            foreach (var target in allRoomBounds)
            {
                if(!ConfirmTimePartner(target))
                {
                    success = false;
                }

                if(!CheckNeighbours(target))
                {
                    success = false;
                }
            }

            if(success)
            {
                Debug.Log("All rooms seem to be connected properly.");
            }
            else
            {
                Debug.LogWarning("Wrong connection detected.");
            }
        }
    }

    void GetObjects()
    {
        if(Selection.gameObjects.Length == 2)
        {
            RoomBounds first = Selection.gameObjects[0].GetComponent<RoomBounds>();
            RoomBounds second = Selection.gameObjects[1].GetComponent<RoomBounds>();

            if(first != null && second != null)
            {
                m_first = first;
                m_second = second;
            }
        }

        m_first = RoomBoundsField(m_first, "First");
        m_second = RoomBoundsField(m_second, "Second");
    }

    RoomBounds RoomBoundsField(RoomBounds target, string label)
    {
        return EditorGUILayout.ObjectField(label, target, typeof(RoomBounds), true) as RoomBounds;
    }

    bool CheckNeighbours(RoomBounds target)
    {
        bool success = true;
        foreach(var neighbour in target.neighbours)
        {
            if(neighbour == null)
            {
                Debug.LogWarning("NEIGHBOUR: " + target.name + " has a null neighbour.");
                success = false;
                continue;
            }

            if(ConfirmNeighbour(target, neighbour))
            {
                // all good
            }
            else
            {
                // bad
                Debug.LogWarning("NEIGHBOUR: " + target.name + " is not properly connected to " + neighbour.name);
                success = false;
            }
        }
        return success;
    }

    bool ConfirmNeighbour(RoomBounds target, RoomBounds targetNeighbour)
    {
        foreach(var adjacent in targetNeighbour.neighbours)
        {
            if(adjacent == target)
            {
                return true;
            }
        }
        return false;
    }

    bool ConfirmTimePartner(RoomBounds target)
    {
        if(target.timePartner == null)
        {
            Debug.LogWarning("TIMEPARTNER: " + target.name + ": time partner is null.");
            return false;
        }

        if(target.timePartner.timePartner != target)
        {
            Debug.LogWarning("TIMEPARTNER: " + target.name + " is not connected to " + target.timePartner + " properly.");
            return false;
        }
        return true;
    }

    void MarkRoomDirty(RoomBounds target)
    {
        EditorUtility.SetDirty(target);
        EditorSceneManager.MarkSceneDirty(target.gameObject.scene);
    }
}
