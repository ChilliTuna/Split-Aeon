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
        EndSpawning();
    }

    public void EndSpawning()
    {
        m_isSpawning = false;
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

        if(isSpawning)
        {
            DrawCube(transform.position, Color.blue);
        }
        else
        {
            switch (spawnType)
            {
                case SpawnType.FIXED:
                    {
                        DrawCube(transform.position, Color.red);
                        break;
                    }
                case SpawnType.DYNAMIC:
                    {
                        DrawCube(transform.position, Color.yellow);
                        break;
                    }
            }
        }
    }
}
