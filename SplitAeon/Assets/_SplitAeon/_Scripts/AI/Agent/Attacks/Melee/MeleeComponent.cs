using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeComponent : AttackType
{
    public HitBox armAttack;

    public override void AttackEnter(Transform attackTarget, Vector3 attackDir)
    {
        //armAttack.hitIsActive = true;
        int debug = 9;
        agent.anim.SetBool("hitBoxActive", false);
        armAttack.hitIsActive = false;

        agent.agentAudio.attackEmitter.Play();
    }

    public override void AttackUpdate()
    {
        
    }

    public override void AttackExit()
    {

    }

    public override void AnimBeginAttackEvent()
    {
        armAttack.hitIsActive = true;
    }

    public override void AnimEndAttackEvent()
    {
        agent.anim.SetBool("hitBoxActive", false);
        armAttack.hitIsActive = false;
    }
}
