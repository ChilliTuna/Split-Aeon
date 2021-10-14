using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputActions
{
    MoveForward,
    MoveBackward,
    MoveLeft,
    MoveRight,
    Jump,
    Shoot,
    TimeTravel,
    ThrowCard,
    Interact,
    ChangeToRevolver,
    ChangeToThompson,
    ChangeToShotgun,
    ChangeToMelee,
    WeaponWheel,
    Reload,
    Sprint,
    Pause
}

public class KeyRebinder : MonoBehaviour
{
    private UserActions userActions;

    private Dictionary<InputActions, InputAction> dictionary;

    public InputActions inputActions;

    private InputAction thisAction;

    public TextMeshProUGUI associatedText;

    private bool hasFoundAction = false;

    private List<InputActions> specialBinds = new List<InputActions> { InputActions.MoveForward, InputActions.MoveBackward, InputActions.MoveLeft, InputActions.MoveRight, InputActions.Sprint };

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private InputControl control;

    private void Awake()
    {
        userActions = new UserActions();
        WriteDictionary();
        hasFoundAction = dictionary.TryGetValue(inputActions, out thisAction);
    }

    private void Start()
    {
        LoadBinding();
        if (hasFoundAction)
        {
            StartCoroutine(UpdateText());
        }
    }

    public void RebindKey()
    {
        if (hasFoundAction)
        {
            if (specialBinds.Contains(inputActions))
            {
                SpecialRebind();
                StartCoroutine(UpdateSpecialRebindText());
                SaveBinding();
                return;
            }
            rebindingOperation = thisAction.PerformInteractiveRebinding().OnComplete(operation => DisposeRebindingOperation()).Start();
            StartCoroutine(UpdateText());
        }
        SaveBinding();
    }

    private void SpecialRebind()
    {
        if (inputActions == InputActions.MoveForward)
        {
        }
    }

    private void WriteDictionary()
    {
        dictionary = new Dictionary<InputActions, InputAction>();
        dictionary.Add(InputActions.MoveForward, userActions.PlayerMap.MoveForward);
        dictionary.Add(InputActions.MoveBackward, userActions.PlayerMap.MoveForward);
        dictionary.Add(InputActions.MoveLeft, userActions.PlayerMap.MoveForward);
        dictionary.Add(InputActions.MoveRight, userActions.PlayerMap.MoveForward);
        dictionary.Add(InputActions.Jump, userActions.PlayerMap.Jump);
        dictionary.Add(InputActions.Shoot, userActions.PlayerMap.Shoot);
        dictionary.Add(InputActions.TimeTravel, userActions.PlayerMap.TimeTravel);
        dictionary.Add(InputActions.ThrowCard, userActions.PlayerMap.ThrowCard);
        dictionary.Add(InputActions.Interact, userActions.PlayerMap.Interact);
        dictionary.Add(InputActions.ChangeToRevolver, userActions.PlayerMap.ChangeToRevolver);
        dictionary.Add(InputActions.ChangeToThompson, userActions.PlayerMap.ChangeToThompson);
        dictionary.Add(InputActions.ChangeToShotgun, userActions.PlayerMap.ChangeToShotgun);
        dictionary.Add(InputActions.ChangeToMelee, userActions.PlayerMap.ChangeToMelee);
        dictionary.Add(InputActions.WeaponWheel, userActions.PlayerMap.WeaponWheel);
        dictionary.Add(InputActions.Reload, userActions.PlayerMap.Reload);
        dictionary.Add(InputActions.Sprint, userActions.PlayerMap.Sprint);
        dictionary.Add(InputActions.Pause, userActions.PlayerMap.Pause);
    }

    private void DisposeRebindingOperation()
    {
        rebindingOperation.Dispose();
    }

    private IEnumerator UpdateText()
    {
        yield return new WaitForSeconds(0.1f);
        control = InputSystem.FindControl(thisAction.bindings[thisAction.GetBindingIndex()].effectivePath);
        if (control.shortDisplayName != null)
        {
            associatedText.text = control.shortDisplayName;
        }
        else
        {
            associatedText.text = control.displayName;
        }
        yield return null;
    }

    private IEnumerator UpdateSpecialRebindText()
    {
        yield return new WaitForSeconds(0.1f);
        //control = InputSystem.FindControl(thisAction.bindings[thisAction.GetBindingIndex()].effectivePath);
        //if (control.shortDisplayName != null)
        //{
        //    associatedText.text = control.shortDisplayName;
        //}
        //else
        //{
        //    associatedText.text = control.displayName;
        //}
        yield return null;
    }

    private void SaveBinding()
    {
        string bindings = thisAction.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString("Controls:" + inputActions.ToString(), bindings);
    }

    private void LoadBinding()
    {
        string controls = PlayerPrefs.GetString("Controls:" + inputActions.ToString(), string.Empty);

        if (!string.IsNullOrEmpty(controls))
        {
            thisAction.LoadBindingOverridesFromJson(controls);
        }
    }
}

public static class BindingLoader
{
    public static void LoadBinding(this InputAction iAction, InputActions key)
    {
        string controls = PlayerPrefs.GetString("Controls:" + key.ToString(), string.Empty);

        if (!string.IsNullOrEmpty(controls))
        {
            iAction.LoadBindingOverridesFromJson(controls);
        }
    }
}