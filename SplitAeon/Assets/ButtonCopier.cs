using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ButtonCopier : MonoBehaviour
{

    Button thisButton;
    public Button buttonToCopy;

    private void Start()
    {
        thisButton = GetComponent<Button>();
    }

    void Update()
    {
        thisButton.interactable = buttonToCopy.IsInteractable();
    }
}
