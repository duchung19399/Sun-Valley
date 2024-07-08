using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.SaveSystem;
using FarmGame.UI;
using UnityEngine;

namespace FarmGame.SceneTransitions {
    public class SceneTransitionSpawner : MonoBehaviour, ISavable {
        private GameObject _player;
        private SceneTransitionTrigger[] _transitionTriggers;

        public int SaveID => SaveIDRepository.SCENE_TRANSITION_SPAWNER;

        private void Awake() {
            Player p;
            if (p = FindAnyObjectByType<Player>()) {
                _player = p.gameObject;
            } else {
                Debug.LogError($"Player not found in the scene.", gameObject);
            }

            _transitionTriggers = FindObjectsOfType<SceneTransitionTrigger>();
        }

        public string GetData() {
            SaveData saveData = new() {TransitionID = -1};
            foreach (SceneTransitionTrigger trigger in _transitionTriggers) {
                if (trigger.Triggered) {
                    saveData = new() {
                        TransitionID = trigger.SceneTriggerID
                    };
                    break;
                }
            }

            return JsonUtility.ToJson(saveData);
        }


        public void RestoreData(string data) {
            if (string.IsNullOrEmpty(data)) return;
            SaveData dataToLoad = JsonUtility.FromJson<SaveData>(data);
            if (dataToLoad.TransitionID >= 0) {
                foreach (var item in _transitionTriggers) {
                    if (item.SceneTriggerID == dataToLoad.TransitionID) {
                        _player.transform.position = item.SpawnPoint.position;
                        ScreenTransitionEffect effect = FindObjectOfType<ScreenTransitionEffect>();
                        if(effect != null) {
                            effect.PlayTransition(true);
                        }
                        return;
                    }
                }
                Debug.LogWarning($"SceneTransitionTrigger with ID {dataToLoad.TransitionID} not found.", gameObject);
            }
        }

        [Serializable]
        public struct SaveData {
            public int TransitionID;

        }
    }
}
