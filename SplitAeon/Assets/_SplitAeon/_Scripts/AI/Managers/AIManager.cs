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

    public float neighbourRadius = 1.5f;

    public bool playerInZone = true;

    public List<AIAgent> allAgents { get { return m_allAgents; } }

    StateMachine<AIManager> zoneStateMachine;

    [Header("Cultist")]
    public GameObject cultistPrefab;
    public uint maxCultistCount = 50;
    public string containerName = "Cultist Container";

    // Cultist object pool
    List<EnemyPoolObject> m_cultistPool = new List<EnemyPoolObject>();
    public List<EnemyPoolObject> cultistPool { get { return m_cultistPool; } }
    int m_currentCultistIndex = 0;
    int m_activeCultistCount = 0;

    public int activeCultistCount { get { return m_activeCultistCount; } }

    //Debug
    [Header("Debug")]
    public bool showNeighbourRadius = false;
    public bool showEnemyGizmos = false;

    bool isInitialised = false;

    private void Awake()
    {
        isInitialised = true;

        InitialiseCultistObjectPool();

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

    void InitialiseCultistObjectPool()
    {
        GameObject cultistContainer = new GameObject(containerName);

        // Find existing cultists
        var agentArray = FindObjectsOfType<AIAgent>();
        foreach (var agent in agentArray)
        {
            m_allAgents.Add(agent);
            agent.aiManager = this;
        }

        // Create Object Pool
        for (int i = 0; i < maxCultistCount; i++)
        {
            var newCultist = Instantiate(cultistPrefab, cultistContainer.transform);
            EnemyPoolObject poolAgent = new EnemyPoolObject(newCultist, this);
            m_cultistPool.Add(poolAgent);

            m_allAgents.Add(poolAgent.agent);
        }
    }

    public bool SetCultistActive(out GameObject cultistObject)
    {
        bool result = false;

        int startIndex = m_currentCultistIndex;
        cultistObject = null;

        do
        {
            m_currentCultistIndex++;
            if (m_currentCultistIndex >= maxCultistCount)
            {
                m_currentCultistIndex = 0;
            }

            EnemyPoolObject target = m_cultistPool[m_currentCultistIndex];
            if (!target.gameObject.activeInHierarchy)
            {
                target.SetActive(true);
                cultistObject = target.gameObject;
                cultistObject.SetActive(true);
                m_activeCultistCount++;
                result = true;
                break;
            }

        } while (m_currentCultistIndex != startIndex);

        return result;
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
