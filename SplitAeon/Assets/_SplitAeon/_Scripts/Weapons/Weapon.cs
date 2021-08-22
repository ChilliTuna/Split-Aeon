using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Manager")]
    public WeaponManager manager;

    public GameObject crosshair;

    [HideInInspector]
    public bool isEquipped;

    public abstract void PrimaryUse();
    public abstract void SecondaryUse();
}
