using UnityEngine;

public class WeaponSway : MonoBehaviour
{

    [Header("Weapon Sway")]
    [Range(0, 0.5f)]
    public float swayAmount;
    [Range(0, 0.5f)]
    public float maxSway;
    public float swayRecoveryRate;

    [Header("Weapon Tilt")]
    [Range(0, 30f)]
    public float tiltAmount;
    [Range(0, 30f)]
    public float maxTilt;
    public float tiltRecoveryAmount;

    [Space(5)]

    public bool rotateOnXAxis = true;
    public bool rotateOnYAxis = true;
    public bool rotateOnZAxis = true;

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

        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initialPosition, Time.deltaTime * swayRecoveryRate);
    }

    void ApplyTilt()
    {
        float tiltX = Mathf.Clamp(mouseY * tiltAmount, -maxTilt, maxTilt);
        float tiltY = Mathf.Clamp(mouseX * tiltAmount, -maxTilt, maxTilt);

        Quaternion finalRotation = Quaternion.Euler(new Vector3(rotateOnXAxis ? -tiltX : 0f, rotateOnYAxis ? tiltY : 0f, rotateOnZAxis ? tiltY : 0f));

        transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * initialRotation, Time.deltaTime * tiltRecoveryAmount);
    }

}
