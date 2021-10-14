using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System;
using System.Collections;

using FMODUnity;

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

    //[Header("Audio")]
    //public AudioSource weaponAudioSource;

    public StudioEventEmitter meleeSounds;

    private int myIndex;

    [HideInInspector]
    public bool shouldTryShooting;

    //Input
    private UserActions userActions;

    #endregion

    private void Awake()
    {
        userActions = new UserActions();
    }

    private void OnEnable()
    {
        EnableInputs();
    }

    void Start()
    {
        weaponIndex = 3;
        player.viewmodelAnimator = weapons[3].GetComponent<Melee>().animator;

        SwitchWeapon(3);
    }

    void Update()
    {
        Shoot();

        foreach (Weapon wep in weapons)
        {
            wep.weaponWheelButton.interactable = wep.isUnlocked;
        }
    }

    public void SwitchWeapon(int index)
    {
        if (weapons[index].isUnlocked)
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

    void Shoot()
    {
        if (shouldTryShooting && !player.isBusy && !player.isRunning)
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
    }

    void Reload()
    {
        if (!player.isBusy)
        {
            weapons[weaponIndex].SecondaryUse();
        }
    }

    void ToggleShooting(bool shouldShoot)
    {
        if (!Globals.isGamePaused)
        {
            shouldTryShooting = shouldShoot;
        }
    }

    void EnableInputs()
    {
        userActions.PlayerMap.Shoot.performed += ctx => ToggleShooting(true);
        userActions.PlayerMap.Shoot.canceled += ctx => ToggleShooting(false);
        userActions.PlayerMap.Shoot.Enable();
        
        userActions.PlayerMap.ChangeToMelee.performed += ctx => SwitchWeapon(3);
        userActions.PlayerMap.ChangeToMelee.Enable();
        
        userActions.PlayerMap.ChangeToRevolver.performed += ctx => SwitchWeapon(0);
        userActions.PlayerMap.ChangeToRevolver.Enable();
        
        userActions.PlayerMap.ChangeToThompson.performed += ctx => SwitchWeapon(1);
        userActions.PlayerMap.ChangeToThompson.Enable();
        
        userActions.PlayerMap.ChangeToShotgun.performed += ctx => SwitchWeapon(2);
        userActions.PlayerMap.ChangeToShotgun.Enable();
        
        userActions.PlayerMap.Reload.performed += ctx => Reload();
        userActions.PlayerMap.Reload.Enable();
    }

    void DisableInputs()
    {
        userActions.PlayerMap.Shoot.Disable();
        userActions.PlayerMap.ChangeToMelee.Disable();
        userActions.PlayerMap.ChangeToRevolver.Disable();
        userActions.PlayerMap.ChangeToThompson.Disable();
        userActions.PlayerMap.ChangeToShotgun.Disable();
        userActions.PlayerMap.Reload.Disable();
    }
}
