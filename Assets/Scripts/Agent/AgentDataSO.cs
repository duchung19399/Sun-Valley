using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.DataStorage.Inventory;
using UnityEngine;

namespace FarmGame.Agent {
    [CreateAssetMenu(fileName = "AgentData", menuName = "Agent/AgentData")]
    public class AgentDataSO : ScriptableObject {
        [SerializeField]
        private int _money;

        public event Action<AgentDataSO> OnDataUpdated;
        public int Money {
            get { return _money; }
            set {
                _money = value;
                OnDataUpdated?.Invoke(this);
            }
        }

        public Inventory Inventory { get; internal set; }

        public string GetData() {
            AgentSaveData saveData = new() {
                Money = _money,
                inventoryData = Inventory.GetDataToSave()
            };
            return JsonUtility.ToJson(saveData);
        }

        public void SetDefaultData() {
            Money = 0;
        }

        public void RestoreData(string data) {
            AgentSaveData loadedData = JsonUtility.FromJson<AgentSaveData>(data);
            Money = loadedData.Money;
            Inventory.RestoreSaveData(loadedData.inventoryData);
        }


        [Serializable]
        public struct AgentSaveData {
            public int Money;
            public string inventoryData;
        }
    }
}
