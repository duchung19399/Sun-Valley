using System;
using System.Collections.Generic;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.TimeSystem;
using FarmGame.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace FarmGame.SellSystem {
    public class SellBoxController : MonoBehaviour {
        [SerializeField]
        private InputReader _inputReader;
        [SerializeField]
        private GameObject _sellBoxCanvas;
        [SerializeField]
        private PauseTimeControllerSO _pauseTimeControllerSO;
        [SerializeField]
        private InventoryRendererUI _playerInventoryRenderer, _sellBoxInventoryRenderer;
        [SerializeField]
        private Inventory _sellBoxInventory;
        private Inventory _playerInventory;

        [SerializeField]
        private ItemDatabaseSO _itemDatabase;

        [SerializeField]
        private ItemSelectionUI _playerItemSelection, _sellBoxItemSelection;
        [SerializeField]
        private ItemDescriptionUI _itemDescriptionUI;
        private Inventory _currentlySelectedInventory;

        [SerializeField]
        private ItemInteractUI _playerInteractor, _sellBoxInteractor;

        public void PrepareSellBox(Inventory inventory) {
            _inputReader.EnableUIInput();
            _inputReader.UIExitEvent += ExitUI;
            _sellBoxCanvas.SetActive(true);

            ConnectToPlayerInventory(inventory);
            PrepareSellBoxInventory();

            _currentlySelectedInventory = _playerInventory;
            _playerItemSelection.EnableController(_inputReader);
            _playerInteractor.EnableController(_inputReader);

            _pauseTimeControllerSO.SetTimePause(true);
        }

        public void TranferItem(ItemSelectionUI selectedWindow) {
            Inventory receivingInventory = selectedWindow == _playerItemSelection ? _sellBoxInventory : _playerInventory;
            if (_currentlySelectedInventory.GetItemDataAt(selectedWindow.SelectedItem) == null) {
                return;
            }

            InventoryItemData item = _currentlySelectedInventory.GetItemDataAt(selectedWindow.SelectedItem);
            ItemData itemDescription = _itemDatabase.GetItemData(item.id);

            if (itemDescription != null && receivingInventory.IsThereSpace(item, itemDescription.MaxStackSize)) {
                receivingInventory.AddItem(item, itemDescription.MaxStackSize);
                _currentlySelectedInventory.RemoveAllItemAt(selectedWindow.SelectedItem);
            }

            UpdateDescription(selectedWindow.SelectedItem);
        }

        private void PrepareSellBoxInventory() {
            _sellBoxInventory.OnUpdateInventory += UpdateSellBoxInventoryUI;
            _sellBoxInventoryRenderer.PrepareItemsToShow(_sellBoxInventory.Capacity);
            UpdateSellBoxInventoryUI(_sellBoxInventory.InventoryContent);
        }

        public void UpdateDescription(int selectedItemIndex) {
            InventoryItemData itemData = _currentlySelectedInventory.GetItemDataAt(selectedItemIndex);
            ItemData descriptionData = itemData != null ? _itemDatabase.GetItemData(itemData.id) : null;
            if (descriptionData == null) {
                _itemDescriptionUI.ResetDescription();
            } else {
                _itemDescriptionUI.UpdateDescription(descriptionData.Image, descriptionData.Name, descriptionData.Description);
            }
        }

        public void SwapWindow(ItemSelectionUI selectedWindow) {
            _playerInventoryRenderer.ResetAllSelection(true);
            _sellBoxInventoryRenderer.ResetAllSelection(true);

            _playerItemSelection.DisableController(_inputReader);
            _sellBoxItemSelection.DisableController(_inputReader);

            _playerInteractor.DisableController(_inputReader);
            _sellBoxInteractor.DisableController(_inputReader);

            selectedWindow.EnableController(_inputReader);
            if (selectedWindow == _sellBoxItemSelection) {
                _sellBoxInteractor.EnableController(_inputReader);

                _currentlySelectedInventory = _sellBoxInventory;
                int itemIndexToSelect = _playerItemSelection.SelectedItem / _playerInventoryRenderer.RowSize * _playerInventoryRenderer.RowSize;
                itemIndexToSelect = Mathf.Clamp(itemIndexToSelect, 0, _sellBoxInventory.Capacity - 1);
                _sellBoxItemSelection.SelectItemAt(itemIndexToSelect);
            } else {
                _playerInteractor.EnableController(_inputReader);

                _currentlySelectedInventory = _playerInventory;
                int itemIndexToSelect = _sellBoxItemSelection.SelectedItem + (_sellBoxInventoryRenderer.RowSize - 1);

                itemIndexToSelect = Mathf.Clamp(itemIndexToSelect, 0, _playerInventory.Capacity - 1);
                _playerItemSelection.SelectItemAt(itemIndexToSelect);
            }
        }

        private void UpdateSellBoxInventoryUI(IEnumerable<InventoryItemData> inventoryContent) {
            UpdateUI(inventoryContent, _sellBoxInventoryRenderer);
        }

        private void UpdateUI(IEnumerable<InventoryItemData> inventoryContent, InventoryRendererUI inventoryRenderer) {
            inventoryRenderer.ResetAllItems();
            int index = 0;
            foreach (var item in inventoryContent) {
                if (index >= inventoryRenderer.InventoryItemsCount) {
                    break;
                }

                if (item != null) {
                    inventoryRenderer.UpdateItem(index, _itemDatabase.GetItemData(item.id).Image, item.count);
                }
                index++;
            }
        }

        private void ConnectToPlayerInventory(Inventory inventory) {
            _playerInventory = inventory;
            _playerInventory.OnUpdateInventory += UpdatePlayerInventoryUI;
            _playerInventoryRenderer.PrepareItemsToShow(_playerInventory.Capacity);
            UpdatePlayerInventoryUI(_playerInventory.InventoryContent);
        }

        private void UpdatePlayerInventoryUI(IEnumerable<InventoryItemData> inventoryContent) {
            UpdateUI(inventoryContent, _playerInventoryRenderer);
        }

        private void ExitUI() {
            _playerItemSelection.DisableController(_inputReader);
            _sellBoxItemSelection.DisableController(_inputReader);
            _playerInteractor.DisableController(_inputReader);
            _sellBoxInteractor.DisableController(_inputReader);


            _sellBoxCanvas.SetActive(false);
            _pauseTimeControllerSO.SetTimePause(false);
            _inputReader.EnablePlayerInput();
            _inputReader.UIExitEvent -= ExitUI;
        }
    }
}
