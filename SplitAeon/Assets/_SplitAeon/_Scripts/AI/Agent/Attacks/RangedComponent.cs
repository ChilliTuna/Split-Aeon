using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedComponent : AttackType
{
    public Projectile projectilePrefab;
    public float shootTime = 0.5f;
    public Transform projectileOrigin;

    bool m_shotWaiting = true;
    float m_shotTimer = 0.0f;

    Transform m_currentTarget;

    public override void AttackEnter(Transform attackTarget, Vector3 attackDir)
    {
        // Ranged attack
        m_shotWaiting = true;
        m_currentTarget = attackTarget;
        m_shotTimer = 0.0f;
    }

    public override void AttackUpdate()
    {
        if(m_shotWaiting)
        {
            m_shotTimer += Time.deltaTime;

            //var stateInfo = agent.anim.GetCurrentAnimatorStateInfo(0);

            //if(stateInfo.normalizedTime > shootTime)
            if (m_shotTimer > shootTime)
            {
                // shoot here
                m_shotWaiting = false;
                ShootProjectile(projectileOrigin.position);
            }
        }
    }

    public override void AttackExit()
    {
        m_currentTarget = null;
    }

    public void ShootProjectile(Vector3 origin)
    {
        Projectile newProjectile = Instantiate(projectilePrefab, origin, Quaternion.identity);
        newProjectile.Shoot(agent, origin, m_currentTarget.position);
    }
}
