using UnityEngine;
using System.Collections.Generic;

public class ZoneManager : MonoBehaviour
{
    public Zone[] zones;

    [HideInInspector]
    public Zone activeZone;

    [HideInInspector]
    public GameManager gameManger;

    [HideInInspector]
    public List<Zone> completedZones;

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
        ChangeSpawnerActiveness(activeZone, true);
    }

    public void ExitZone()
    {
        ChangeSpawnerActiveness(activeZone, false);
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
    public void ChangeSpawnerActiveness(Zone zone)
    {
        zone.SetActiveness(!zone.isActive);
    }

    /// <summary>
    /// Sets activeness
    /// </summary>
    /// <param name="zone"></param>
    /// <param name="activeness"></param>
    public void ChangeSpawnerActiveness(Zone zone, bool activeness)
    {
        zone.SetActiveness(activeness);
    }

}