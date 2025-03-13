using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.DataStorage.Inventory;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]

public class InputReader : ScriptableObject, GameInput.IPlayerActions, GameInput.IUIActions {

    private GameInput _gameInput;

    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction InteractEvent = delegate { };

    public event UnityAction SwapToolEvent = delegate { };
    public event UnityAction ToggleInventoryEvent = delegate { };


    /**
        * This event is used to notify the UI input
        */
    public event UnityAction UIExitEvent = delegate { };
    public event UnityAction UICloseInventoryEvent = delegate { };
    public event UnityAction UIInteractEvent = delegate { };
    public event UnityAction<Vector2> UIMoveEvent = delegate { };



    private void OnEnable() {
        if (_gameInput == null) {
            _gameInput = new GameInput();
            _gameInput.Player.SetCallbacks(this);
            _gameInput.UI.SetCallbacks(this);
        }
        _gameInput.Player.Enable();

    }

    private void OnDisable() {
        DisableAllInput();
    }


    public void DisablePlayerInput() {
        _gameInput.Player.Disable();
    }

    public void SetBlockPlayerInput(bool value) {
        if (value) {
            _gameInput.Player.Disable();
        } else {
            _gameInput.Player.Enable();
        }
    }
    public void EnablePlayerInput() {
        _gameInput.Player.Enable();
        _gameInput.UI.Disable();
    }
    public void EnableUIInput() {
        _gameInput.UI.Enable();
        _gameInput.Player.Disable();
    }

    private void DisableAllInput() {
        _gameInput.Player.Disable();
        _gameInput.UI.Disable();
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            InteractEvent?.Invoke();
        }
    }

    public void OnMovement(InputAction.CallbackContext context) {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSwapTool(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            SwapToolEvent?.Invoke();
        }
    }

    public void OnToggleInventory(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            ToggleInventoryEvent?.Invoke();
        }
    }

    public void OnNavigate(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            UIMoveEvent?.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnSubmit(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            UIInteractEvent?.Invoke();
        }
    }

    public void OnExit(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            UIExitEvent?.Invoke();
        }
    }

    public void OnCloseInventory(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            UICloseInventoryEvent?.Invoke();
        }
    }
}

