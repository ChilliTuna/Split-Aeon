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
    public float crouchSpeed;

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
    public bool isCrouching;
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
    public Camera viewmodelCam;

    [Space(5)]

    public float standingCameraHeight;
    public float crouchingCameraHeight;

    private Vector3 cameraPosStanding;
    private Vector3 cameraPosCrouched;

    private Vector3 cameraHeight;

    [Space(5)]

    public int cameraFOV;
    public int sprintFOVIncrease;

    private int cameraSprintFOV;
    private float currentFOV;

    [HideInInspector]
    public float recoilVertical, recoilHorizontal;

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
    float crouchBlend;

    #endregion

    #endregion

    private void Start()
    {

        #region Initialization

        cameraPosStanding = new Vector3(0, standingCameraHeight, 0);
        cameraPosCrouched = new Vector3(0, crouchingCameraHeight, 0);

        cameraHeight = cameraPosStanding;

        Cursor.lockState = CursorLockMode.Locked;
        movementSpeed = walkSpeed;

        cam.transform.localPosition = cameraPosStanding;
        cam.transform.rotation = Quaternion.identity;

        cameraSprintFOV = cameraFOV + sprintFOVIncrease;

        currentFOV = cam.fieldOfView;

        #endregion

    }

    void Update()
    {

        #region Player Movement

        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

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

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        playerVelocity.y += gravity * Time.deltaTime;

        controller.Move(playerVelocity * Time.deltaTime);

        #endregion

        #region Camera Movement

        float mouseXAxis = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseYAxis = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

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

        #region Running & Crouching

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && Input.GetAxis("Vertical") > 0)
        {
            isRunning = true;

            movementSpeed = sprintSpeed;
        }
        else
        {
            isRunning = false;
        }

        if (Input.GetKey(KeyCode.LeftControl) && !isRunning)
        {
            isCrouching = true;
            movementSpeed = crouchSpeed;
        }
        else
        {
            isCrouching = false;
        }

        cameraHeight.y = crouchBlend;

        cam.transform.localPosition = cameraHeight;


        if (!isRunning && !isCrouching)
        {
            movementSpeed = walkSpeed;
        }

        #endregion

        #region Animation

        if (isCrouching)
        {
            crouchBlend = Mathf.Lerp(crouchBlend, crouchingCameraHeight, Time.deltaTime * 4f);
        }
        else
        {
            crouchBlend = Mathf.Lerp(crouchBlend, standingCameraHeight, Time.deltaTime * 4f);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            GetComponent<Health>().Hit();
        }

        if (xMovement != 0 || zMovement != 0)
        {

            viewmodelAnimator.SetBool("isMoving", true);

            if (isRunning)
            {
                weaponMoveBlend = Mathf.Lerp(weaponMoveBlend, 1, Time.deltaTime * 4f);
            }
            else
            {
                weaponMoveBlend = Mathf.Lerp(weaponMoveBlend, 0f, Time.deltaTime * 4f);
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
        viewmodelCam.fieldOfView = cam.fieldOfView;

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

}
