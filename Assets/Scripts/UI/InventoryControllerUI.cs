using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.TimeSystem;
using UnityEngine;

namespace FarmGame.UI {
    [RequireComponent(typeof(InventoryRendererUI))]
    public class InventoryControllerUI : MonoBehaviour {
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private GameObject _inventoryCanvas;
        [SerializeField] private InventoryItemUpdaterUI _inventoryItemUpdaterUI;
        [SerializeField] private ItemDatabaseSO _itemDatabase;

        private InventoryRendererUI _inventoryRendererUI;

        private Inventory _inventoryTempReference;
        [SerializeField]
        private ItemSelectionUI _itemSelectionUI;
        [SerializeField]
        private ItemDescriptionUI _itemDescriptionUI;
        [SerializeField]
        private PauseTimeControllerSO _pauseTimeControllerSO;


        private void Awake() {
            _inventoryRendererUI = GetComponent<InventoryRendererUI>();
        }

        public void ShowInventory(Inventory inventory) {

            _inventoryCanvas.SetActive(true);

            _inputReader.EnableUIInput();
            _inputReader.UIExitEvent += HideInventory;
            _inputReader.UICloseInventoryEvent += HideInventory;

            _inventoryRendererUI.PrepareItemsToShow(inventory.Capacity);
            _inventoryRendererUI.ResetAllItems();


            _inventoryTempReference = inventory;
            _inventoryTempReference.OnUpdateInventory += UpdateInventoryItems;
            _itemSelectionUI.EnableController(_inputReader);

            UpdateInventoryItems(inventory.InventoryContent);
            _pauseTimeControllerSO.SetTimePause(true);
        }

        private void UpdateInventoryItems(IEnumerable<InventoryItemData> inventoryContent) {
            _inventoryRendererUI.ResetAllItems();
            int index = 0;
            foreach (var item in inventoryContent) {
                if (item != null) {
                    ItemData itemData = _itemDatabase.GetItemData(item.id);
                    _inventoryItemUpdaterUI.UpdateElement(index, itemData, item);
                }
                index++;
            }
        }

        public void UpdateDescription(int selectedItemIndex) {
            InventoryItemData item = _inventoryTempReference.GetItemDataAt(selectedItemIndex);
            ItemData description = item == null ? null : _itemDatabase.GetItemData(item.id);
            if (description == null) {
                _itemDescriptionUI.ResetDescription();
            } else {
                _itemDescriptionUI.UpdateDescription(description.Image, description.Name, _itemDatabase.GetItemDescription(item.id));
            }
        }

        private void HideInventory() {
            _inputReader.EnablePlayerInput();
            _inputReader.UIExitEvent -= HideInventory;
            _inputReader.UICloseInventoryEvent -= HideInventory;
            _inventoryCanvas.SetActive(false);

            _inventoryTempReference.OnUpdateInventory -= UpdateInventoryItems;
            _itemSelectionUI.DisableController(_inputReader);

            _pauseTimeControllerSO.SetTimePause(false);
        }
    }
}
