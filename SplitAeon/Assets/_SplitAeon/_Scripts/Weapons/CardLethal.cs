using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLethal : MonoBehaviour
{

    public int damage;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponent<Health>())
        {
            StickToSurface(collision);

            collision.collider.gameObject.GetComponent<Health>().Hit(damage, collision.collider);
        }
        else if (collision.collider.gameObject.GetComponent<Target>())
        {
            StickToSurface(collision);

            collision.collider.gameObject.GetComponent<Target>().Hit();
        }
        else
        {
            StickToSurface(collision);
        }
    }

    void StickToSurface(Collision collision)
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;

        transform.parent = collision.transform;

        transform.position = collision.GetContact(0).point;
    }

}
