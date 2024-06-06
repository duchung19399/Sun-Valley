using System;
using FarmGame.Farming;
using FarmGame.Interact;
using FarmGame.Tools;
using UnityEngine;

namespace FarmGame.Agent {
    public class Player : MonoBehaviour, IAgent {


        [SerializeField] private InputReader _inputReader;

        [SerializeField] private AgentMover _agentMover;
        [SerializeField] private AgentAnimation _agentAnimation;
        [SerializeField] private InteractionDetector _interactionDetector;

        [SerializeField] private RuntimeAnimatorController _hoeAnimatorController;

        private Tool _selectedTool = new HoeTool(ToolType.Hoe);

        public InputReader InputReader { get => _inputReader; }
        public AgentMover AgentMover { get => _agentMover; }
        public AgentAnimation AgentAnimation { get => _agentAnimation; }
        public InteractionDetector InteractionDetector { get => _interactionDetector; }
        public Tool SelectedTool { get => _selectedTool; }
        [SerializeField]
        private FieldDetector _fieldDetector;
        public FieldDetector FieldDetectorObject {
            get => _fieldDetector;
        }

        [SerializeField] private FieldController _fieldController;

        private bool _blockedInput = false;
        public bool BlockedInput {
            get => _blockedInput;
            set {
                _blockedInput = value;
                _agentMover.Stopped = value;
                _inputReader.SetBlockPlayerInput(value);
            }
        }


        public FieldController FieldController => _fieldController;

        private void OnEnable() {
            _inputReader.MoveEvent += OnMove;
            _inputReader.InteractEvent += Interact;
        }

        private void Awake() {
            _agentMover.OnMoveStateChanged += _agentAnimation.SetMoving;
        }

        private void Start() {
            _selectedTool.Equip(this);
        }

        private void Interact() {
            _selectedTool.ToolAnimator = _hoeAnimatorController;
            _selectedTool.UseTool(this);
        }

        private void OnMove(Vector2 moveValue) {
            _agentMover.SetMoveVector(moveValue);
            _agentAnimation.ChangeDirection(moveValue);
            _agentAnimation.ToolAnimation.ChangeDirection(moveValue);
            _interactionDetector.SetInteractDirection(moveValue);
            _fieldDetector.SetInteractDirection(moveValue);

            //nếu call như này thì không cần dùng event nhưng player sẽ quản lý là chỗ quản lý bool isMoving
            // agentAnimation.SetMoving(e.moveInputValue.magnitude > 0.1f);
        }

        private void OnDisable() {
            _agentMover.OnMoveStateChanged -= _agentAnimation.SetMoving;
        }
    }
}
