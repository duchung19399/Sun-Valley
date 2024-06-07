using FarmGame.Farming;
using FarmGame.Interact;
using FarmGame.Tools;

namespace FarmGame.Agent {
    public interface IAgent {
        InputReader InputReader { get; }
        AgentMover AgentMover { get; }
        AgentAnimation AgentAnimation { get; }
        bool BlockedInput { get; set; }
        InteractionDetector InteractionDetector { get; }
        ToolsBag ToolsBag { get; }
        FieldDetector FieldDetectorObject { get; }
        FieldController FieldController { get; }
    }
}