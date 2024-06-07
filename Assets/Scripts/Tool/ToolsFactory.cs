using System.Collections;
using System.Collections.Generic;
using FarmGame.DataStorage;
using UnityEngine;

namespace FarmGame.Tools {
    public static class ToolsFactory {
        public static Tool CreateTool(ItemData data) {
            Tool tool = data.ToolType switch {
                ToolType.Hand => new HandTool(data.ToolType),
                ToolType.Hoe => new HoeTool(data.ToolType),
                _ => throw new System.NotImplementedException($"Tool {data.ToolType} not implemented")
            };
            tool.ToolAnimator = data.ToolAnimator;
            return tool;
        }
    }
}
