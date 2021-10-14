using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class RoomPlacerSceneManager
{
    static RoomPlacer m_roomPlacer = null;
    static List<RoomPairData> m_roomDataList = null;

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
        m_roomDataList = RoomPairData.FindRoomPairList(target.roomPairs);
        m_sceneViewDelegate = FirstOnSceneAction;
    }

    public static void FindRoomPlacer()
    {
        SetRoomPlacer(Object.FindObjectOfType<RoomPlacer>());
    }

    public static void ClearRoomPlacer()
    {
        m_roomPlacer = null;
        m_roomDataList = null;
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
        foreach(RoomPairData roomPairData in m_roomDataList)
        {
            if(roomPairData.ValidateRoomPair(m_roomPlacer.offset))
            {
                // Change was detected
                
            }
        }
    }

    public static void AddRoomPairData(RoomPlacer.RoomPair roomPair)
    {
        m_roomDataList.Add(new RoomPairData(roomPair));
    }

    public static void RemoveRoomPairData(RoomPlacer.RoomPair roomPair)
    {
        RoomPairData removeTarget = null;
        for(int i = 0; i < m_roomDataList.Count; i++)
        {
            if (m_roomDataList[i].roomPair == roomPair)
            {
                removeTarget = m_roomDataList[i];
                break;
            }
        }
        if (removeTarget != null)
        {
            m_roomDataList.Remove(removeTarget);
        }
        else
        {
            Debug.LogWarning("Something went wrong with removing. The target could not be found. Ensure that you reset the the Room Placer Manager before continuing.");
        }
    }
}

// This class is used to maintain data between scene updates.
class RoomPairData
{
    RoomPlacer.RoomPair m_roomPair;
    RoomValues pastRoomValues;
    RoomValues futureRoomValues;

    Vector3 currentPositionPast { get { return m_roomPair.pastRoom.transform.position; } }
    Quaternion currentRotationPast { get { return m_roomPair.pastRoom.transform.rotation; } }
    Vector3 currentScalePast { get { return m_roomPair.pastRoom.transform.localScale; } }

    Vector3 currentPositionFuture { get { return m_roomPair.futureRoom.transform.position; } }
    Quaternion currentRotationFuture { get { return m_roomPair.futureRoom.transform.rotation; } }
    Vector3 currentScaleFuture { get { return m_roomPair.futureRoom.transform.localScale; } }

    public RoomPlacer.RoomPair roomPair { get { return m_roomPair; } }

    public RoomPairData(RoomPlacer.RoomPair roomPair)
    {
        this.m_roomPair = roomPair;

        pastRoomValues = new RoomValues(roomPair.pastRoom);
        futureRoomValues = new RoomValues(roomPair.futureRoom);
    }

    public static List<RoomPairData> FindRoomPairList(List<RoomPlacer.RoomPair> roomPairList)
    {
        List<RoomPairData> resultList = new List<RoomPairData>();
        foreach (RoomPlacer.RoomPair roomPair in roomPairList)
        {
            resultList.Add(new RoomPairData(roomPair));
        }
        return resultList;
    }

    public bool HasPastChanged()
    {
        return pastRoomValues.HasChanged(currentPositionPast, currentRotationPast, currentScalePast);
    }

    public bool HasFutureChanged()
    {
        return futureRoomValues.HasChanged(currentPositionFuture, currentRotationFuture, currentScaleFuture);
    }

    // This will check if a change needs to be made and will then apply that change to the affected room.
    // Returns true if a change was detected.
    public bool ValidateRoomPair(Vector3 offset)
    {
        if(roomPair.pastRoom == null || roomPair.futureRoom == null)
        {
            return false;
        }

        if (HasPastChanged())
        {
            roomPair.AlignToPast(offset);
            return true;
        }
        if (HasFutureChanged())
        {
            roomPair.AlignToFuture(offset);
            return true;
        }
        return false;
    }
}

struct RoomValues
{
    Vector3 m_prevPosition;
    Quaternion m_prevRotation;
    Vector3 m_prevScale;

    public RoomValues(GameObject roomTransform)
    {
        if (roomTransform != null)
        {
            m_prevPosition = roomTransform.transform.position;
            m_prevRotation = roomTransform.transform.rotation;
            m_prevScale = roomTransform.transform.localScale;
        }
        else
        {
            m_prevPosition = Vector3.zero;
            m_prevRotation = Quaternion.identity;
            m_prevScale = Vector3.zero;
        }
    }

    public bool HasChanged(Vector3 currentPosition, Quaternion currentRotation, Vector3 currentScale)
    {
        bool result = DetectChange(m_prevPosition, currentPosition) || DetectChange(m_prevRotation, currentRotation) || DetectChange(m_prevScale, currentScale);
        m_prevPosition = currentPosition;
        m_prevRotation = currentRotation;
        m_prevScale = currentScale;
        return result;
    }

    bool DetectChange(Vector3 prev, Vector3 current)
    {
        return prev != current;
    }

    bool DetectChange(Quaternion prev, Quaternion current)
    {
        return prev != current;
    }
}