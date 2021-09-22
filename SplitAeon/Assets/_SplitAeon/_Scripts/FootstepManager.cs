using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{


    string currentSurface;

    public Footstepper stepper;

    public LayerMask mask;

    public float surfaceCheckInterval;

    public List<StepSurface> surfaces = new List<StepSurface>();

    public AudioClip[] fallbackStepSounds;


    void Start()
    {
        SetCurrentSurface(surfaces[0]);
        InvokeRepeating("CheckSurface", 1, surfaceCheckInterval);
    }

    private void Update()
    {

    }

    public void SetCurrentSurface(StepSurface surface)
    {
        if (currentSurface != surface.name)
        {
            if (surface.walkSounds.Length != 0)
            {
                stepper.walkClips = surface.walkSounds;
            }
            else
            {
                stepper.walkClips = fallbackStepSounds;
            }

            if (surface.runSounds.Length != 0)
            {
                stepper.runClips = surface.runSounds;
            }
            else
            {
                stepper.runClips = fallbackStepSounds;
            }
        }
    }

    public void CheckSurface()
    {
        foreach (Collider col in Physics.OverlapSphere(gameObject.transform.position, 0.3f, ~mask))
        {
            foreach (StepSurface s in surfaces)
            {
                if (col.transform.tag == s.name)
                {
                    //Debug.LogWarning("This is a floor of type " + s.name);
                    SetCurrentSurface(s);
                }

            }
        }
    }


}

[System.Serializable]
public class StepSurface
{
    public string name;

    public AudioClip[] walkSounds;
    public AudioClip[] runSounds;

    //public AudioClip jumpSound;
    //public AudioClip landSound;


}
