using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    [Header("Weapon Sway")]
    public float swayAmount;
    public float maxSway;
    public float swaySmoothAmount;

    [Header("Weapon Tilt")]
    public float tiltAmount;
    public float maxTilt;
    public float tiltSmoothAmount;

    [Space(5)]

    public bool rotationX = true;
    public bool rotationY = true;
    public bool rotationZ = true;

    [Header("Internals")]
    private float mouseX;
    private float mouseY;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * swayAmount * -1;
        mouseY = Input.GetAxis("Mouse Y") * swayAmount * -1;

        ApplySway();
        ApplyTilt();

    }

    void ApplySway()
    {
        float swayX = Mathf.Clamp(mouseX * swayAmount, -maxSway, maxSway);
        float swayY = Mathf.Clamp(mouseY * swayAmount, -maxSway, maxSway);

        Vector3 finalPosition = new Vector3(swayX, swayY, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * swaySmoothAmount);
    }

    void ApplyTilt()
    {
        float tiltX = Mathf.Clamp(mouseY * tiltAmount, -maxTilt, maxTilt);
        float tiltY = Mathf.Clamp(mouseX * tiltAmount, -maxTilt, maxTilt);

        Quaternion finalRotation = Quaternion.Euler(new Vector3(rotationX ? -tiltX : 0f, rotationY ? tiltY : 0f, rotationZ ? tiltY : 0f));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, Time.deltaTime * tiltSmoothAmount);
    }

}
