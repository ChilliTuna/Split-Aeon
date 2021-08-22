using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolObject
{
    AgentObjectPool m_objectPool;

    GameObject m_gameObject;
    AIAgent m_agent;
    bool m_isActive = false;

    public GameObject gameObject { get { return m_gameObject; } }
    public AIAgent agent { get { return m_agent; } }
    public bool isActive { get { return m_isActive; } }

    public EnemyPoolObject(AgentObjectPool objectPool, GameObject agentGameObject, AIManager aiManager, bool isActive = false)
    {
        m_objectPool = objectPool;

        m_gameObject = agentGameObject;
        
        m_agent = m_gameObject.GetComponent<AIAgent>();
        m_agent.aiManager = aiManager;
        m_agent.Init();

        m_isActive = isActive;

        agentGameObject.GetComponent<AIAgent>().attachedPoolObject = this;

        m_gameObject.SetActive(m_isActive);
    }

    public void SetActive(bool value)
    {
        m_isActive = value;
    }

    public void Disable()
    {
        m_objectPool.DisableObject(this);
    }
}
