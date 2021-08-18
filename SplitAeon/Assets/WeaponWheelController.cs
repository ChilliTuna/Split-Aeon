using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWheelController : MonoBehaviour
{

    public Animator anim;
    public bool isOpen = false;

    Player player;

    public KeyCode weaponWheelButton;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        if (Input.GetKeyDown(weaponWheelButton))
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
}
