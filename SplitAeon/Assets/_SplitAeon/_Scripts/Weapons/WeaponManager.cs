using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public Text ammoPoolReadout;
    public Text loadedAmmoReadout;
    public Text cardPoolReadout;

    [Space(10)]

    [Header("Card Lethal")]
    public int maxCardLethals;
    public float cardLethalDamage;
    public Transform lethalSpawnLocation;
    public GameObject cardLethalPrefab;

    [HideInInspector]
    public int cardLethalPool;

    [Space(10)]

    [Header("Audio")]
    public AudioSource weaponAudioSource;

    public AudioClip[] lethalThrowClips;

    [Space(10)]

    [Header("Controls")]
    public KeyCode reloadKey;
    public KeyCode cardLethalKey;

    #endregion

    void Start()
    {
        SelectWeapon(0);
        cardLethalPool = maxCardLethals;
    }

    void Update()
    {
        weapons[weaponIndex].mouseDown = Input.GetKey(KeyCode.Mouse0);


        ammoPoolReadout.text = weapons[weaponIndex].ammoPool.ToString();
        loadedAmmoReadout.text = weapons[weaponIndex].ammoLoaded.ToString();
        cardPoolReadout.text = cardLethalPool.ToString();

        if (Input.GetKeyDown(cardLethalKey))
        {
            if (!player.isBusy)
            {
                Debug.Log("Throwing card");
                ThrowCardLethal();
            }
        }

        if (Input.GetKeyDown(reloadKey))
        {
            if (!player.isBusy)
            {
                weapons[weaponIndex].TryReload();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectWeapon(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectWeapon(2);
        }

        // REMOVE ME, DEBUG USE ONLY
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            weapons[weaponIndex].ammoPool = weapons[weaponIndex].maxAmmo;
            weapons[weaponIndex].ammoLoaded = weapons[weaponIndex].clipSize;
        }

    }

    void SelectWeapon(int index)
    {
        weaponIndex = index;
        int i = 0;

        foreach (Weapon wep in weapons)
        {
            if (i == weaponIndex)
            {
                wep.gameObject.SetActive(true);
                wep.crosshair.SetActive(true);
                player.viewmodelAnimator = wep.animator;
            }
            else
            {
                wep.gameObject.SetActive(false);
                wep.crosshair.SetActive(false);
            }

            i++;

        }

    }

    public void ThrowCardLethal()
    {
        if (cardLethalPool > 0)
        {
            Debug.LogWarning("Throwing Card");

            weaponAudioSource.PlayOneShot(lethalThrowClips[Mathf.FloorToInt(Random.Range(0, lethalThrowClips.Length))]);

            GameObject thrownLethal;
            thrownLethal = Instantiate(cardLethalPrefab, lethalSpawnLocation.transform.position, Quaternion.identity);

            thrownLethal.GetComponent<Rigidbody>().velocity = lethalSpawnLocation.TransformDirection(0, 3, 20);
            thrownLethal.GetComponent<Rigidbody>().AddRelativeTorque(0, 90, 0);

            cardLethalPool -= 1;

        }
    }
}
