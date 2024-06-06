using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using UnityEngine;

namespace FarmGame.Tools {
    public abstract class Tool {
        public ToolType ToolType { get; }

        public RuntimeAnimatorController ToolAnimator { get; set; }

        public Vector2Int ToolRange { get; set; } = Vector2Int.one;
        protected Tool(ToolType toolType) {
            this.ToolType = toolType;
        }

        public virtual void Equip(IAgent agent) { }
        public virtual void Unequip(IAgent agent) { }

        public abstract void UseTool(IAgent agent);
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
