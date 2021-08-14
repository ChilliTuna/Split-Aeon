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

    public AudioClip[] lethalThrowClips;

    [Space(10)]

    [Header("Controls")]
    public KeyCode reloadKey;

    private int myIndex;

    #endregion

    void Start()
    {
        weapons[0].gameObject.SetActive(true);
        weapons[0].crosshair.SetActive(true);
        weaponIndex = 0;
        player.viewmodelAnimator = weapons[0].animator;
        //SwitchWeapon(0);
    }

    void Update()
    {
        weapons[weaponIndex].mouseDown = Input.GetKey(KeyCode.Mouse0);

        ammoReadout.text = weapons[weaponIndex].ammoLoaded.ToString() + "/" + weapons[weaponIndex].ammoPool.ToString();
        weaponName.text = weapons[weaponIndex].weaponName.ToString();


        if (Input.GetKeyDown(reloadKey))
        {
            if (!player.isBusy)
            {
                weapons[weaponIndex].TryReload();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2);
        }


        // REMOVE ME, DEBUG USE ONLY
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            weapons[weaponIndex].ammoPool = weapons[weaponIndex].maxAmmo;
            weapons[weaponIndex].ammoLoaded = weapons[weaponIndex].clipSize;
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
                wep.crosshair.SetActive(true);
                player.viewmodelAnimator = wep.animator;
                player.isBusy = false;
            }
            else
            {
                wep.gameObject.SetActive(false);
                wep.crosshair.SetActive(false);
            }

            i++;

        }
    }


}
