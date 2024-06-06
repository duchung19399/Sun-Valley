using System.Collections.Generic;
using FarmGame.Agent;

namespace FarmGame.Tools {
    public class HoeTool : Tool {
        public HoeTool(ToolType toolType) : base(toolType) { }

        public override void Equip(IAgent agent) {
            agent.FieldDetectorObject.StartChecking(ToolRange);
        }

        public override void Unequip(IAgent agent) {
            agent.FieldDetectorObject.StopChecking();
        }

        public override void UseTool(IAgent agent) {

            List<UnityEngine.Vector2> detectPosition = agent.FieldDetectorObject.ValidSelectionPositions;

            agent.BlockedInput = true;
            agent.AgentAnimation.OnAnimationEnd.AddListener(() => {
                foreach (UnityEngine.Vector2 worldPosition in detectPosition) {
                    agent.FieldController.PrepareField(worldPosition);
                }
                agent.BlockedInput = false;
            });

            if (ToolAnimator != null) {
                agent.AgentAnimation.ToolAnimation.SetAnimatorController(ToolAnimator);
                agent.AgentAnimation.ToolAnimation.PlayAnimation();
            }
            agent.AgentAnimation.PlayAnimation(AnimationType.Swing);
        }
    }
}
