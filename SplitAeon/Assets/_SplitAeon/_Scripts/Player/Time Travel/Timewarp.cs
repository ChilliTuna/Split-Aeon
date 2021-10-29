using FMODUnity;
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

    private CharacterController controller;
    public GameObject player;
    public Animator watchViewmodelAnimator;

    public StudioEventEmitter warpSound;

    public float offsetAmount = 100;

    public bool shouldDoWarpChecking = true;

    private WarpChecker toPastWarpChecker;
    private WarpChecker toFutureWarpChecker;

    public GameObject warpingBlockedText;
    public GameObject warpWarningImage;

    public float warpDelay = 1;

    private CustomTimer timer = new CustomTimer();

    [Space]
    public UnityEvent onTimeWarp;

    private UserActions userActions;

    private void Awake()
    {
        userActions = new UserActions();
    }

    private void OnEnable()
    {
        userActions.PlayerMap.TimeTravel.LoadBinding(InputActions.TimeTravel);
        userActions.PlayerMap.TimeTravel.performed += ctx => TryWarp();
        userActions.PlayerMap.TimeTravel.Enable();
    }

    private void OnDisable()
    {
        userActions.PlayerMap.TimeTravel.Disable();
    }

    private void Start()
    {
        volume.profile.TryGet(out cromAb);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out exposure);

        controller = player.GetComponent<CharacterController>();
        watchViewmodelAnimator = GameObject.Find("PlayerMagicViewmodel").GetComponent<Animator>();

        toPastWarpChecker = transform.Find("PastWarpChecker").GetComponent<WarpChecker>();
        toPastWarpChecker.transform.parent = player.transform;
        toPastWarpChecker.GoToCorrectPosition(true, offsetAmount);

        toFutureWarpChecker = transform.Find("FutureWarpChecker").GetComponent<WarpChecker>();
        toFutureWarpChecker.transform.parent = player.transform;
        toFutureWarpChecker.GoToCorrectPosition(false, offsetAmount);
    }

    private void Update()
    {
        timer.Count();

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
        if (timer.GetIsActive())
        {
            if (timer.GetCurrentTime() < warpDelay)
            {
                return;
            }
            timer.Stop();
            timer.Reset();
        }
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
        //player.GetComponent<Player>().viewmodelAnimator.SetTrigger("Warp");

        //DoWarp();

        watchViewmodelAnimator.SetTrigger("Warp");

    }

    public void DoWarp()
    {
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

        ChangeWorldInternal(!isInPast);

        timer.Start();

        onTimeWarp.Invoke();

        TriggerTeleportEffect();
    }

    private void TriggerTeleportEffect()
    {
        if (warpSound)
        {
            warpSound.Play();
        }
        cromAb.intensity.value = 1;
        bloom.intensity.value = 1;
        exposure.compensation.value = 5;
    }

    private void ChangeWorldInternal(bool newIsInPresent)
    {
        isInPast = newIsInPresent;
        Globals.isInPast = newIsInPresent;
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

public class CustomTimer
{
    private float currentTime = 0;

    private bool isActive = false;

    public void Count()
    {
        if (isActive)
        {
            currentTime += Time.deltaTime;
        }
    }

    public void Start()
    {
        isActive = true;
    }

    public void Stop()
    {
        isActive = false;
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public void Reset()
    {
        currentTime = 0;
    }
}