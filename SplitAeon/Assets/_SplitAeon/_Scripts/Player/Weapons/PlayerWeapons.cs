using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapons : MonoBehaviour
{
    [Header("Player")]
    public Player player;
    public Camera playerCam;
    public LayerMask playerMask;

    [Header("Revolver")]

    public int revolverMaxAmmo;
    public int revolverReloadAmount;
    private int revolverAmmoLoaded;
    public float revolverReloadTime;
    public float revolverDamage;

    public GameObject revolverImpactEffect;

    [HideInInspector]
    public int revolverAmmoPool;

    [Header("GUI (Revolver)")]
    public Text ammoPool;
    public Text loadedAmmo;

    [Header("Effects (Revolver)")]
    public Transform revolverMuzzlePosition;
    public GameObject revolverMuzzleFlashPrefab;

    [Space(15)]

    [Header("Card Lethal")]
    public int maxCardLethals;
    public float cardLethalDamage;

    public Transform lethalSpawnLocation;

    [HideInInspector]
    public int cardLethalPool;

    public GameObject cardLethalPrefab;

    [Header("GUI (Card Lethal)")]
    public Text cardPool;

    [Space(15)]

    [Header("Audio")]
    public AudioSource source;

    public AudioClip[] revolverShootClips;
    public AudioClip revolverClickClip;
    public AudioClip revolverReloadClip;

    public AudioClip lethalThrow;

    [Space(10)]
    
    [Header("Controls")]
    public KeyCode reloadKey;
    public KeyCode cardLethalKey;


    bool fiftyTwoPickup = false;


    void Start()
    {
        revolverAmmoLoaded = revolverReloadAmount;
        revolverAmmoPool = revolverMaxAmmo; // Remove me later!
        cardLethalPool = maxCardLethals;
    }

    void Update()
    {
        Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward, Color.red);

        ammoPool.text = revolverAmmoPool.ToString();
        loadedAmmo.text = revolverAmmoLoaded.ToString();
        cardPool.text = cardLethalPool.ToString();


        if (fiftyTwoPickup)
        {
            if (cardLethalPool != 0)
            {
                ThrowCardLethal();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!player.isBusy)
            {
                if (!player.isRunning)
                {
                    ShootRevolver();
                }
            }
        }

        if (Input.GetKeyDown(cardLethalKey))
        {
            if (!player.isBusy)
            {
                ThrowCardLethal();
            }
        }

        if (Input.GetKeyDown(reloadKey))
        {
            if (!player.isBusy)
            {
                if (revolverAmmoLoaded != revolverReloadAmount)
                {
                    TryReload();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            fiftyTwoPickup = true;
        }


    }

    public void ShootRevolver()
    {
        if (revolverAmmoLoaded > 0)
        {

            player.viewmodelAnimator.SetTrigger("Shoot");

            source.PlayOneShot(revolverShootClips[Mathf.FloorToInt(Random.Range(0, revolverShootClips.Length))]);
            Instantiate(revolverMuzzleFlashPrefab, revolverMuzzlePosition);
            revolverAmmoLoaded -= 1;

            RaycastHit hit;

            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, float.PositiveInfinity, ~playerMask))
            {
                if (hit.collider.gameObject.GetComponent<Target>())
                {
                    CreateImpactChilded(revolverImpactEffect, hit);

                    hit.collider.gameObject.GetComponent<Target>().Hit();
                }
                else
                {
                    CreateImpactFree(revolverImpactEffect, hit);
                }

            }

        }
        else
        {
            source.PlayOneShot(revolverClickClip);
        }
    }

    public void ThrowCardLethal()
    {
        if (cardLethalPool > 0)
        {
            Debug.LogWarning("Throwing Card");

            GameObject thrownLethal;
            thrownLethal = Instantiate(cardLethalPrefab, lethalSpawnLocation.transform.position, Quaternion.identity);

            thrownLethal.GetComponent<Rigidbody>().velocity = lethalSpawnLocation.TransformDirection(0, 3, 20);
            thrownLethal.GetComponent<Rigidbody>().AddRelativeTorque(0, 90, 0); 

            cardLethalPool -= 1;

        }
    }

    public void TryReload()
    {
        if (revolverAmmoLoaded == revolverReloadAmount || revolverAmmoPool == 0)
        {
            return;
        }

        player.viewmodelAnimator.SetTrigger("Reload");
        source.PlayOneShot(revolverReloadClip);
    }

    public void LoadAmmo()
    {
        while(revolverAmmoLoaded != revolverReloadAmount && revolverAmmoPool > 0)
        {         
            revolverAmmoLoaded += 1;
            revolverAmmoPool -= 1;
        }
    }

    public void SetBusyState()
    {
        player.isBusy = true;
    }

    public void ClearBusyState()
    {
        player.isBusy = false;
    }

    public void CreateImpactChilded(GameObject impactPrefab, RaycastHit hitData)
    {

        Debug.LogWarning("Creating bullet impact");

        GameObject impact = Instantiate(impactPrefab, hitData.point, Quaternion.LookRotation(hitData.normal), hitData.collider.gameObject.transform);

    }

    public void CreateImpactFree(GameObject impactPrefab, RaycastHit hitData)
    {
        Debug.LogWarning("Creating bullet impact");

        GameObject impact = Instantiate(impactPrefab, hitData.point, Quaternion.LookRotation(hitData.normal));
    }

}
