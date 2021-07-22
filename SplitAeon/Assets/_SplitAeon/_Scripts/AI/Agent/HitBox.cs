using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public AIAgent attachedAgent;
    public bool hitIsActive = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(hitIsActive)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                attachedAgent.DamagePlayer();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if(hitIsActive)
        {
            CapsuleCollider collider = GetComponent<CapsuleCollider>();

            Vector3 scale = new Vector3();

            scale.x = collider.radius;
            scale.y = collider.height;
            scale.z = collider.radius;

            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.color = Color.red;
            Gizmos.DrawCube(collider.center, scale);
        }
    }
}
