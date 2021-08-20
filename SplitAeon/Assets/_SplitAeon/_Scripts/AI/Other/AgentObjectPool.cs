using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentObjectPool 
{
    // Cultist object pool
    List<EnemyPoolObject> m_objectPool = new List<EnemyPoolObject>();
    public List<EnemyPoolObject> objectPool { get { return m_objectPool; } }
    int m_currentIndex = 0;
    int m_activeCount = 0;

    int m_maxCount = 0;

    public int activeCount { get { return m_activeCount; } }
    public List<AIAgent> activeAgents
    {
        get
        {
            List<AIAgent> result = new List<AIAgent>();
            foreach(var poolAgent in objectPool)
            {
                if(poolAgent.isActive)
                {
                    result.Add(poolAgent.agent);
                }
            }
            return result;
        }
    }

    public void InitialiseObjectPool(AIManager manager, string containerName, int maxCount, GameObject enemyPrefab)
    {
        m_maxCount = maxCount;

        GameObject cultistContainer = new GameObject(containerName);

        // Find existing cultists
        var agentArray = Object.FindObjectsOfType<AIAgent>();
        foreach (var agent in agentArray)
        {
            manager.allAgents.Add(agent);
            agent.aiManager = manager;
        }

        // Create Object Pool
        for (int i = 0; i < maxCount; i++)
        {
            var newCultist = Object.Instantiate(enemyPrefab, cultistContainer.transform);
            EnemyPoolObject poolAgent = new EnemyPoolObject(this, newCultist, manager);
            m_objectPool.Add(poolAgent);

            manager.allAgents.Add(poolAgent.agent);
        }
    }

    public bool SetObjectActive(out GameObject targetPoolObject)
    {
        bool result = false;

        int startIndex = m_currentIndex;
        targetPoolObject = null;

        do
        {
            m_currentIndex++;
            if (m_currentIndex >= m_maxCount)
            {
                m_currentIndex = 0;
            }

            EnemyPoolObject target = m_objectPool[m_currentIndex];
            if (!target.gameObject.activeInHierarchy)
            {
                target.SetActive(true);
                targetPoolObject = target.gameObject;
                targetPoolObject.SetActive(true);
                m_activeCount++;
                result = true;
                break;
            }

        } while (m_currentIndex != startIndex);

        return result;
    }

    public void DisableObject(EnemyPoolObject target)
    {
        target.SetActive(false); // refernce to EnemyPoolObject
        target.gameObject.SetActive(false); // refernce to Unity's GameObject
        m_activeCount--;
    }
}
