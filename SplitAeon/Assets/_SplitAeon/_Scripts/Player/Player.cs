using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Variables

    [HideInInspector] public CharacterController controller;

    #region Movement

    [Header("Movement")]
    private float movementSpeed;

    public float walkSpeed;
    public float sprintSpeed;

    #endregion Movement

    #region Jumping

    [Header("Jumping")]
    public float gravity;

    public float jumpHeight;
    private Vector3 playerVelocity;

    #endregion Jumping

    #region States

    [HideInInspector]
    public bool isRunning;

    [HideInInspector]
    public bool isMoving;

    [HideInInspector]
    public bool isBusy;

    #endregion States

    #region Ground Check

    [Header("Ground Check")]
    public Transform groundCheck;

    public float groundDistance = 0.3f;
    public LayerMask ignoreMask;
    private bool isGrounded;

    #endregion Ground Check

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

    #endregion Camera

    #region Animation

    [Header("Animation")]
    [HideInInspector]
    public Animator viewmodelAnimator;

    #endregion Animation

    #region Footsteps

    [Header("Footsteps")]
    public Footstepper stepper;

    #endregion Footsteps

    #region Blends

    private float weaponMoveBlend;

    #endregion Blends

    #region Inputs

    private UserActions userActions;

    private InputAction movementForward;
    private InputAction movementRight;

    #endregion Inputs

    #endregion Variables

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

        #endregion Initialization
    }

    private void OnEnable()
    {
        userActions.PlayerMap.Jump.LoadBinding(InputActions.Jump);
        EnableInputs();
    }

    private void Update()
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

        if (movementForward.ReadValue<float>() <= 0)
        {
            EndSprint();
        }

        #region Ground Check

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, ~ignoreMask);

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        #endregion Ground Check

        Vector3 move = transform.right * xMovement + transform.forward * zMovement;

        if (move.magnitude > 1)
        {
            move /= move.magnitude;
        }

        controller.Move(move * movementSpeed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

        #endregion Player Movement

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

        #endregion Recoil Management

        xRotation -= mouseYAxis;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseXAxis);

        #endregion Camera Movement

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

        #endregion Animation
    }

    private void OnDisable()
    {
        DisableInputs();
    }

    #endregion Unity Functions

    #region Movement Actions

    private void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void StartSprint()
    {
        if (movementForward.ReadValue<float>() > 0)
        {
            isRunning = true;
            movementSpeed = sprintSpeed;
        }
    }

    private void EndSprint()
    {
        isRunning = false;
        movementSpeed = walkSpeed;
    }

    #endregion Movement Actions

    #region Input Functions

    public void EnableInputs()
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

    public void DisableInputs()
    {
        movementForward.Disable();
        movementRight.Disable();
        userActions.PlayerMap.Jump.Disable();
        userActions.PlayerMap.Sprint.Disable();
    }

    #endregion Input Functions
}