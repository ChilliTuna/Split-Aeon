using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Scriptables/AISettings")]
public class AISettings : ScriptableObject
{
    [Header("Type")]
    public EnemyType enemyType;

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
    public float rotationLerpSpeed = 0.1f;
    public float afterAttackLerpSpeed = 0.01f;

    [Header("Death")]
    public float bodyDecayTime = 5.0f;
}
