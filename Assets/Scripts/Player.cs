using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.Input;
using UnityEngine;

namespace FarmGame.Agent {
    public class Player : MonoBehaviour {

        [SerializeField] private AgentMover agentMover;
        [SerializeField] private FarmPlayerInput farmPlayerInput;
        [SerializeField] private AgentAnimation agentAnimation;

        private void Awake() {
            farmPlayerInput.OnMoveInputValueChange += FarmPlayerInput_OnMove;
            agentMover.OnMoveStateChanged += agentAnimation.SetMoving;
        }

        private void FarmPlayerInput_OnMove(object sender, FarmPlayerInput.OnMoveInputAgrs e) {
            agentMover.SetMoveVector(e.moveValue);
            agentAnimation.ChangeDirection(e.moveValue);

            //nếu call như này thì không cần dùng event nhưng player sẽ quản lý là chỗ quản lý bool isMoving
            // agentAnimation.SetMoving(e.moveInputValue.magnitude > 0.1f);
        }

        private void OnDisable() {
            farmPlayerInput.OnMoveInputValueChange -= FarmPlayerInput_OnMove;
            agentMover.OnMoveStateChanged -= agentAnimation.SetMoving;
        }
    }
}
