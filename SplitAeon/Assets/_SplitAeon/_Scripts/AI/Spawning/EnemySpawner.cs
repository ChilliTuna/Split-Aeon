using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public AIManager aiManager;
    [HideInInspector] public SpawnTracker spawnTracker;

    public SpawnSettings settings;
    
    Transform m_playerTransform;
    Camera m_playerCam;

    float m_spawnTimer = 0.0f;

    int m_currentMiniWaveRemain = 0;
    float m_miniWaveTimer = 0.0f;

    List<SpawnLocation> m_possibleLocations = new List<SpawnLocation>();
    List<SpawnLocation> m_validOnCameraLocations = new List<SpawnLocation>();
    List<SpawnLocation> m_offCameraLocations = new List<SpawnLocation>();


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
            SpawnLocation location = GetSpawnLocation(agent);
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

    void FindPossibleLocations(Bounds bounds)
    {
        m_possibleLocations.Clear();

        m_offCameraLocations.Clear();
        m_validOnCameraLocations.Clear();

        HashSet<SpawnLocation> playerAdjacentLocations = spawnTracker.GetPlayerAdjacentCellSpawnLocations();

        foreach (var location in playerAdjacentLocations)
        {
            // Check to see if this location is spawnable.
            if (location.spawnType == SpawnLocation.SpawnType.FIXED)
            {
                // if it's fixed we don't need to bother checking camera views.
                m_possibleLocations.Add(location);
                continue;
            }

            bounds = location.FindBounds(bounds);

            if (location.IsInCameraView(m_playerCam, bounds))
            {
                // location is in cam view

                if (location.AgentBoundsRayCheck(bounds, m_playerCam.transform.position, settings.environmentMask))
                {
                    // location is safe to spawn at
                    m_validOnCameraLocations.Add(location);
                    m_possibleLocations.Add(location);
                }
            }
            else
            {
                // location is off screen
                m_offCameraLocations.Add(location);
                m_possibleLocations.Add(location);
            }
        }
    }

    SpawnLocation GetSpawnLocation(AIAgent agent)
    {
        FindPossibleLocations(agent.GetBounds());

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
        if(!Application.isPlaying)
        {
            return;
        }

        Gizmos.color = Color.red;

        Gizmos.matrix = Matrix4x4.TRS(m_playerCam.transform.position, m_playerCam.transform.rotation, new Vector3(m_playerCam.aspect, 1.0f, 1.0f));
        Gizmos.DrawFrustum(Vector3.zero, m_playerCam.fieldOfView, m_playerCam.farClipPlane, m_playerCam.nearClipPlane, 1.0f);
    }
}
