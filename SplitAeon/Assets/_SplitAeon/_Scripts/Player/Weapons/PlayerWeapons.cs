using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapons : MonoBehaviour
{
    [Header("Player")]
    public Player player;
    public Camera playerCam;

    [Header("Revolver")]

    public int revolverMaxAmmo;
    public int revolverReloadAmount;
    private int revolverAmmoLoaded;
    public float revolverReloadTime;
    public float revolverDamage;

    public GameObject revolverImpactEffect;

    [HideInInspector]
    public int revolverAmmoPool;


    [Header("Ammo Display (Revolver)")]
    public Text ammoPool;
    public Text loadedAmmo;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip[] shootClips;
    public AudioClip gunClickClip;
    public AudioClip reloadClip;

    [Header("Effects")]
    public Transform revolverMuzzlePosition;
    public GameObject revolverMuzzleFlashPrefab;


    void Start()
    {
        revolverAmmoLoaded = revolverReloadAmount;
        revolverAmmoPool = revolverMaxAmmo; // Remove me later!
    }

    void Update()
    {
        Debug.DrawRay(playerCam.transform.position, playerCam.transform.forward, Color.red);

        ammoPool.text = revolverAmmoPool.ToString();
        loadedAmmo.text = revolverAmmoLoaded.ToString();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!player.isBusy)
            {
                Shoot();
            }
        }        
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!player.isBusy)
            {
                TryReload();
            }
        }
    }

    public void Shoot()
    {
        if (revolverAmmoLoaded > 0)
        {
            source.PlayOneShot(shootClips[Mathf.FloorToInt(Random.Range(0, shootClips.Length))]);
            Instantiate(revolverMuzzleFlashPrefab, revolverMuzzlePosition);
            revolverAmmoLoaded -= 1;

            RaycastHit hit;
            int layerMask = 1 << 18;

            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, float.PositiveInfinity, layerMask))
            {
                if (hit.collider.gameObject.GetComponent<Target>())
                {

                    CreateImpact(revolverImpactEffect, hit);

                    hit.collider.gameObject.GetComponent<Target>().Hit();


                }
            }

        }
        else
        {
            source.PlayOneShot(gunClickClip);
        }
    }

    public void TryReload()
    {
        if (revolverAmmoLoaded == revolverReloadAmount || revolverAmmoPool == 0)
        {
            return;
        }

        player.viewmodelAnimator.SetTrigger("Reload");
        source.PlayOneShot(reloadClip);
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

    public void CreateImpact(GameObject impactPrefab, RaycastHit hitData)
    {

        Debug.LogWarning("Creating bullet impact");

        GameObject impact = Instantiate(impactPrefab, hitData.point, Quaternion.LookRotation(hitData.normal), hitData.collider.gameObject.transform);

        //impact.transform.rotation.SetLookRotation(hitData.normal);
        //impact.transform.position = hitData.point;

    }

}
