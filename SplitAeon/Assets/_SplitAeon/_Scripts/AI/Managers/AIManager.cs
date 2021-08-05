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

    public float neighbourRadius = 1.5f;

    public bool playerInZone = true;

    public List<AIAgent> allAgents { get { return m_allAgents; } }

    StateMachine<AIManager> zoneStateMachine;

    [Header("Cultist")]
    public GameObject cultistPrefab;
    public int maxCultistCount = 50;
    public string cultistContainerName = "Cultist Container";

    // Cultist object pool
    public List<EnemyPoolObject> cultistPool { get { return m_cultistAgentPool.objectPool; } }

    AgentObjectPool m_cultistAgentPool;

    public int activeCultistCount { get { return m_cultistAgentPool.activeCultistCount; } }


    //Debug
    [Header("Debug")]
    public bool showNeighbourRadius = false;
    public bool showEnemyGizmos = false;

    bool isInitialised = false;

    private void Awake()
    {
        isInitialised = true;

        m_cultistAgentPool = new AgentObjectPool();
        m_cultistAgentPool.InitialiseCultistObjectPool(this, cultistContainerName, maxCultistCount, cultistPrefab);

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
        if(!playerInZone)
        {
            return;
        }

        FindNeighbours();
    }

    public bool SetCultistActive(out GameObject cultistObject)
    {
        return m_cultistAgentPool.SetCultistActive(out cultistObject);
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

    public void ChangeZoneState(ZoneStateIndex state)
    {
        zoneStateMachine.ChangeState((int)state);
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
