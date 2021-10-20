using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

using GameObjectPair = RoomPlacer.GameObjectPair;

[InitializeOnLoad]
public static class RoomPlacerSceneManager
{
    static RoomPlacer m_roomPlacer = null;
    static List<RoomPlacerEditorList<RoomPairData>> m_roomDataListArray = null;

    delegate void VoidActionDelegate();
    static VoidActionDelegate m_sceneViewDelegate;

    static bool m_isInitialised = false;

    static RoomPlacerSceneManager()
    {
        Init();
    }

    // This is called before any gameobject are loaded into the scene
    public static void Init()
    {
        if(m_isInitialised)
        {
            return;
        }
        m_isInitialised = true;

        m_sceneViewDelegate = FirstOnSceneAction;
        SceneView.duringSceneGui += OnScene;
        EditorSceneManager.sceneOpened += SceneOpened;
        EditorSceneManager.sceneClosed += SceneClosed;

        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    static void SceneOpened(Scene scene, OpenSceneMode mode)
    {
        FindRoomPlacer();
    }

    static void SceneClosed(Scene scene)
    {
        // Check if the user is exiting edit mode (eg. pressed play)
        ClearRoomPlacer();
    }

    // This is called after Gameobject are loaded into the scene
    static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindRoomPlacer();
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            FindRoomPlacer();
        }
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            
        }
    }

    public static void SetRoomPlacer(RoomPlacer target)
    {
        if (target == null)
        {
            ClearRoomPlacer();
            return;
        }

        m_roomPlacer = target;
        InitroomDataListArray(target);
        m_sceneViewDelegate = FirstOnSceneAction;
    }

    public static void MarkDirty()
    {
        EditorUtility.SetDirty(m_roomPlacer);
        EditorSceneManager.MarkSceneDirty(m_roomPlacer.gameObject.scene);
    }

    public static void InitroomDataListArray(RoomPlacer roomPlacer)
    {
        m_roomDataListArray = new List<RoomPlacerEditorList<RoomPairData>>();
        for(int i = 0; i < roomPlacer.roomPairsList.Count; i++)
        {
            var toAdd = new RoomPlacerEditorList<RoomPairData>(roomPlacer, i);
            m_roomDataListArray.Add(toAdd);
        }
    }

    public static void AddNewList(RoomPlacer roomPlacer)
    {
        var toAdd = new RoomPlacerEditorList<RoomPairData>(roomPlacer, roomPlacer.roomPairsList.Count - 1);
        m_roomDataListArray.Add(toAdd);
    }

    public static void RemoveBottomList()
    {
        m_roomDataListArray.RemoveAt(m_roomDataListArray.Count - 1);
    }

    public static void FindRoomPlacer()
    {
        SetRoomPlacer(Object.FindObjectOfType<RoomPlacer>());
    }

    public static void ClearRoomPlacer()
    {
        m_roomPlacer = null;
        if(m_roomDataListArray != null)
        {
            foreach(var list in m_roomDataListArray)
            {
                list.Clear();
            }
        }
        m_sceneViewDelegate = FirstOnSceneAction;
    }

    public static bool CompareRoomPlacer(RoomPlacer comparator)
    {
        return m_roomPlacer == comparator;
    }

    private static void OnScene(SceneView sceneview)
    {
        m_sceneViewDelegate();
    }

    static void FirstOnSceneAction()
    {
        if (m_roomPlacer == null)
        {
            return;
        }
        OnSceneAction();
        m_sceneViewDelegate = OnSceneAction;
    }

    static void OnSceneAction()
    {
        for(int i = 0; i < m_roomDataListArray.Count; i++)
        {
            m_roomDataListArray[i].OnSceneAction();
        }
    }

    public static void AddRoomPairData(RoomPlacer.GameObjectPair roomPair, int listIndex, bool value)
    {
        m_roomDataListArray[listIndex].Add(roomPair, value);
    }

    public static void RemoveRoomPairData(RoomPlacer.GameObjectPair roomPair, int listIndex)
    {
        m_roomDataListArray[listIndex].Remove(roomPair);
    }

    public static void SwitchValidators(int listIndex, bool value)
    {
        m_roomDataListArray[listIndex].SetValidators(value);
    }

    public static void TrySetTrackersRoomPairList(int listIndex)
    {
        m_roomDataListArray[listIndex].TrySetTrackers();
    }

    public static void SmartAlignRooms(int listIndex, RoomPlacer.GameObjectPairList list)
    {
        var dataList = m_roomDataListArray[listIndex];
        if(list.isRoomItemList)
        {
            foreach (var pairData in dataList.roomDataList)
            {
                if(pairData.roomPair.IsFullyAssigned())
                {
                    Vector3 itemPrevOffset = pairData.futureRoomValues.prevPosition - pairData.pastRoomValues.prevPosition;
                    pairData.roomPair.AlignItemToRoom(itemPrevOffset);
                }
            }
        }
        else
        {
            list.AlignAllRoomPairsToPast();
        }
    }
}