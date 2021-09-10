using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveKillTracker : MonoBehaviour
{

    public bool isTrackingEnabled;

    public int killTarget;

    [HideInInspector]
    public int killCount;

    public UnityEvent onTargetReached;

    public void LogKill()
    {
        if (isTrackingEnabled)
        {
            killCount++;

            if (killCount == killTarget)
            {
                onTargetReached.Invoke();
                ResetKills();
                isTrackingEnabled = false;
            }

        }
    }

    public void ResetKills()
    {
        killCount = 0;
    }

    public void ToggleBool(bool b)
    {
        isTrackingEnabled = b;
    }

}
