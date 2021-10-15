using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomBounds : MonoBehaviour
{
    Bounds entryBounds;
    Bounds exitBounds;

    public float exitExpansion = 5.0f;

    public RoomBounds[] neighbours;

    public UnityEvent enterRoom;
    public UnityEvent exitRoom;

    void Awake()
    {
        entryBounds.center = transform.position;
        entryBounds.size = Vector3.zero;
        List<Transform> allObjectsHeap = FindAllChildren();
        var allBounds = FindAllBounds(allObjectsHeap);
        FindMinAndMax(allBounds, out Vector3 min, out Vector3 max);
        entryBounds.SetMinMax(min, max);

        exitBounds.center = entryBounds.center;
        exitBounds.SetMinMax(min, max);
        exitBounds.Expand(exitExpansion);
    }

    private void Start()
    {
        
    }

    List<Transform> FindAllChildren()
    {
        List<Transform> allObjectsHeap = new List<Transform>();
        allObjectsHeap.Add(transform);

        for (int i = 0; i < allObjectsHeap.Count; i++)
        {
            FindChildren(allObjectsHeap, allObjectsHeap[i]);
        }

        return allObjectsHeap;
    }

    void FindChildren(List<Transform> allObjectsHeap, Transform transform)
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            allObjectsHeap.Add(child);
        }
    }

    List<Bounds> FindAllBounds(List<Transform> allObjectsHeap)
    {
        List<Bounds> allBounds = new List<Bounds>();

        foreach(Transform obj in allObjectsHeap)
        {
            var collider = obj.GetComponent<Collider>();

            if(collider != null)
            {
                allBounds.Add(collider.bounds);
            }
        }

        return allBounds;
    }

    void FindMinAndMax(List<Bounds> allBounds, out Vector3 min, out Vector3 max)
    {
        min = allBounds[0].min;
        max = allBounds[0].max;

        for(int i = 1; i < allBounds.Count; i++)
        {
            Bounds current = allBounds[i];
            min = FindMins(min, current.min);
            max = FindMaxs(max, current.max);
        }
    }

    Vector3 FindMins(Vector3 currentMin, Vector3 value)
    {
        currentMin.x = FindMin(currentMin.x, value.x);
        currentMin.y = FindMin(currentMin.y, value.y);
        currentMin.z = FindMin(currentMin.z, value.z);

        return currentMin;
    }

    float FindMin(float currentMin, float value)
    {
        if (value < currentMin)
        {
            return value;
        }
        return currentMin;
    }

    Vector3 FindMaxs(Vector3 currentMax, Vector3 value)
    {
        currentMax.x = FindMax(currentMax.x, value.x);
        currentMax.y = FindMax(currentMax.y, value.y);
        currentMax.z = FindMax(currentMax.z, value.z);

        return currentMax;
    }

    float FindMax(float currentMax, float value)
    {
        if (value > currentMax)
        {
            return value;
        }
        return currentMax;
    }

    public float Distance(Vector3 point)
    {
        Vector3 closestPoint = entryBounds.ClosestPoint(point);
        return (closestPoint - point).magnitude;
    }

    public bool EntryContainsPoint(Vector3 point)
    {
        return entryBounds.Contains(point);
    }    

    public bool ExitContainsPoint(Vector3 point)
    {
        return exitBounds.Contains(point);
    }

    //private void OnDrawGizmos()
    //{
    //    if(Application.isPlaying)
    //    {
    //        Color colour = Color.red;
    //        colour.a *= 0.4f;
    //        Gizmos.color = colour;
    //
    //        Gizmos.DrawCube(entryBounds.center, entryBounds.size);
    //
    //        colour = Color.green;
    //        colour.a *= 0.4f;
    //
    //        Gizmos.color = colour;
    //        Gizmos.DrawCube(exitBounds.center, exitBounds.size);
    //    }
    //}
}
