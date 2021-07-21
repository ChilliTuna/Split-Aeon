using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    AIAgent agentRef;
    List<IState> m_states = new List<IState>();

    IState m_currentState;
    int m_currentIndex = 0;

    public int currentIndex { get { return m_currentIndex; } }

    public StateMachine(AIAgent agent)
    {
        agentRef = agent;
    }

    public bool Init()
    {
        if(m_states.Count > 0)
        {
            m_currentState = m_states[0];
            m_currentState.Enter(agentRef);
            m_currentIndex = 0;
            return true;
        }
        return false;
    }

    public void AddState(IState newState)
    {
        m_states.Add(newState);
    }

    public void ChangeState(int index)
    {
        m_currentState.Exit(agentRef);

        m_currentState = m_states[index];
        m_currentIndex = index;

        m_currentState.Enter(agentRef);
    }

    public void Update()
    {
        m_currentState.Update(agentRef);
    }
}

public interface IState
{
    void Enter(AIAgent agent);

    void Exit(AIAgent agent);

    void Update(AIAgent agent);
}
