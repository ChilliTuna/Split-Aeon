using UnityEngine;
using UnityEngine.UI;
public class Weapon : MonoBehaviour
{

    [Header("Weapon Manager")]
    public WeaponManager manager;

    [Header("Base Weapon Data")]
    public float damage;
    public float range;

    [Header("Ammo Data")]
    public int maxAmmo;
    public int clipSize;

    public int startingAmmo;

    [Range(0, 20)]
    public float reloadTime;

    [Header("Fire Mode")]
    public bool isFullAuto;
    public float fireRate;

    public int bulletsPerShot;

    public GameObject crosshair;

    [Header("Variation")]

    [Range(0, 0.1f)]
    public float bulletSpread;

    [Range(0, 4)]
    public float horizontalRecoil;
    [Range(0, 0.5f)]
    public float verticalRecoil;

    [Header("Audio")]
    public AudioClip[] shootClips;
    public AudioClip reloadClip;
    public AudioClip emptyClip;

    [Header("Effects")]
    public Transform muzzlePosition;

    public GameObject muzzleFlashPrefab;
    public GameObject impactPrefab;

    public float impactDecay;

    public ParticleSystem ejection;



    // Runtime Variables

    [HideInInspector]
    public bool mouseDown;

    [HideInInspector]
    public int ammoLoaded;

    [HideInInspector]
    public int ammoPool;

    bool waitForTriggerRelease;
    float timeUntilNextShot;

    private void Start()
    {
        ammoPool = startingAmmo;
        ammoLoaded = clipSize;
    }

    void Update()
    {
        if (mouseDown && !manager.player.isBusy && !manager.player.isRunning)
        {
            if (isFullAuto)
            {
                if (timeUntilNextShot <= 0)
                {
                    Shoot();
                    timeUntilNextShot = fireRate;
                }
            }
            else
            {
                if (timeUntilNextShot <= 0)
                {
                    if (waitForTriggerRelease == false)
                    {
                        waitForTriggerRelease = true;
                        Shoot();
                        timeUntilNextShot = fireRate;
                    }
                }
            }                  
        }
        else
        {
            waitForTriggerRelease = false;
        }

        timeUntilNextShot -= 1 * Time.deltaTime;

    }

    void Shoot()
    {
        if (ammoLoaded > 0)
        {
            manager.player.viewmodelAnimator.SetTrigger("Shoot");
            CreateMuzzleFlash();
            ammoLoaded -= 1;

            if (ejection)
            {
                ejection.Emit(1);
            }

            AddRecoil(verticalRecoil, Random.Range(-horizontalRecoil, horizontalRecoil));

            for (int i = 0; i < bulletsPerShot; i++)
            {
                RaycastHit hit;

                Vector3 bulletDirection = manager.playerCam.transform.forward;

                bulletDirection.x += Random.Range(-bulletSpread, bulletSpread);
                bulletDirection.y += Random.Range(-bulletSpread, bulletSpread);

                if (Physics.Raycast(manager.playerCam.transform.position, bulletDirection, out hit, float.PositiveInfinity, ~manager.playerMask))
                {
                    if (hit.collider.gameObject.GetComponent<Health>())
                    {
                        CreateImpactChilded(hit);

                        hit.collider.gameObject.GetComponent<Health>().Hit(damage);
                    }
                    else if (hit.collider.gameObject.GetComponent<Target>())
                    {
                        CreateImpactChilded(hit);

                        hit.collider.gameObject.GetComponent<Target>().Hit();
                    }
                    else
                    {
                        CreateImpactFree(hit);
                    }
                }
            }

        }
        else
        {
            manager.weaponAudioSource.PlayOneShot(emptyClip);
        }
    }

    void AddRecoil(float vertical, float horizontal)
    {
        manager.player.recoilHorizontal = horizontal;
        manager.player.recoilVertical = vertical;
    }

    public void TryReload()
    {
        if (ammoLoaded == clipSize || ammoPool == 0)
        {
            return;
        }

        //manager.player.viewmodelAnimator.SetTrigger("Reload");  -- this needs to call the reload through animation in the future
        manager.weaponAudioSource.PlayOneShot(reloadClip);

        LoadAmmo(); // -- Get rid of me later, should be done through animation events

    }

    public void LoadAmmo()
    {
        while (ammoLoaded != clipSize && ammoPool > 0)
        {
            ammoLoaded += 1;
            ammoPool -= 1;
        }
    }

    void CreateMuzzleFlash()
    {
        manager.weaponAudioSource.PlayOneShot(shootClips[Mathf.FloorToInt(Random.Range(0, shootClips.Length))]);
        Instantiate(muzzleFlashPrefab, muzzlePosition);
    }

    public void CreateImpactChilded(RaycastHit hitData)
    {
        GameObject impact = Instantiate(impactPrefab, hitData.point, Quaternion.LookRotation(hitData.normal), hitData.collider.gameObject.transform);
        Destroy(impact, impactDecay);
    }

    public void CreateImpactFree(RaycastHit hitData)
    {
        GameObject impact = Instantiate(impactPrefab, hitData.point, Quaternion.LookRotation(hitData.normal));
        Destroy(impact, impactDecay);
    }
}
