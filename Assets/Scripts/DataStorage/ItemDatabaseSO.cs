using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FarmGame.DataStorage {
    [CreateAssetMenu(fileName = "ItemDatabaseSO", menuName = "DataStorage/ItemDatabaseSO", order = 0)]
    public class ItemDatabaseSO : ScriptableObject {
        [SerializeField] private CropDatabaseSO _cropDatabase;
        [SerializeField] private List<ItemData> _gameItems = new();

        private void Awake() {
            for (int i = 0; i < _gameItems.Count; i++) {
                if (_gameItems[i] != null && _gameItems[i].ID != -1)
                    _gameItems[i].ID = i;
            }
        }

        public ItemData GetItemData(int id) {
            return _gameItems.Where(item => item.ID == id).FirstOrDefault();
        }

        public string GetItemDescription(int id) {
            ItemData item = GetItemData(id);
            if (item == null)
                return null;

            string baseDescription = item.GetDescription();
            if (item.CropTypeIndex > -1) {
                CropData crop = _cropDatabase.GetCropData(item.CropTypeIndex);
                if (crop != null) {
                    baseDescription += $"Season : {crop.GrowthSeasonIndex}\n";
                }
            }
            return baseDescription;
        }
    }
}
