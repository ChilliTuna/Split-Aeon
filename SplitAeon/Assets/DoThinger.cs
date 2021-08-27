using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoThinger : MonoBehaviour
{

    public float lockTime;


    public AudioSource source;
    public AudioClip clip;


    public Animator anim;

    float timer;
    bool locked; 

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            locked = false;
        }
        else
        {
            locked = true;
        }
    }

    public void SetLockTime()
    {
        if (!locked)
        {
            timer = lockTime;
        }
    }

    public void PlaySound()
    {  
        if (!locked)
        {
            source.PlayOneShot(clip);
        }

    }

    public void TriggerAnim(string triggerName)
    {
        if (!locked)
        {
            anim.SetTrigger(triggerName);
        }

    }

}
