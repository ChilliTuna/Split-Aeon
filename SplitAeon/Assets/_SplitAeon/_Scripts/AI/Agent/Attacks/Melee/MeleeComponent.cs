using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeComponent : AttackType
{
    public HitBox armAttack;

    public override void AttackEnter(Transform attackTarget, Vector3 attackDir)
    {
        armAttack.hitIsActive = true;
    }

    public override void AttackUpdate()
    {
        armAttack.hitIsActive = agent.anim.GetBool("hitBoxActive");
    }

    public override void AttackExit()
    {
        agent.anim.SetBool("hitBoxActive", false);
        armAttack.hitIsActive = false;
    }
}
