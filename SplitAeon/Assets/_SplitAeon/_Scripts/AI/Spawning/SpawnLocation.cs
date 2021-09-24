using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
    public enum SpawnType
    {
        FIXED,
        DYNAMIC
    }

    public SpawnType spawnType = 0;
    bool m_isSpawning = false;

    public bool isSpawning { get { return m_isSpawning; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSpawning()
    {
        m_isSpawning = true;

        // DEBUGGONG PURPOSE: END THE SPAWNING STRAIGHT AWAY
        // Ideally the spawn would last as long as the animation that plays it
        //EndSpawning();
    }

    public void EndSpawning()
    {
        m_isSpawning = false;
    }

    bool IsInCameraView(Camera camera, Bounds agentBounds)
    {
        var frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
        bool inCameraView = GeometryUtility.TestPlanesAABB(frustumPlanes, GetComponent<CapsuleCollider>().bounds);
        return inCameraView;
    }

    public bool IsSpawnable(Vector3 targetPos, LayerMask environmentMask, Camera playerCam, Bounds agentBounds, float agentHeight, float agentRadius)
    {
        switch (spawnType)
        {
            case SpawnType.FIXED:
                {
                    return true;
                }
            case SpawnType.DYNAMIC:
                {
                    Vector3 position = transform.position;
                    Vector3 toTarget = targetPos - position;
                    Vector3 point1Offset = Vector3.up * agentRadius;
                    Vector3 point2Offset = Vector3.up * (agentHeight - agentRadius);

                    if (IsInCameraView(playerCam, agentBounds))
                    {
                        // Bounds are inside player frustum
                        return false;
                    }
                    return true;

                    if (Physics.CapsuleCast(position + point1Offset, position + point2Offset, 0.5f, toTarget, out RaycastHit hitInfo, toTarget.magnitude, environmentMask))
                    {
                        // Object is in the way of the player
                        return true;
                    }
                    return false;
                }
            default:
                {
                    return true;
                }
        }
    }

    private void OnDrawGizmos()
    {
        void DrawCube(Vector3 position, Color colour)
        {
            Gizmos.color = colour;
            Gizmos.DrawWireCube(position, Vector3.one);

            colour *= 0.4f;
            Gizmos.color = colour;
            Gizmos.DrawCube(position, Vector3.one);
        }

        Color drawColour = Color.white;
        if(isSpawning)
        {
            drawColour = Color.blue;
        }
        else
        {
            switch (spawnType)
            {
                case SpawnType.FIXED:
                    {
                        drawColour = Color.red;
                        break;
                    }
                case SpawnType.DYNAMIC:
                    {
                        drawColour = Color.yellow;
                        break;
                    }
            }
        }

        // Debug things
        if(IsInCameraView(Camera.main, GetComponent<CapsuleCollider>().bounds))
        {
            drawColour = Color.green;
        }
        // end debug things

        DrawCube(transform.position, drawColour);
    }
}
