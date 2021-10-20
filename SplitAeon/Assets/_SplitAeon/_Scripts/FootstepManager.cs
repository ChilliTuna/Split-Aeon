using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FMODUnity;
using FMODUnityResonance;

public class FootstepManager : MonoBehaviour
{


    string currentSurface;

    public Footstepper stepper;

    public LayerMask mask;

    public float surfaceCheckInterval;

    public List<string> surfaces = new List<string>();

    void Start()
    {
        SetCurrentSurface(surfaces[0]);
        InvokeRepeating("CheckSurface", 1, surfaceCheckInterval);
    }

    public void SetCurrentSurface(string surface)
    {
        if (currentSurface != surface)
        {
            if (surface == "Marble")
            {
                stepper.footsteps.SetParameter("Material", 0);

                stepper.footsteps.Params[0].Value = 0;

                Debug.Log("Setting material to marble");

            }
            else if (surface == "Wood")
            {
                stepper.footsteps.SetParameter("Material", 1);

                stepper.footsteps.Params[0].Value = 1;

                Debug.Log("Setting material to wood");

            }
            else if (surface == "Carpet")
            {
                stepper.footsteps.SetParameter("Material", 2);

                stepper.footsteps.Params[0].Value = 2;

                Debug.Log("Setting material to carpet");

            }

        }
    }

    public void CheckSurface()
    {
        foreach (Collider col in Physics.OverlapSphere(gameObject.transform.position, 0.3f, ~mask))
        {
            foreach (string s in surfaces)
            {
                if (col.transform.tag == s)
                {
                    SetCurrentSurface(s);
                }

            }
        }
    }
}
