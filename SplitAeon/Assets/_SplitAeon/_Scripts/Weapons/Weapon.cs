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

    public ParticleSystem shellParticle;
    public bool emitOnShoot;
    public int shellNumber;

    [Header("Animations")]

    public Animator animator;

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

            if (isFullAuto)
            {
                animator.SetBool("ShootHold", false);
            }
        }

        timeUntilNextShot -= 1 * Time.deltaTime;

    }

    void Shoot()
    {
        if (ammoLoaded > 0)
        {
            animator.SetTrigger("Shoot");

            if (isFullAuto)
            {
                animator.SetBool("ShootHold", true);
            }

            CreateMuzzleFlash();
            ammoLoaded -= 1;

            if (shellParticle)
            {
                if (emitOnShoot)
                {
                    EjectShell();
                }
            }

            AddRecoil(verticalRecoil, Random.Range(-horizontalRecoil, horizontalRecoil));

            for (int i = 0; i < bulletsPerShot; i++)
            {
                RaycastHit hit;

                //manager.playerCam.transform.forward

                Vector3 bulletDirection = new Vector3(0, 0, 1);

                bulletDirection.x += Random.Range(-bulletSpread, bulletSpread);
                bulletDirection.y += Random.Range(-bulletSpread, bulletSpread);

                bulletDirection = manager.playerCam.transform.localToWorldMatrix * bulletDirection;

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

            if (isFullAuto)
            {
                animator.SetBool("ShootHold", false);
            }
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

        manager.player.viewmodelAnimator.SetTrigger("Reload");  //-- this needs to call the reload through animation in the future
        manager.weaponAudioSource.PlayOneShot(reloadClip);

        //LoadAmmo(); // -- Get rid of me later, should be done through animation events

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

    public void EnableBusyState()
    {
        manager.player.isBusy = true;
        Debug.Log("Player is now busy");
    }

    public void DisableBusyState()
    {
        manager.player.isBusy = false;
        Debug.Log("Player is no longer busy");
    }

    public void EjectShell()
    {
        if (shellParticle)
        {
            shellParticle.Emit(shellNumber);
            
        }
    }

}
