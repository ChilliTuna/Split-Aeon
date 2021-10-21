using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using GameObjectPair = RoomPlacer.GameObjectPair;

[CustomEditor(typeof(RoomPlacer))]
public class RoomPlacerEditor : Editor
{
    Vector3 m_prevOffset = Vector3.zero;

    bool m_isEditing = false;

    bool m_shouldAddList = false;
    bool m_shouldRemoveList = false;

    bool m_showListSettings = false;

    static int _elementPixelSpace = 21;
    static int _beginPixelSpace = 10;
    static int _endPixelSpace = 5;

    public static bool showLayoutOptions = false;

    string pastElementName = "Past Room";
    string futureElementName = "Future Room";

    string roomElementName = "Room";
    string itemElementName = "Item";

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
        ValidateListChanges(roomPlacer);
    }

    void ValidateOffset(RoomPlacer roomPlacer)
    {
        if (m_prevOffset != roomPlacer.offset)
        {
            for(int i = 0; i < roomPlacer.roomPairsList.Count; i++)
            {
                RoomPlacerSceneManager.SmartAlignRooms(i, roomPlacer.roomPairsList[i]);
            }
            RoomPlacerSceneManager.MarkDirty();
        }
        m_prevOffset = roomPlacer.offset;
    }

    void ValidateListChanges(RoomPlacer roomPlacer)
    {
        if(m_shouldAddList)
        {
            // Add list to room placer and manager
            roomPlacer.CreateNewList();
            RoomPlacerSceneManager.AddNewList(roomPlacer);
        }

        if(m_shouldRemoveList)
        {
            // Add list to room placer and manager
            roomPlacer.RemoveLastList();
            RoomPlacerSceneManager.RemoveBottomList();
        }
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

        ListsButtonControls();

        for (int i = 0; i < roomPlacer.roomPairsList.Count; i++)
        {
            FoldOutList(roomPlacer, roomPlacer.roomPairsList[i], i);
        }

        if (showLayoutOptions)
        {
            GUILayout.Label("Layout Options", EditorStyles.boldLabel);
            ExtraOptionsLayout();
        }
    }

    void ListsButtonControls()
    {
        if(m_isEditing)
        {
            GUILayout.Space(2);
        }

        EditorGUILayout.BeginHorizontal();
        m_shouldAddList = GUILayout.Button("Add New List");
        m_shouldRemoveList = GUILayout.Button("Remove Bottom List");

        string settingsButtonText = "Show Settings";
        if(m_showListSettings)
        {
            settingsButtonText = "Hide Settings";
        }

        if(GUILayout.Button(settingsButtonText))
        {
            m_showListSettings = !m_showListSettings;
        }
        EditorGUILayout.EndHorizontal();
    }

    void FoldOutList(RoomPlacer roomPlacer, RoomPlacer.GameObjectPairList list, int listIndex)
    {
        FoldOutGameObjectPairs(ref list.shouldShow, roomPlacer, list.listName, listIndex);
    }

    void FoldOutGameObjectPairs(ref bool foldOut, RoomPlacer roomPlacer, string title, int listIndex)
    {
        if(!m_showListSettings)
        {
            foldOut = EditorGUILayout.Foldout(foldOut, title, true, EditorStyles.foldoutHeader);
        }
        else
        {
            var list = roomPlacer.roomPairsList[listIndex];
            list.listName = EditorGUILayout.TextField(list.listName);
            GUILayout.Space(-1);
        }

        if (foldOut)
        {
            RoomPairListLayout(roomPlacer, listIndex);

            ButtonsLayout(roomPlacer, listIndex);
        }
    }

    void RoomPairLayout(RoomPlacer.GameObjectPairList list, RoomPlacer.GameObjectPair roomPair, int listIndex)
    {
        if(!m_isEditing)
        {
            GUILayout.Label(roomPair.roomName, EditorStyles.boldLabel);
        }
        else
        {
            GUILayout.Space(-2);
            EditorGUILayout.BeginHorizontal();
            roomPair.roomName = EditorGUILayout.TextField(roomPair.roomName);

            if (GUILayout.Button("Remove"))
            {
                RoomPlacerSceneManager.RemoveRoomPairData(roomPair, listIndex);
                list.roomPairs.Remove(roomPair);
            }

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(-2);
        }

        bool pastHasChanged;
        bool futureHasChanged;

        if (list.isRoomItemList)
        {
            roomPair.pastRoom = TryLayoutObjectField(roomElementName, roomPair.pastRoom, out pastHasChanged);
            roomPair.futureRoom = TryLayoutObjectField(itemElementName, roomPair.futureRoom, out futureHasChanged);
        }
        else
        {
            roomPair.pastRoom = TryLayoutObjectField(pastElementName, roomPair.pastRoom, out pastHasChanged);
            roomPair.futureRoom = TryLayoutObjectField(futureElementName, roomPair.futureRoom, out futureHasChanged);
        }

        if(pastHasChanged || futureHasChanged)
        {
            RoomPlacerSceneManager.TrySetTrackersRoomPairList(listIndex);
            RoomPlacerSceneManager.MarkDirty();
        }

        if(m_isEditing)
        {
            GUILayout.Space(-1);
        }
        GUILayout.Space(_elementPixelSpace);
    }

    bool DetectGameObjectChange(GameObject previous, GameObject potentialChange)
    {
        return previous != potentialChange;
    }

    GameObject TryLayoutObjectField(string label, GameObject gameObject, out bool hasChanged)
    {
        var before = gameObject;
        var after = EditorGUILayout.ObjectField(label, gameObject, typeof(GameObject), true) as GameObject;
        hasChanged = DetectGameObjectChange(before, after);
        return after;
    }

    void RoomPairListLayout(RoomPlacer roomPlacer, int listIndex)
    {
        var list = roomPlacer.roomPairsList[listIndex];
        if(m_showListSettings)
        {
            ShowListSettings(list, listIndex);
        }
        var roomPairs = list.roomPairs;
        for (int i = 0; i < roomPairs.Count; i++)
        {
            var roomPair = roomPairs[i];
            RoomPairLayout(list, roomPair, listIndex);
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

    void ShowListSettings(RoomPlacer.GameObjectPairList list, int listIndex)
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Settings: ");
        bool prevToggle = list.isRoomItemList;
        list.isRoomItemList = GUILayout.Toggle(list.isRoomItemList, "Room Items");
        bool afterToggle = list.isRoomItemList;
        if(prevToggle != afterToggle)
        {
            Debug.Log("Toggled");
            RoomPlacerSceneManager.SwitchValidators(listIndex, afterToggle);
            RoomPlacerSceneManager.MarkDirty();
        }

        EditorGUILayout.EndHorizontal();
    }

    void ButtonsLayout(RoomPlacer roomPlacer, int listIndex)
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add New to " + roomPlacer.roomPairsList[listIndex].listName))
        {
            var newRoom = new RoomPlacer.GameObjectPair();
            var list = roomPlacer.roomPairsList[listIndex];
            list.roomPairs.Add(newRoom);
            RoomPlacerSceneManager.AddRoomPairData(newRoom, listIndex, list.isRoomItemList);

        }
        if (GUILayout.Button("Edit Room Pairs"))
        {
            m_isEditing = !m_isEditing;
        }
        EditorGUILayout.EndHorizontal();
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