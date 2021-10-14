using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    float xMovement;
    float zMovement;

    public bool lockMovement;

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

    #endregion

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

    void Update()
    {

        #region Player Movement

        if (!lockMovement)
        {
            xMovement = Input.GetAxis("Horizontal");
            zMovement = Input.GetAxis("Vertical");
        }
        else
        {
            xMovement = 0f;
            zMovement = 0f;
        }

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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !lockMovement)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

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

        #region Running

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0)
        {
            isRunning = true;

            movementSpeed = sprintSpeed;
        }
        else
        {
            isRunning = false;
        }

        if (!isRunning)
        {
            movementSpeed = walkSpeed;
        }

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

    public void LockPlayerMovement()
    {
        lockMovement = true;
    }

    public void UnlockPlayerMovement()
    {
        lockMovement = false;
    }
}
