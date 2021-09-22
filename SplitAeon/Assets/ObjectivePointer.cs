using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectivePointer : MonoBehaviour
{
    public float pingTime;

    public Transform player;
    public Transform target;

    public GameObject UIPointer;

    private NavMeshPath path;

    public LineRenderer rend;


    private float timer = 0f;

    void Start()
    {
        path = new NavMeshPath();
        timer = 0f;
    }

    void Update()
    {
        if (!target)
        {
            UIPointer.SetActive(false);
            return;
        }      
       
        timer += Time.deltaTime;

        if (timer > pingTime)
        {
            timer -= pingTime;
            NavMesh.CalculatePath(player.position, target.position, NavMesh.AllAreas, path);
        }

        for (int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }

        if (path.corners.Length >= 3)
        {
            SetPointerDirection(path.corners[2]);
        }
        else if (path.corners.Length == 2)
        {
            SetPointerDirection(path.corners[1]);
        }
        else
        {
            SetPointerDirection(target.position);
        }

    }

    void SetPointerDirection(Vector3 target)
    {
        transform.LookAt(target, Vector3.up);

        Vector3 angles = player.transform.rotation.eulerAngles - transform.rotation.eulerAngles;

        UIPointer.transform.rotation = Quaternion.Euler(0, 0, angles.y);
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }
}
