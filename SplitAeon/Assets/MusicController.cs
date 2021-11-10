using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FMODUnity;

public class MusicController : MonoBehaviour
{

    public StudioEventEmitter atmosphere;
    public StudioEventEmitter combat1;
    public StudioEventEmitter combat2;

    void Start()
    {
      
        atmosphere.Play();

    }

    public void PlayAtmoshere()
    {
        combat1.Stop();
        combat2.Stop();

        atmosphere.Play();
    }

    public void PlayCombatOne()
    {
        atmosphere.Stop();
        combat2.Stop();

        combat1.Play();
    }

    public void PlayCombatTwo()
    {
        combat1.Stop();
        atmosphere.Stop();

        combat2.Play();
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayAtmoshere();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayCombatOne();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayCombatTwo();
        }
        */
    }
}
