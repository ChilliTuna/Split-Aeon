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

    public int revolverAmmoPool;
    public int revolverReloadAmount;
    private int revolverAmmoLoaded;
    public float revolverReloadTime;
    public float revolverDamage;

    [Header("Ammo Display (Revolver)")]
    public Text ammoPool;
    public Text loadedAmmo;

    [Header("Audio")]
    public AudioSource source;
    public AudioClip[] shootClips;
    public AudioClip gunClickClip;
    public AudioClip reloadClip;

    void Start()
    {
        revolverAmmoLoaded = revolverReloadAmount;
    }

    void Update()
    {

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
        // do shoot

        if (revolverAmmoLoaded > 0)
        {
            source.PlayOneShot(shootClips[Mathf.FloorToInt(Random.Range(0, shootClips.Length))]);

            revolverAmmoLoaded -= 1;

            RaycastHit hit;
          
            if (Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 20, 18))
            {
                if (hit.collider.GetComponent<Target>())
                {
                    hit.collider.GetComponent<Target>().TakeDamage(revolverDamage);
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
        if (revolverAmmoLoaded == revolverReloadAmount)
        {
            return;
        }

        if (revolverAmmoLoaded == 0 && revolverAmmoPool == 0)
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

}
