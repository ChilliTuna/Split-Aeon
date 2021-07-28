using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UniversalInteractable : Interactable
{
    [Tooltip("Display \"Press [F] to \" before the description")]
    public bool promtInteractKey;

    [Tooltip("The text that should be displayed when viewing the object")]
    public string lookAtDescription;

    public UnityEvent onInteractEvents;

    public override string GetDescription()
    {
        if (promtInteractKey)
        {
            return "Press <color=green>[F]</color> to " + lookAtDescription;
        }
        else
        {
            return lookAtDescription;
        }
    }

    public override void Interact()
    {
        onInteractEvents.Invoke();
    }

}
