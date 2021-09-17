using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public AIManager aiManager;
    [HideInInspector] public SpawnTracker spawnTracker;

    List<SpawnLocation> m_fixedSpawnLocations;
    List<SpawnLocation> m_dynamicSpawnLocations;

    public SpawnSettings settings;

    [Header("Initialiser values")]
    public Vector3 boxSize = new Vector3(5.0f, 5.0f, 5.0f);
    
    Transform m_playerTransform;
    Camera m_playerCam;

    float m_spawnTimer = 0.0f;

    int m_currentMiniWaveRemain = 0;
    float m_miniWaveTimer = 0.0f;

    List<SpawnLocation> m_possibleLocations = new List<SpawnLocation>();

    List<AgentObjectPool> m_enemyObjectPools;

    bool m_isInitialised = false;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void Init()
    {
        if(m_isInitialised)
        {
            return;
        }
        m_isInitialised = true;

        m_playerTransform = aiManager.playerTransform;
        m_playerCam = aiManager.playerCam;

        m_fixedSpawnLocations = new List<SpawnLocation>();
        m_dynamicSpawnLocations = new List<SpawnLocation>();

        SpawnLocation[] allSpawnLocations = FindObjectsOfType<SpawnLocation>();
        foreach (var spawnPos in allSpawnLocations)
        {
            Vector3 position = spawnPos.transform.position;
            Vector3 halfBox = boxSize / 2;
            // width check
            if (position.x < transform.position.x - halfBox.x)
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

            if (spawnPos.spawnType == SpawnLocation.SpawnType.DYNAMIC)
            {
                m_dynamicSpawnLocations.Add(spawnPos);
            }
            else if (spawnPos.spawnType == SpawnLocation.SpawnType.FIXED)
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
            if (m_spawnTimer > settings.waveSeperationTime)
            {
                m_spawnTimer -= settings.waveSeperationTime;
                InitiateMiniWave(aiManager.aliveCount);
            }

            if (m_currentMiniWaveRemain > 0)
            {
                m_miniWaveTimer += Time.deltaTime;
                if (m_miniWaveTimer > settings.miniWaveTime)
                {
                    m_miniWaveTimer -= settings.miniWaveTime;
                    MiniWaveSpawn();
                }
            }
        }
    }

    void FindObjectPools()
    {
        m_enemyObjectPools = aiManager.enemyObjectPools;
    }

    AgentObjectPool GetRandomPool()
    {
        // find the target pool from aiManager

        // use float values to find chance of spawn

        // use randomRange to find final result
        int randIndex = Random.Range(0, m_enemyObjectPools.Count);

        return m_enemyObjectPools[randIndex];
    }

    public bool GetEnemyAgent(out AIAgent agent)
    {
        return aiManager.SetPoolObjectActive(GetRandomPool(), out agent);
    }

    public void Spawn()
    {
        if (GetEnemyAgent(out AIAgent agent))
        {
            // spawn will succeed
            Vector3 spawnPosition = Vector3.zero;
            SpawnLocation location = GetSpawnLocation();
            if(location != null)
            {
                location.StartSpawning();

                spawnPosition = location.transform.position;
            }
            else
            {
                // no available spawns some how. Likely there are no spawns around the player
            }

            SpawnEvent(agent, spawnPosition, AIStates.StateIndex.chasePlayer);
        }
        else
        {
            // spawn will fail
        }
    }

    void SpawnEvent(AIAgent agent, Vector3 position, AIStates.StateIndex stateIndex)
    {
        agent.ChangeState(stateIndex);
        agent.navAgent.Warp(position);

        agent.ResetHealth();

        aiManager.spawnEvent.Invoke();
    }

    public void SpawnPassive(Vector3 position, List<Transform> patrolNodes)
    {
        if (GetEnemyAgent(out AIAgent agent))
        {
            agent.patrolNodes = patrolNodes;

            SpawnEvent(agent, position, AIStates.StateIndex.idle);
        }
        else
        {
            // spawn will fail
        }
    }

    SpawnLocation GetSpawnLocation()
    {
        m_possibleLocations.Clear();

        HashSet<SpawnLocation> playerAdjacentLocations = spawnTracker.GetPlayerAdjacentCellSpawnLocations();

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
            SpawnLocation possibleSpawn = m_possibleLocations[rand];

            // If this random location is already spawning, then we need to search for another location that is free.
            int start = rand;
            do
            {
                if (possibleSpawn.isSpawning)
                {
                    rand++;
                    rand = rand % m_possibleLocations.Count;
                    possibleSpawn = m_possibleLocations[rand];
                }
                else
                {
                    return possibleSpawn;
                }
            } while (rand != start);
        }
        return null;
    }

    public void InitiateMiniWave(int currentActiveAgentCount)
    {
        int amountToAdd = settings.desiredWaveCount;
        int roof = settings.maximumEnemyPopulation + settings.allowableOverSpawnLimit - currentActiveAgentCount;
        int min = settings.allowableOverSpawnLimit;
        if (roof >= min)
        {
            amountToAdd = Mathf.Clamp(amountToAdd, min, roof);
        }
        else
        {
            amountToAdd = min;
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
