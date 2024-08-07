using System;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.Farming;
using FarmGame.Interact;
using FarmGame.SaveSystem;
using FarmGame.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.Agent {
    public class Player : MonoBehaviour, IAgent, ISavable {


        [SerializeField] private InputReader _inputReader;

        [SerializeField] private AgentMover _agentMover;
        [SerializeField] private AgentAnimation _agentAnimation;
        [SerializeField] private InteractionDetector _interactionDetector;

        [SerializeField] private RuntimeAnimatorController _hoeAnimatorController;

        public InputReader InputReader { get => _inputReader; }
        public AgentMover AgentMover { get => _agentMover; }
        public AgentAnimation AgentAnimation { get => _agentAnimation; }
        public InteractionDetector InteractionDetector { get => _interactionDetector; }

        [SerializeField]
        private FieldDetector _fieldDetector;
        public FieldDetector FieldDetectorObject {
            get => _fieldDetector;
        }

        [SerializeField] private FieldController _fieldController;

        [SerializeField] private ItemDatabaseSO _itemDatabase;

        [field: SerializeField] public ToolsBag ToolsBag { get; private set; }

        [SerializeField] private ToolsSelectionUI _toolsSelectionUI;

        [SerializeField] private Inventory _inventory;
        public Inventory Inventory => _inventory;

        private bool _blockedInput = false;
        public bool BlockedInput {
            get => _blockedInput;
            set {
                _blockedInput = value;
                _agentMover.Stopped = value;
                if (_blockedInput)
                    _agentAnimation.SetMoving(false);
                _inputReader.SetBlockPlayerInput(value);
            }
        }
        [field: SerializeField]
        public AgentDataSO AgentData { get; private set; }

        public FieldController FieldController => _fieldController;

        public int SaveID => SaveIDRepository.PLAYER_DATA_ID;

        public UnityEvent<Inventory> OnToggleInventoryUI;

        private void OnEnable() {
            _inputReader.MoveEvent += OnMove;
            _inputReader.InteractEvent += Interact;
            _inputReader.SwapToolEvent += SwapTool;
            _inputReader.ToggleInventoryEvent += ToggleInventoryUI;

            ToolsBag.OnToolBagUpdated += _toolsSelectionUI.UpdateUI;
        }

        private void ToggleInventoryUI() {
            OnToggleInventoryUI?.Invoke(Inventory);
        }

        private void SwapTool() {
            ToolsBag.SelectNextTool(this);
        }

        private void Awake() {
            BlockedInput = false;
            _agentMover.OnMoveStateChanged += _agentAnimation.SetMoving;
        }

        private void Start() {
            ToolsBag.Initialize(this);
            Debug.Log("<color=red> Resetting Agent Data </color>");
            AgentData.Money = 0;
        }

        private void Interact() {
            ToolsBag.CurrentTool.UseTool(this);
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

            _inputReader.MoveEvent -= OnMove;
            _inputReader.InteractEvent -= Interact;
            _inputReader.SwapToolEvent -= SwapTool;
            _inputReader.ToggleInventoryEvent -= ToggleInventoryUI;

            _agentMover.OnMoveStateChanged -= _agentAnimation.SetMoving;

            ToolsBag.OnToolBagUpdated -= _toolsSelectionUI.UpdateUI;
        }

        public string GetData() {
            return AgentData.GetData();
        }

        public void RestoreData(string data) {
            AgentData.Inventory = Inventory;
            if(string.IsNullOrEmpty(data)) {
                AgentData.SetDefaultData();
            } else {
                AgentData.RestoreData(data);
            }
        }
    }
}
