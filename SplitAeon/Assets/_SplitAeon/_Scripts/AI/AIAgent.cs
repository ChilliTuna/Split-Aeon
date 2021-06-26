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

    public StateMachine stateMachine { get { return m_stateMachine; } }
    public NavMeshAgent navAgent { get { return m_navAgent; } }

    // Debug
    [Header("Debugging")]
    [SerializeField]
    StateIndex currentState;
    
    public List<Transform> wanderNodes;
    public int currentWanderIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        m_navAgent = GetComponent<NavMeshAgent>();

        // Create State Machine
        m_stateMachine = new StateMachine(this);

        // Add States
        StateBucket.SetUpStateMachine(m_stateMachine);
    }

    // Update is called once per frame
    void Update()
    {
        m_stateMachine.Update();

        // Debugging
        currentState = (StateIndex)m_stateMachine.currentIndex;
    }
}
