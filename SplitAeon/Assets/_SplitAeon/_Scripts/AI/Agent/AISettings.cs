﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Scriptables/AISettings")]
public class AISettings : ScriptableObject
{
    [Header("Type")]
    public EnemyType enemyType;

    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float angularSpeed = 120.0f;
    public float acceleration = 8.0f;

    [Header("Gameplay")]
    public float health = 30;

    [Header("Idle")]
    public float maxIdleTime = 2.0f;
    public float minIdleTime = 1.0f;

    [Header("Wander")]
    public float stoppingDistance = 0.5f;

    [Header("Aggro")]
    public float aggresionRadius = 4.0f;
    public float orbWalkRadius = 1.0f;
    public float attackChargeRadius = 1.0f;
    public float attackChargeMax = 1.0f;
    public float attackChargeRate = 1.0f;

    [Header("Attack")]
    public float attackDamage = 10.0f;
    public float rotationLerpSpeed = 0.1f;
    public float afterAttackLerpSpeed = 0.01f;

    [Header("Death")]
    public float bodyDecayTime = 5.0f;

    private void OnValidate()
    {
        if(Application.isPlaying)
        {
            switch (enemyType)
            {
                case EnemyType.cultist:
                    {
                        var managers = FindObjectsOfType<AIManager>();
                        foreach (var manager in managers)
                        {
                            foreach(var agent in manager.activeAgents)
                            {
                                if(agent.settings.enemyType == EnemyType.cultist)
                                {
                                    agent.StabiliseSettings();
                                }
                            }
                        }
                        break;
                    }
                case EnemyType.belcher:
                    {
                        var managers = FindObjectsOfType<AIManager>();
                        foreach (var manager in managers)
                        {
                            foreach (var agent in manager.activeAgents)
                            {
                                if (agent.settings.enemyType == EnemyType.belcher)
                                {
                                    agent.StabiliseSettings();
                                }
                            }
                        }
                        break;
                    }
                default:
                    {
                        Debug.Log("Pick an enemyType");
                        break;
                    }
            }
        }
    }
}
