using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIStates;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    public AISettings settings;

    StateMachine m_stateMachine;
    NavMeshAgent m_navAgent;

    public NavMeshAgent navAgent { get { return m_navAgent; } }

    float m_distToPlayerSquared;

    public float distToPlayerSquared { get { return m_distToPlayerSquared; } }

    // Debug
    [Header("Debugging")]
    [SerializeField]
    StateIndex currentState;
    
    public List<Transform> wanderNodes;
    public int currentWanderIndex = 0;

    public Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();

        // Create State Machine
        m_stateMachine = new StateMachine(this);

        // Add States
        StateBucket.SetUpStateMachine(m_stateMachine);

        m_distToPlayerSquared = (playerTransform.position - transform.position).sqrMagnitude;
    }

    // Update is called once per frame
    void Update()
    {
        m_stateMachine.Update();

        // Debugging
        currentState = (StateIndex)m_stateMachine.currentIndex;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, settings.aggresionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, settings.attackChargeRadius);
    }
}
