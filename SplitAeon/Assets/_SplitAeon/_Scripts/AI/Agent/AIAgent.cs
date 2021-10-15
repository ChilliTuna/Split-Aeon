﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIStates;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AIManager aiManager;
    [HideInInspector]public EnemyPoolObject attachedPoolObject;

    public AISettings settings;
    public Animator anim;
    public AttackType attack;
    public CapsuleCollider charCollider;

    bool m_isInitialised = false;
    StateMachine<AIAgent> m_stateMachine;
    NavMeshAgent m_navAgent;
    Ragdoll m_ragdoll;
    Health m_health;

    float m_distToPlayerSquared;

    List<Neighbour> m_neighbours = new List<Neighbour>();

    public List<Collider> headColliders;
    public List<Collider> upperTorsoColliders;
    public List<Collider> lowerTorsoColliders;
    public List<Collider> limbColliders;

    public NavMeshAgent navAgent { get { return m_navAgent; } }
    public float distToPlayerSquared { get { return m_distToPlayerSquared; } }
    public Transform playerTransform { get { return aiManager.playerTransform; } }
    public Ragdoll ragdoll {  get { return m_ragdoll; } }
    public Health health { get { return m_health; } }
    public List<Neighbour> neighbours { get { return m_neighbours; } }

    public float currentSpeed { get { return m_navAgent.velocity.magnitude; } }

    // Debug
    [Header("Debugging")]
    [SerializeField]
    StateIndex debugCurrentState;

    public int debugNeighbourCount = 0;
    
    public List<Transform> patrolNodes;
    public int currentPatrolIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        m_stateMachine.Update();

        anim.SetFloat("moveSpeed", currentSpeed / navAgent.speed);

        // Debugging
        debugCurrentState = (StateIndex)m_stateMachine.currentIndex;
        debugNeighbourCount = neighbours.Count;
    }

    private void LateUpdate()
    {
        m_distToPlayerSquared = (playerTransform.position - transform.position).sqrMagnitude;
    }

    public void Init()
    {
        if(m_isInitialised)
        {
            return;
        }

        m_isInitialised = true;

        m_navAgent = GetComponent<NavMeshAgent>();
        m_ragdoll = GetComponent<Ragdoll>();
        m_health = GetComponent<Health>();

        // Create State Machine
        m_stateMachine = new StateMachine<AIAgent>(this);

        // Add States
        StateBucket.SetUpStateMachine(m_stateMachine);

        m_stateMachine.Init();

        m_distToPlayerSquared = (playerTransform.position - transform.position).sqrMagnitude;

        StabiliseSettings();
        ResetHealth();
    }

    public void ChangeState(int stateIndex)
    {
        m_stateMachine.ChangeState(stateIndex);
    }

    public void ChangeState(StateIndex stateIndex)
    {
        ChangeState((int)stateIndex);
    }

    public void StartNavigating()
    {
        m_navAgent.isStopped = false;
        m_navAgent.updatePosition = true;
    }

    public void StopNavigating()
    {
        m_navAgent.isStopped = true;
        m_navAgent.updatePosition = false;
    }

    public void DamagePlayer()
    {
        aiManager.playerHealth.Hit(settings.attackDamage, null);
        aiManager.damagePlayerEvent.Invoke();
    }

    public void BeforeHit()
    {
        Collider hitCollider = health.hitCollider;

        RagdollExtra.RagdollType type = hitCollider.GetComponent<RagdollExtra>().GetRagdollType();
        switch (type)
        {
            case RagdollExtra.RagdollType.head:
                {
                    health.damageMultiplier = settings.headMultiplier;
                    break;
                }
            case RagdollExtra.RagdollType.upperTorso:
                {
                    health.damageMultiplier = settings.upperTorsoMultipler;
                    break;
                }
            case RagdollExtra.RagdollType.lowerTorso:
                {
                    health.damageMultiplier = settings.lowerTorsoMultiplier;
                    break;
                }
            case RagdollExtra.RagdollType.limb:
                {
                    health.damageMultiplier = settings.limbMultiplier;
                    break;
                }
        }
    }

    public void Die()
    {
        if(m_stateMachine.currentIndex == (int)StateIndex.dead)
        {
            // Check if this is already in the dead state and if it is, do nothing
            return;
        }

        // This would ideally have an animation as well as some sort of clean up for corpses
        // For now just Change state to dead which will activate a ragdoll
        aiManager.agentdeathEvent.Invoke();
        ChangeState(StateIndex.dead);
    }

    public void ResetHealth()
    {
        m_health.health = m_health.maxHealth;
    }

    public void DisablePoolObject()
    {
        attachedPoolObject.Disable();
    }

    public void Seek()
    {
        if(neighbours.Count == 0)
        {
            return;
        }

        Vector3 seekDir = Vector3.zero;

        foreach(var neighbour in neighbours)
        {
            seekDir -= neighbour.offset;
        }

        seekDir /= neighbours.Count;
        seekDir = Vector3.ClampMagnitude(seekDir, m_navAgent.speed);

        m_navAgent.Move(seekDir * Time.deltaTime);
    }

    // This function is called to ensure that the agents components match the agent's settings
    public void StabiliseSettings()
    {
        m_navAgent.speed = settings.moveSpeed;
        m_navAgent.angularSpeed = settings.angularSpeed;
        m_navAgent.acceleration = settings.acceleration;

        m_health.maxHealth = settings.health;
    }

    public Bounds GetBounds()
    {
        return charCollider.bounds;
    }

    private void OnDrawGizmos()
    {
        if(aiManager == null || !aiManager.showEnemyGizmos)
        {
            return;
        }
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, settings.aggresionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, settings.attackChargeRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, settings.orbWalkRadius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, settings.aggresionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, settings.attackChargeRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, settings.orbWalkRadius);
    }
}
