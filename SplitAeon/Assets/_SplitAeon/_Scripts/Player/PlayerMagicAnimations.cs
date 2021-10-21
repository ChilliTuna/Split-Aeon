using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicAnimations : MonoBehaviour
{

    public Animator animator;

    public void TriggerDiscovery()
    {
        animator.SetTrigger("Discovery");
    }

    public void TriggerWarp()
    {
        animator.SetTrigger("Warp");
    }

    public void TriggerCardThrow()
    {
        animator.SetTrigger("Throw");
    }

}
