using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.UI;
using UnityEngine;

namespace FarmGame.SceneTransitions {
    public class SceneTransitionTrigger : MonoBehaviour {
        private SceneTransitionManager _sceneManager;
        [SerializeField]
        private string _sceneReferenceName;
        [field: SerializeField]
        public Transform SpawnPoint { get; private set; }
        public bool Triggered { get; private set; }
        [field: SerializeField]
        public int SceneTriggerID { get; private set; }

        private void Awake() {
            _sceneManager = FindObjectOfType<SceneTransitionManager>();
            if (_sceneManager == null) {
                Debug.LogError("SceneTransitionManager not found in the scene", gameObject);
            }

            if (SpawnPoint == null) {
                Debug.LogError("SpawnPoint cant be null", gameObject);
            }
            if (string.IsNullOrEmpty(_sceneReferenceName)) {
                Debug.LogError("SceneReferenceName cant be null or empty", gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                other.gameObject.GetComponentInParent<Player>().BlockedInput = true;
                ScreenTransitionEffect effect = FindAnyObjectByType<ScreenTransitionEffect>();
                effect.OnTransitionFinished += () => {
                    Triggered = true;
                    _sceneManager.LoadScene(_sceneReferenceName);
                };
                effect.PlayTransition(false);
            }
        }
    }
}
