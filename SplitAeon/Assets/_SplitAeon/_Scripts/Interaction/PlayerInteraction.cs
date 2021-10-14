using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance;
    public Text interactionText;

    private int interactionLayerMask = 1 << 19;

    private Camera cam;

    private bool successfulHit = false;

    private Interactable currentInteractable;

    private UserActions userActions;

    private void Awake()
    {
        userActions = new UserActions();
    }

    private void OnEnable()
    {
        userActions.PlayerMap.Interact.performed += ctx => HandleInteraction(currentInteractable);
        userActions.PlayerMap.Interact.Enable();
    }

    private void OnDisable()
    {
        userActions.PlayerMap.Interact.Disable();
    }

    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        successfulHit = false;

        if (Physics.Raycast(ray, out hit, interactionDistance, interactionLayerMask))
        {
            currentInteractable = hit.collider.GetComponent<Interactable>();

            if (currentInteractable != null)
            {
                interactionText.text = currentInteractable.GetDescription();
                successfulHit = true;
            }
        }

        if (!successfulHit)
        {
            interactionText.text = "";
        }
    }

    private void HandleInteraction(Interactable interactable)
    {
        if (successfulHit)
        {
            if (currentInteractable != null)
            {
                interactable.Interact();
            }
        }
    }
}