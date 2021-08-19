using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{

    public Melee weapon;
    public bool damageActive;
    public bool pushActive;

    private BoxCollider col;

    private void Start()
    {
        col = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (damageActive)
        {
            if (other.GetComponent<Health>() && !other.CompareTag("Player"))
            {
                other.GetComponent<Health>().Hit(weapon.damage);
                Debug.Log("Hit " + other.name);
            }
        }       
    }

    public void EnableDamage()
    {
        col.enabled = true;
        damageActive = true;
        Invoke("DisableDamage", 0.75f);
    }

    public void DisableDamage()
    {
        col.enabled = true;
        damageActive = false;
    }

    public void EnablePush()
    {
        col.enabled = true;
        pushActive = true;
        Invoke("DisablePush", 0.75f);
    }

    public void DisablePush()
    {
        col.enabled = true;
        pushActive = false;
    }

}
