using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.Tools;
using UnityEngine;

namespace FarmGame.Interact {
    public class PickUpInteraction : MonoBehaviour, IInteractable {

        [field: SerializeField]
        public List<ToolType> UsableTools { get; set; } = new List<ToolType>();

        public UnityEngine.Events.UnityEvent OnPickUp;
        public bool CanInteract(IAgent agent) => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);
        public void Interact(IAgent agent) {
            Destroy(gameObject);
            OnPickUp.Invoke();
        }
    }
}
