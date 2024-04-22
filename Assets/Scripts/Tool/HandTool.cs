using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.Interact;
using UnityEngine;

namespace FarmGame.Tools {
    public class HandTool : Tool {
        public HandTool(ToolType toolType) : base(toolType) { }
        public override void UseTool(Player agent) {
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

    public enum ToolType {
        None,
        Hand,
        Hoe,
        WateringCan,
        Axe,
        Pickaxe,
        Sickle,
        Hammer
    }
}
