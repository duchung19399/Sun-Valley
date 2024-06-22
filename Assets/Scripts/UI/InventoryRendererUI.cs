using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.UI {
    public class InventoryRendererUI : MonoBehaviour {
        [SerializeField] private GameObject _inventoryUIItemPrefab;
        [SerializeField] private Transform _inventoryItemParent;

        private List<ItemControllerUI> _inventoryItems = new();

        [SerializeField] private int _rowSize = 7;

        public int RowSize { get => _rowSize; }
        public int InventoryItemsCount { get => _inventoryItems.Count; }

        public void PrepareItemsToShow(int capacity) {
            foreach (Transform item in _inventoryItemParent) {
                Destroy(item.gameObject);
            }

            _inventoryItems.Clear();
            for (int i = 0; i < capacity; i++) {
                GameObject item = Instantiate(_inventoryUIItemPrefab, _inventoryItemParent);
                _inventoryItems.Add(item.GetComponent<ItemControllerUI>());
            }
        }

        public void UpdateItem(int index, Sprite sprite, int itemCount) {

            GetItemAt(index).UpdateData(sprite, itemCount);
        }

        private ItemControllerUI GetItemAt(int index) {
            if (index >= _inventoryItems.Count || index < 0) {
                throw new IndexOutOfRangeException($"Index {index} out of range");
            }
            return _inventoryItems[index];
        }

        public void ResetItem(int index) {
            GetItemAt(index).ResetData();
        }

        public void ResetAllItems() {
            foreach (var item in _inventoryItems) {
                item.ResetData();
            }
        }

        internal void SelectItem(int selectedItemIndex) {
            GetItemAt(selectedItemIndex).Outline.SetOutline(true, Mode.Select);
        }

        internal void ResetAllSelection(bool v) {
            for (int i = 0; i < _inventoryItems.Count; i++) {
                ItemControllerUI controller = GetItemAt(i);
                if (v == false && controller.Outline.IsMasked) {
                    MarkItem(i, true);
                    continue;
                }
                controller.Outline.SetOutline(false);
            }
        }

        private void MarkItem(int index, bool resetSelection = true) {
            ItemControllerUI controller = GetItemAt(index);
            if (resetSelection)
                controller.Outline.SetOutline(false);
            else
                controller.Outline.SetOutline(true, Mode.Mark);
        }
    }
}
