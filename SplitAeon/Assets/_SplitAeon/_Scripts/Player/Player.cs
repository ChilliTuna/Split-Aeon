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

    #region Unity Functions

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

    #endregion

    #region Movement Actions

    void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void StartSprint()
    {
        isRunning = true;
        movementSpeed = sprintSpeed;
    }

    void EndSprint()
    {
        isRunning = false;
        movementSpeed = walkSpeed;
    }

    #endregion

    #region Input Functions

    void EnableInputs()
    {
        movementForward = userActions.PlayerMap.MoveForward;
        movementForward.Enable();

        movementRight = userActions.PlayerMap.MoveRight;
        movementRight.Enable();

        userActions.PlayerMap.Jump.performed += ctx => Jump();
        userActions.PlayerMap.Jump.Enable();

        userActions.PlayerMap.Sprint.performed += ctx => StartSprint();
        userActions.PlayerMap.Sprint.canceled += ctx => EndSprint();
        userActions.PlayerMap.Sprint.Enable();
    }

    void DisableInputs()
    {
        movementForward.Disable();
        movementRight.Disable();
        userActions.PlayerMap.Jump.Disable();
        userActions.PlayerMap.Sprint.Disable();
    }

    #endregion
}
