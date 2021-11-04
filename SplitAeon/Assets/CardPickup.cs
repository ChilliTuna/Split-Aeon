using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPickup : MonoBehaviour
{

    CardManager cards;

    public int cardIndex;

    public int pickupAmount;

    void Start()
    {
        cards = GameObject.Find("Player").GetComponentInChildren<CardManager>();
    }

    public void PickupCard()
    {
        cards.cards[cardIndex].isUnlocked = true;
        cards.SetCardType(cardIndex);

        if (cards.cards[cardIndex].cardPool == cards.cards[cardIndex].cardMaxPool)
        {
            return;
        }

        cards.cards[cardIndex].cardPool += pickupAmount;

        if (cards.cards[cardIndex].cardPool > cards.cards[cardIndex].cardMaxPool)
        {
            cards.cards[cardIndex].cardPool = cards.cards[cardIndex].cardMaxPool;
        }

        gameObject.SetActive(false);

    }

}
