using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Scriptables/AISettings")]
public class AISettings : ScriptableObject
{
    [Header("Idle")]
    public float maxIdleTime = 2.0f;
    public float minIdleTime = 1.0f;

    [Header("Wander")]
    public float stoppingDistance = 0.5f;

    [Header("Aggro")]
    public float aggresionRadius = 4.0f;
    public float attackRadius = 1.0f;
}
