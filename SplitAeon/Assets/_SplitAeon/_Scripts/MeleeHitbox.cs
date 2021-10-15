﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{

    public Melee weapon;
    public bool damageActive;
    public bool pushActive;

    bool hasHit;

    private void OnTriggerStay(Collider other)
    {
        if (damageActive && !hasHit)
        {
            if (!other.CompareTag("Player"))
            {
                var health = other.gameObject.GetComponentInParent<Health>();
                var target = other.gameObject.GetComponentInParent<Target>();

                if (health)
                {
                    health.Hit(weapon.damage, other);
                    Debug.Log("Hit " + other.name);
                    DisableDamage();
                }
                else if (target)
                {
                    target.Hit();
                    Debug.Log("Hit " + other.name);
                    DisableDamage();
                }
            }

        }       
    }

    public void EnableDamage()
    {
        damageActive = true;
        Invoke("DisableDamage", 0.75f);
    }

    public void DisableDamage()
    {
        damageActive = false;
    }

    public void EnablePush()
    {
        pushActive = true;
        Invoke("DisablePush", 0.75f);
    }

    public void DisablePush()
    {
        pushActive = false;
    }


}
