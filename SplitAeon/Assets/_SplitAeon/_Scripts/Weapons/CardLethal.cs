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
            collision.collider.gameObject.GetComponent<Health>().Hit(damage);
        }
        else if (collision.collider.gameObject.GetComponent<Target>())
        {
            collision.collider.gameObject.GetComponent<Target>().Hit();
        }
    }

}
