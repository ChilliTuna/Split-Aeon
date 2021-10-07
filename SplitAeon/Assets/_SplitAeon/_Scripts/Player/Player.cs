using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Player : MonoBehaviour
{

    #region Variables

    [HideInInspector] public CharacterController controller;

    #region Movement

    [Header("Movement")]
    private float movementSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    #endregion

    #region Jumping

    [Header("Jumping")]

    public float gravity;
    public float jumpHeight;
    private Vector3 playerVelocity;

    #endregion

    #region States

    [HideInInspector]
    public bool isRunning;
    [HideInInspector]
    public bool isMoving;
    [HideInInspector]
    public bool isBusy;

    #endregion

    #region Ground Check

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask ignoreMask;
    bool isGrounded;

    #endregion

    #region Camera

    [Header("Camera")]
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    public Camera cam;

    [Space(5)]

    public int cameraFOV;
    public int sprintFOVIncrease;

    private int cameraSprintFOV;
    private float currentFOV;

    [HideInInspector]
    public float recoilVertical, recoilHorizontal;

    [HideInInspector]
    public bool lockMouse = false;

    #endregion

    #region Animation

    [Header("Animation")]
    [HideInInspector]
    public Animator viewmodelAnimator;

    #endregion

    #region Footsteps

    [Header("Footsteps")]
    public Footstepper stepper;

    #endregion

    #region Blends

    float weaponMoveBlend;

    #endregion

    #region Inputs

    private UserActions userActions;

    private InputAction movementForward;
    private InputAction movementRight;

    #endregion

    #endregion

    private void Awake()
    {
        userActions = new UserActions();
    }

    private void Start()
    {
        #region Initialization

        Cursor.lockState = CursorLockMode.Locked;
        movementSpeed = walkSpeed;

        cam.transform.rotation = Quaternion.identity;

        cameraSprintFOV = cameraFOV + sprintFOVIncrease;

        currentFOV = cam.fieldOfView;

        #endregion
    }

    private void OnEnable()
    {
        EnableInputs();
    }

    void Update()
    {

        #region Player Movement

        float xMovement = movementRight.ReadValue<float>();
        float zMovement = movementForward.ReadValue<float>();

        if (xMovement == 0 & zMovement == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        #region Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ~ignoreMask);

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        #endregion

        Vector3 move = transform.right * xMovement + transform.forward * zMovement;

        if (move.magnitude > 1)
        {
            move /= move.magnitude;
        }

        controller.Move(move * movementSpeed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

        #endregion

        #region Camera Movement

        float mouseXAxis;
        float mouseYAxis;

        if (!lockMouse)
        {
            mouseXAxis = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseYAxis = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        }
        else
        {
            mouseXAxis = 0;
            mouseYAxis = 0;
        }

        #region Recoil Management

        mouseXAxis += recoilHorizontal;
        mouseYAxis += recoilVertical;

        if (recoilVertical > 0)
        {
            recoilVertical -= 0.4f * Time.deltaTime;

            if (recoilVertical < 0)
            {
                recoilVertical = 0;
            }
        }

        if (recoilHorizontal > 0)
        {
            recoilHorizontal -= 0.4f * Time.deltaTime;

            if (recoilHorizontal < 0)
            {
                recoilHorizontal = 0;
            }
        }
        else if (recoilHorizontal < 0)
        {
            recoilHorizontal += 0.4f * Time.deltaTime;

            if (recoilHorizontal > 0)
            {
                recoilHorizontal = 0;
            }

        }

        #endregion

        xRotation -= mouseYAxis;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseXAxis);

        #endregion

        #region Animation

        if (xMovement != 0 || zMovement != 0)
        {

            viewmodelAnimator.SetBool("isMoving", true);

            if (isRunning)
            {
                weaponMoveBlend = Mathf.Lerp(weaponMoveBlend, 1, Time.deltaTime * 10f);
            }
            else
            {
                weaponMoveBlend = Mathf.Lerp(weaponMoveBlend, 0f, Time.deltaTime * 10f);
            }

        }
        else
        {
            viewmodelAnimator.SetBool("isMoving", false);
        }

        if (isRunning)
        {
            currentFOV = Mathf.Lerp(currentFOV, cameraSprintFOV, Time.deltaTime * 4f);
        }
        else
        {
            currentFOV = Mathf.Lerp(currentFOV, cameraFOV, Time.deltaTime * 4f);
        }

        viewmodelAnimator.SetFloat("MovementBlend", weaponMoveBlend);

        cam.fieldOfView = currentFOV;

        if (isGrounded)
        {
            stepper.stopSounds = false;
        }
        else
        {
            stepper.stopSounds = true;
        }


        #endregion

    }

    private void OnDisable()
    {
        DisableInputs();
    }

    #region MovementActions

    void Jump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void StartSprint(InputAction.CallbackContext obj)
    {
        isRunning = true;
        movementSpeed = sprintSpeed;
    }

    void EndSprint(InputAction.CallbackContext obj)
    {
        isRunning = false;
        movementSpeed = walkSpeed;
    }

    #endregion

    #region InputFunctions

    void EnableInputs()
    {
        movementForward = userActions.PlayerMap.MoveForward;
        movementForward.Enable();

        movementRight = userActions.PlayerMap.MoveRight;
        movementRight.Enable();

        userActions.PlayerMap.Jump.performed += Jump;
        userActions.PlayerMap.Jump.Enable();

        userActions.PlayerMap.Sprint.performed += StartSprint;
        userActions.PlayerMap.Sprint.canceled += EndSprint;
        userActions.PlayerMap.Sprint.Enable();

        //userActions.PlayerMap.Shoot.performed += Shoot;
        //userActions.PlayerMap.Shoot.Enable();
        //
        //userActions.PlayerMap.ThrowCard.performed += ThrowCard;
        //userActions.PlayerMap.ThrowCard.Enable();
        //
        //userActions.PlayerMap.Interact.performed += Interact;
        //userActions.PlayerMap.Interact.Enable();
        //
        //userActions.PlayerMap.Weapon1.performed += ChangeToWeapon1;
        //userActions.PlayerMap.Weapon1.Enable();
        //
        //userActions.PlayerMap.Weapon2.performed += ChangeToWeapon2;
        //userActions.PlayerMap.Weapon2.Enable();
        //
        //userActions.PlayerMap.Weapon3.performed += ChangeToWeapon3;
        //userActions.PlayerMap.Weapon3.Enable();
        //
        //userActions.PlayerMap.Weapon4.performed += ChangeToWeapon4;
        //userActions.PlayerMap.Weapon4.Enable();
        //
        //userActions.PlayerMap.WeaponWheel.performed += WeaponWheel;
        //userActions.PlayerMap.WeaponWheel.Enable();
        //
        //userActions.PlayerMap.Reload.performed += Reload;
        //userActions.PlayerMap.Reload.Enable();
        //


    }

    void DisableInputs()
    {
        movementForward.Disable();
        movementRight.Disable();
        userActions.PlayerMap.Jump.Disable();
        //userActions.PlayerMap.Shoot.Disable();
        //userActions.PlayerMap.ThrowCard.Disable();
        //userActions.PlayerMap.Interact.Disable();
        //userActions.PlayerMap.Weapon1.Disable();
        //userActions.PlayerMap.Weapon2.Disable();
        //userActions.PlayerMap.Weapon3.Disable();
        //userActions.PlayerMap.Weapon4.Disable();
        //userActions.PlayerMap.WeaponWheel.Disable();
        //userActions.PlayerMap.Reload.Disable();
        //userActions.PlayerMap.Sprint.Disable();
    }

    #endregion
}
