using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using UnityEngine;

namespace FarmGame.Tools {
    public class SeedPlacementTool : Tool, IQuantity {

        public int CropID { get; set; } = 0;
        private int _quantity = 1;

        public int Quantity {
            get => _quantity;
            set { _quantity = value; }
        }

        public SeedPlacementTool(int itemID, string data) : base(itemID, data) {
            this.ToolType = ToolType.SeedPlacer;
            RestoreSavedData(data);
        }

        public override void Equip(IAgent agent) {
            agent.FieldDetectorObject.StartChecking(ToolRange);
        }

        public override void Unequip(IAgent agent) {
            agent.FieldDetectorObject.StopChecking();
        }

        public override string GetDataToSave() {
            return JsonUtility.ToJson(new SeedToolData() { cropID = CropID, quantity = _quantity });
        }

        public override void RestoreSavedData(string data) {
            if (string.IsNullOrEmpty(data)) {
                throw new System.Exception("Failed to restore saved data for SeedPlacementTool");
            }

            SeedToolData saveData = JsonUtility.FromJson<SeedToolData>(data);
            CropID = saveData.cropID;
            _quantity = saveData.quantity;
        }

        public override void UseTool(IAgent agent) {
            if (agent.FieldDetectorObject.ValidSelectionPositions.Count < 0) {
                return;
            }

            agent.BlockedInput = true;
            agent.AgentAnimation.PlayAnimation(AnimationType.PickUp);
            OnPerformedAction?.Invoke();
            agent.AgentAnimation.OnAnimationEnd.AddListener(() => {
                foreach (var pos in agent.FieldDetectorObject.ValidSelectionPositions) {
                    if (agent.FieldController.CanPlaceCropsHere(pos)) {
                        agent.FieldController.PlaceCrop(pos, CropID);
                    } else {
                        Debug.Log("Cannot place crop here");
                    }
                }
                _quantity--;
                OnFinishedAction?.Invoke(agent);
                agent.BlockedInput = false;
            });
            agent.FieldController.PrintCropStatus();
        }

        public override bool IsToolStillValid() {
            return _quantity > 0;
        }
    }

    [Serializable]
    public struct SeedToolData {
        public int cropID;
        public int quantity;
    }
}
