using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance;

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public PlayerInputActions playerInputActions;

    public enum Binding
    {
        Move_UP,
        Move_DOWN,
        Move_LEFT,
        Move_RIGHT,
        Interact,
        InteactAlternate,
        Pause,
        GamepadInteract,
        GamepadInteractAlt,
        GamepadPause
    }

    private const string PLAYER_PREFS_BINDING = "InputBinding";


    private void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDING))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDING));
        }

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }


    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (Gamemanager.Instance.IsGamePlaying())
            OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (Gamemanager.Instance.IsGamePlaying())
            OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_UP:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_DOWN:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_LEFT:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_RIGHT:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteactAlternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinging(Binding binding, Action onActionRebound)
    {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingindex = 0;

        switch (binding)
        {
            default:
            case Binding.Move_UP:
                inputAction = playerInputActions.Player.Move;
                bindingindex = 1;
                break;
            case Binding.Move_DOWN:
                inputAction = playerInputActions.Player.Move;
                bindingindex = 2;
                break;
            case Binding.Move_LEFT:
                inputAction = playerInputActions.Player.Move;
                bindingindex = 3;
                break;
            case Binding.Move_RIGHT:
                inputAction = playerInputActions.Player.Move;
                bindingindex = 4;
                break;
            case Binding.Interact:
                inputAction = playerInputActions.Player.Interact;
                bindingindex = 0;
                break;
            case Binding.InteactAlternate:
                inputAction = playerInputActions.Player.InteractAlternate;
                bindingindex = 0;
                break;
            case Binding.Pause:
                inputAction = playerInputActions.Player.Pause;
                bindingindex = 0;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingindex).OnComplete(callback =>
        {
            callback.Dispose();
            playerInputActions.Player.Enable();
            onActionRebound();

            PlayerPrefs.SetString(PLAYER_PREFS_BINDING, playerInputActions.SaveBindingOverridesAsJson());
            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        }).Start();
    }
}
