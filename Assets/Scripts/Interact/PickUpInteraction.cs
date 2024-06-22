using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.DataStorage;
using FarmGame.Tools;
using UnityEngine;
using FarmGame.DataStorage.Inventory;

namespace FarmGame.Interact {
    [RequireComponent(typeof(ItemInfo))]
    public class PickUpInteraction : MonoBehaviour, IInteractable {

        private ItemInfo _itemInfo;
        [SerializeField]
        private bool _destroyOnPickUp = true;
        [field: SerializeField]
        public ItemDatabaseSO ItemDatabase { get; set; }


        [field: SerializeField]
        public List<ToolType> UsableTools { get; set; } = new List<ToolType>();

        public UnityEngine.Events.UnityEvent OnPickUp;

        private void Awake() {
            _itemInfo = GetComponent<ItemInfo>();
        }
        public bool CanInteract(IAgent agent) => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);
        public void Interact(IAgent agent) {
            Destroy(gameObject);
            OnPickUp.Invoke();

            InventoryItemData _itemData = new InventoryItemData(_itemInfo.itemDatabaseIndex, _itemInfo.itemCount, _itemInfo.itemQuality);

            ItemData itemDescription = ItemDatabase.GetItemData(_itemInfo.itemDatabaseIndex);
            int stackSize = itemDescription.CanBeStacked ? itemDescription.MaxStackSize : 1;
            if (agent.Inventory != null && _itemInfo.itemCount > 0 && agent.Inventory.IsThereSpace(_itemData, stackSize)) {
                agent.Inventory.AddItem(_itemData, stackSize);
                Debug.Log(agent.Inventory);
                OnPickUp?.Invoke();

                if (_destroyOnPickUp) {
                    Destroy(gameObject);
                } else {
                    _itemInfo.itemCount = 0;
                }
            }
        }
    }
}
