using System.Collections;
using System.Collections.Generic;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using UnityEngine;

namespace FarmGame.UI {
    public class InventoryItemUpdaterUI : MonoBehaviour {
        [SerializeField] private InventoryRendererUI _inventoryRendererUI;

        public void UpdateElement(int index, ItemData itemData, InventoryItemData inventoryItemData) {
            _inventoryRendererUI.UpdateItem(index, itemData.Image, inventoryItemData.count);
        }

        public void ClearElements() {
            _inventoryRendererUI.ResetAllItems();
        }
    }
}
