using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBottle : MonoBehaviour
{

    public MeshRenderer rend;
    public ParticleSystem particle;
    public AudioClip[] clips;

    AudioSource source;
    MeshCollider coll;


    private void Start()
    {
        source = GetComponent<AudioSource>();
        coll = GetComponentInChildren<MeshCollider>();
    }

    public void Break()
    {
        source.PlayOneShot(clips[Mathf.FloorToInt(Random.Range(0, clips.Length))]);

        coll.enabled = false;
        rend.enabled = false;
        particle.Play();

        Destroy(this.gameObject, 5);
    }

}
