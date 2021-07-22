﻿using UnityEngine;
using UnityEngine.Events;

public class GUIController : MonoBehaviour
{
    public static bool isGamePaused = false;

    public UnityEvent onPressEscape;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onPressEscape.Invoke();
        }
    }
}