using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FarmGame.Input {
    public class FarmPlayerInput : MonoBehaviour {
        private const string ACTION_PLAYER_MOVEMENT = "Player/Movement";
        private const string ACTION_PLAYER_INTERACT = "Player/Interact";

        [SerializeField] private PlayerInput playerInput;
        [field: SerializeField]
        public Vector2 MoveInputValue { get; private set; }

        public event EventHandler<OnMoveInputAgrs> OnMoveInputValueChange;
        public class OnMoveInputAgrs {
            public Vector2 moveValue;
        }

        public event EventHandler OnInteractInput;

        private void OnEnable() {
            playerInput.actions[ACTION_PLAYER_MOVEMENT].performed += Move;
            playerInput.actions[ACTION_PLAYER_MOVEMENT].canceled += Move;
            playerInput.actions[ACTION_PLAYER_INTERACT].performed += Interact;
        }

        private void Move(InputAction.CallbackContext context) {
            MoveInputValue = context.ReadValue<Vector2>();
            OnMoveInputValueChange?.Invoke(this, new OnMoveInputAgrs { moveValue = MoveInputValue });
        }

        private void Interact(InputAction.CallbackContext context) {
            OnInteractInput?.Invoke(this, EventArgs.Empty);
        }

        private void OnDisable() {
            playerInput.actions[ACTION_PLAYER_MOVEMENT].performed -= Move;
            playerInput.actions[ACTION_PLAYER_MOVEMENT].canceled -= Move;
            playerInput.actions[ACTION_PLAYER_INTERACT].performed -= Interact;
        }
    }


}
