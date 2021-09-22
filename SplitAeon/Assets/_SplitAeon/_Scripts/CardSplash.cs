using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSplash : MonoBehaviour
{
    public int damage;
    public float timeToExplode;

    public float explosionRange;

    public LayerMask mask;

    float countdown;
    bool hasExploded;

    AudioSource source;

    public AudioClip cardExplodeSound;

    public ParticleSystem explosionEffect;

    public MeshRenderer[] renderers;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        countdown = timeToExplode;
    }

    private void Update()
    {
        countdown -= Time.deltaTime;

        if (countdown <= 0 && !hasExploded)
        {
            Explode();
        }

    }

    void Explode()
    {
        hasExploded = true;

        explosionEffect.Play();

        source.PlayOneShot(cardExplodeSound);

        foreach (MeshRenderer rend in renderers)
        {
            rend.enabled = false;
        }

        foreach (Collider ob in Physics.OverlapSphere(transform.position, explosionRange, mask))
        {

            Debug.Log(ob.name);

            if (ob.GetComponentInParent<Health>())
            {
                ob.GetComponentInParent<Health>().Hit(damage);
            }
        }

    }
}
