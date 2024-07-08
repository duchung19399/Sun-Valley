using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FarmGame.SceneTransitions;
using UnityEngine;

namespace FarmGame.SaveSystem {
    public class SaveManager : MonoBehaviour {
        [SerializeField]
        private string _saveDataKey = "SaveDataFarm";
        private string _gameSaveFileName = "FarmSavedData";
        [SerializeField]
        private bool _mainMenuFlag = false;
        private List<ObjectSaveData> _unusedData = new();

        public bool SaveDataPresent { get; private set; }

        private void Start() {
            if (_mainMenuFlag) {
                LoadDataFromFile();
                return;
            }

            LoadGameState();
            FindObjectOfType<SceneTransitionManager>().OnBeforeLoadScene += SaveGameState;
        }

        public void SaveDataToFile() {
            string data = GetDataToSave();
            if (WriteToFile(_gameSaveFileName, data)) {
                Debug.Log("Saved data to file");
            } else {
                Debug.LogError("Failed to save data to file");
            }
        }

        private bool WriteToFile(string saveFileName, string data) {
            string fullPath = Path.Combine(Application.persistentDataPath, saveFileName + ".txt");
            Debug.Log($"{fullPath}");

            try {
                File.WriteAllText(fullPath, data);
                return true;
            } catch (System.Exception e) {
                Debug.LogError($"Failed to write to file: {e.Message}");
            }
            return false;
        }

        public void SaveGameState() {
            string data = GetDataToSave();
            PlayerPrefs.SetString(_saveDataKey, data);
        }

        private string GetDataToSave() {
            IEnumerable<ISavable> savableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>();
            List<ObjectSaveData> saveData = new();
            foreach (ISavable obj in savableObjects) {
                saveData.Add(new ObjectSaveData {
                    ID = obj.SaveID,
                    Data = obj.GetData()
                });
            }
            saveData.AddRange(_unusedData);
            return JsonUtility.ToJson(new SaveData { DataList = saveData });
        }

        private void LoadGameState() {
            string dataAsString = PlayerPrefs.GetString(_saveDataKey);
            RestoreData(dataAsString);
        }

        private void RestoreData(string dataAsString) {
            IEnumerable<ISavable> savableObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>();
            SaveData saveData = string.IsNullOrEmpty(dataAsString) ? new SaveData() : JsonUtility.FromJson<SaveData>(dataAsString);
            _unusedData.Clear();

            if (saveData.DataList != null) {
                _unusedData = saveData.DataList
                    .Where(data => savableObjects
                    .Any(savable => savable.SaveID == data.ID) == false)
                    .ToList();
            }

            foreach (ISavable obj in savableObjects) {
                string dataToLoad = null;
                if (saveData.DataList != null && saveData.DataList.Count > 0) {
                    dataToLoad = saveData.DataList.FirstOrDefault(d => d.ID == obj.SaveID).Data;
                }
                obj.RestoreData(dataToLoad);
            }
        }

        private void LoadDataFromFile() {
            ReadFromFile(_gameSaveFileName, out string data);
            RestoreData(data);
        }

        private bool ReadFromFile(string gameSaveFileName, out string data) {
            string fullPath = Path.Combine(Application.persistentDataPath, gameSaveFileName + ".txt");

            data = string.Empty;
            try {
                data = File.ReadAllText(fullPath);
                SaveDataPresent = string.IsNullOrEmpty(data) == false;
                return true;
            } catch (System.Exception e) {
                Debug.LogError($"Failed to read from file: {e.Message}");
            }
            return false;
        }

        public void ResetSaveData() {
            PlayerPrefs.DeleteKey(_saveDataKey);
            WriteToFile(_gameSaveFileName, string.Empty);
            _unusedData.Clear();
            // SaveDataPresent = false;
        }

        [Serializable]
        public struct ObjectSaveData {
            public int ID;
            public string Data;
        }

        [Serializable]
        public struct SaveData {
            public List<ObjectSaveData> DataList;
        }

    }

}
