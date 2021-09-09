using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public AIManager aiManager;
    [HideInInspector] public SpawnManager spawnManager;

    List<SpawnLocation> m_fixedSpawnLocations;
    List<SpawnLocation> m_dynamicSpawnLocations;

    [Header("Wave control")]
    public int miniWaveEnemyCount = 5;
    public float miniWaveTime = 0.01f;

    public int targetEnemyCount = 5;
    public int allowableOverSpawnLimit = 2;

    public float spawnTime = 5.0f;

    [Header("Initialiser values")]
    public EnemyType enemyType;
    public Vector3 boxSize = new Vector3(5.0f, 5.0f, 5.0f);
    
    Transform m_playerTransform;
    Camera m_playerCam;

    float m_spawnTimer = 0.0f;

    int m_currentMiniWaveRemain = 0;
    float m_miniWaveTimer = 0.0f;

    List<SpawnLocation> m_possibleLocations = new List<SpawnLocation>();

    List<AgentObjectPool> m_enemyObjectPools;

    private void Awake()
    {
        m_playerTransform = aiManager.playerTransform;
        m_playerCam = aiManager.playerCam;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_fixedSpawnLocations = new List<SpawnLocation>();
        m_dynamicSpawnLocations = new List<SpawnLocation>();

        SpawnLocation[] allSpawnLocations = FindObjectsOfType<SpawnLocation>();
        foreach (var spawnPos in allSpawnLocations)
        {
            Vector3 position = spawnPos.transform.position;
            Vector3 halfBox = boxSize / 2;
            // width check
            if(position.x < transform.position.x - halfBox.x)
            {
                continue;
            }
            if (position.x > transform.position.x + halfBox.x)
            {
                continue;
            }

            // height check
            if (position.y < transform.position.y - halfBox.y)
            {
                continue;
            }
            if (position.y > transform.position.y + halfBox.y)
            {
                continue;
            }

            // length check
            if (position.z < transform.position.z - halfBox.z)
            {
                continue;
            }
            if (position.z > transform.position.z + halfBox.z)
            {
                continue;
            }

            if(spawnPos.spawnType == SpawnLocation.SpawnType.DYNAMIC)
            {
                m_dynamicSpawnLocations.Add(spawnPos);
            }
            else if(spawnPos.spawnType == SpawnLocation.SpawnType.FIXED)
            {
                m_fixedSpawnLocations.Add(spawnPos);
            }
        }

        FindObjectPools();
    }

    // Update is called once per frame
    void Update()
    {
        if(aiManager.playerInTimePeriod)
        {
            m_spawnTimer += Time.deltaTime;
            if (m_spawnTimer > spawnTime)
            {
                m_spawnTimer -= spawnTime;
                InitiateMiniWave(aiManager.activeAgentCount);
            }

            if (m_currentMiniWaveRemain > 0)
            {
                m_miniWaveTimer += Time.deltaTime;
                if (m_miniWaveTimer > miniWaveTime)
                {
                    m_miniWaveTimer -= miniWaveTime;
                    MiniWaveSpawn();
                }
            }
        }
    }

    void FindObjectPools()
    {
        m_enemyObjectPools = new List<AgentObjectPool>();

        if(enemyType.HasFlag(EnemyType.cultist))
        {
            m_enemyObjectPools.Add(aiManager.cultistPool);
        }
        if (enemyType.HasFlag(EnemyType.belcher))
        {
            m_enemyObjectPools.Add(aiManager.belcherPool);
        }
    }

    AgentObjectPool GetRandomPool()
    {
        // find the target pool from aiManager

        // use float values to find chance of spawn

        // use randomRange to find final result
        int randIndex = Random.Range(0, m_enemyObjectPools.Count);

        return m_enemyObjectPools[randIndex];
    }

    public void Spawn()
    {
        GameObject enemyObject;
        if(aiManager.SetPoolObjectActive(GetRandomPool(), out enemyObject))
        {
            // spawn will succeed
            SpawnLocation location = GetSpawnLocation();
            if(location != null)
            {
                location.StartSpawning();

                AIAgent agent = enemyObject.GetComponent<AIAgent>();

                agent.ChangeState(AIStates.StateIndex.chasePlayer);
                agent.navAgent.Warp(location.transform.position);

                aiManager.spawnEvent.Invoke();
            }
            else
            {
                // no available spawns some how. Likely there are no spawns around the player
            }
        }
        else
        {
            // spawn will fail
        }
    }

    SpawnLocation GetSpawnLocation()
    {
        m_possibleLocations.Clear();

        HashSet<SpawnLocation> playerAdjacentLocations = spawnManager.GetPlayerAdjacentCellSpawnLocations();

        //foreach(var fixedSpawn in m_fixedSpawnLocations)
        //{
        //    m_possibleLocations.Add(fixedSpawn);
        //}
        //
        //foreach (var dynamicSpawn in m_dynamicSpawnLocations)
        //{
        //    Vector3 position = dynamicSpawn.transform.position;
        //    Vector3 toPlayer = m_playerTransform.position - position;
        //    if (Physics.Raycast(position, toPlayer, toPlayer.magnitude))
        //    {
        //        // Object is in the way of the player
        //        m_possibleLocations.Add(dynamicSpawn);
        //    }
        //}

        foreach (var location in playerAdjacentLocations)
        {
            switch(location.spawnType)
            {
                case SpawnLocation.SpawnType.FIXED:
                    {
                        m_possibleLocations.Add(location);
                        break;
                    }
                case SpawnLocation.SpawnType.DYNAMIC:
                    {
                        Vector3 position = location.transform.position;
                        Vector3 toPlayer = m_playerTransform.position - position;
                        if (Physics.Raycast(position, toPlayer, toPlayer.magnitude))
                        {
                            // Object is in the way of the player
                            m_possibleLocations.Add(location);
                        }
                        break;
                    }
            }
        }

        // Just simply random for now
        if(m_possibleLocations.Count > 0)
        {
            int rand = Random.Range(0, m_possibleLocations.Count);
            return m_possibleLocations[rand];
        }
        return null;
    }

    public void InitiateMiniWave(int currentActiveAgentCount)
    {
        int amountToAdd = miniWaveEnemyCount - currentActiveAgentCount;
        if(amountToAdd <= 0)
        {
            amountToAdd = allowableOverSpawnLimit;
        }

        m_currentMiniWaveRemain = amountToAdd;
        m_miniWaveTimer = 0.0f;
    }

    void MiniWaveSpawn()
    {
        m_currentMiniWaveRemain--;
        Spawn();
    }

    private void OnDrawGizmos()
    {
        Transform drawPlayer = m_playerTransform;
        if(m_playerTransform == null)
        {
            drawPlayer = aiManager.playerTransform;
        }

        if(m_dynamicSpawnLocations == null)
        {
            return;
        }

        foreach (var dynamicSpawn in m_dynamicSpawnLocations)
        {
            Vector3 position = dynamicSpawn.transform.position;
            Vector3 toPlayer = drawPlayer.position - position;
            if (Physics.Raycast(position, toPlayer, out RaycastHit hitInfo, toPlayer.magnitude))
            {
                Gizmos.color = Color.red;
                // Object is in the way of the player
                Gizmos.DrawLine(position, hitInfo.point);
            }
            else
            {
                Gizmos.color = Color.blue;
                // no object
                Gizmos.DrawLine(position, drawPlayer.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color colour = Color.magenta;

        colour.a *= 0.4f;
        Gizmos.color = colour;
        Gizmos.DrawCube(transform.position, boxSize);
    }
}
