using UnityEngine;

public class WarpChecker : MonoBehaviour
{
    private short collisionCount;

    public LayerMask ignoredLayers;

    [HideInInspector]
    public float offsetVal;

    [HideInInspector]
    public bool isInPast;

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

    public bool IsAbleToWarp()
    {
        return (collisionCount <= 0);
    }

    public bool DoWarpCheck()
    {

        return IsAbleToWarp();
    }
}