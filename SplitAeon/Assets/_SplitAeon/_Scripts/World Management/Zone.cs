using UnityEngine;

public class Zone : MonoBehaviour
{
    private Collider zoneCollider;

    [HideInInspector]
    public ZoneManager zoneManager;

    public EnemySpawner[] enemySpawners;

    // Start is called before the first frame update
    private void Start()
    {
        zoneCollider = gameObject.GetComponent<Collider>();
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
}