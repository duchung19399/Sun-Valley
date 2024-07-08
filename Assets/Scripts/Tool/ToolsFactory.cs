using System;
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
                ToolType.WateringCan => new WateringCanTool(data.ID, dataStr),
                _ => throw new System.NotImplementedException($"Tool {data.ToolType} not implemented")
            };
            tool.ToolAnimator = data.ToolAnimator;
            tool.ToolRange = data.ToolRange;
            return tool;
        }

        public static string GetToolData(ItemData itemDescription, int quantity = 1) {
            if(itemDescription.ToolType == ToolType.SeedPlacer) {
                return JsonUtility.ToJson(new SeedToolData {
                    cropID = itemDescription.CropTypeIndex,
                    quantity = quantity
                });
            }
            if(itemDescription.ToolType == ToolType.WateringCan) {
                return "0";
            }
            return null;
        }
    }
}
