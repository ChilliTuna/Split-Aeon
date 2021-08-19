﻿using System.Collections;
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

        if (playerWeapons.weapons[weaponIndex].GetComponent<Gun>().ammoPool == playerWeapons.weapons[weaponIndex].GetComponent<Gun>().maxAmmo)
        {
            return;
        }

        playerWeapons.weapons[weaponIndex].GetComponent<Gun>().ammoPool += ammoPickupAmount;

        if (playerWeapons.weapons[weaponIndex].GetComponent<Gun>().ammoPool > playerWeapons.weapons[weaponIndex].GetComponent<Gun>().maxAmmo)
        {
            playerWeapons.weapons[weaponIndex].GetComponent<Gun>().ammoPool = playerWeapons.weapons[weaponIndex].GetComponent<Gun>().maxAmmo;
        }

        if (isOneTimePickup)
        {
            Destroy(this.gameObject);
        }

    }
}
