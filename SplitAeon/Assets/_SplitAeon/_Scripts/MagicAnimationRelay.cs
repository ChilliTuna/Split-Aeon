using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAnimationRelay : MonoBehaviour
{

    public GameObject TW;

    public CardManager cardManager;

    private void Awake()
    {
        TW = FindObjectOfType<GameManager>().gameObject;
    }

    public void ThrowCard()
    {
        cardManager.ThrowCardLethal();
    }

    public void TimeWarp()
    {
        TW.GetComponent<Timewarp>().DoWarp();
    }


}
