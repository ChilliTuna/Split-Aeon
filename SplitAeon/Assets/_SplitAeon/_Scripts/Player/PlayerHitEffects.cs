using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHitEffects : MonoBehaviour
{
    public AudioClip[] hurtClips;
    public AudioSource source;

    public Animator hitEffect;

    public void PlayHitEffects()
    {
        hitEffect.SetTrigger("Hit");
        source.PlayOneShot(hurtClips[Random.Range(0, hurtClips.Length)]);
    }

}
