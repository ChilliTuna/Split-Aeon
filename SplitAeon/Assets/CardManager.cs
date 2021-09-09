using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Card
{
    public GameObject cardPrefab;
    public Sprite cardSprite;
    public float cardThrowForce;
    public float cardThrowLift;
    public float cardSpin;
}

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

    [Header("Card Types")]

    public Card regularCard;
    public Card warpingCard;
    public Card piercingCard;
    public Card splashCard;

    [Header("Audio")]
    public AudioSource weaponAudioSource;

    public AudioClip[] lethalThrowClips;

    [HideInInspector]
    public int cardLethalPool;

    private float throwForce, throwLift, cardSpin;


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
                SetCurrentCard(regularCard);
                break;

            case CardTypes.Warping:
                SetCurrentCard(warpingCard);
                break;

            case CardTypes.Piercing:
                SetCurrentCard(piercingCard);
                break;

            case CardTypes.Splash:
                SetCurrentCard(splashCard);
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

    public void SetCurrentCard(Card card)
    {
        selectedCard = card.cardPrefab;
        cardImage.sprite = card.cardSprite;
        throwForce = card.cardThrowForce;
        throwLift = card.cardThrowLift;
        cardSpin = card.cardSpin;
    }

    public void ThrowCardLethal()
    {
        if (cardLethalPool > 0)
        {
            Debug.LogWarning("Throwing Card");

            weaponAudioSource.PlayOneShot(lethalThrowClips[Mathf.FloorToInt(Random.Range(0, lethalThrowClips.Length))]);

            GameObject thrownLethal;
            thrownLethal = Instantiate(selectedCard, lethalSpawnLocation.transform.position, Quaternion.identity);

            thrownLethal.GetComponent<Rigidbody>().velocity = lethalSpawnLocation.TransformDirection(0, throwLift, throwForce);
            thrownLethal.GetComponent<Rigidbody>().AddRelativeTorque(0, cardSpin, 0);

            cardLethalPool -= 1;

        }
    }
}
