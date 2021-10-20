using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using GameObjectPair = RoomPlacer.GameObjectPair;

public class RoomPlacerEditorList<PairDataType> where PairDataType : TransformPairData
{
    RoomPlacer m_roomPlacer;
    List<PairDataType> m_roomDataList = null;

    int listIndex;

    public List<PairDataType> roomDataList {  get { return m_roomDataList; } }

    public RoomPlacerEditorList(RoomPlacer target, int listArrayIndex)
    {
        Init(target, listArrayIndex);
    }
    public void Init(RoomPlacer target, int listArrayIndex)
    {
        m_roomPlacer = target;
        listIndex = listArrayIndex;
        var list = target.roomPairsList[listArrayIndex];
        m_roomDataList = TransformPairData.FindTransformPairList<PairDataType>(list.roomPairs, list.isRoomItemList);
    }

    public void Clear()
    {
        m_roomPlacer = null;
        m_roomDataList = null;
    }

    public void OnSceneAction()
    {
        foreach (PairDataType roomPairData in m_roomDataList)
        {
            roomPairData.OnSceneAction(m_roomPlacer);
        }
    }

    public void Add(RoomPlacer.GameObjectPair roomPair, bool value)
    {
        m_roomDataList.Add(TransformPairData.CreateNew<PairDataType>(roomPair, value));
    }

    public void Remove(RoomPlacer.GameObjectPair roomPair)
    {
        PairDataType removeTarget = null;
        for (int i = 0; i < m_roomDataList.Count; i++)
        {
            if (m_roomDataList[i].gameObjectPair == roomPair)
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

    public void TrySetTrackers()
    {
        foreach(var pairData in m_roomDataList)
        {
            if(pairData.TrySetTrackers())
            {
                pairData.SetInitialTransforms(m_roomPlacer);
            }
        }
    }

    public void SetValidators(bool value)
    {
        foreach (var pairData in m_roomDataList)
        {
            pairData.CreateValidator(pairData, value);
        }
    }
}

public class TransformTracker
{
    Transform m_currentTransform;
    TransformValues prevValues;

    public Vector3 prevPosition { get { return prevValues.position; } }
    public Quaternion prevRotation { get { return prevValues.rotation; } }
    public Vector3 prevScale { get { return prevValues.scale; } }

    public Vector3 currentPosition { get { return m_currentTransform.position; } }
    public Quaternion currentRotation { get { return m_currentTransform.rotation; } }
    public Vector3 currentScale { get { return m_currentTransform.localScale; } }

    public TransformTracker(Transform target)
    {
        m_currentTransform = target;
        prevValues = new TransformValues(target);
    }

    public void Update()
    {
        prevValues.position = currentPosition;
        prevValues.rotation = currentRotation;
        prevValues.scale = currentScale;
    }

    public bool HasChanged()
    {
        bool result = prevValues.position != currentPosition || prevValues.rotation != currentRotation || prevValues.scale != currentScale;
        return result;
    }
}

// This class is used to maintain data between scene updates.
public abstract class TransformPairData
{
    public RoomPlacer.GameObjectPair gameObjectPair;
    protected TransformTracker firstValues;
    protected TransformTracker secondValues;

    IValidateTransformData validator;

    bool trackersAreSet = false;

    public TransformPairData(RoomPlacer.GameObjectPair roomPair, bool value)
    {
        Init(roomPair);
    }

    protected void Init(RoomPlacer.GameObjectPair gameObjectPair)
    {
        this.gameObjectPair = gameObjectPair;

        TrySetTrackers();
    }

    public bool TrySetTrackers()
    {
        if(gameObjectPair.pastRoom != null && gameObjectPair.futureRoom != null)
        {
            firstValues = new TransformTracker(gameObjectPair.pastRoom.transform);
            secondValues = new TransformTracker(gameObjectPair.futureRoom.transform);
            trackersAreSet = true;
            return true;
        }
        else
        {
            firstValues = null;
            secondValues = null;
            trackersAreSet = false;
            return false;
        }
    }

    public void SetInitialTransforms(RoomPlacer roomPlacer)
    {
        validator.SetInitalTransform(roomPlacer);
    }

    public void SetValidator(IValidateTransformData validator)
    {
        this.validator = validator;
    }

    public abstract void CreateValidator(TransformPairData roomPairData, bool value);

    public void Update()
    {
        firstValues.Update();
        secondValues.Update();
    }

    public bool ValidateTransformPair(RoomPlacer roomPlacer)
    {
        return validator.Validate(roomPlacer);
    }

    public void OnSceneAction(RoomPlacer roomPlacer)
    {
        if (trackersAreSet)
        {
            if (ValidateTransformPair(roomPlacer))
            {
                // Change was detected

            }
            Update();
        }
    }

    public static List<PairDataType> FindTransformPairList<PairDataType>(List<RoomPlacer.GameObjectPair> roomPairList, bool value) where PairDataType : TransformPairData
    {
        List<PairDataType> resultList = new List<PairDataType>();
        foreach (RoomPlacer.GameObjectPair roomPair in roomPairList)
        {
            resultList.Add(CreateNew<PairDataType>(roomPair, value));
        }
        return resultList;
    }

    public static PairDataType CreateNew<PairDataType>(RoomPlacer.GameObjectPair roomPair, bool value)
    {
        object newThing = System.Activator.CreateInstance(typeof(PairDataType), roomPair, value);
        return (PairDataType)newThing;
    }
}

public interface IValidateTransformData
{
    bool Validate(RoomPlacer roomPlacer);

    void SetInitalTransform(RoomPlacer roomPlacer);
}

public abstract class IValidateTransformPairBase<PairDataType> : IValidateTransformData
{
    protected PairDataType pairData;

    public IValidateTransformPairBase(PairDataType pairData)
    {
        Init(pairData);
    }

    public void Init(PairDataType pairData)
    {
        this.pairData = pairData;
    }

    bool IValidateTransformData.Validate(RoomPlacer roomPlacer)
    {
        return Validate(roomPlacer);
    }

    void IValidateTransformData.SetInitalTransform(RoomPlacer roomPlacer)
    {
        SetInitalTransform(roomPlacer);
    }

    protected abstract bool Validate(RoomPlacer roomPlacer);

    protected abstract void SetInitalTransform(RoomPlacer roomPlacer);
}

// This class is used to maintain data between scene updates.
public class RoomPairData : TransformPairData
{
    public TransformTracker pastRoomValues { get { return firstValues; } }
    public TransformTracker futureRoomValues { get { return secondValues; } }

    public RoomPlacer.GameObjectPair roomPair { get { return gameObjectPair; } }

    public bool pastHasChanged { get { return firstValues.HasChanged(); } }
    public bool futureHasChanged { get { return secondValues.HasChanged(); } }

    public RoomPairData(RoomPlacer.GameObjectPair roomPair, bool value) : base(roomPair, value)
    {
        CreateValidator(this, value);
    }

    public override void CreateValidator(TransformPairData pairData, bool value)
    {
        RoomPairData roomPairData = pairData as RoomPairData;

        if (value)
        {
            SetValidator(new IValidateRoomItemPair(roomPairData));
        }
        else
        {
            SetValidator(new IValidateRoomPair(roomPairData));
        }
    }
}

public class IValidateRoomPair : IValidateTransformPairBase<RoomPairData>
{
    public IValidateRoomPair(RoomPairData roomPairData) : base(roomPairData) { }

    // This will check if a change needs to be made and will then apply that change to the affected room.
    // Returns true if a change was detected.
    protected override bool Validate(RoomPlacer roomPlacer)
    {
        RoomPlacer.GameObjectPair roomPair = pairData.roomPair;

        if (roomPair.pastRoom == null || roomPair.futureRoom == null)
        {
            return false;
        }

        if (pairData.pastHasChanged)
        {
            roomPair.AlignToPast(roomPlacer.offset);
            return true;
        }
        if (pairData.futureHasChanged)
        {
            roomPair.AlignToFuture(roomPlacer.offset);
            return true;
        }
        return false;
    }

    protected override void SetInitalTransform(RoomPlacer roomPlacer)
    {
        RoomPlacer.GameObjectPair roomPair = pairData.roomPair;

        if (roomPair.pastRoom == null || roomPair.futureRoom == null)
        {
            return;
        }

        roomPair.AlignToPast(roomPlacer.offset);
    }
}

public class IValidateRoomItemPair : IValidateTransformPairBase<RoomPairData>
{
    public IValidateRoomItemPair(RoomPairData roomPairData) : base(roomPairData) { }

    // This will check if a change needs to be made and will then apply that change to the affected room.
    // Returns true if a change was detected.
    protected override bool Validate(RoomPlacer roomPlacer)
    {
        RoomPlacer.GameObjectPair roomPair = pairData.roomPair;

        if (roomPair.pastRoom == null || roomPair.futureRoom == null)
        {
            return false;
        }

        if (pairData.pastHasChanged)
        {
            Vector3 itemPrevOffset = pairData.futureRoomValues.prevPosition - pairData.pastRoomValues.prevPosition;
            //itemPrevOffset = pairData.roomPair.futureRoom.transform.localToWorldMatrix * itemPrevOffset;
            roomPair.AlignItemToRoom(itemPrevOffset);
            return true;
        }
        if (pairData.futureHasChanged)
        {
            return true;
        }
        return false;
    }

    protected override void SetInitalTransform(RoomPlacer roomPlacer)
    {
        
    }
}

struct TransformValues
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

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
}
