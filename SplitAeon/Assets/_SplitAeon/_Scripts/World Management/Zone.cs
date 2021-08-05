using UnityEngine;

public class Zone : MonoBehaviour
{
    [Tooltip("0 = infinite enemies")]
    public uint maxEnemyCount;

    [HideInInspector]
    public uint deadEnemies;

    public bool shouldEnemiesSpawn = false;

    [HideInInspector]
    public ZoneManager zoneManager;

    public EnemySpawner[] enemySpawners;

    [HideInInspector]
    public bool isActive = false;

    // Start is called before the first frame update
    private void Start()
    {
        //zoneCollider = gameObject.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            zoneManager.EnterZone(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            zoneManager.ExitZone();
        }
    }

    private void IncreaseDeathCount()
    {
        deadEnemies++;
    }
}