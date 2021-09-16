using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Scriptables/AIManagerSettings")]
public class AIManagerSettings : ScriptableObject
{
    [System.Serializable]
    public struct EnemyInfo
    {
        public GameObject targetPrefab;
        public int maxCount;
        public string containerName;
        public EnemyType enemyType;
    }

    public EnemyInfo[] enemies;

    public List<AgentObjectPool> CreateObjectPools(AIManager aiManager)
    {
        List<AgentObjectPool> enemyPoolList = new List<AgentObjectPool>();
        foreach(EnemyInfo enemyInfo in enemies)
        {
            enemyPoolList.Add(CreatePool(aiManager, enemyInfo));
        }
        return enemyPoolList;
    }

    static AgentObjectPool CreatePool(AIManager aiManager, EnemyInfo enemyInfo)
    {
        AgentObjectPool enemyPool = new AgentObjectPool();
        enemyPool.InitialiseObjectPool(aiManager, enemyInfo.containerName, enemyInfo.maxCount, enemyInfo.targetPrefab, enemyInfo.enemyType);
        return enemyPool;
    }
}
