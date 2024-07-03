using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.Tools;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.Interact {
    public class FillInWaterInteraction : MonoBehaviour, IInteractable {
        [field: SerializeField]
        public List<ToolType> UsableTools { get; set; } = new() { ToolType.WateringCan };
        public UnityEvent OnInteract;
        public bool UseResource { get; set; }

        public bool CanInteract(IAgent agent) => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);

        public void Interact(IAgent agent) {
            agent.ToolsBag.RestoreCurrentTool(agent);
            OnInteract?.Invoke();
        }

    }
}
