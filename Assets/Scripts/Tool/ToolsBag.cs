using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FarmGame.Agent;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using UnityEngine;

namespace FarmGame.Tools {
    public class ToolsBag : MonoBehaviour {
        [SerializeField]
        private ItemDatabaseSO _itemDatabase;
        private int _selectedToolIndex = 0;
        [SerializeField]
        private List<int> _initialTools;
        [SerializeField]
        private Inventory _toolsBagInventory;

        public event Action<int, List<Sprite>, int?> OnToolBagUpdated;

        private List<Tool> _newBag = new();
        public Tool CurrentTool => _newBag[_selectedToolIndex];

        [SerializeField]
        private int _handToolID = 4;

        private void Start() {
            for (int i = 0; i < _initialTools.Count; i++) {
                ItemData itemData = _itemDatabase.GetItemData(_initialTools[i]);
                string data = null;
                int _quantity = 1;
                if (itemData.ToolType == ToolType.SeedPlacer) {
                    data = JsonUtility.ToJson(new SeedToolData() { cropID = itemData.CropTypeIndex, quantity = 4 });
                    _quantity = 4;
                }

                _toolsBagInventory.AddItem(new InventoryItemData(itemData.ID, _quantity, -1, data), itemData.MaxStackSize);
            }
            UpdateToolsBag(_toolsBagInventory.InventoryContent);
        }

        private void UpdateToolsBag(IEnumerable<InventoryItemData> inventoryContent) {
            _newBag = new List<Tool>();
            AddDefaultHandTool(); //add hand tool đầu để nó luôn có index 0
            int index = 0;

            foreach (InventoryItemData tool in inventoryContent) {
                if (tool != null) {
                    ItemData toolData = _itemDatabase.GetItemData(tool.id);
                    if (toolData == null || toolData.ToolType == ToolType.None) {
                        Debug.LogError($"Item with ID {tool.id} is not a tool");
                        continue;
                    }
                    Tool newTool = ToolsFactory.CreateTool(toolData, tool.data);
                    if (newTool is IQuantity quantity) {
                        quantity.Quantity = tool.count;
                        _toolsBagInventory.AddItemAt(index, new InventoryItemData(toolData.ID, tool.count, tool.quality, newTool.GetDataToSave()));

                    }
                    _newBag.Add(newTool);
                }
                index++;
            }
            if (_selectedToolIndex >= _newBag.Count) {
                _selectedToolIndex = 0;
            }
        }

        private void AddDefaultHandTool() {
            ItemData handToolData = _itemDatabase.GetItemData(_handToolID);
            Tool handTool = ToolsFactory.CreateTool(handToolData, null);
            _newBag.Add(handTool);
        }

        public void Initialize(IAgent agent) {
            SwapTool(_selectedToolIndex, agent);
        }

        public void SelectNextTool(IAgent agent) {
            SwapTool(_selectedToolIndex + 1, agent);
        }

        private void SwapTool(int newIndex, IAgent agent) {
            if (_newBag[_selectedToolIndex] != null) {
                UnequipTool(agent);
            }

            _selectedToolIndex = newIndex;

            if (_selectedToolIndex >= _newBag.Count) {
                _selectedToolIndex = 0;
            }
            ItemData itemData = _itemDatabase.GetItemData(_newBag[_selectedToolIndex].ItemIndex);
            Debug.Log($"Swapping to tool {itemData.Name}");
            EquipTool(agent);

            SendUpdateMessage();
        }

        private void SendUpdateMessage() {
            int? count = null;
            ItemData selectedToolData = _itemDatabase.GetItemData(_newBag[_selectedToolIndex].ItemIndex);
            if (selectedToolData.ToolType == ToolType.SeedPlacer) {
                count = _toolsBagInventory.GetItemDataAt(_selectedToolIndex - 1).count;
            } else if(selectedToolData.ToolType == ToolType.WateringCan) {
                count = ((WateringCanTool)CurrentTool).NumberOfUses;
            }

            List<Sprite> toolSprites = new List<Sprite>();
            foreach (Tool tool in _newBag) {
                ItemData itemData = _itemDatabase.GetItemData(tool.ItemIndex);
                if (itemData != null)
                    toolSprites.Add(itemData.Image);
            }
            OnToolBagUpdated?.Invoke(_selectedToolIndex, toolSprites, count);
        }

        private void EquipTool(IAgent agent) {
            _newBag[_selectedToolIndex].Equip(agent);
            _newBag[_selectedToolIndex].OnFinishedAction += UpdateInventoryData;
        }

        private void UpdateInventoryData(IAgent agent) {
            Tool tool = _newBag[_selectedToolIndex];
            string data = tool.GetDataToSave();
            if (string.IsNullOrEmpty(data)) {
                return;
            }

            int inventoryIndex = _selectedToolIndex - 1;// hand is not in inventory
            if (inventoryIndex > 0) {
                if (tool.IsToolStillValid()) {
                    int quantity = 1;
                    if (tool is IQuantity) {
                        quantity = ((IQuantity)tool).Quantity;
                    }
                    //modify the data of the tool in the inventory
                    _toolsBagInventory.AddItemAt(inventoryIndex, new InventoryItemData(tool.ItemIndex, quantity, -1, data));
                } else {
                    List<InventoryItemData> items = _toolsBagInventory.InventoryContent.ToList();
                    _toolsBagInventory.Clear();

                    for (int i = 0; i < items.Count; i++) {
                        if (i == inventoryIndex || items[i] == null) {
                            continue;
                        }
                        _toolsBagInventory.AddItem(items[i], _itemDatabase.GetItemData(items[i].id).MaxStackSize);

                    }
                    UpdateToolsBag(_toolsBagInventory.InventoryContent);
                }
                SwapTool(_selectedToolIndex, agent);
            }

        }

        private void UnequipTool(IAgent agent) {
            _newBag[_selectedToolIndex].Unequip(agent);
            _newBag[_selectedToolIndex].OnFinishedAction = null;
            _newBag[_selectedToolIndex].OnPerformedAction = null;
            _newBag[_selectedToolIndex].OnStartedAction = null;


        }

        public void RestoreCurrentTool(IAgent agent) {
            if(CurrentTool.ToolType == ToolType.WateringCan) {
                ((WateringCanTool)CurrentTool).Refill();
            }
            UpdateInventoryData(agent);
        }
    }
}
