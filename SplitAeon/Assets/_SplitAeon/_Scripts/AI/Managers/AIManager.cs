﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AIStates.Manager;

public class AIManager : MonoBehaviour
{
    public AIManagerSettings settings;

    List<AIAgent> m_allAgents = new List<AIAgent>();

    public Transform playerTransform;
    public Camera playerCam;
    public Health playerHealth;

    public UnityEvent agentdeathEvent;
    public UnityEvent spawnEvent;
    public UnityEvent damagePlayerEvent;

    public float neighbourRadius = 1.5f;

    public bool playerInTimePeriod = true;

    StateMachine<AIManager> zoneStateMachine;
    List<AgentObjectPool> m_enemyObjectPools;

    public List<AgentObjectPool> enemyObjectPools { get { return m_enemyObjectPools; } }
    public List<AIAgent> allAgents { get { return m_allAgents; } }
    public List<AIAgent> activeAgents { get { return GetAllActiveAgents(); } }

    int m_aliveCount = 0;
    public int aliveCount { get { return m_aliveCount; } }

    //Debug
    [Header("Debug")]
    public bool showNeighbourRadius = false;
    public bool showEnemyGizmos = false;

    bool isInitialised = false;

    private void Awake()
    {
        isInitialised = true;

        m_enemyObjectPools = settings.CreateObjectPools(this);

        zoneStateMachine = new StateMachine<AIManager>(this);
        zoneStateMachine.AddState(new InsideZone());
        zoneStateMachine.AddState(new OutsideZone());
        zoneStateMachine.Init();

        spawnEvent.AddListener(() => { m_aliveCount++; });
        agentdeathEvent.AddListener(() => { m_aliveCount--; });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LateUpdate()
    {
        if(!playerInTimePeriod)
        {
            return;
        }

        FindNeighbours();
    }

    // Sets pool object to active from the target object pool and gets the resulting gameobject
    public bool SetPoolObjectActive(AgentObjectPool targetPool, out AIAgent resultObject)
    {
        return targetPool.SetObjectActive(out resultObject);
    }

    public bool GetNextPoolObject(AgentObjectPool targetPool, out AIAgent resultObject)
    {
        return targetPool.GetNextObject(out resultObject);
    }

    public void SetTargetObjectActive(AgentObjectPool targetPool, AIAgent targetObject)
    {
        targetPool.SetTargetObjectActive(targetObject);
    }

    public void AllAggro()
    {
        foreach(var agent in m_allAgents)
        {
            if(agent.gameObject.activeInHierarchy)
            {
                agent.ChangeState(AIStates.StateIndex.chasePlayer);
            }
        }
    }

    public void FindNeighbours()
    {
        foreach(AIAgent agent in m_allAgents)
        {
            agent.neighbours.Clear();
        }

        for(int i = 0; i < m_allAgents.Count; i++)
        {
            for(int j = i + 1; j < m_allAgents.Count; j++)
            {
                AIAgent first = m_allAgents[i];
                AIAgent second = m_allAgents[j];

                Neighbour neighbour = Neighbour.empty;
                neighbour.FindNeighbour(first, second);

                float neighbourRadSqr = neighbourRadius * neighbourRadius;

                if (neighbour.distSqrd < neighbourRadSqr)
                {
                    first.neighbours.Add(neighbour);
                    second.neighbours.Add(-neighbour);
                }
            }
        }
    }

    public void TogglePlayerInsideState()
    {
        ZoneStateIndex index = (ZoneStateIndex)zoneStateMachine.currentIndex;
        switch(index)
        {
            case ZoneStateIndex.inside:
                {
                    ChangeZoneState(ZoneStateIndex.outside);
                    break;
                }
            case ZoneStateIndex.outside:
                {
                    ChangeZoneState(ZoneStateIndex.inside);
                    break;
                }
        }
    }

    public void ChangeZoneState(ZoneStateIndex state)
    {
        zoneStateMachine.ChangeState((int)state);
    }

    public void SetAgentPoolActiveInTimePeriod(bool value)
    {
        playerInTimePeriod = value;

        foreach (AgentObjectPool enemyPool in m_enemyObjectPools)
        {
            foreach (EnemyPoolObject poolAgent in enemyPool.objectPool)
            {
                // Check if the agent should be active in it's zone
                if (poolAgent.isActive)
                {
                    poolAgent.gameObject.SetActive(value);
                }
            }
        }
    }

    public List<AIAgent> GetAllActiveAgents()
    {
        List<AIAgent> result = new List<AIAgent>();
        foreach (AgentObjectPool enemyPool in m_enemyObjectPools)
        {
            foreach (AIAgent activeAgent in enemyPool.activeAgents)
            {
                result.Add(activeAgent);
            }
        }
        return result;
    }

    private void OnDrawGizmos()
    {
        List<AIAgent> drawList;

        if(isInitialised)
        {
            drawList = m_allAgents;
        }
        else
        {
            drawList = new List<AIAgent>();

            var agentArray = FindObjectsOfType<AIAgent>();

            foreach (var agent in agentArray)
            {
                drawList.Add(agent);
            }
        }

        if(showNeighbourRadius)
        {
            foreach (var agent in drawList)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(agent.transform.position, neighbourRadius);
            }
        }
    }
}
