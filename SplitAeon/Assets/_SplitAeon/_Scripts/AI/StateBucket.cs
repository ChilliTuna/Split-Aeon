using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIStates
{
    public enum StateIndex
    {
        idle,
        wander,
        attack
    }

    public static class StateBucket
    {
        public static void SetUpStateMachine(StateMachine target)
        {
            target.AddState(new Idle());
            target.AddState(new Wander());
            target.AddState(new Attack());

            target.Init();
        }
    }

    public class Idle : IState
    {
        float m_timer = 0.0f;
        float m_currentTargetTime = 0.0f;

        void IState.Enter(AIAgent agent)
        {
            // set up idle values
            m_timer = 0.0f;
            m_currentTargetTime = Random.Range(agent.settings.minIdleTime, agent.settings.maxIdleTime);

            agent.navAgent.isStopped = true;
        }

        void IState.Update(AIAgent agent)
        {
            // Check Idle behaviour
            if(m_timer > m_currentTargetTime)
            {
                // bam, no more idle
                agent.stateMachine.ChangeState((int)StateIndex.wander);
            }

            // Update idle timers
            m_timer += Time.deltaTime;
        }

        void IState.Exit(AIAgent agent)
        {
            // clean up idle Values
        }
    }

    public class Wander : IState
    {
        Vector3 m_wanderTarget = Vector3.zero;

        void IState.Enter(AIAgent agent)
        {
            // set up state values
            m_wanderTarget = agent.wanderNodes[agent.currentWanderIndex].position;

            agent.navAgent.isStopped = false;
            agent.navAgent.SetDestination(m_wanderTarget);
        }

        void IState.Update(AIAgent agent)
        {
            // Check state behaviour
            if(agent.navAgent.pathPending)
            {
                return;
            }

            if(agent.navAgent.remainingDistance <= agent.settings.stoppingDistance)
            {
                // arrived at wander node
                agent.currentWanderIndex++;

                if(agent.currentWanderIndex >= agent.wanderNodes.Count)
                {
                    agent.currentWanderIndex = 0;
                }

                agent.stateMachine.ChangeState((int)StateIndex.idle);
            }
        }

        void IState.Exit(AIAgent agent)
        {
            // clean up state Values
        }
    }

    public class Attack : IState
    {
        void IState.Enter(AIAgent agent)
        {
            // set up state values
        }

        void IState.Update(AIAgent agent)
        {
            // Check state behaviour

        }

        void IState.Exit(AIAgent agent)
        {
            // clean up state Values
        }
    }
}
