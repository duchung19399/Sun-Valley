using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.Tools;
using UnityEngine;

namespace FarmGame.Interact {
    public class WateringInteraction : MonoBehaviour, IInteractable {
        [field: SerializeField]
        public List<ToolType> UsableTools { get; set; } = new() { ToolType.WateringCan };
        public bool UseResource { get; set; }

        public bool CanInteract(IAgent agent) => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);

        public void Interact(IAgent agent) {
            //agent.FieldController.WaterCropAt(transform.position);
        }
    }
}
