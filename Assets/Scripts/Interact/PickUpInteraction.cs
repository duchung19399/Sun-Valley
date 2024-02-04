using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.Tools;
using UnityEngine;

namespace FarmGame.Interact {
    public class PickUpInteraction : MonoBehaviour, IInteractable {

        [field: SerializeField]
        public List<ToolType> UsableTools { get; set; } = new List<ToolType>();
        public bool CanInteract(Player agent) => UsableTools.Contains(agent.SelectedTool.ToolType);
        public void Interact(Player agent) {
            Destroy(gameObject);
        }
    }
}
