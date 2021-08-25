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
        Push();
    }

    void Swing()
    {
        Debug.Log("Melee Swing");
        animator.SetTrigger("Swing");

        manager.weaponAudioSource.PlayOneShot(swingClips[Mathf.FloorToInt(Random.Range(0, swingClips.Length))]);

        hitbox.EnableDamage();

    }

    void Push()
    {
        Debug.Log("Melee Push");
        animator.SetTrigger("Push");

        manager.weaponAudioSource.PlayOneShot(pushClips[Mathf.FloorToInt(Random.Range(0, pushClips.Length))]);

        hitbox.EnableDamage();

    }
}
