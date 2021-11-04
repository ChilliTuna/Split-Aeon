using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FutureAmbience : MonoBehaviour
{

    public GameObject emitter;

    void Update()
    {
        emitter.SetActive(!Globals.isInPast);
    }
}
