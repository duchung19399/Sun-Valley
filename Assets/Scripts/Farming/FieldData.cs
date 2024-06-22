using System.Collections;
using System.Collections.Generic;
using FarmGame.DataStorage;
using UnityEngine;

namespace FarmGame.Farming {
    public class FieldData : MonoBehaviour {
        public Dictionary<Vector3Int, Crop> crops = new();
        public List<Vector3Int> preparedFields = new();
        public HashSet<Vector3Int> debris = new();
        public HashSet<Vector3Int> removedDebris = new();

        public void PrintCropStatus() {
            foreach (var crop in crops) {
                Debug.Log($"Crop at {crop.Key} is {crop.Value}");
            }
        }
    }

    public class Crop {
        public bool Watered { get; set; }
        public int Progress { get; set; }
        public int Regress { get; set; }
        public int ID { get; set; }
        public bool Ready {get; set;}
        public bool Dead {get; set;}
        public int GrowthLevel {get; set;} = 0;

        public Crop(int id) {
            ID = id;
            Watered = false;
            Progress = 0;
            Regress = 0;
            Ready = false;
            Dead = false;
        }

        public override string ToString() {
            return $"Crop: {ID}, Watered: {Watered}, Progress: {Progress}, Regress: {Regress}, Ready: {Ready}, Dead: {Dead}, GrowthLevel: {GrowthLevel}";
        }

        public int GetQuality() {
            return 1;
        }

    }
}
