using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FarmGame.DataStorage {
    [CreateAssetMenu(fileName = "CropDataSO", menuName = "DataStorage/CropDataSO", order = 0)]
    public class CropDatabaseSO : ScriptableObject {
        [SerializeField] private List<CropData> _cropData = new();

        public CropData GetCropData(int id) {
            return _cropData.Where(crop => crop.ID == id).FirstOrDefault();
        }

    }
}
