﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.Events;

public class Timewarp : MonoBehaviour
{
    public Volume volume;

    ChromaticAberration cromAb;
    Bloom bloom;
    Exposure exposure;

    bool isInPast = true;

    public CharacterController controller;
    public GameObject player;

    public AudioClip[] clips;
    public AudioSource source;

    public float offsetAmount = 100;

    [Space]
    public UnityEvent onTimeWarp;

    private WarpChecker warpChecker;

    private void Start()
    {
        volume.profile.TryGet(out cromAb);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out exposure);

        warpChecker = transform.Find("WarpChecker").GetComponent<WarpChecker>();
        warpChecker.transform.parent = player.transform;
        warpChecker.offsetVal = offsetAmount;
        warpChecker.isInPast = isInPast;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryWarp();
        }

        if (cromAb.intensity.value >= 0)
        {
            cromAb.intensity.value -= 1f * Time.deltaTime;
        }

        if (exposure.compensation.value >= 0)
        {
            exposure.compensation.value -= 3 * Time.deltaTime;
        }

        if (bloom.intensity.value >= 0)
        {
            bloom.intensity.value -= 2 * Time.deltaTime;
        }
    }

    public void TryWarp()
    {
        // for now, just teleport, do check for objects here

        SwapWorlds();

    }


    public void SwapWorlds()
    {
        player.GetComponent<Player>().viewmodelAnimator.SetTrigger("Warp");

        onTimeWarp.Invoke();

        if (isInPast)
        {
            controller.enabled = false;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - offsetAmount, player.transform.position.z);
            controller.enabled = true;
        }
        else
        {
            controller.enabled = false;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + offsetAmount, player.transform.position.z);
            controller.enabled = true;
        }

        TriggerTeleportEffect();

        ChangeWorldInternal(!isInPast);
    }

    void TriggerTeleportEffect()
    {
        source.PlayOneShot(clips[Mathf.FloorToInt(Random.Range(0, clips.Length))]);

        cromAb.intensity.value = 1;
        bloom.intensity.value = 1;
        exposure.compensation.value = 5;
    }

    void ChangeWorldInternal(bool newIsInPresent)
    {
        isInPast = newIsInPresent;
        GetComponent<GameManager>().isInPresent = newIsInPresent;
    }
}
