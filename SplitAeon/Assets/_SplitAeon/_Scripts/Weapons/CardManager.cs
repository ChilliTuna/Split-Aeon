using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using FMODUnity;

[System.Serializable]
public class Card
{
    [Header("Card Assets")]
    public GameObject cardPrefab;
    public Sprite cardSprite;

    [Header("Card Physics")]
    public float cardThrowForce;
    public float cardThrowLift;
    public float cardSpin;

    [Header("Card Pooling")]
    public int cardPool;
    public int cardMaxPool;
    public int cardStartingPool;

    [Header("Unlock State")]
    public bool isUnlocked;
    public Button weaponWheelButton;

}

public class CardManager : MonoBehaviour
{
    #region Variables

    [Header("Player")]
    public Player player;
    public Camera playerCam;
    public LayerMask playerMask;
    public PlayerMagicAnimations magicAnims;

    [Space(10)]

    [Header("Cards")]
    private int cardIndex;
    public Card[] cards;

    [Header("Universal Data")]
    public Transform lethalSpawnLocation;
    public Image cardImage;

    [Header("Weapon GUI")]
    public TextMeshProUGUI cardPoolReadout;

    [Header("Audio")]

    public StudioEventEmitter cardSounds;

    private float throwForce, throwLift, cardSpin;

    //Input
    private UserActions userActions;

    #endregion

    private void Awake()
    {
        userActions = new UserActions();
    }

    void Start()
    {
        foreach (Card c in cards)
        {
            c.cardPool = c.cardStartingPool;
        }
    }

    private void OnEnable()
    {
        userActions.PlayerMap.ThrowCard.LoadBinding(InputActions.ThrowCard);
        userActions.PlayerMap.ThrowCard.performed += ctx => ThrowCard();
        userActions.PlayerMap.ThrowCard.Enable();
    }

    private void OnDisable()
    {
        userActions.PlayerMap.ThrowCard.Disable();
    }

    void Update()
    {
        cardPoolReadout.text = cards[cardIndex].cardPool.ToString();
        cardImage.sprite = cards[cardIndex].cardSprite;

        foreach (Card c in cards)
        {
            c.weaponWheelButton.interactable = c.isUnlocked;
        }

    }

    void ThrowCard()
    {
        if (!player.isBusy && cards[cardIndex].cardPool > 0)
        {
            player.viewmodelAnimator.SetTrigger("Switch");
            Invoke("TriggerCardThrowAnimation", 0.3f);
        }
    }

    public void SetCardType(int index)
    {
        cardIndex = index;
    }

    public void ThrowCardLethal()
    {
        if (cards[cardIndex].cardPool > 0)
        {

            throwForce = cards[cardIndex].cardThrowForce;
            throwLift = cards[cardIndex].cardThrowLift;
            cardSpin = cards[cardIndex].cardSpin;

            cardSounds.Play();

            GameObject thrownLethal;
            thrownLethal = Instantiate(cards[cardIndex].cardPrefab, lethalSpawnLocation.transform.position, Quaternion.identity);

            thrownLethal.GetComponent<Rigidbody>().velocity = lethalSpawnLocation.TransformDirection(0, throwLift, throwForce);
            thrownLethal.GetComponent<Rigidbody>().AddRelativeTorque(0, cardSpin, 0);

            cards[cardIndex].cardPool -= 1;

        }
    }

    void TriggerCardThrowAnimation()
    {
        magicAnims.TriggerCardThrow();
    }
}
