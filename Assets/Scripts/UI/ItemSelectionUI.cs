using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.UI {
    [RequireComponent(typeof(InventoryRendererUI))]
    public class ItemSelectionUI : MonoBehaviour {
        [SerializeField]
        private int _selectedItemIndex = 0;

        private InventoryRendererUI _inventoryRendererUI;
        public UnityEvent<Direction, int> OnSelectOutsideOfBounds;
        public UnityEvent<int> OnSelectInsideChange;
        public int SelectedItem => _selectedItemIndex;

        private void Awake() {
            _inventoryRendererUI = GetComponent<InventoryRendererUI>();
        }


        public void EnableController(InputReader inputReader) {
            inputReader.UIMoveEvent += SelectItem;
            if (_inventoryRendererUI == null) Awake();

            _inventoryRendererUI.SelectItem(_selectedItemIndex);
            SelectItem(Vector2.zero);
        }

        public void DisableController(InputReader inputReader) {
            inputReader.UIMoveEvent -= SelectItem;
        }

        private void SelectItem(Vector2 arg0) {
            Vector2Int input = Vector2Int.RoundToInt(arg0);
            int newIndex;
            Direction direction;
            (newIndex, direction) = FindDirection(input);

            int rowSize = _inventoryRendererUI.RowSize;

            int currentRow = _selectedItemIndex / rowSize;
            int newRow = newIndex / rowSize;
            int currentColumn = _selectedItemIndex % rowSize;
            int newColumn = newIndex % rowSize;

            if (newIndex > -1 && newIndex < _inventoryRendererUI.InventoryItemsCount
                && (currentRow == newRow || currentColumn == newColumn)) {
                SelectItemAt(newIndex);
                return;
            } else {
                OnSelectOutsideOfBounds?.Invoke(direction, _selectedItemIndex);
            }
        }

        public void SelectItemAt(int newIndex) {
            _inventoryRendererUI.ResetAllSelection(false);
            _selectedItemIndex = newIndex;
            _inventoryRendererUI.SelectItem(_selectedItemIndex);
            OnSelectInsideChange?.Invoke(_selectedItemIndex);
        }

        private (int, Direction) FindDirection(Vector2Int input) {

            int newIndex = 0;
            Direction direction = Direction.None;
            int rowSize = _inventoryRendererUI.RowSize;
            if (input.x == 1) {
                newIndex = _selectedItemIndex + 1;
                direction = Direction.Right;
            } else if (input.x == -1) {
                newIndex = _selectedItemIndex - 1;
                direction = Direction.Left;
            } else if (input.y == 1) {
                newIndex = _selectedItemIndex - rowSize;
                direction = Direction.Up;
            } else if (input.y == -1) {
                newIndex = _selectedItemIndex + rowSize;
                direction = Direction.Down;
            }

            return (newIndex, direction);
        }

        public void WrapHorizontalMovementSelection(Direction direction, int index) {
            if (direction == Direction.Left) {
                int wrappedIndex = index + _inventoryRendererUI.RowSize - 1;
                int currentRow = index / _inventoryRendererUI.RowSize;
                int newRow = wrappedIndex / _inventoryRendererUI.RowSize;
                if (wrappedIndex >= _inventoryRendererUI.InventoryItemsCount || currentRow != newRow) {
                    return;
                }

                _inventoryRendererUI.ResetAllSelection(false);
                _selectedItemIndex = wrappedIndex;
                _inventoryRendererUI.SelectItem(_selectedItemIndex);
                OnSelectInsideChange?.Invoke(_selectedItemIndex);

            }

            if (direction == Direction.Right) {
                int wrappedIndex = index - _inventoryRendererUI.RowSize + 1;
                int currentRow = index / _inventoryRendererUI.RowSize;
                int newRow = wrappedIndex / _inventoryRendererUI.RowSize;
                if (wrappedIndex < 0 || currentRow != newRow) {
                    return;
                }

                _inventoryRendererUI.ResetAllSelection(false);
                _selectedItemIndex = wrappedIndex;
                _inventoryRendererUI.SelectItem(_selectedItemIndex);
                OnSelectInsideChange?.Invoke(_selectedItemIndex);
            }
        }
    }

    public enum Direction {
        Up,
        Down,
        Left,
        Right,
        None
    }
}
