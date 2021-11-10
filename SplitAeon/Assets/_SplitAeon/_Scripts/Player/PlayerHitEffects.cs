using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using FMODUnity;

public class PlayerHitEffects : MonoBehaviour
{

    public StudioEventEmitter emitter;

    public Animator hitEffect;

    public void PlayHitEffects()
    {
        hitEffect.SetTrigger("Hit");
        emitter.Play();
    }

}
