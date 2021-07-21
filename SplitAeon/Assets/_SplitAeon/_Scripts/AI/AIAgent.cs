﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIStates;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    [HideInInspector]public AIManager aiManager;

    public AISettings settings;
    public Animator anim;
    public HitBox armAttack;

    StateMachine<AIAgent> m_stateMachine;
    NavMeshAgent m_navAgent;
    Ragdoll m_ragdoll;

    float m_distToPlayerSquared;

    List<Neighbour> m_neighbours = new List<Neighbour>();

    public NavMeshAgent navAgent { get { return m_navAgent; } }
    public float distToPlayerSquared { get { return m_distToPlayerSquared; } }
    public Transform playerTransform { get { return aiManager.playerTransform; } }
    public Ragdoll ragdoll {  get { return m_ragdoll; } }
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
        m_navAgent = GetComponent<NavMeshAgent>();
        m_ragdoll = GetComponent<Ragdoll>();

        // Create State Machine
        m_stateMachine = new StateMachine<AIAgent>(this);

        // Add States
        StateBucket.SetUpStateMachine(m_stateMachine);

        m_distToPlayerSquared = (playerTransform.position - transform.position).sqrMagnitude;
    }

    // Update is called once per frame
    void Update()
    {
        m_stateMachine.Update();

        anim.SetFloat("moveSpeed", currentSpeed);

        // Debugging
        debugCurrentState = (StateIndex)m_stateMachine.currentIndex;
        debugNeighbourCount = neighbours.Count;
    }

    private void LateUpdate()
    {
        m_distToPlayerSquared = (playerTransform.position - transform.position).sqrMagnitude;
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
        aiManager.damagePlayerEvent.Invoke();
    }

    public void Die()
    {
        // This would ideally have an animation as well as some sort of clean up for corpses
        // For now just Change state to dead which will activate a ragdoll
        ChangeState(StateIndex.dead);
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

    private void OnDrawGizmos()
    {
        return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, settings.aggresionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, settings.attackChargeRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, settings.orbWalkRadius);
    }
}
