//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.1.1
//     from Assets/_SplitAeon/_Prefabs/Management/UserActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @UserActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @UserActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""UserActions"",
    ""maps"": [
        {
            ""name"": ""PlayerMap"",
            ""id"": ""540620ae-95d0-4d2d-8a3b-7408705aef84"",
            ""actions"": [
                {
                    ""name"": ""MoveForward"",
                    ""type"": ""Button"",
                    ""id"": ""62df7fa2-4db8-4e8b-8855-46359c067e52"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveRight"",
                    ""type"": ""Button"",
                    ""id"": ""a0adcf77-1e6a-4ddf-b895-6b70fd03f17b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""3c44863a-7235-431b-a9d7-3382cf34e232"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""c6ee5f24-1f32-4b9c-97d2-15abdf82eae8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""TimeTravel"",
                    ""type"": ""Button"",
                    ""id"": ""a34f9f5a-882a-47d4-bbb4-9c0d9ac260cd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ThrowCard"",
                    ""type"": ""Button"",
                    ""id"": ""4bf0c22e-9ad9-4645-babd-b4987462fd92"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""2d76976c-323e-4a64-9a68-c324f1b3345d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeToRevolver"",
                    ""type"": ""Button"",
                    ""id"": ""23d4197e-40a0-4915-8684-74bf15723057"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeToThompson"",
                    ""type"": ""Button"",
                    ""id"": ""dd06e90f-784c-4905-b369-7f1db0a35198"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeToShotgun"",
                    ""type"": ""Button"",
                    ""id"": ""b34df450-32c2-40b0-8b4f-8c7b697a5020"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeToMelee"",
                    ""type"": ""Button"",
                    ""id"": ""063c1d80-bdc6-4ab7-959a-a9f11a045beb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""WeaponWheel"",
                    ""type"": ""Button"",
                    ""id"": ""a48da3a6-aaac-4f39-81e4-27960de70335"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""c70d084d-fc76-442b-8e96-0fe46d1ba210"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""08624768-7734-432a-b26c-36232e56d50c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""de0479c7-9a42-433c-82ed-e3ae49628490"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""MoveForward"",
                    ""id"": ""094b7b9e-7b04-4668-a694-3e2ad1eebbca"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveForward"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b748ccf3-671e-4bdc-82ba-70e0315a737a"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveForward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""92179b0f-203f-4d71-b491-5180fd631d50"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveForward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""653b2b32-9690-4c5a-bdbb-9d6ece54ae7b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""MoveRight"",
                    ""id"": ""084cedff-0bc1-4249-9452-e1493f8e1f42"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRight"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""f33c8051-3ef6-44bf-9bbb-77ceebda4cc3"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""19b1d218-f548-4eb2-883d-ff0dafb81afd"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""02cdd390-7d96-47ca-97f1-ff08b5794a3d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""caa92002-6046-4828-8741-6627cb3f267c"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TimeTravel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf21d07a-4b19-48ae-9d91-66a99ef0b569"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowCard"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""84e603c4-269c-40c4-9347-5552ebd6f038"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b9ba5907-8576-473c-a817-31da0a915f80"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeToRevolver"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""94cb3fa4-abf7-4b71-b105-5c50c3415800"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeToThompson"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f49ae23f-e882-4332-8527-5a9ca7b42e17"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeToShotgun"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2b66551f-95f5-4c21-9b42-001b964924c0"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeToMelee"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3c3f7dc0-065a-490a-bf67-429efde67a56"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""WeaponWheel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0f49e97e-da43-4276-915d-cc39f67e52b9"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""376f0194-375a-47f0-95fa-265565341706"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cba1f28e-a09d-4cb9-9d90-6b38a57e349c"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerMap
        m_PlayerMap = asset.FindActionMap("PlayerMap", throwIfNotFound: true);
        m_PlayerMap_MoveForward = m_PlayerMap.FindAction("MoveForward", throwIfNotFound: true);
        m_PlayerMap_MoveRight = m_PlayerMap.FindAction("MoveRight", throwIfNotFound: true);
        m_PlayerMap_Jump = m_PlayerMap.FindAction("Jump", throwIfNotFound: true);
        m_PlayerMap_Shoot = m_PlayerMap.FindAction("Shoot", throwIfNotFound: true);
        m_PlayerMap_TimeTravel = m_PlayerMap.FindAction("TimeTravel", throwIfNotFound: true);
        m_PlayerMap_ThrowCard = m_PlayerMap.FindAction("ThrowCard", throwIfNotFound: true);
        m_PlayerMap_Interact = m_PlayerMap.FindAction("Interact", throwIfNotFound: true);
        m_PlayerMap_ChangeToRevolver = m_PlayerMap.FindAction("ChangeToRevolver", throwIfNotFound: true);
        m_PlayerMap_ChangeToThompson = m_PlayerMap.FindAction("ChangeToThompson", throwIfNotFound: true);
        m_PlayerMap_ChangeToShotgun = m_PlayerMap.FindAction("ChangeToShotgun", throwIfNotFound: true);
        m_PlayerMap_ChangeToMelee = m_PlayerMap.FindAction("ChangeToMelee", throwIfNotFound: true);
        m_PlayerMap_WeaponWheel = m_PlayerMap.FindAction("WeaponWheel", throwIfNotFound: true);
        m_PlayerMap_Reload = m_PlayerMap.FindAction("Reload", throwIfNotFound: true);
        m_PlayerMap_Sprint = m_PlayerMap.FindAction("Sprint", throwIfNotFound: true);
        m_PlayerMap_Pause = m_PlayerMap.FindAction("Pause", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerMap
    private readonly InputActionMap m_PlayerMap;
    private IPlayerMapActions m_PlayerMapActionsCallbackInterface;
    private readonly InputAction m_PlayerMap_MoveForward;
    private readonly InputAction m_PlayerMap_MoveRight;
    private readonly InputAction m_PlayerMap_Jump;
    private readonly InputAction m_PlayerMap_Shoot;
    private readonly InputAction m_PlayerMap_TimeTravel;
    private readonly InputAction m_PlayerMap_ThrowCard;
    private readonly InputAction m_PlayerMap_Interact;
    private readonly InputAction m_PlayerMap_ChangeToRevolver;
    private readonly InputAction m_PlayerMap_ChangeToThompson;
    private readonly InputAction m_PlayerMap_ChangeToShotgun;
    private readonly InputAction m_PlayerMap_ChangeToMelee;
    private readonly InputAction m_PlayerMap_WeaponWheel;
    private readonly InputAction m_PlayerMap_Reload;
    private readonly InputAction m_PlayerMap_Sprint;
    private readonly InputAction m_PlayerMap_Pause;
    public struct PlayerMapActions
    {
        private @UserActions m_Wrapper;
        public PlayerMapActions(@UserActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveForward => m_Wrapper.m_PlayerMap_MoveForward;
        public InputAction @MoveRight => m_Wrapper.m_PlayerMap_MoveRight;
        public InputAction @Jump => m_Wrapper.m_PlayerMap_Jump;
        public InputAction @Shoot => m_Wrapper.m_PlayerMap_Shoot;
        public InputAction @TimeTravel => m_Wrapper.m_PlayerMap_TimeTravel;
        public InputAction @ThrowCard => m_Wrapper.m_PlayerMap_ThrowCard;
        public InputAction @Interact => m_Wrapper.m_PlayerMap_Interact;
        public InputAction @ChangeToRevolver => m_Wrapper.m_PlayerMap_ChangeToRevolver;
        public InputAction @ChangeToThompson => m_Wrapper.m_PlayerMap_ChangeToThompson;
        public InputAction @ChangeToShotgun => m_Wrapper.m_PlayerMap_ChangeToShotgun;
        public InputAction @ChangeToMelee => m_Wrapper.m_PlayerMap_ChangeToMelee;
        public InputAction @WeaponWheel => m_Wrapper.m_PlayerMap_WeaponWheel;
        public InputAction @Reload => m_Wrapper.m_PlayerMap_Reload;
        public InputAction @Sprint => m_Wrapper.m_PlayerMap_Sprint;
        public InputAction @Pause => m_Wrapper.m_PlayerMap_Pause;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMap; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMapActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerMapActions instance)
        {
            if (m_Wrapper.m_PlayerMapActionsCallbackInterface != null)
            {
                @MoveForward.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnMoveForward;
                @MoveForward.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnMoveForward;
                @MoveForward.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnMoveForward;
                @MoveRight.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnMoveRight;
                @MoveRight.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnMoveRight;
                @MoveRight.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnMoveRight;
                @Jump.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnJump;
                @Shoot.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnShoot;
                @TimeTravel.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnTimeTravel;
                @TimeTravel.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnTimeTravel;
                @TimeTravel.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnTimeTravel;
                @ThrowCard.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnThrowCard;
                @ThrowCard.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnThrowCard;
                @ThrowCard.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnThrowCard;
                @Interact.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnInteract;
                @ChangeToRevolver.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnChangeToRevolver;
                @ChangeToRevolver.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnChangeToRevolver;
                @ChangeToRevolver.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnChangeToRevolver;
                @ChangeToThompson.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnChangeToThompson;
                @ChangeToThompson.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnChangeToThompson;
                @ChangeToThompson.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnChangeToThompson;
                @ChangeToShotgun.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnChangeToShotgun;
                @ChangeToShotgun.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnChangeToShotgun;
                @ChangeToShotgun.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnChangeToShotgun;
                @ChangeToMelee.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnChangeToMelee;
                @ChangeToMelee.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnChangeToMelee;
                @ChangeToMelee.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnChangeToMelee;
                @WeaponWheel.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnWeaponWheel;
                @WeaponWheel.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnWeaponWheel;
                @WeaponWheel.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnWeaponWheel;
                @Reload.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnReload;
                @Sprint.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnSprint;
                @Pause.started -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_PlayerMapActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_PlayerMapActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveForward.started += instance.OnMoveForward;
                @MoveForward.performed += instance.OnMoveForward;
                @MoveForward.canceled += instance.OnMoveForward;
                @MoveRight.started += instance.OnMoveRight;
                @MoveRight.performed += instance.OnMoveRight;
                @MoveRight.canceled += instance.OnMoveRight;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @TimeTravel.started += instance.OnTimeTravel;
                @TimeTravel.performed += instance.OnTimeTravel;
                @TimeTravel.canceled += instance.OnTimeTravel;
                @ThrowCard.started += instance.OnThrowCard;
                @ThrowCard.performed += instance.OnThrowCard;
                @ThrowCard.canceled += instance.OnThrowCard;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
                @ChangeToRevolver.started += instance.OnChangeToRevolver;
                @ChangeToRevolver.performed += instance.OnChangeToRevolver;
                @ChangeToRevolver.canceled += instance.OnChangeToRevolver;
                @ChangeToThompson.started += instance.OnChangeToThompson;
                @ChangeToThompson.performed += instance.OnChangeToThompson;
                @ChangeToThompson.canceled += instance.OnChangeToThompson;
                @ChangeToShotgun.started += instance.OnChangeToShotgun;
                @ChangeToShotgun.performed += instance.OnChangeToShotgun;
                @ChangeToShotgun.canceled += instance.OnChangeToShotgun;
                @ChangeToMelee.started += instance.OnChangeToMelee;
                @ChangeToMelee.performed += instance.OnChangeToMelee;
                @ChangeToMelee.canceled += instance.OnChangeToMelee;
                @WeaponWheel.started += instance.OnWeaponWheel;
                @WeaponWheel.performed += instance.OnWeaponWheel;
                @WeaponWheel.canceled += instance.OnWeaponWheel;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public PlayerMapActions @PlayerMap => new PlayerMapActions(this);
    public interface IPlayerMapActions
    {
        void OnMoveForward(InputAction.CallbackContext context);
        void OnMoveRight(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
        void OnTimeTravel(InputAction.CallbackContext context);
        void OnThrowCard(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnChangeToRevolver(InputAction.CallbackContext context);
        void OnChangeToThompson(InputAction.CallbackContext context);
        void OnChangeToShotgun(InputAction.CallbackContext context);
        void OnChangeToMelee(InputAction.CallbackContext context);
        void OnWeaponWheel(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
}
