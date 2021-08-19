using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Timewarp : MonoBehaviour
{
    public Volume volume;

    private ChromaticAberration cromAb;
    private Bloom bloom;
    private Exposure exposure;

    private bool isInPast = true;

    public CharacterController controller;
    public GameObject player;

    public AudioClip[] clips;
    public AudioSource source;

    public float offsetAmount = 100;

    public bool shouldDoWarpChecking = true;

    private WarpChecker toPastWarpChecker;
    private WarpChecker toFutureWarpChecker;

    public GameObject warpingBlockedText;

    public GameObject warpWarningImage;

    [Space]
    public UnityEvent onTimeWarp;

    private void Start()
    {
        volume.profile.TryGet(out cromAb);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out exposure);

        toPastWarpChecker = transform.Find("PastWarpChecker").GetComponent<WarpChecker>();
        toPastWarpChecker.transform.parent = player.transform;
        toPastWarpChecker.GoToCorrectPosition(true, offsetAmount);

        toFutureWarpChecker = transform.Find("FutureWarpChecker").GetComponent<WarpChecker>();
        toFutureWarpChecker.transform.parent = player.transform;
        toFutureWarpChecker.GoToCorrectPosition(false, offsetAmount);
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

        if (isInPast)
        {
            ToggleWarpWarning(!toFutureWarpChecker.DoWarpCheck());
        }
        else
        {
            ToggleWarpWarning(!toPastWarpChecker.DoWarpCheck());
        }
    }

    public void TryWarp()
    {
        // for now, just teleport, do check for objects here
        if (shouldDoWarpChecking)
        {
            if (isInPast)
            {
                if (!toFutureWarpChecker.DoWarpCheck())
                {
                    StartCoroutine(ToggleGameObjectForTime(warpingBlockedText, 2));
                    return;
                }
            }
            else
            {
                if (!toPastWarpChecker.DoWarpCheck())
                {
                    StartCoroutine(ToggleGameObjectForTime(warpingBlockedText, 2));
                    return;
                }
            }
        }
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
            //warpWarningImage.SetActive(toPastWarpChecker.DoWarpCheck());
        }
        else
        {
            controller.enabled = false;
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + offsetAmount, player.transform.position.z);
            controller.enabled = true;
            //warpWarningImage.SetActive(toFutureWarpChecker.DoWarpCheck());
        }

        TriggerTeleportEffect();

        ChangeWorldInternal(!isInPast);
    }

    private void TriggerTeleportEffect()
    {
        source.PlayOneShot(clips[Mathf.FloorToInt(Random.Range(0, clips.Length))]);

        cromAb.intensity.value = 1;
        bloom.intensity.value = 1;
        exposure.compensation.value = 5;
    }

    private void ChangeWorldInternal(bool newIsInPresent)
    {
        isInPast = newIsInPresent;
        GetComponent<GameManager>().isInPresent = newIsInPresent;
    }

    public IEnumerator ToggleGameObjectForTime(GameObject gameObject, float period)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
        yield return new WaitForSeconds(period);
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    public void ToggleWarpWarning(bool newActive)
    {
        warpWarningImage.SetActive(newActive);
    }
}