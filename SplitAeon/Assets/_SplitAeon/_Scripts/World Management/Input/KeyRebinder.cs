﻿using System.Collections;
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

    private List<InputActions> specialBinds = new List<InputActions> { InputActions.MoveForward, InputActions.MoveRight };

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
        if (specialBinds.Contains(inputActions))
        {
            UpdateSpecialRebindText();
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
            if (specialBinds.Contains(inputActions))
            {
                SpecialRebind();
                return;
            }
            rebindingOperation = thisAction.PerformInteractiveRebinding().OnComplete(operation => FinaliseRebind()).Start();
        }
    }

    private void SpecialRebind()
    {
        for (int i = 1; i < 3; i++)
        {
            rebindingOperation = thisAction.PerformInteractiveRebinding(thisAction.GetBindingIndex() + i).OnComplete(operation => FinaliseRebind()).Start();
        }
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
        dictionary.Add(InputActions.Pause, userActions.PlayerMap.Pause);
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

    private void UpdateSpecialRebindText()
    {
        InputControl con1 = InputSystem.FindControl(thisAction.bindings[thisAction.GetBindingIndex() + 1].effectivePath);
        InputControl con2 = InputSystem.FindControl(thisAction.bindings[thisAction.GetBindingIndex() + 2].effectivePath);
        string outText = "";
        if (con1.shortDisplayName != null)
        {
            outText = con1.shortDisplayName;
        }
        else
        {
            outText = con1.displayName;
        }
        outText += " / ";
        if (con2.shortDisplayName != null)
        {
            outText += con2.shortDisplayName;
        }
        else
        {
            outText += con2.displayName;
        }
        associatedText.text = outText;
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