﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public AIManager aiManager;

    List<SpawnLocation> m_fixedSpawnLocations;
    List<SpawnLocation> m_dynamicSpawnLocations;

    public int miniWaveEnemyCount = 5;
    public float miniWaveTime = 0.01f;

    public int targetEnemyCount = 5;
    public int allowableOverSpawnLimit = 2;

    List<EnemyPoolObject> cultistPool;
    Transform playerTransform;
    Camera playerCam;

    public float spawnTime = 5.0f;
    float m_spawnTimer = 0.0f;

    int m_currentMiniWaveRemain = 0;
    float m_miniWaveTimer = 0.0f;

    [Header("Box")]
    public Vector3 boxSize = new Vector3(5.0f, 5.0f, 5.0f);

    List<SpawnLocation> m_possibleLocations = new List<SpawnLocation>();

    private void Awake()
    {
        cultistPool = aiManager.cultistPool;
        playerTransform = aiManager.playerTransform;
        playerCam = aiManager.playerCam;
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
            if (position.z > transform.position.x + halfBox.z)
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
    }

    // Update is called once per frame
    void Update()
    {
        if(aiManager.playerInZone)
        {
            m_spawnTimer += Time.deltaTime;
            if (m_spawnTimer > spawnTime)
            {
                m_spawnTimer -= spawnTime;
                InitiateMiniWave();
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

    public void Spawn()
    {
        GameObject cultist;
        if(aiManager.SetCultistActive(out cultist))
        {
            // spawn will succeed
            SpawnLocation location = GetSpawnLocation();
            location.StartSpawning();

            AIAgent agent = cultist.GetComponent<AIAgent>();

            agent.ChangeState(AIStates.StateIndex.chasePlayer);
            agent.navAgent.Warp(location.transform.position);
        }
        else
        {
            // spawn will fail
        }
    }

    SpawnLocation GetSpawnLocation()
    {
        m_possibleLocations.Clear();

        foreach(var fixedSpawn in m_fixedSpawnLocations)
        {
            m_possibleLocations.Add(fixedSpawn);
        }

        foreach (var dynamicSpawn in m_dynamicSpawnLocations)
        {
            Vector3 position = dynamicSpawn.transform.position;
            Vector3 toPlayer = playerTransform.position - position;
            if (Physics.Raycast(position, toPlayer, toPlayer.magnitude))
            {
                // Object is in the way of the player
                m_possibleLocations.Add(dynamicSpawn);
            }
        }

        // Just simply random for now
        int rand = Random.Range(0, m_possibleLocations.Count);

        return m_possibleLocations[rand];
    }

    public void InitiateMiniWave()
    {
        int amountToAdd = miniWaveEnemyCount - aiManager.activeCultistCount;
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
        Transform drawPlayer = playerTransform;
        if(playerTransform == null)
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
