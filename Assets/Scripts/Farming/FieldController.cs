using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.DataStorage;
using FarmGame.Interact;
using FarmGame.TimeSystem;
using Unity.VisualScripting;
using UnityEngine;
using static FarmGame.TimeSystem.TimeManager;

namespace FarmGame.Farming {
    public class FieldController : MonoBehaviour {
        private FieldRenderer _fieldRenderer;
        [SerializeField]
        private FieldData _fieldData;
        [SerializeField]
        private CropDatabaseSO _cropDatabase;
        [SerializeField]
        private ItemDatabaseSO _itemDatabase;


        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private AudioClip _prepareFieldSound, _placeCropSound, _wateringFieldSound;

        private TimeManager _timeManager;

        private TimeEventArgs _previousTimeArgs;

        private void Awake() {
            _fieldRenderer = FindObjectOfType<FieldRenderer>(true);
            if (_fieldData == null) {
                _fieldData = FindObjectOfType<FieldData>();
                if (_fieldData == null) {
                    Debug.LogError("FieldData not found", gameObject);
                }
            }

            if (_timeManager = FindObjectOfType<TimeManager>(true)) {
                _timeManager.OnDayProgress += AffectCrops;
            } else {
                Debug.LogError("TimeManager not found", gameObject);
            }
        }

        private void AffectCrops(object sender, TimeEventArgs timeArgs) {
            if (_previousTimeArgs != null && _previousTimeArgs.CurrentDay == timeArgs.CurrentDay) {
                return;
            }
            _previousTimeArgs = timeArgs;
            foreach (var keyValue in _fieldData.crops) {
                Crop crop = keyValue.Value;
                CropData data = _cropDatabase.GetCropData(crop.ID);
                if (data == null) {
                    Debug.LogError($"No data found for id {crop.ID}");
                    continue;
                }
                if (crop.Dead) continue;
                if (((timeArgs.CurrentSeason + 1) & data.GrowthSeasonIndex) != (timeArgs.CurrentSeason + 1)) {
                    if (timeArgs.SeasonChanged) {
                        crop.Dead = true;
                    } else {
                        continue;
                    }
                }
                ModifyCropStatus(crop, data, keyValue.Key);
                if (crop.Regress >= data.WiltThreshold || crop.Dead) {
                    crop.Dead = true;
                    WiltCrop(keyValue.Key);
                }
            }
            PrintCropStatus();
        }

        private void WiltCrop(Vector3Int key) {
            if (_fieldRenderer == null) return;
            Vector3Int cropPosition = _fieldRenderer.GetTilemapTilePosition(key);
            _fieldRenderer.WiltCropVisualization(cropPosition);
        }

        private void ModifyCropStatus(Crop crop, CropData data, Vector3Int position) {
            if (crop.Ready) {
                crop.Regress++;

            } else {
                if (crop.Watered) {
                    crop.Watered = false;
                    if (crop.Regress > 0) {

                        crop.Regress--;
                    } else {
                        crop.Progress++;
                        if (crop.Progress > data.GrowthDelayPerStage) {
                            crop.GrowthLevel++;
                            crop.Progress = 0;
                            UpdateCropAt(position, crop.ID, crop.GrowthLevel);
                            if (crop.GrowthLevel == data.Sprites.Count - 1) {
                                crop.Ready = true;
                                ClearFieldAt(position);
                                if (_fieldRenderer != null) {
                                    PickUpInteraction pickUpInteraction = _fieldRenderer.MakeCropCollectable(position, data, crop.GetQuality(), _itemDatabase);
                                    pickUpInteraction.OnPickUp.AddListener(() => {
                                        RemoveCropAt(position);
                                    });
                                }
                            }
                        }
                    }
                } else {
                    if (crop.GrowthLevel > 0) {
                        crop.Regress++;
                    }
                }
            }
        }

        private void UpdateCropAt(Vector3Int position, int iD, int growthLevel) {
            if (_fieldRenderer == null) return;
            Vector3Int tilePosition = _fieldRenderer.GetTilemapTilePosition(position);
            CropData data = _cropDatabase.GetCropData(iD);
            if (data == null) {
                Debug.LogError($"No data found for id {iD}", gameObject);
                return;
            } else {
                _fieldRenderer.UpdateCropVisualization(tilePosition, data.Sprites[growthLevel], growthLevel > 0);
                if (growthLevel < 1) _audioSource.PlayOneShot(_placeCropSound);
            }
        }

        private void RemoveCropAt(Vector3Int position) {
            _fieldData.crops.Remove(position);
            if (_fieldRenderer != null) {
                _fieldRenderer.RemoveCropAt(position);
            }
        }

        private void ClearFieldAt(Vector3Int position) {
            _fieldData.preparedFields.Remove(position);
            RecreatePreparedFieldPosition();
        }

        private void RecreatePreparedFieldPosition() {
            if (_fieldRenderer == null) return;
            _fieldRenderer.ClearPreparedField();
            foreach (var fieldPosition in _fieldData.preparedFields) {
                bool watered = _fieldData.crops.ContainsKey(fieldPosition) ? _fieldData.crops[fieldPosition].Watered : false;
                _fieldRenderer.PrepareFieldAt(fieldPosition, watered);
            }
        }

        public void PrepareField(Vector3 worldPosition) {
            if (_fieldRenderer == null) return;
            Vector3Int tilePosition = _fieldRenderer.GetTilemapTilePosition(worldPosition);

            if (_fieldData.preparedFields.Contains(tilePosition)) return;

            _fieldRenderer.PrepareFieldAt(tilePosition);
            _audioSource.PlayOneShot(_prepareFieldSound);
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
            if (data == null) {
                Debug.LogError($"No data found for id {cropID}");
                return;
            }
            Debug.Log("Creating visual crop");
            _fieldRenderer.CreateCropVisualization(tilePosition, data.Sprites[growthLevel], growthLevel > 0);
            if (playSound) {
                _audioSource.PlayOneShot(_placeCropSound);
            }

            PrintCropStatus();
        }

        public void PrintCropStatus() {
            _fieldData.PrintCropStatus();
        }

        public bool IsThereCropAt(Vector2 pos) => _fieldData.crops.ContainsKey(_fieldRenderer.GetTilemapTilePosition(pos));

        public void WaterCropAt(Vector2 pos) {
            Vector3Int tilePosition = _fieldRenderer.GetTilemapTilePosition(pos);
            bool result = WaterCropUpdateData(tilePosition);
            if(result == false) return;

            _fieldRenderer.WaterCropAt(tilePosition);
            _audioSource.PlayOneShot(_wateringFieldSound);
        }

        private bool WaterCropUpdateData(Vector3Int tilePosition) {
            if(_fieldData.crops.ContainsKey(tilePosition) == false) {
                return false;
            }
            _fieldData.crops[tilePosition].Watered = true;
            return true;
        }
    }
}
