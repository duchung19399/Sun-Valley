using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.SellSystem;
using FarmGame.Tools;
using UnityEngine;

namespace FarmGame.Interact {
    public class ToolBoxInteraction : MonoBehaviour, IInteractable {
        [SerializeField] private StorageBoxController _toolBoxController;
        
        public List<ToolType> UsableTools { get; set; } = new() { ToolType.Hand };
        public bool UseResource { get; set; }

        public bool CanInteract(IAgent agent) => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);

        public void Interact(IAgent agent) {
            _toolBoxController.PrepareStorageBox(agent.ToolsBag.GetInventory());
        }
    }
}
