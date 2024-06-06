using System;
using System.Collections.Generic;
using FarmGame.Tools;
using UnityEngine;

namespace FarmGame.DataStorage {
    [Serializable]
    public class CropData : ISerializationCallbackReceiver {
        public string Name;
        [Min(0)]
        public int ID;
        public int ProducedItemId;
        public List<Sprite> Sprites;
        [Min(1)]
        public int GrowthDelayPerStage;
        [Min(1)]
        public int WiltThreshold;

        [SerializeField]
        private Season _growthSeason;
        [field: SerializeField]
        public int GrowthSeasonIndex { get; private set; }
        [SerializeField]
        private List<ToolType> _collectTools;
        public List<ToolType> GetCollectTools => new List<ToolType>(_collectTools);

        public void OnBeforeSerialize() {
            return;
        }

        public void OnAfterDeserialize() {
            GrowthSeasonIndex = (int)_growthSeason;
        }
    }
}


namespace FarmGame {
    public enum Season {
        Spring,
        Summer,
        Autumn,
        Winter
    }
}