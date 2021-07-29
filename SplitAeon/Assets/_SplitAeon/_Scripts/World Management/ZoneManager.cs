using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public AIManager aiManager;

    public Zone[] zones;

    //[HideInInspector]
    public Zone activeZone;

    [HideInInspector]
    public GameManager gameManger;

    private void Start()
    {
        AssignToZones();
    }

    public void EnterZone(Zone zone)
    {
        for (int i = 0; i < zones.Length; i++)
        {
            if (zones[i] == zone)
            {
                activeZone = zone;
            }
        }
        SetSpawnerActiveness(activeZone, true);
    }

    public void ExitZone()
    {
        SetSpawnerActiveness(activeZone, true);
        activeZone = null;
    }

    private void AssignToZones()
    {
        for (int i = 0; i < zones.Length; i++)
        {
            zones[i].zoneManager = this;
        }
    }

    /// <summary>
    /// Inverts activeness
    /// </summary>
    /// <param name="zone"></param>
    public void SetSpawnerActiveness(Zone zone)
    {
        foreach (EnemySpawner enemySpawner in zone.enemySpawners)
        {
            enemySpawner.enabled = !enemySpawner.isActiveAndEnabled;
        }
    }

    /// <summary>
    /// Sets activeness
    /// </summary>
    /// <param name="zone"></param>
    /// <param name="newActive"></param>
    public void SetSpawnerActiveness(Zone zone, bool newActive)
    {
        foreach (EnemySpawner enemySpawner in zone.enemySpawners)
        {
            enemySpawner.enabled = newActive;
        }
    }

}