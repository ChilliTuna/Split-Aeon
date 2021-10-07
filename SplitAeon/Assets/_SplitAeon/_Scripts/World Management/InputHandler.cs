using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private UserActions userActions;
    private InputAction movementForward;
    private InputAction movementRight;

    private void OnEnable()
    {
        movementForward = userActions.PlayerMap.MoveForward;
        movementForward.Enable();
        movementRight = userActions.PlayerMap.MoveRight;
        movementRight.Enable(); 


    }
}
