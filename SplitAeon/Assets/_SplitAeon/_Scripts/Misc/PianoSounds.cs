using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoSounds : MonoBehaviour
{

    private AudioSource source;
    public AudioClip[] clips;

    public AudioClip secret;

    bool isLocked = false;

    float timer;
    int secretCount;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer -= Time.deltaTime;    

        if (timer <= 0)
        {
            secretCount = 0;
        }    
    }

    public void PlayRandomNote()
    {
        if (isLocked)
        {
            return;
        }

        if (secretCount == 10)
        {
            secretCount = 0;

            LockSound();
            TriggerSecret();
            Invoke("UnlockSound", 30f);
        }
        else
        {
            source.PlayOneShot(clips[Mathf.FloorToInt(Random.Range(0, clips.Length))]);

            secretCount++;
            timer = 3;
        }
    }

    public void TriggerSecret()
    {
        source.PlayOneShot(secret);
    }

    public void LockSound()
    {
        isLocked = true;
    }

    public void UnlockSound()
    {
        isLocked = false;
    }
}
