using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.Input;
using FarmGame.Interact;
using FarmGame.Tools;
using Unity.VisualScripting;
using UnityEngine;

namespace FarmGame.Agent {
    public class Player : MonoBehaviour {

        [SerializeField] private AgentMover _agentMover;
        [SerializeField] private FarmPlayerInput _farmPlayerInput;
        [SerializeField] private AgentAnimation _agentAnimation;
        [SerializeField] private InteractionDetector _interactionDetector;

        private Tool _selectedTool = new HandTool(ToolType.Hand);

        public AgentMover AgentMover { get => _agentMover; }
        public FarmPlayerInput FarmPlayerInput { get => _farmPlayerInput; }
        public AgentAnimation AgentAnimation { get => _agentAnimation; }
        public InteractionDetector InteractionDetector { get => _interactionDetector; }
        public Tool SelectedTool { get => _selectedTool; }


        private void Awake() {
            _farmPlayerInput.OnMoveInputValueChange += FarmPlayerInput_OnMove;
            _farmPlayerInput.OnInteractInput += FarmPlayerInput_Interact;
            _agentMover.OnMoveStateChanged += _agentAnimation.SetMoving;

        }

        private void FarmPlayerInput_Interact(object sender, EventArgs e) {
            _selectedTool.UseTool(this);


        }

        private void FarmPlayerInput_OnMove(object sender, FarmPlayerInput.OnMoveInputAgrs e) {
            _agentMover.SetMoveVector(e.moveValue);
            _agentAnimation.ChangeDirection(e.moveValue);
            _interactionDetector.SetInteractDirection(e.moveValue);

            //nếu call như này thì không cần dùng event nhưng player sẽ quản lý là chỗ quản lý bool isMoving
            // agentAnimation.SetMoving(e.moveInputValue.magnitude > 0.1f);
        }

        private void OnDisable() {
            _farmPlayerInput.OnMoveInputValueChange -= FarmPlayerInput_OnMove;
            _agentMover.OnMoveStateChanged -= _agentAnimation.SetMoving;
        }
    }
}
