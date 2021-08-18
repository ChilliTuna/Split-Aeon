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

    public Image cardImage;

    private GameObject selectedCard;

    private enum CardTypes
    {
        Regular,
        Warping,
        Piercing,
        Splash
    }

    CardTypes cardTypes;

    [Header("Weapon GUI")]
    public TextMeshProUGUI cardPoolReadout;

    [Header("Regular Card")]
    public GameObject regularCardPrefab;
    public Sprite regularCardSprite;

    [Header("Warping Card")]
    public GameObject warpingCardPrefab;
    public Sprite warpingCardSprite;

    [Header("Piercing Card")]
    public GameObject piercingCardPrefab;
    public Sprite piercingCardSprite;

    [Header("Splash Card")]
    public GameObject splashCardPrefab;
    public Sprite splashCardSprite;

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

        switch (cardTypes)
        {
            case CardTypes.Regular:
                selectedCard = regularCardPrefab;
                cardImage.sprite = regularCardSprite;
                break;

            case CardTypes.Warping:
                selectedCard = warpingCardPrefab;
                cardImage.sprite = warpingCardSprite;
                break;

            case CardTypes.Piercing:
                selectedCard = piercingCardPrefab;
                cardImage.sprite = piercingCardSprite;
                break;

            case CardTypes.Splash:
                selectedCard = splashCardPrefab;
                cardImage.sprite = splashCardSprite;
                break;
        }

    }

    public void SetCardType(int index)
    {
        if (index == 0)
        {
            cardTypes = CardTypes.Regular;
        }
        else if (index == 1)
        {
            cardTypes = CardTypes.Warping;
        }
        else if (index == 2)
        {
            cardTypes = CardTypes.Piercing;
        }
        else if (index == 3)
        {
            cardTypes = CardTypes.Splash;
        }
        else
        {
            cardTypes = CardTypes.Regular;
        }
    }

    public void ThrowCardLethal()
    {
        if (cardLethalPool > 0)
        {
            Debug.LogWarning("Throwing Card");

            weaponAudioSource.PlayOneShot(lethalThrowClips[Mathf.FloorToInt(Random.Range(0, lethalThrowClips.Length))]);

            GameObject thrownLethal;
            thrownLethal = Instantiate(selectedCard, lethalSpawnLocation.transform.position, Quaternion.identity);

            thrownLethal.GetComponent<Rigidbody>().velocity = lethalSpawnLocation.TransformDirection(0, 3, 20);
            thrownLethal.GetComponent<Rigidbody>().AddRelativeTorque(0, 90, 0);

            cardLethalPool -= 1;

        }
    }
}
