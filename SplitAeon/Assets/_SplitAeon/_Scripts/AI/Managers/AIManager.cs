using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AIStates.Manager;

public class AIManager : MonoBehaviour
{
    List<AIAgent> m_allAgents = new List<AIAgent>();
    
    public Transform playerTransform;
    public Camera playerCam;

    public UnityEvent damagePlayerEvent;
    public UnityEvent agentdeathEvent;
    public UnityEvent spawnEvent;

    public float neighbourRadius = 1.5f;

    public bool playerInTimePeriod = true;

    public List<AIAgent> allAgents { get { return m_allAgents; } }
    public List<AIAgent> activeAgents 
    {
        get 
        {
            List<AIAgent> result = new List<AIAgent>(m_cultistAgentPool.activeAgents);
            foreach(var belcher in m_belcherAgentPool.activeAgents)
            {
                result.Add(belcher);
            }
            return result;
        } 
    }

    StateMachine<AIManager> zoneStateMachine;

    [Header("Cultist")]
    public GameObject cultistPrefab;
    public int maxCultistCount = 50;
    public string cultistContainerName = "Cultist Container";

    // Cultist object pool
    AgentObjectPool m_cultistAgentPool;

    [Header("Belcher")]
    public GameObject belcherPrefab;
    public int maxBelcherCount = 25;
    public string belcherContainerName = "Belcher Container";

    // Belcher object pool
    AgentObjectPool m_belcherAgentPool;


    public int activeAgentCount { get { return activeCultistCount + activeBelcherCount; } }

    public int activeCultistCount { get { return m_cultistAgentPool.activeCount; } }
    public AgentObjectPool cultistPool { get { return m_cultistAgentPool; } }

    public int activeBelcherCount { get { return m_belcherAgentPool.activeCount; } }
    public AgentObjectPool belcherPool { get { return m_belcherAgentPool; } }


    //Debug
    [Header("Debug")]
    public bool showNeighbourRadius = false;
    public bool showEnemyGizmos = false;

    bool isInitialised = false;

    private void Awake()
    {
        isInitialised = true;

        m_cultistAgentPool = new AgentObjectPool();
        m_cultistAgentPool.InitialiseObjectPool(this, cultistContainerName, maxCultistCount, cultistPrefab, EnemyType.cultist);

        m_belcherAgentPool = new AgentObjectPool();
        m_belcherAgentPool.InitialiseObjectPool(this, belcherContainerName, maxBelcherCount, belcherPrefab, EnemyType.belcher);

        zoneStateMachine = new StateMachine<AIManager>(this);
        zoneStateMachine.AddState(new InsideZone());
        zoneStateMachine.AddState(new OutsideZone());
        zoneStateMachine.Init();
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
    public bool SetPoolObjectActive(AgentObjectPool targetPool, out GameObject resultObject)
    {
        return targetPool.SetObjectActive(out resultObject);
    }

    public bool SetCultistActive(out GameObject cultistObject)
    {
        return m_cultistAgentPool.SetObjectActive(out cultistObject);
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

        foreach (var poolAgent in cultistPool.objectPool)
        {
            // Check if the agent should be active in it's zone
            if (poolAgent.isActive)
            {
                poolAgent.gameObject.SetActive(value);
            }
        }

        foreach (var poolAgent in belcherPool.objectPool)
        {
            // Check if the agent should be active in it's zone
            if (poolAgent.isActive)
            {
                poolAgent.gameObject.SetActive(value);
            }
        }
    }

    public void AddExistingAgent(AIAgent agent, EnemyType enemyType)
    {
        switch(enemyType)
        {
            case EnemyType.cultist:
                {
                    m_cultistAgentPool.AddExistingAgent(agent);
                    break;
                }
            case EnemyType.belcher:
                {
                    m_belcherAgentPool.AddExistingAgent(agent);
                    break;
                }
        }
    }

    public List<AIAgent> GetAllActiveAgents()
    {
        List<AIAgent> activeAgents = new List<AIAgent>(m_cultistAgentPool.activeAgents);
        foreach(var belcher in m_belcherAgentPool.activeAgents)
        {
            activeAgents.Add(belcher);
        }
        return activeAgents;
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
