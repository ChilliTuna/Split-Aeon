using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileSettings settings;

    float m_timer = 0.0f;

    Rigidbody m_body;
    float m_playerDistance = 0.0f;

    [HideInInspector] public AIAgent owner;

    // Start is called before the first frame update
    void Start()
    {
        m_body = GetComponent<Rigidbody>();
        m_body.velocity = transform.forward * settings.force * m_playerDistance;
        m_timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        //transform.position += transform.forward * speed * Time.deltaTime;

        if(m_timer > settings.lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot(AIAgent owner, Vector3 origin, Vector3 destination)
    {
        this.owner = owner;
        transform.position = origin;

        Vector3 launchVector = destination - origin;

        m_playerDistance = launchVector.magnitude;

        launchVector.y = 0.0f;
        float scaled = settings.upAngle;
        scaled /= 90;
        launchVector = Vector3.RotateTowards(launchVector, Vector3.up, scaled, 0);
        transform.LookAt(origin + launchVector);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            owner.DamagePlayer();
            Destroy(gameObject);
        }
    }
}
