using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWheelController : MonoBehaviour
{
    private UserActions userActions;

    public bool isOpen = false;

    public Animator cardsButtons;
    public Animator weaponsButtons;

    public Animator topBar;
    public Animator bottomBar;

    public Animator mask;

    public CanvasGroup wheel;

    Player player;

    private void Awake()
    {
        userActions = new UserActions();
    }

    private void OnEnable()
    {
        userActions.PlayerMap.WeaponWheel.LoadBinding(InputActions.WeaponWheel);
        userActions.PlayerMap.WeaponWheel.performed += ctx => ToggleWeaponWheel();
        userActions.PlayerMap.WeaponWheel.Enable();
    }

    private void OnDisable()
    {
        userActions.PlayerMap.WeaponWheel.Disable();
    }

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        player.lockMouse = isOpen;
    }

    public void OpenWeaponWheel()
    {
        isOpen = true;

        CheckWheelState();
    }

    public void CloseWeaponWheel()
    {
        isOpen = false;

        CheckWheelState();
    }

    void CheckWheelState()
    {
        if (isOpen)
        {
            player.isBusy = true;

            cardsButtons.SetBool("isOpen", true);
            weaponsButtons.SetBool("isOpen", true);

            topBar.SetBool("isOpen", true);
            bottomBar.SetBool("isOpen", true);

            mask.SetBool("isOpen", true);

            FadeIn();

            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            player.isBusy = false;

            cardsButtons.SetBool("isOpen", false);
            weaponsButtons.SetBool("isOpen", false);

            topBar.SetBool("isOpen", false);
            bottomBar.SetBool("isOpen", false);

            mask.SetBool("isOpen", false);

            FadeOut();

            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void ToggleWeaponWheel()
    {
        if (isOpen)
        {
            CloseWeaponWheel();
        }
        else if (!player.isBusy)
        {
            OpenWeaponWheel();
        }
    }

    public void FadeIn()
    {
        StartCoroutine(FadeGroupIn(0.2f));
        Invoke("Show", 0.2f);
    }

    public void FadeOut()
    {
        StartCoroutine(FadeGroupOut(0.5f));
        Invoke("Hide", 0.5f);
    }

    public void Show()
    {
        wheel.alpha = 1;
    }

    public void Hide()
    {
        wheel.alpha = 0;
    }

    public IEnumerator FadeGroupIn(float time)
    {
        wheel.alpha = 0;

        while (wheel.alpha < 1.0f)
        {
            wheel.alpha += (Time.deltaTime / time);
            yield return null;
        }
    }

    public IEnumerator FadeGroupOut(float time)
    {
        wheel.alpha = 1;

        while (wheel.alpha > 0.0f)
        {
            wheel.alpha -= (Time.deltaTime / time);
            yield return null;
        }
    }
}
