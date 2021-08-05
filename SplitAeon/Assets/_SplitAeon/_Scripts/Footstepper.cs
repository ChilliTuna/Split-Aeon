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

    public enum Surface
    {
        Wood,
        Concrete
    }

    Surface currentSurface;

    public AudioSource stepSource;

    [Header("Wood")]
    public AudioClip[] walkWoodClips;
    public AudioClip[] runWoodClips;

    [Header("Concrete")]
    public AudioClip[] walkConcreteClips;
    public AudioClip[] runConcreteClips;


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

        if (player.isMoving)
        {
            timer -= 1 * Time.deltaTime;

            if(timer <= 0)
            {
                Step();
                timer = delay;
            }

        }
    }

    public void Step()
    {
        if(currentSurface == Surface.Wood)
        {
            if (player.isRunning)
            {
                stepSource.PlayOneShot(runWoodClips[Random.Range(0, runWoodClips.Length)]);
            }
            else
            {
                stepSource.PlayOneShot(walkWoodClips[Random.Range(0, walkWoodClips.Length)]);
            }
        }
        else if (currentSurface == Surface.Concrete)
        {
            if (player.isRunning)
            {
                stepSource.PlayOneShot(runConcreteClips[Random.Range(0, runConcreteClips.Length)]);
            }
            else
            {
                stepSource.PlayOneShot(walkConcreteClips[Random.Range(0, walkConcreteClips.Length)]);
            }
        }
    }
}
