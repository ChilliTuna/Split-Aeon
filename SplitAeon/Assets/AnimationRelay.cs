using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRelay : MonoBehaviour
{

    public Weapon weapon;

    public void LoadAmmo()
    {
        weapon.LoadAmmo();
    }

    public void EnableBusyState()
    {
        weapon.EnableBusyState();
    }

    public void DisableBusyState()
    {
        weapon.DisableBusyState();
    }

    public void EjectShell()
    {
        weapon.EjectShell();
    }
}
