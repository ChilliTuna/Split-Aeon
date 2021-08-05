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

    public void IncreaseDeathCount()
    {
        deadEnemies++;
        if (deadEnemies >= maxEnemyCount)
        {
            SetActiveness(false);
        }
    }

    public void SetActiveness(bool state)
    {
        isActive = state;
        if (state == true)
        {
            if (deadEnemies < maxEnemyCount)
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
}