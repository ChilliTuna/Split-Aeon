using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputActions
{
    MoveForward,
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
    Sprint
}

public class KeyRebinder : MonoBehaviour
{
    private UserActions userActions;

    private Dictionary<InputActions, InputAction> dictionary;

    public InputActions inputActions;

    private InputAction thisAction;

    public TextMeshProUGUI associatedText;

    private bool hasFoundAction = false;

    public bool isComposite = false;

    public int index = 0;

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
    }

    private void Update()
    {
        if (isComposite)
        {
            UpdateCompositeRebindText();
        }
        else
        {
            UpdateText();
        }
    }

    public void RebindKey()
    {
        if (hasFoundAction)
        {
            if (isComposite)
            {
                RebindComposite();
            }
            else
            {
            rebindingOperation = thisAction.PerformInteractiveRebinding().OnMatchWaitForAnother(0.1f).OnComplete(operation => FinaliseRebind()).Start();
            }
        }
    }

    private void RebindComposite()
    {
        rebindingOperation = thisAction.PerformInteractiveRebinding(thisAction.GetBindingIndex() + 1 + index).OnMatchWaitForAnother(0.1f).OnComplete(operation => FinaliseRebind()).Start();
    }

    private void WriteDictionary()
    {
        dictionary = new Dictionary<InputActions, InputAction>();
        dictionary.Add(InputActions.MoveForward, userActions.PlayerMap.MoveForward);
        dictionary.Add(InputActions.MoveRight, userActions.PlayerMap.MoveRight);
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
    }

    private void FinaliseRebind()
    {
        SaveBinding();
        rebindingOperation.Dispose();
    }

    private void UpdateText()
    {
        control = InputSystem.FindControl(thisAction.bindings[thisAction.GetBindingIndex()].effectivePath);
        if (control.shortDisplayName != null)
        {
            associatedText.text = control.shortDisplayName;
        }
        else
        {
            associatedText.text = control.displayName;
        }
    }

    private void UpdateCompositeRebindText()
    {
        control = InputSystem.FindControl(thisAction.bindings[thisAction.GetBindingIndex() + 1 + index].effectivePath);
        if (control.shortDisplayName != null)
        {
            associatedText.text = control.shortDisplayName;
        }
        else
        {
            associatedText.text = control.displayName;
        }
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