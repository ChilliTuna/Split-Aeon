using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public AIManager aiManager;

    public List<Transform> fixedSpawnLocations;
    public List<Transform> dynamicSpawnLocations;

    bool m_isSpawning = false;

    List<GameObject> cultistPool;

    public float spawnTime = 5.0f;
    float m_spawnTimer = 0.0f;

    private void Awake()
    {
        cultistPool = aiManager.cultistPool;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_spawnTimer += Time.deltaTime;
        if(m_spawnTimer > spawnTime)
        {
            m_spawnTimer -= spawnTime;
            Spawn();
        }
    }

    public void Spawn()
    {
        GameObject cultist;
        if(aiManager.SetCultistActive(out cultist))
        {
            // spawn will succeed
            cultist.transform.position = GetSpawnPosition();

            cultist.GetComponent<AIAgent>().ChangeState(AIStates.StateIndex.chasePlayer);
        }
        else
        {
            // spawn will fail
        }
    }

    Vector3 GetSpawnPosition()
    {
        // Just simply random for now
        int fixedRand = Random.Range(0, fixedSpawnLocations.Count);

        return fixedSpawnLocations[fixedRand].position;
    }
}
