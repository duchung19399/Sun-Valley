using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.DataStorage;
using FarmGame.DataStorage.Inventory;
using FarmGame.SaveSystem;
using FarmGame.TimeSystem;
using UnityEngine;

namespace FarmGame.SellSystem {
    [RequireComponent(typeof(Inventory))]
    public class SellManager : MonoBehaviour, ISavable {
        [SerializeField, Range(0, 23)]
        private int _sellHour = 12;
        private bool _readyToSell = true;
        [SerializeField]
        private ItemDatabaseSO _itemDatabase;
        [SerializeField]
        private AgentDataSO _agentData;
        private TimeManager _timeManager;
        private Inventory _sellBoxInventory;

        public int SaveID => SaveIDRepository.SELL_MANAGER_ID;

        private void Awake() {
            _sellBoxInventory = GetComponent<Inventory>();
            _timeManager = FindObjectOfType<TimeManager>(true);
            if (_timeManager == null) {
                Debug.LogWarning("TimeManager not found", gameObject);
                return;
            }
            _timeManager.OnClockProgress += TrySellingItems;
            _timeManager.OnDayProgress += ResetSellSystem;
        }

        private void Start() {
        
        }

        private void ResetSellSystem(object sender, TimeManager.TimeEventArgs e) {
            if(e.TheSameDay) return;
            if(_readyToSell) {
                PerformSelling();
            }
            _readyToSell = true;
            Debug.Log("Resetting the SellSytsem");
        }

        private void PerformSelling() {
            _readyToSell = false;
            Debug.Log("Selling Items...");
            foreach(var item in _sellBoxInventory.InventoryContent) {
                if(item == null) continue;
                ItemData description = _itemDatabase.GetItemData(item.id);
                _agentData.Money += description.Price * item.count;
            }

            _sellBoxInventory.Clear();
        }

        private void TrySellingItems(object sender, TimeManager.TimeEventArgs e) {
            if(e.CurrentTime.Hours == _sellHour && _readyToSell) {
                PerformSelling();
            }
        }

        public string GetData() {
            SellSystemSaveData saveData = new() {
                inventoryData = _sellBoxInventory.GetDataToSave(),
                readyToSell = _readyToSell
            };
            return JsonUtility.ToJson(saveData);
        }

        public void RestoreData(string data) {
            if (string.IsNullOrEmpty(data)) return;
            SellSystemSaveData loadedData = JsonUtility.FromJson<SellSystemSaveData>(data);
            _sellBoxInventory.RestoreSaveData(loadedData.inventoryData);
            _readyToSell = loadedData.readyToSell;
        }

        [Serializable]
        public struct SellSystemSaveData {
            public string inventoryData;
            public bool readyToSell;
        }
    }
}
