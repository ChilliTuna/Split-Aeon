using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 5.0f;

    public float lifeTime = 5.0f;
    float m_timer = 0.0f;

    Rigidbody m_body;

    [HideInInspector] public AIAgent owner;

    // Start is called before the first frame update
    void Start()
    {
        m_body = GetComponent<Rigidbody>();
        m_body.velocity = transform.forward * speed;
        m_timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        //transform.position += transform.forward * speed * Time.deltaTime;

        if(m_timer > lifeTime)
        {
            Destroy(gameObject);
        }
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
