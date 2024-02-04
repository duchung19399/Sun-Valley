using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.Input;
using FarmGame.Interact;
using UnityEngine;

namespace FarmGame.Agent {
    public class Player : MonoBehaviour {

        [SerializeField] private AgentMover agentMover;
        [SerializeField] private FarmPlayerInput farmPlayerInput;
        [SerializeField] private AgentAnimation agentAnimation;

        [SerializeField] private InteractionDetector interactionDetector;

        private void Awake() {
            farmPlayerInput.OnMoveInputValueChange += FarmPlayerInput_OnMove;
            farmPlayerInput.OnInteractInput += FarmPlayerInput_Interact;
            agentMover.OnMoveStateChanged += agentAnimation.SetMoving;

        }

        private void FarmPlayerInput_Interact(object sender, EventArgs e) {
            foreach (PickUpInteraction item in interactionDetector.PerformDetection()) {
                if (item.CanInteract()) {
                    agentMover.Stopped = true;
                    agentAnimation.OnAnimationEnd.AddListener(() => {
                        item.Interact(this);
                        agentMover.Stopped = false;
                    });
                    agentAnimation.PlayAnimation(AnimationType.PickUp);
                }
            }
        }

        private void FarmPlayerInput_OnMove(object sender, FarmPlayerInput.OnMoveInputAgrs e) {
            agentMover.SetMoveVector(e.moveValue);
            agentAnimation.ChangeDirection(e.moveValue);
            interactionDetector.SetInteractDirection(e.moveValue);

            //nếu call như này thì không cần dùng event nhưng player sẽ quản lý là chỗ quản lý bool isMoving
            // agentAnimation.SetMoving(e.moveInputValue.magnitude > 0.1f);
        }

        private void OnDisable() {
            farmPlayerInput.OnMoveInputValueChange -= FarmPlayerInput_OnMove;
            agentMover.OnMoveStateChanged -= agentAnimation.SetMoving;
        }
    }
}
