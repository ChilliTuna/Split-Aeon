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
        public static void SetUpStateMachine(StateMachine<AIAgent> target)
        {
            target.AddState(new Idle());
            target.AddState(new Patrol());
            target.AddState(new ChasePlayer());
            target.AddState(new AttackPlayer());
            target.AddState(new Dead());
        }
    }

    public abstract class AgentState : IState<AIAgent>
    {
        void IState<AIAgent>.Enter(AIAgent agent)
        {
            Enter(agent);
        }

        void IState<AIAgent>.Update(AIAgent agent)
        {
            Update(agent);
        }

        void IState<AIAgent>.Exit(AIAgent agent)
        {
            Exit(agent);
        }

        public abstract void Enter(AIAgent agent);
        public abstract void Update(AIAgent agent);
        public abstract void Exit(AIAgent agent);
    }

    public abstract class MovementState : IState<AIAgent>
    {
        void IState<AIAgent>.Enter(AIAgent agent)
        {
            Enter(agent);
        }

        void IState<AIAgent>.Update(AIAgent agent)
        {
            agent.Seek();

            Update(agent);
        }

        void IState<AIAgent>.Exit(AIAgent agent)
        {
            Exit(agent);
        }

        public abstract void Enter(AIAgent agent);
        public abstract void Update(AIAgent agent);
        public abstract void Exit(AIAgent agent);
    }

    public class Idle : AgentState
    {
        float m_timer = 0.0f;
        float m_currentTargetTime = 0.0f;

        public override void Enter(AIAgent agent)
        {
            // set up idle values
            m_timer = 0.0f;
            m_currentTargetTime = Random.Range(agent.settings.minIdleTime, agent.settings.maxIdleTime);

            agent.StopNavigating();
        }

        public override void Update(AIAgent agent)
        {
            // Check for player Radius
            if(agent.settings.aggresionRadius * agent.settings.aggresionRadius > agent.distToPlayerSquared)
            {
                agent.ChangeState(StateIndex.chasePlayer);
                return;
            }

            // Check Idle behaviour
            if(agent.patrolNodes.Count == 0)
            {
                agent.ChangeState(StateIndex.idle);
                return;
            }

            if(m_timer > m_currentTargetTime)
            {
                // bam, no more idle
                agent.ChangeState(StateIndex.wander);
            }

            // Update idle timers
            m_timer += Time.deltaTime;
        }

        public override void Exit(AIAgent agent)
        {
            // clean up idle Values
        }
    }

    public class Patrol : AgentState
    {
        Vector3 m_patrolTarget = Vector3.zero;

        public override void Enter(AIAgent agent)
        {
            // set up state values
            m_patrolTarget = agent.patrolNodes[agent.currentPatrolIndex].position;

            agent.StartNavigating();
            agent.navAgent.SetDestination(m_patrolTarget);
        }

        public override void Update(AIAgent agent)
        {
            // Check for player Radius
            if (agent.settings.aggresionRadius * agent.settings.aggresionRadius > agent.distToPlayerSquared)
            {
                agent.ChangeState(StateIndex.chasePlayer);
                return;
            }

            // Check state behaviour
            if (agent.navAgent.pathPending)
            {
                return;
            }

            if(agent.navAgent.remainingDistance <= agent.settings.stoppingDistance)
            {
                // arrived at wander node
                agent.currentPatrolIndex++;

                if(agent.currentPatrolIndex >= agent.patrolNodes.Count)
                {
                    agent.currentPatrolIndex = 0;
                }

                agent.ChangeState(StateIndex.idle);
            }
        }

        public override void Exit(AIAgent agent)
        {
            // clean up state Values
        }
    }

    public class ChasePlayer : AgentState
    {
        Transform m_playerTransform;
        float attackCharge = 0.0f;

        public override void Enter(AIAgent agent)
        {
            // set up state values
            m_playerTransform = agent.playerTransform;

            agent.StartNavigating();
            agent.navAgent.SetDestination(m_playerTransform.position);

            attackCharge = 0.0f;
        }

        public override void Update(AIAgent agent)
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
                    return;
                }
            }
        }

        public override void Exit(AIAgent agent)
        {
            // clean up state Values
            //agent.navAgent.SetDestination(agent.transform.position);
        }
    }

    public class AttackPlayer : AgentState
    {
        Transform m_playerTransform;
        Vector3 m_originalLocation;
        Vector3 m_attackDirection;

        public override void Enter(AIAgent agent)
        {
            // set up state values
            m_playerTransform = agent.playerTransform;
            m_originalLocation = agent.transform.position;
            m_attackDirection = (agent.playerTransform.position - agent.transform.position).normalized;

            // Set up animation
            agent.StopNavigating();

            agent.anim.SetTrigger("attack");
            agent.anim.SetBool("isAttacking", true);
            agent.anim.SetBool("lockRotation", true);

            // Set up attack type
            agent.attack.AttackEnter(m_playerTransform, m_attackDirection);
        }

        public override void Update(AIAgent agent)
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

            // attack type update
            agent.attack.AttackUpdate();

            // check if the agent is still attacking
            if(!agent.anim.GetBool("isAttacking"))
            {
                agent.ChangeState(StateIndex.chasePlayer);
                return;
            }
        }

        public override void Exit(AIAgent agent)
        {
            // clean up state Values
            // clean up animation
            agent.anim.SetBool("isAttacking", false);
            agent.anim.SetBool("lockRotation", false);

            // Clean up attack type
            agent.attack.AttackExit();
        }
    }

    public class Dead : AgentState
    {
        float m_timer = 0.0f;

        public override void Enter(AIAgent agent)
        {
            // set up state values
            agent.StopNavigating();
            agent.ragdoll.RagdollOn = true;
            agent.charCollider.enabled = false;

            m_timer = 0.0f;
        }

        public override void Update(AIAgent agent)
        {
            if(m_timer > agent.settings.bodyDecayTime)
            {
                agent.attachedPoolObject.Disable();
            }

            m_timer += Time.deltaTime;
        }

        public override void Exit(AIAgent agent)
        {
            // clean up state Values
            agent.ragdoll.RagdollOn = false;
            agent.charCollider.enabled = true;
        }
    }
}
