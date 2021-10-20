using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using FMODUnity;

public class Footstepper : MonoBehaviour
{

    public Player player;

    public float walkDelay;
    public float runDelay;

    float delay;
    float timer;

    public StudioEventEmitter footsteps;


    public bool stopSounds;

    void Start()
    {
        timer = walkDelay;
    }

    void Update()
    {

        if (player.isRunning)
        {
            delay = runDelay;
        }
        else
        {
            delay = walkDelay;
        }

        if (!stopSounds)
        {
            if (player.isMoving)
            {
                timer -= 1 * Time.deltaTime;

                if (timer <= 0)
                {
                    Step();
                    timer = delay;
                }

            }
        }


    }

    public void Step()
    {
        
        if (!player.isRunning)
        {
            footsteps.SetParameter("Speed Switch", 0);

            footsteps.Params[1].Value = 0;

            Debug.Log("Setting speed to walking");
        }
        else
        {
            footsteps.SetParameter("Speed Switch", 1);

            footsteps.Params[1].Value = 1;

            Debug.Log("Setting speed to running");
        }

        footsteps.Play();

    }
}
