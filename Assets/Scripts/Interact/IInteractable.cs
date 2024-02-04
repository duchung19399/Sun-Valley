using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.Tools;

namespace FarmGame.Interact {
    public interface IInteractable {

        List<ToolType> UsableTools { get; set; }

        bool CanInteract(Player agent);
        void Interact(Player agent);
    }
}