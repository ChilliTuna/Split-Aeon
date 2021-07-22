﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIManager : MonoBehaviour
{
    List<AIAgent> m_allAgents = new List<AIAgent>();

    public Transform playerTransform;

    public UnityEvent damagePlayerEvent;

    public float neighbourRadius = 1.5f;

    public List<AIAgent> allAgents { get { return m_allAgents; } }

    //Debug
    [Header("Debug")]
    public bool showNeighbourRadius = false;
    public bool showEnemyGizmos = false;

    bool isInitialised = false;

    private void Awake()
    {
        var agentArray = FindObjectsOfType<AIAgent>();

        foreach(var agent in agentArray)
        {
            m_allAgents.Add(agent);
            agent.aiManager = this;
        }

        isInitialised = true;
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
        FindNeighbours();
    }

    public void AllAggro()
    {
        foreach(var agent in m_allAgents)
        {
            agent.ChangeState(AIStates.StateIndex.chasePlayer);
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
