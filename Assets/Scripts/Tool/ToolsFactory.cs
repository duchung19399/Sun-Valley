using System.Collections;
using System.Collections.Generic;
using FarmGame.DataStorage;
using UnityEngine;

namespace FarmGame.Tools {
    public static class ToolsFactory {
        public static Tool CreateTool(ItemData data, string dataStr = null) {
            Tool tool = data.ToolType switch {
                ToolType.Hand => new HandTool(data.ID, dataStr),
                ToolType.Hoe => new HoeTool(data.ID, dataStr),
                ToolType.SeedPlacer => new SeedPlacementTool(data.ID, dataStr),
                _ => throw new System.NotImplementedException($"Tool {data.ToolType} not implemented")
            };
            tool.ToolAnimator = data.ToolAnimator;
            tool.ToolRange = data.ToolRange;
            return tool;
        }
    }
}
