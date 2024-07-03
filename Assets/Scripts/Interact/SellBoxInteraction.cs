using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.SellSystem;
using FarmGame.Tools;
using UnityEngine;

namespace FarmGame.Interact {
    public class SellBoxInteraction : MonoBehaviour, IInteractable {
        [field: SerializeField]
        public List<ToolType> UsableTools { get; set; } = new() {ToolType.Hand};
        public bool UseResource { get; set; }
        [SerializeField] 
        private SellBoxController _sellBoxController;

        public bool CanInteract(IAgent agent) => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);

        public void Interact(IAgent agent) {
            _sellBoxController.PrepareSellBox(agent.Inventory);
        }
    }
}
