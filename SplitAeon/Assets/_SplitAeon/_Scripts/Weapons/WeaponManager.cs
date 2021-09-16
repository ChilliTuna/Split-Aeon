using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;

public class WeaponManager : MonoBehaviour
{

    #region Variables

    [Header("Player")]
    public Player player;
    public Camera playerCam;
    public LayerMask playerMask;

    [Space(10)]

    [Header("Weapons")]
    private int weaponIndex;
    public Weapon[] weapons;

    [Space(10)]

    [Header("Weapon GUI")]
    public CanvasGroup cg;
    public TextMeshProUGUI weaponName;
    public TextMeshProUGUI ammoReadout;

    [Space(10)]

    [Header("Audio")]
    public AudioSource weaponAudioSource;

    [Space(10)]

    [Header("Controls")]
    public KeyCode reloadKey;

    private int myIndex;

    [HideInInspector]
    public bool mouseDown;

    #endregion

    void Start()
    {
        weaponIndex = 3;
        player.viewmodelAnimator = weapons[3].GetComponent<Melee>().animator;

        SwitchWeapon(3);
    }

    void Update()
    {

        mouseDown = Input.GetKey(KeyCode.Mouse0);

        if (mouseDown && !player.isBusy && !player.isRunning)
        {
            weapons[weaponIndex].PrimaryUse();
        }
        else
        {
            if (weapons[weaponIndex].GetComponent<Gun>())
            {
                weapons[weaponIndex].GetComponent<Gun>().waitForTriggerRelease = false;

                if (weapons[weaponIndex].GetComponent<Gun>().isFullAuto)
                {
                    weapons[weaponIndex].GetComponent<Gun>().animator.SetBool("ShootHold", false);
                }
            }
            else if (weapons[weaponIndex].GetComponent<Melee>())
            {
                weapons[weaponIndex].GetComponent<Melee>().waitForTriggerRelease = false;
            }

        }

        if (Input.GetKeyDown(reloadKey))
        {
            if (!player.isBusy)
            {
                weapons[weaponIndex].SecondaryUse();
            }
        }

        foreach (Weapon wep in weapons)
        {
            wep.weaponWheelButton.interactable = wep.isUnlocked;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (weapons[3].isUnlocked)
            {
                SwitchWeapon(3);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (weapons[0].isUnlocked)
            {
                SwitchWeapon(0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (weapons[1].isUnlocked)
            {
                SwitchWeapon(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (weapons[2].isUnlocked)
            {
                SwitchWeapon(2);
            }
        }
    }

    public void SwitchWeapon(int index)
    {
        if (myIndex == index)
        {
            return;
        }

        player.viewmodelAnimator.SetTrigger("Switch");
        player.isBusy = true;
        myIndex = index;
        StartCoroutine(FadeOutWeaponGUI(cg));
        Invoke("SetCurrentWeapon", 0.7f);

    }

    private IEnumerator FadeInWeaponGUI(CanvasGroup group)
    {
        while (group.alpha < 1)
        {
            group.alpha += 1 / 0.25f * Time.deltaTime;
            yield return 0;
        }
    }

    private IEnumerator FadeOutWeaponGUI(CanvasGroup group)
    {
        while (group.alpha > 0)
        {
            group.alpha -= 1 / 0.25f * Time.deltaTime;
            yield return 0;
        }
    }

    void SetCurrentWeapon()
    {
        weaponIndex = myIndex;
        int i = 0;

        StartCoroutine(FadeInWeaponGUI(cg));

        foreach (Weapon wep in weapons)
        {
            if (i == weaponIndex)
            {
                wep.gameObject.SetActive(true);
                wep.isEquipped = true;              
                player.isBusy = false;
                wep.crosshair.SetActive(true);
            }
            else
            {
                wep.isEquipped = false;
                wep.gameObject.SetActive(false);
                wep.crosshair.SetActive(false);
            }

            i++;

        }
    }

    public void EnableBusyState()
    {
        player.isBusy = true;
    }

    public void DisableBusyState()
    {
        player.isBusy = false;
    }

    public void UnlockWeapon(int weaponIndex)
    {
        weapons[weaponIndex].isUnlocked = true;
    }

    public void LockWeapon(int weaponIndex)
    {
        weapons[weaponIndex].isUnlocked = false;
    }
}
