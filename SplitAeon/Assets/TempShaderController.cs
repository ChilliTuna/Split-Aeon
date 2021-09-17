using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempShaderController : MonoBehaviour
{
    float value = 0.5f;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Y))
        {
            value -= 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            value += 0.1f;
        }

        if (value > 1)
            value = 1;

        if (value < 0)
            value = 0;

        GetComponent<MeshRenderer>().material.SetFloat("Vector1_E89E4155", value);

    }
}
