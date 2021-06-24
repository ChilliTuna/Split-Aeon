using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Timewarp : MonoBehaviour
{
    public Volume volume;

    ChromaticAberration cromAb;
    Bloom bloom;
    Exposure exposure;

    bool isInPresent = true;

    public CharacterController controller;
    public GameObject player;

    public AudioClip[] clips;
    public AudioSource source;
    

    private void Start()
    {
        volume.profile.TryGet(out cromAb);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out exposure);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwapWorlds();
        }

        if (cromAb.intensity.value >= 0)
        {
            cromAb.intensity.value -= 0.5f * Time.deltaTime;
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

    public void SwapWorlds()
    {

        if (isInPresent)
        {
            controller.enabled = false;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 50, player.transform.position.z);
            controller.enabled = true;
        }
        else
        {
            controller.enabled = false;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 50, player.transform.position.z);
            controller.enabled = true;
        }

        TriggerTeleportEffect();

        isInPresent = !isInPresent;
    }

    void TriggerTeleportEffect()
    {

        source.PlayOneShot(clips[Mathf.FloorToInt(Random.Range(0, clips.Length))]);

        cromAb.intensity.value = 1;
        bloom.intensity.value = 1;
        exposure.compensation.value = 3;
    }
}
