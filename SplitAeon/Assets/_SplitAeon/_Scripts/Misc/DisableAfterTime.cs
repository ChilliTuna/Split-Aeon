using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    public float timeToDisable;
    float timer;

    void Awake()
    {
        timer = timeToDisable;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = timeToDisable;
            gameObject.SetActive(false);
        }

    }
}
