using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.DataStorage;
using Unity.VisualScripting;
using UnityEngine;

namespace FarmGame.Farming {
    public class FieldController : MonoBehaviour {
        private FieldRenderer _fieldRenderer;
        [SerializeField]
        private FieldData _fieldData;
        [SerializeField]
        private CropDatabaseSO _cropDatabase;


        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip _prepareFieldSound, _placeCropSound;

        private void Awake() {
            _fieldRenderer = FindObjectOfType<FieldRenderer>(true);
            if (_fieldData == null) {
                _fieldData = FindObjectOfType<FieldData>();
                if (_fieldData == null) {
                    Debug.LogError("FieldData not found", gameObject);
                }
            }
        }

        public void PrepareField(Vector3 worldPosition) {
            if (_fieldRenderer == null) return;
            Vector3Int tilePosition = _fieldRenderer.GetTilemapTilePosition(worldPosition);

            if (_fieldData.preparedFields.Contains(tilePosition)) return;

            _fieldRenderer.PrepareFieldAtPosition(tilePosition);
            audioSource.PlayOneShot(_prepareFieldSound);
            _fieldData.preparedFields.Add(tilePosition);
        }

        public bool CanPlaceCropsHere(Vector2 position) {
            Vector3Int tilePosition = _fieldRenderer.GetTilemapTilePosition(position);
            return _fieldData.preparedFields.Contains(tilePosition) && _fieldData.crops.ContainsKey(tilePosition) == false;
        }

        public void PlaceCrop(Vector2 position, int cropID, int growthLevel = 0, bool playSound = true) {
            if (_fieldRenderer == null) return;

            Vector3Int tilePosition = _fieldRenderer.GetTilemapTilePosition(position);
            if (_fieldData.crops.ContainsKey(tilePosition) == false) {
                _fieldData.crops[tilePosition] = new Crop(cropID);
            }
            CropData data = _cropDatabase.GetCropData(cropID);
            if(data == null) {
                Debug.LogError($"No data found for id {cropID}");
                return;
            }
            Debug.Log("Creating visual crop");
            _fieldRenderer.CreateCropVisualization(tilePosition, data.Sprites[growthLevel], growthLevel > 0);
            if(playSound) {
                audioSource.PlayOneShot(_placeCropSound);
            }

            PrintCropStatus();
        }

        public void PrintCropStatus() {
            _fieldData.PrintCropStatus();
        }
    }
}
