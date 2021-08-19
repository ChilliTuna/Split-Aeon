using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WarpChecker : MonoBehaviour
{
    public short collisionCount;

    public List<GameObject> collidingObjects;

    public LayerMask ignoredLayers;

    public bool hasSecondary = false;

    public WarpChecker secondaryChecker;

    private void OnTriggerEnter(Collider other)
    {
        if (ignoredLayers == (ignoredLayers | (1 << other.gameObject.layer)))
        {
            return;
        }
        collisionCount++;
        collidingObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (ignoredLayers == (ignoredLayers | (1 << other.gameObject.layer)))
        {
            return;
        }
        collisionCount--;
        collidingObjects.Remove(other.gameObject);
    }

    public bool DoWarpCheck()
    {
        if (hasSecondary)
        {
            if (secondaryChecker.collisionCount <= 0)
            {
                return (secondaryChecker.collisionCount <= 1);
            }
        }
        return (collisionCount <= 0);
    }

    public void GoToCorrectPosition(bool isInPast, float offset)
    {
        if (isInPast)
        {
            transform.localPosition = new Vector3(0, offset + 0.75f, 0);
        }
        else
        {
            transform.localPosition = new Vector3(0, -offset + 0.75f, 0);
        }
        GetComponent<CapsuleCollider>().enabled = true;
        if (hasSecondary)
        {
            secondaryChecker.GetComponent<CapsuleCollider>().enabled = true;
        }
    }
}