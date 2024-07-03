using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.Tools;

namespace FarmGame.Interact {
    public interface IInteractable {

        List<ToolType> UsableTools { get; set; }
        bool UseResource { get; set;}

        bool CanInteract(IAgent agent);
        void Interact(IAgent agent);
    }
}