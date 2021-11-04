using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{

    Health player;
    float timer;

    public float damageTick;
    public float damageAmount;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Health>();
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (timer <= damageTick)
            {
                timer = 0;

                player.Damage(damageAmount);
            }
        }
    }



}
