using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FarmGame.Input {
    public class FarmPlayerInput : MonoBehaviour {
        private const string ACTION_PLAYER_MOVEMENT = "Player/Movement";

        [SerializeField] private PlayerInput playerInput;
        [field: SerializeField]
        public Vector2 MoveInputValue { get; private set; }

        public event EventHandler<OnMoveInputAgrs> OnMoveInputValueChange;
        public class OnMoveInputAgrs {
            public Vector2 moveValue;
        }

        private void OnEnable() {
            playerInput.actions[ACTION_PLAYER_MOVEMENT].performed += Move;
            playerInput.actions[ACTION_PLAYER_MOVEMENT].canceled += Move;
        }

        private void Move(InputAction.CallbackContext context) {
            MoveInputValue = context.ReadValue<Vector2>();
            OnMoveInputValueChange?.Invoke(this, new OnMoveInputAgrs { moveValue = MoveInputValue });
        }

        private void OnDisable() {
            playerInput.actions[ACTION_PLAYER_MOVEMENT].performed -= Move;
            playerInput.actions[ACTION_PLAYER_MOVEMENT].canceled -= Move;
        }
    }


}
