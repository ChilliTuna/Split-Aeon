using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRelay : MonoBehaviour
{

    public Weapon weapon;

    public void LoadAmmo()
    {
        weapon.GetComponent<Gun>().LoadAmmo();
    }

    public void EnableBusyState()
    {
        weapon.manager.EnableBusyState();
    }

    public void DisableBusyState()
    {
        weapon.manager.DisableBusyState();
    }

    public void EjectShell()
    {
        weapon.GetComponent<Gun>().EjectShell();
    }
}
