using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveAsset : MonoBehaviour
{
    public void EnableAsset()
    {
        gameObject.SetActive(true);
    }

    public void DisableAsset()
    {
        gameObject.SetActive(false);
    }

    public void EnableTimeWarping(Timewarp warp)
    {
        warp.enabled = true;
    }

    public void DisableTimeWarping(Timewarp warp)
    {
        warp.enabled = false;
    }
        
    public void DestroyThisObject()
    {
        Destroy(gameObject);
    }

    public void DestroyOtherObject(GameObject ob)
    {
        Destroy(ob);
    }

    public void EnableInteraction(UniversalInteractable interactable)
    {
        interactable.enabled = true;
    }

    public void DisableInteraction(UniversalInteractable interactable)
    {
        interactable.enabled = false;
    }

}
