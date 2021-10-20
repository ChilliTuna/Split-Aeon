using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWheelController : MonoBehaviour
{
    private UserActions userActions;

    public Animator anim;
    public bool isOpen = false;

    Player player;

    private void Awake()
    {
        userActions = new UserActions();
    }

    private void OnEnable()
    {
        userActions.PlayerMap.WeaponWheel.LoadBinding(InputActions.WeaponWheel);
        userActions.PlayerMap.WeaponWheel.performed += ctx => ToggleWeaponWheel();
        userActions.PlayerMap.WeaponWheel.Enable();
    }

    private void OnDisable()
    {
        userActions.PlayerMap.WeaponWheel.Disable();
    }

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        player.lockMouse = isOpen;
    }

    public void OpenWeaponWheel()
    {
        isOpen = true;

        CheckWheelState();
    }

    public void CloseWeaponWheel()
    {
        isOpen = false;

        CheckWheelState();
    }

    void CheckWheelState()
    {
        if (isOpen)
        {
            player.isBusy = true;

            anim.SetBool("isOpen", true);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            player.isBusy = false;

            anim.SetBool("isOpen", false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void ToggleWeaponWheel()
    {
        if (isOpen)
        {
            CloseWeaponWheel();
        }
        else if (!player.isBusy)
        {
            OpenWeaponWheel();
        }
    }
}
