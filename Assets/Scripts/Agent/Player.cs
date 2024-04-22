using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.Interact;
using FarmGame.Tools;
using UnityEngine;

namespace FarmGame.Agent {
    public class Player : MonoBehaviour {


        [SerializeField] private InputReader _inputReader;

        [SerializeField] private AgentMover _agentMover;
        [SerializeField] private AgentAnimation _agentAnimation;
        [SerializeField] private InteractionDetector _interactionDetector;

        private Tool _selectedTool = new HandTool(ToolType.Hand);

        public AgentMover AgentMover { get => _agentMover; }
        public AgentAnimation AgentAnimation { get => _agentAnimation; }
        public InteractionDetector InteractionDetector { get => _interactionDetector; }
        public Tool SelectedTool { get => _selectedTool; }

        private bool _blockedInput = false;
        public bool BlockedInput {
            get => _blockedInput;
            set {
                _blockedInput = value;
                _agentMover.Stopped = value;
                _inputReader.SetBlockPlayerInput(value);
            }
        }

        private void OnEnable() {
            _inputReader.MoveEvent += OnMove;
            _inputReader.InteractEvent += Interact;
        }

        private void Awake() {
            _agentMover.OnMoveStateChanged += _agentAnimation.SetMoving;

        }

        private void Interact() {
            _selectedTool.UseTool(this);
        }

        private void OnMove(Vector2 moveValue) {
            _agentMover.SetMoveVector(moveValue);
            _agentAnimation.ChangeDirection(moveValue);
            _interactionDetector.SetInteractDirection(moveValue);

            //nếu call như này thì không cần dùng event nhưng player sẽ quản lý là chỗ quản lý bool isMoving
            // agentAnimation.SetMoving(e.moveInputValue.magnitude > 0.1f);
        }

        private void OnDisable() {
            _agentMover.OnMoveStateChanged -= _agentAnimation.SetMoving;
        }
    }
}
