using UnityEngine;
using UnityEngine.Events;

public class Zone : MonoBehaviour
{
    [Tooltip("0 = infinite enemies")]
    public uint maxEnemyCount;

    [HideInInspector]
    public uint spawnedEnemies;

    [HideInInspector]
    public uint aliveEnemyCount;

    [HideInInspector]
    public bool shouldEnemiesSpawn = false;

    [HideInInspector]
    public ZoneManager zoneManager;

    public EnemySpawner[] enemySpawners;

    [HideInInspector]
    public bool isActive = false;

    [HideInInspector]
    public bool isComplete = false;

    public Color gizmoColour = Color.blue;

    public UnityEvent onCompleteZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            zoneManager.EnterZone(this);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.GetComponent<Player>())
    //    {
    //        zoneManager.ExitZone(this);
    //    }
    //}

    public void IncreaseAliveCount()
    {
        spawnedEnemies++;
        aliveEnemyCount++;
        if (spawnedEnemies >= maxEnemyCount && maxEnemyCount != 0)
        {
            SetActiveness(false);
        }
    }

    public void DecreaseAliveCount()
    {
        aliveEnemyCount--;

        if (aliveEnemyCount == 0)
        {
            SetAsComplete();
        }
    }

    public void SetActiveness(bool state)
    {
        isActive = state;
        if (state == true)
        {
            if (spawnedEnemies < maxEnemyCount || maxEnemyCount == 0)
            {
                shouldEnemiesSpawn = true;
                ToggleSpawners(true);
            }
            else
            {
                ToggleSpawners(false);
            }
        }
        else
        {
            shouldEnemiesSpawn = false;
            ToggleSpawners(false);
        }
    }

    public void ToggleSpawners(bool activeness)
    {
        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.enabled = activeness;
        }
    }

    public void ToggleSpawners()
    {
        foreach (EnemySpawner enemySpawner in enemySpawners)
        {
            enemySpawner.enabled = !enemySpawner.enabled;
        }
    }

    private void OnDrawGizmos()
    {
        void DrawCube(Transform transform, Color colour)
        {
            Gizmos.color = colour;
            Gizmos.DrawWireCube(transform.position, transform.lossyScale);
            Gizmos.DrawIcon(transform.position, "Zone Icon", false, Color.yellow);

            colour *= 0.4f;
            Gizmos.color = colour;
            Gizmos.DrawCube(transform.position, transform.lossyScale);
        }
        DrawCube(transform, gizmoColour);
    }

    public void SetAsComplete()
    {
        isComplete = true;
        zoneManager.completedZones.Add(this);
        onCompleteZone.Invoke();
        zoneManager.ExitZone(this);
    }

    public void ResetZone()
    {
        spawnedEnemies = 0;
        aliveEnemyCount = 0;
        isComplete = false;
    }
}