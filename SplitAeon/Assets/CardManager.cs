using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardManager : MonoBehaviour
{
    [Header("Controls")]
    public KeyCode cardLethalKey;

    [Header("Player")]
    public Player player;
    public Camera playerCam;
    public LayerMask playerMask;

    [Header("Universal Data")]
    public int maxCardLethals;
    public Transform lethalSpawnLocation;

    [Header("Weapon GUI")]
    public TextMeshProUGUI cardPoolReadout;

    [Header("Regular Card")]
    public float cardLethalDamage;
    public GameObject cardLethalPrefab;

    [Header("Audio")]
    public AudioSource weaponAudioSource;

    public AudioClip[] lethalThrowClips;

    [HideInInspector]
    public int cardLethalPool;


    void Start()
    {
        cardLethalPool = maxCardLethals;
    }

    void Update()
    {
        cardPoolReadout.text = cardLethalPool.ToString();

        if (Input.GetKeyDown(cardLethalKey))
        {
            if (!player.isBusy)
            {
                ThrowCardLethal();
            }
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
