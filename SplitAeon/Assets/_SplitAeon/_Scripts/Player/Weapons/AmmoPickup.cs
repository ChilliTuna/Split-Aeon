using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{

    Player player;
    PlayerWeapons playerWeapons;

    public int ammoPickupAmount;

    public bool isOneTimePickup;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        playerWeapons = GameObject.Find("Player").GetComponent<PlayerWeapons>();

    }

    private void OnTriggerEnter(Collider other)
    {

        if (playerWeapons.revolverAmmoPool == playerWeapons.revolverMaxAmmo)
        {
            return;
        }

        playerWeapons.revolverAmmoPool += ammoPickupAmount;

        if (playerWeapons.revolverAmmoPool > playerWeapons.revolverMaxAmmo)
        {
            playerWeapons.revolverAmmoPool = playerWeapons.revolverMaxAmmo;
        }

        if (isOneTimePickup)
        {
            Destroy(this.gameObject);
        }

    }
}
