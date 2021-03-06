using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPiercing : MonoBehaviour
{
    public int damage;
    public int maxHitTargets;

    private int hitCounter = 0;

    private void OnTriggerEnter(Collider other)
    {
        var health = other.gameObject.GetComponentInParent<Health>();
        if (health)
        {
            health.Hit(damage, other);
            hitCounter++;
        }
        else if (other.gameObject.GetComponent<Target>())
        {
            other.gameObject.GetComponent<Target>().Hit();
            hitCounter++;
        }
    }

    private void Update()
    {
        if (hitCounter >= maxHitTargets)
        {
            Destroy(gameObject);
        }
    }
}
