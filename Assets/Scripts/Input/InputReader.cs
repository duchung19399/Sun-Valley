using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]

public class InputReader : ScriptableObject, GameInput.IPlayerActions {

    private GameInput _gameInput;

    public event UnityAction<Vector2> MoveEvent = delegate { };
    public event UnityAction InteractEvent = delegate { };

    public event UnityAction SwapToolEvent = delegate { };

    private void OnEnable() {
        if (_gameInput == null) {
            _gameInput = new GameInput();
            _gameInput.Player.SetCallbacks(this);
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
    }

    private void DisableAllInput() {
        _gameInput.Player.Disable();
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            InteractEvent.Invoke();
        }
    }

    public void OnMovement(InputAction.CallbackContext context) {
        MoveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSwapTool(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            SwapToolEvent.Invoke();
        }
    }
}

