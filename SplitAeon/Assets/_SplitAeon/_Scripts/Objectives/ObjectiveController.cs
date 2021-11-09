using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveController : MonoBehaviour
{
    public ObjectiveAsset[] objectiveAssets;

    public void EnableAssets()
    {
        foreach (var asset in objectiveAssets)
        {
            asset.EnableAsset();
        }
    }

    public void DisableAssets()
    {
        foreach (var asset in objectiveAssets)
        {
            asset.DisableAsset();
        }
    }
}
