using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class ObjectiveTrigger : MonoBehaviour
{

    public UnityEvent onEnterEvents;

    public bool destroyOnTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onEnterEvents.Invoke();

            if (destroyOnTriggerEnter)
            {
                Destroy(this.gameObject);
            }
        }

    }


}
