using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadAnimationRelay : MonoBehaviour
{
    public PlayerWeapons weapons;

    public void StartReload()
    {
        weapons.TryReload();
    }

    public void LoadAmmo()
    {
        weapons.LoadAmmo();
    }

    public void SetBusyState()
    {
        weapons.SetBusyState();
    }

    public void ClearBusyState()
    {
        weapons.ClearBusyState();
    }

}
