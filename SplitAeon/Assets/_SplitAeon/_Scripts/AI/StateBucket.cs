using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AIStates
{
    public enum StateIndex
    {
        idle,
        wander,
        chasePlayer,
        attackPlayer,
        dead
    }

    public static class StateBucket
    {
        public static void SetUpStateMachine(StateMachine target)
        {
            target.AddState(new Idle());
            target.AddState(new Wander());
            target.AddState(new ChasePlayer());
            target.AddState(new AttackPlayer());
            target.AddState(new Dead());

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

            agent.StopNavigating();
        }

        void IState.Update(AIAgent agent)
        {
            // Check for player Radius
            if(agent.settings.aggresionRadius * agent.settings.aggresionRadius > agent.distToPlayerSquared)
            {
                agent.ChangeState(StateIndex.chasePlayer);
            }

            // Check Idle behaviour
            if(m_timer > m_currentTargetTime)
            {
                // bam, no more idle
                agent.ChangeState(StateIndex.wander);
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

            agent.StartNavigating();
            agent.navAgent.SetDestination(m_wanderTarget);
        }

        void IState.Update(AIAgent agent)
        {
            // Check for player Radius
            if (agent.settings.aggresionRadius * agent.settings.aggresionRadius > agent.distToPlayerSquared)
            {
                agent.ChangeState(StateIndex.chasePlayer);
            }

            // Check state behaviour
            if (agent.navAgent.pathPending)
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

                agent.ChangeState(StateIndex.idle);
            }
        }

        void IState.Exit(AIAgent agent)
        {
            // clean up state Values
        }
    }

    public class ChasePlayer : IState
    {
        Transform m_playerTransform;
        float attackCharge = 0.0f;

        void IState.Enter(AIAgent agent)
        {
            // set up state values
            m_playerTransform = agent.playerTransform;

            agent.StartNavigating();
            agent.navAgent.SetDestination(m_playerTransform.position);

            attackCharge = 0.0f;
        }

        void IState.Update(AIAgent agent)
        {
            Vector3 toPlayer = (m_playerTransform.position - agent.transform.position).normalized;
            toPlayer *= agent.settings.orbWalkRadius;


            // Check state behaviour
            agent.navAgent.SetDestination(m_playerTransform.position - toPlayer);

            if(agent.distToPlayerSquared < agent.settings.attackChargeRadius * agent.settings.attackChargeRadius)
            {
                // in range to charge attack
                attackCharge += Time.deltaTime * agent.settings.attackChargeRate;

                if(attackCharge > agent.settings.attackChargeMax)
                {
                    // The agent has successfully begun it's attack
                    agent.ChangeState(StateIndex.attackPlayer);
                }
            }
        }

        void IState.Exit(AIAgent agent)
        {
            // clean up state Values
            //agent.navAgent.SetDestination(agent.transform.position);
        }
    }

    public class AttackPlayer : IState
    {
        Transform m_playerTransform;
        Vector3 m_originalLocation;
        Vector3 m_attackDirection;

        void IState.Enter(AIAgent agent)
        {
            // set up state values
            m_playerTransform = agent.playerTransform;
            m_originalLocation = agent.transform.position;
            m_attackDirection = (agent.playerTransform.position - agent.transform.position).normalized;

            agent.StopNavigating();

            agent.anim.SetTrigger("attack");
            agent.anim.SetBool("isAttacking", true);
            agent.anim.SetBool("lockRotation", true);
            agent.armAttack.hitIsActive = true;
        }

        void IState.Update(AIAgent agent)
        {
            // this would be the logic where the attack could take place and wait until it has ended.
            if(agent.anim.GetBool("lockRotation"))
            {
                // only aim towards the initial attack direction
                agent.transform.forward = Vector3.Slerp(agent.transform.forward, m_attackDirection, agent.settings.rotationLerpSpeed);
            }
            else
            {
                // start looking towards the player
                agent.transform.forward = Vector3.Slerp(agent.transform.forward, m_playerTransform.position - agent.transform.position, agent.settings.afterAttackLerpSpeed);
            }

            agent.armAttack.hitIsActive = agent.anim.GetBool("hitBoxActive");

            if(!agent.anim.GetBool("isAttacking"))
            {
                agent.ChangeState(StateIndex.chasePlayer);
            }
        }

        void IState.Exit(AIAgent agent)
        {
            // clean up state Values
            agent.anim.SetBool("isAttacking", false);
            agent.anim.SetBool("lockRotation", false);
            agent.armAttack.hitIsActive = false;

            agent.anim.SetBool("hitBoxActive", false);
        }
    }

    public class Dead : IState
    {
        void IState.Enter(AIAgent agent)
        {
            // set up state values
            agent.StopNavigating();
            agent.ragdoll.RagdollOn = true;
        }

        void IState.Update(AIAgent agent)
        {

        }

        void IState.Exit(AIAgent agent)
        {
            // clean up state Values
            agent.ragdoll.RagdollOn = false;
        }
    }
}
