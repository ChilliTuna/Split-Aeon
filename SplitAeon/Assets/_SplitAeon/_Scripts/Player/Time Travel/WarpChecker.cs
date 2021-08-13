using UnityEngine;

public class WarpChecker : MonoBehaviour
{
    private short collisionCount;

    public LayerMask ignoredLayers;

    private void OnTriggerEnter(Collider other)
    {
        if (ignoredLayers == (ignoredLayers | (1 << other.gameObject.layer)))
        {
            return;
        }
        collisionCount++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (ignoredLayers == (ignoredLayers | (1 << other.gameObject.layer)))
        {
            return;
        }
        collisionCount--;
    }
}