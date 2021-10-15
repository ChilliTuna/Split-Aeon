using UnityEngine;
using UnityEngine.UI;

public class Gun : Weapon
{

    [Header("Base Weapon Data")]
    public string weaponName;
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
    public float muzzleFlashTime;

    public GameObject impactPrefab;

    public float impactDecay;

    public ParticleSystem shellParticle;
    public bool emitOnShoot;
    public int shellNumber;

    [Header("Animations")]

    public Animator animator;

    [Header("Unlock State")]



    // Runtime Variables

    [HideInInspector]
    public bool mouseDown;

    [HideInInspector]
    public int ammoLoaded;

    [HideInInspector]
    public int ammoPool;

    [HideInInspector]
    public bool waitForTriggerRelease;

    float timeUntilNextShot;

    private void Start()
    {
        ammoPool = startingAmmo;
        ammoLoaded = clipSize;
    }

    void Update()
    {
        timeUntilNextShot -= 1 * Time.deltaTime;

        if (isEquipped)
        {
            manager.ammoReadout.text = ammoLoaded.ToString() + "/" + ammoPool.ToString();
            manager.weaponName.text = weaponName.ToString();


            manager.player.viewmodelAnimator = animator;

            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                ammoPool = maxAmmo;
                ammoLoaded = clipSize;
            }
        }

    }

    public override void PrimaryUse()
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

    public override void SecondaryUse()
    {
        TryReload();
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

            ShowMuzzleFlash();
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
                    var health = hit.collider.gameObject.GetComponentInParent<Health>();
                    if (health)
                    {
                        CreateImpactChilded(hit);

                        health.Hit(damage, hit.collider);
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

        manager.player.viewmodelAnimator.SetTrigger("Reload");
        manager.weaponAudioSource.PlayOneShot(reloadClip);
    }

    public void LoadAmmo()
    {
        while (ammoLoaded != clipSize && ammoPool > 0)
        {
            ammoLoaded += 1;
            ammoPool -= 1;
        }
    }

    void ShowMuzzleFlash()
    {
        manager.weaponAudioSource.PlayOneShot(shootClips[Mathf.FloorToInt(Random.Range(0, shootClips.Length))]);

        muzzleFlashPrefab.SetActive(true);

        Invoke("HideMuzzleFlash", 0.075f);

        //Instantiate(muzzleFlashPrefab, muzzlePosition);
    }

    void HideMuzzleFlash()
    {
        muzzleFlashPrefab.SetActive(false);

    }

    public void CreateImpactChilded(RaycastHit hitData)
    {
        GameObject decal = DecalPool.POOL.GetPooledObject();

        if (decal != null)
        {
            decal.transform.position = hitData.point;
            decal.transform.rotation = Quaternion.LookRotation(hitData.normal);
            decal.transform.parent = hitData.collider.gameObject.transform;
            decal.SetActive(true);
        }

        //GameObject impact = Instantiate(impactPrefab, hitData.point, Quaternion.LookRotation(hitData.normal), hitData.collider.gameObject.transform);
        //Destroy(impact, impactDecay);
    }

    public void CreateImpactFree(RaycastHit hitData)
    {
        GameObject decal = DecalPool.POOL.GetPooledObject();

        if (decal != null)
        {
            decal.transform.position = hitData.point;
            decal.transform.rotation = Quaternion.LookRotation(hitData.normal);
            decal.SetActive(true);
        }

        //GameObject impact = Instantiate(impactPrefab, hitData.point, Quaternion.LookRotation(hitData.normal));
        //Destroy(impact, impactDecay);
    }

    public void EjectShell()
    {
        if (shellParticle)
        {
            shellParticle.Emit(shellNumber);

        }
    }
}
