using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTimeScaleControl : MonoBehaviour
{
    [Range(0.0f, 1.0f)] public float timeScale = 1.0f;

    private void OnValidate()
    {
        if(Application.isPlaying)
        {
            Time.timeScale = timeScale;
        }
    }
}
