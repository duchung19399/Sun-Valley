using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.SaveSystem;
using UnityEngine;

namespace FarmGame.Tools {
    public class ToolBoxSystem : MonoBehaviour, ISavable {
        public int SaveID => SaveIDRepository.TOOL_BOX_ID;

        [SerializeField]
        private Inventory _toolBoxInventory;
        [SerializeField]
        private List<InitialToolData> _initialTools;

        [SerializeField]
        private ItemDatabaseSO _itemDatabase;

        public string GetData() {
            return _toolBoxInventory.GetDataToSave();
        }

        public void RestoreData(string data) {
            if (string.IsNullOrEmpty(data)) {
                foreach(var item in _initialTools) {
                    if(item == null) continue;
                    ItemData itemDescription = _itemDatabase.GetItemData(item.ID);
                    InventoryItemData inventoryItem = new InventoryItemData(item.ID, item.amount, -1, ToolsFactory.GetToolData(itemDescription, item.amount));
                    _toolBoxInventory.AddItem(inventoryItem, itemDescription.MaxStackSize);

                }
            } else {
                _toolBoxInventory.RestoreSaveData(data);
            }
        }

        [Serializable]
        public class InitialToolData {
            public int ID;
            public int amount = 1;
        }
    }
}
