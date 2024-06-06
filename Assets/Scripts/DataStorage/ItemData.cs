using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FarmGame.Tools;
using UnityEngine;

namespace FarmGame.DataStorage {
    [Serializable]
    public class ItemData {
        public string Name;

        [Header("General Data"), Space]
        public int ID = -1;
        public string Description;
        public Sprite Image;
        public bool CanBeStacked = false;
        public int MaxStackSize = 1;

        [Header("Item Data"), Space]
        public bool CanBeThrowAway = true;
        public bool Consumable = false;
        public int EnergyBoost = 0;
        public int Price = 0;

        [Header("Tool Data"), Space]
        public ToolType ToolType = ToolType.None;
        public Vector2Int ToolRange = Vector2Int.one;
        public RuntimeAnimatorController ToolAnimator;

        [Header("Crop Data"), Space]
        public int CropTypeIndex = 0;

        [Header("Item Visualization Data"), Space]
        public GameObject Prefab;

        public string GetDescription() {
            StringBuilder sb = new StringBuilder();
            sb.Append(Description);
            sb.Append("\n");
            if(ToolType == ToolType.None && CropTypeIndex == 0) {
                if(Price > 0) {
                    sb.Append($"Price: {Price} $ \n");
                }
                if(Consumable) {
                    sb.Append($"Energy Boost: {EnergyBoost} \n");
                }
            
            }
            return sb.ToString();
        }
    }
}
