using UnityEngine;
using UnityEngine.UI;

public abstract class Weapon : MonoBehaviour
{
    [Header("Weapon Manager")]
    public WeaponManager manager;

    public GameObject crosshair;

    [HideInInspector]
    public bool isEquipped;

    public bool isUnlocked;
    public Button weaponWheelButton;

    public abstract void PrimaryUse();
    public abstract void SecondaryUse();
}
