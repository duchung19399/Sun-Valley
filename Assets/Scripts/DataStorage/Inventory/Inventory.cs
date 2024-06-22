using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FarmGame.DataStorage.Inventory {
    public class Inventory : MonoBehaviour {
        private InventoryItemData[] _inventoryContent;
        public IEnumerable<InventoryItemData> InventoryContent => _inventoryContent;
        [SerializeField]
        private int _capacity;
        public int Capacity => _capacity;

        public event Action<IEnumerable<InventoryItemData>> OnUpdateInventory;

        private void Awake() {
            if (_inventoryContent == null) {
                _inventoryContent = new InventoryItemData[_capacity];
            }
        }

        public void ChangeCapacity(int newCapacity) {
            if (newCapacity < _capacity) {
                Array.Resize(ref _inventoryContent, newCapacity);
            }
            _capacity = newCapacity;
        }

        public int AddItem(InventoryItemData item, int stackSize) {
            int quantityRemaining = item.count;
            if (stackSize > 1) {
                for (int i = 0; i < _capacity; i++) {
                    if (_inventoryContent[i] != null && _inventoryContent[i].id == item.id && _inventoryContent[i].quality == item.quality) {
                        int freeSpace = stackSize - _inventoryContent[i].count;
                        int quantityToAdd = quantityRemaining;
                        if (quantityToAdd > freeSpace) {
                            quantityToAdd = freeSpace;
                            quantityRemaining -= freeSpace;
                        } else {
                            quantityRemaining = 0;
                        }

                        _inventoryContent[i] = new InventoryItemData(item.id, _inventoryContent[i].count + quantityToAdd, item.quality, item.data);

                        OnUpdateInventory?.Invoke(_inventoryContent);
                        if (quantityRemaining == 0) {
                            return 0;
                        }
                    }
                }
            }

            if (quantityRemaining > 0) {
                for (int i = 0; i < _capacity; i++) {
                    if (_inventoryContent[i] == null) {
                        int quantityToAdd;
                        if (stackSize > 1) {
                            quantityToAdd = quantityRemaining > stackSize ? stackSize : quantityRemaining;
                        } else {
                            quantityToAdd = 1;
                        }

                        _inventoryContent[i] = new InventoryItemData(item.id, quantityToAdd, item.quality, item.data);
                        quantityRemaining -= quantityToAdd;

                        OnUpdateInventory?.Invoke(_inventoryContent);
                        if (quantityRemaining == 0) {
                            return 0;
                        }
                    }
                }
            }
            return quantityRemaining;

        }

        public bool IsThereSpace(InventoryItemData item, int stackSize) {
            if (stackSize > 1) {
                return _inventoryContent.Any(data => data == null) || _inventoryContent.Where(_data => _data.id == item.id && _data.quality == item.quality && _data.count < stackSize).Count() > 0;
            }

            return _inventoryContent.Any(data => data == null);
        }

        public override string ToString() {
            StringBuilder sb = new();
            sb.Append("Inventory content: ");
            foreach (var item in InventoryContent) {
                if (item == null)
                    sb.Append("NULL, ");
                else
                    sb.Append(item.id + $"({item.count}), ");
            }
            return sb.ToString();
        }

        public InventoryItemData GetItemDataAt(int index) {
            if (index >= _capacity || index < 0) {
                return null;
            }
            return _inventoryContent[index];
        }

        public bool SetItemDataAt(int index, InventoryItemData item) {
            if (index >= _capacity || index < 0) {
                return false;
            }
            _inventoryContent[index] = item;
            OnUpdateInventory?.Invoke(InventoryContent);
            return true;
        }

        public void RemoveAllItem(InventoryItemData item) {
            int index = Array.IndexOf(_inventoryContent, item);
            if (index > -1) {
                _inventoryContent[index] = null;
                OnUpdateInventory?.Invoke(InventoryContent);
            }
        }

        public bool RemoveAllItemAt(int index) {
            if (index >= _capacity || index < 0) {
                return false;
            }
            _inventoryContent[index] = null;
            OnUpdateInventory?.Invoke(InventoryContent);
            return true;
        }

        public bool AddItemAt(int index, InventoryItemData item) {
            if (index >= _capacity || index < 0) {
                return false;
            }
            _inventoryContent[index] = item;
            OnUpdateInventory?.Invoke(InventoryContent);
            return true;
        }

        public void Clear() {
            for (int i = 0; i < _inventoryContent.Length; i++) {
                _inventoryContent[i] = null;
            }
        }
    }
    public record InventoryItemData(int id, int count, int quality, string data = null);
}

namespace System.Runtime.CompilerServices {
    public class IsExternalInit { }
}