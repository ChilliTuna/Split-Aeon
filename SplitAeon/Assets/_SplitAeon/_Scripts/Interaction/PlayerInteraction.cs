﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance;
    public Text interactionText;

    int interactionLayerMask = 1 << 19;
    
    Camera cam;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        interactionText = GameObject.Find("InteractionTextField").GetComponent<Text>();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        bool successfulHit = false;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayerMask))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                HandleInteraction(interactable);
                interactionText.text = interactable.GetDescription();
                successfulHit = true;
            }
        }

        if (!successfulHit)
        {
            interactionText.text = "";
        }
    }

    void HandleInteraction(Interactable interactable)
    {
        KeyCode key = KeyCode.F;

        if (Input.GetKeyDown(key))
        {
            interactable.Interact();
        }
    }
}