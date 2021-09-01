using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWorldTools : MonoBehaviour
{   
    public void DisableThisObject()
    {
        gameObject.SetActive(false);
    }

    public void EnableObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

    public void DisableObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void ToggleObject(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

}
