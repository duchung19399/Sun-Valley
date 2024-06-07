using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.DataStorage;
using UnityEngine;

namespace FarmGame.Tools {
    public class ToolsBag : MonoBehaviour {
        [SerializeField]
        private ItemDatabaseSO _itemDatabase;
        private int _selectedToolIndex = 0;
        [SerializeField]
        private List<int> _initialTools;

        public event Action<int, List<Sprite>, int?> OnToolBagUpdated;

        private Tool _currentTool;
        public Tool CurrentTool => _currentTool;

        public void Initialize(IAgent agent) {
            SwapTool(_selectedToolIndex, agent);
        }

        public void SelectNextTool(IAgent agent) {
            SwapTool(_selectedToolIndex + 1, agent);
        }

        private void SwapTool(int newIndex, IAgent agent) {
            if (_currentTool != null) {
                UnequipTool(agent);
            }

            _selectedToolIndex = newIndex;

            if (_selectedToolIndex >= _initialTools.Count) {
                _selectedToolIndex = 0;
            }
            ItemData itemData = _itemDatabase.GetItemData(_initialTools[_selectedToolIndex]);

            Debug.Log($"Swapping to tool {itemData.Name}");
            _currentTool = ToolsFactory.CreateTool(itemData);
            EquipTool(agent);

            SendUpdateMessage();
        }

        private void SendUpdateMessage() {
            List<Sprite> toolSprites = new List<Sprite>();
            foreach (int ID in _initialTools) {
                ItemData itemData = _itemDatabase.GetItemData(ID);
                if (itemData != null)
                    toolSprites.Add(itemData.Image);
            }
            OnToolBagUpdated?.Invoke(_selectedToolIndex, toolSprites, null);
        }

        private void EquipTool(IAgent agent) {
            _currentTool.Equip(agent);
        }

        private void UnequipTool(IAgent agent) {
            _currentTool.Unequip(agent);
        }
    }
}
