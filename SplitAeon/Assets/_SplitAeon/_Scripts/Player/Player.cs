using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{

    #region Variables
    public CharacterController controller;

    [Header("Movement Variables")]
    private float movementSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float gravity;
    public float jumpHeight;
    private Vector3 playerVelocity;

    public bool isRunning;
    private bool isCrouching;

    [HideInInspector]
    public bool isMoving;

    public GameObject cameraPosStanding;
    public GameObject cameraPosCrouched;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask ignoreMask;
    bool isGrounded;

    [Header("Camera Movement")]
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    public Camera cam;
    public Camera viewmodelCam;

    public int cameraFOV;
    public int sprintFOVIncrease;

    private int cameraSprintFOV;
    private float currentFOV;

    [HideInInspector]
    public float recoilVertical, recoilHorizontal;

    [Header("Animation")]
    public Animator viewmodelAnimator;

    [HideInInspector]
    public bool isBusy;

    public Footstepper stepper;


    #endregion

    float tempBlend;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        movementSpeed = walkSpeed;

        cam.transform.localPosition = cameraPosStanding.transform.localPosition;
        cam.transform.rotation = Quaternion.identity;

        cameraSprintFOV = cameraFOV + sprintFOVIncrease;

        currentFOV = cam.fieldOfView;

        //source.clip = startWarp;
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

            cam.transform.localPosition = cameraPosCrouched.transform.localPosition;
        }
        else
        {
            isCrouching = false;

            cam.transform.localPosition = cameraPosStanding.transform.localPosition;
        }

        if (!isRunning && !isCrouching)
        {
            movementSpeed = walkSpeed;
        }

        #endregion

        #region Animation

        if (Input.GetKeyDown(KeyCode.E))
        {
            viewmodelAnimator.SetTrigger("Warp");
        }

        if (xMovement != 0 || zMovement != 0)
        {

            viewmodelAnimator.SetBool("isMoving", true);

            if (isRunning)
            {
                tempBlend = Mathf.Lerp(tempBlend, 1, Time.deltaTime * 4f);
            }
            else
            {
                tempBlend = Mathf.Lerp(tempBlend, 0f, Time.deltaTime * 4f);
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

        viewmodelAnimator.SetFloat("MovementBlend", tempBlend);

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
