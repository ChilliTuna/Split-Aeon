using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{

    WeaponManager playerWeapons;

    public int weaponIndex;

    public int ammoPickupAmount;

    public bool isOneTimePickup;

    void Start()
    {
        playerWeapons = GameObject.Find("Player").GetComponentInChildren<WeaponManager>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (playerWeapons.weapons[weaponIndex].ammoPool == playerWeapons.weapons[weaponIndex].maxAmmo)
        {
            return;
        }

        playerWeapons.weapons[weaponIndex].ammoPool += ammoPickupAmount;

        if (playerWeapons.weapons[weaponIndex].ammoPool > playerWeapons.weapons[weaponIndex].maxAmmo)
        {
            playerWeapons.weapons[weaponIndex].ammoPool = playerWeapons.weapons[weaponIndex].maxAmmo;
        }

        if (isOneTimePickup)
        {
            Destroy(this.gameObject);
        }

    }
}
