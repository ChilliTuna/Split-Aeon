using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDecay : MonoBehaviour
{

    public float decayTime;

    private void Update()
    {
        decayTime -= Time.deltaTime;

        if (decayTime < 0)
        {
            Destroy(this.gameObject);
        }

    }
}
