using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using UnityEngine;

namespace FarmGame.Tools {
    public abstract class Tool {
        public ToolType ToolType { get; protected set;}

        public RuntimeAnimatorController ToolAnimator { get; set; }

        public Vector2Int ToolRange { get; set; } = Vector2Int.one;
        public int ItemIndex { get; set; }
        protected Tool(int itemID, string data) {
            this.ItemIndex = itemID;
            // RestoreSavedData(data);
        }

        public virtual void RestoreSavedData(string data) {
            
        }

        public virtual string GetDataToSave() => String.Empty;
        public abstract bool IsToolStillValid();    

        public Action OnPerformedAction, OnStartedAction;
        public Action<IAgent> OnFinishedAction;

        public virtual void Equip(IAgent agent) { }
        public virtual void Unequip(IAgent agent) { }

        public abstract void UseTool(IAgent agent);
    }

    public enum ToolType {
        None,
        Hand,
        Hoe,
        SeedPlacer,
        WateringCan,
        Axe,
        Pickaxe,
        Sickle,
        Hammer
    }
}
