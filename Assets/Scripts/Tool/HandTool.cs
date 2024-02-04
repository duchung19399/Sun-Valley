using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Tools {
    public class HandTool {
        public ToolType ToolType { get; }
        public HandTool(ToolType toolType) {
            ToolType = toolType;
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
