using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GUIController : MonoBehaviour
{
    public UnityEvent onPressEscape;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            onPressEscape.Invoke();
        }
    }
}
