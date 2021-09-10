using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAnimationRelay : MonoBehaviour
{

    public Timewarp TW;

    public CardManager cardManager;

    private void Awake()
    {
        TW = FindObjectOfType<Timewarp>();
    }

    public void ThrowCard()
    {
        cardManager.ThrowCardLethal();
    }

    public void TimeWarp()
    {
        TW.TryWarp();
    }


}
