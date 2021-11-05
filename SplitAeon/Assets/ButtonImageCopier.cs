using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ButtonImageCopier : MonoBehaviour
{
    Image thisImage;
    public Button buttonToCopy;

    private void Start()
    {
        thisImage = GetComponent<Image>();
    }

    void Update()
    {
        thisImage.enabled = buttonToCopy.IsInteractable();
    }
}
