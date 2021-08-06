using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstepper : MonoBehaviour
{

    public Player player;

    public float walkDelay;
    public float runDelay;

    float delay;
    float timer;

    public AudioSource stepSource;

    //[HideInInspector]
    public AudioClip[] walkClips;
    //[HideInInspector]
    public AudioClip[] runClips;

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
            stepSource.PlayOneShot(walkClips[Random.Range(0, walkClips.Length)]);
        }
        else
        {
            stepSource.PlayOneShot(runClips[Random.Range(0, runClips.Length)]);
        }
        
    }
}
