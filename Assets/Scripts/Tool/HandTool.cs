using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.Interact;
using UnityEngine;

namespace FarmGame.Tools {
    public class HandTool : Tool {
        public HandTool(int itemID, string data) : base(itemID, data) {
            this.ToolType = ToolType.Hand;
        }

        public override void Equip(IAgent agent) {
            agent.FieldDetectorObject.StartChecking(ToolRange);
        }

        public override bool IsToolStillValid() {
            return true;
        }

        public override void Unequip(IAgent agent) {
            agent.FieldDetectorObject.StopChecking();
        }

        public override void UseTool(IAgent agent) {
            IEnumerable<IInteractable> interactables = null;
            if (agent.FieldDetectorObject.IsNearField) {
                List<Vector2> detectPosition = agent.FieldDetectorObject.ValidSelectionPositions;
                if (detectPosition.Count > 0) {
                    interactables = agent.InteractionDetector.PerformDetection(detectPosition[0]);
                }
            }

            if (interactables == null)
                interactables = agent.InteractionDetector.PerformDetection();

            foreach (IInteractable item in agent.InteractionDetector.PerformDetection()) {
                if (item.CanInteract(agent)) {
                    agent.BlockedInput = true;
                    agent.AgentAnimation.OnAnimationEnd.AddListener(() => {
                        item.Interact(agent);
                        agent.BlockedInput = false;
                    });
                    agent.AgentAnimation.PlayAnimation(AnimationType.PickUp);
                }
            }
        }
    }
}
