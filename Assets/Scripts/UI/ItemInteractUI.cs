using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.UI {
    public class ItemInteractUI : MonoBehaviour {
        [SerializeField]
        private ItemSelectionUI _itemSelectionUI;
        public UnityEvent<ItemSelectionUI> OnItemSelected;

        public void InteractWithItem() {
            OnItemSelected?.Invoke(_itemSelectionUI);
        }

        public void EnableController(InputReader inputReader) {
            inputReader.UIInteractEvent += InteractWithItem;
        }

        public void DisableController(InputReader inputReader) {
            inputReader.UIInteractEvent -= InteractWithItem;
        }

    }
}
