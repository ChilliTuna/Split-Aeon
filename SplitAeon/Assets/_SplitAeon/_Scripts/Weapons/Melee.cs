using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon
{

    [Header("Base Weapon Data")]
    public string weaponName;
    public float damage;

    public float pushForce;

    public MeleeHitbox hitbox;

    [Header("Audio")]
    public AudioClip[] swingClips;
    public AudioClip[] pushClips;

    [Header("Animations")]
    public Animator animator;

    // Runtime Variables
    [HideInInspector]
    public bool mouseDown;
    [HideInInspector]
    public bool waitForTriggerRelease;

    void Start()
    {
    }

    void Update()
    {
        if (isEquipped)
        {
            manager.ammoReadout.text = "";
            manager.weaponName.text = weaponName.ToString();
            manager.player.viewmodelAnimator = animator;
        }
    }

    public override void PrimaryUse()
    {
        if (waitForTriggerRelease == false)
        {
            waitForTriggerRelease = true;
            Swing();
        }

    }

    public override void SecondaryUse()
    {

    }


    void Swing()
    {

        if (manager.player.isBusy)
        {
            return;
        }

        Debug.Log("Melee Swing");
        animator.SetTrigger("Swing");
        animator.SetBool("SwingSide", !animator.GetBool("SwingSide"));

        manager.meleeSounds.Play();

        hitbox.EnableDamage();

    }

}
