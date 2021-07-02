﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIManager : MonoBehaviour
{
    List<AIAgent> m_allAgents = new List<AIAgent>();

    public Transform playerTransform;

    public UnityEvent damagePlayerEvent;

    private void Awake()
    {
        var agentArray = FindObjectsOfType<AIAgent>();

        foreach(var agent in agentArray)
        {
            m_allAgents.Add(agent);
            agent.aiManager = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AllAggro()
    {
        foreach(var agent in m_allAgents)
        {
            agent.ChangeState(AIStates.StateIndex.chasePlayer);
        }
    }
}
