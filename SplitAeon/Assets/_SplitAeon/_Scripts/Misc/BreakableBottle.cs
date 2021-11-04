using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FMOD;
using FMODUnity;

public class BreakableBottle : MonoBehaviour
{

    public MeshRenderer rend;
    public ParticleSystem particle;

    StudioEventEmitter emitter;

    MeshCollider coll;
    BoxCollider boxColl;


    private void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();
        coll = GetComponentInChildren<MeshCollider>();
        boxColl = GetComponentInChildren<BoxCollider>();
    }

    public void Break()
    {
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
