using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCopier : MonoBehaviour
{

    public Camera mainCam;

    void Update()
    {
        transform.rotation = mainCam.transform.rotation;
        transform.position = new Vector3(mainCam.transform.position.x, mainCam.transform.position.y - 50, mainCam.transform.position.z);
    }
}
