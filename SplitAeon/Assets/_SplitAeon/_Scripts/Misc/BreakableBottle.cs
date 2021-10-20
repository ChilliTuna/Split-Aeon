using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FMOD;
using FMODUnity;

public class BreakableBottle : MonoBehaviour
{

    public MeshRenderer rend;
    public ParticleSystem particle;
    public AudioClip[] clips;

    //AudioSource source;

    StudioEventEmitter emitter;

    MeshCollider coll;
    BoxCollider boxColl;


    private void Start()
    {
        //source = GetComponent<AudioSource>();
        emitter = GetComponent<StudioEventEmitter>();
        coll = GetComponentInChildren<MeshCollider>();
        boxColl = GetComponentInChildren<BoxCollider>();
    }

    public void Break()
    {
        //source.PlayOneShot(clips[Mathf.FloorToInt(Random.Range(0, clips.Length))]);

        emitter.Play();

        if (coll)
        {
            coll.enabled = false;
        }

        if (boxColl)
        {
            boxColl.enabled = false;
        }

        rend.enabled = false;
        particle.Play();
        
        Destroy(this.gameObject, 5);
    }

}
