using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.SceneTransitions;
using FarmGame.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace FarmGame.Menu {
    public class MainMenu : MonoBehaviour {
        [SerializeField] private string _inHouseSceneName = "InHouseScene";
        [SerializeField] private ScreenTransitionEffect _screenTransitionEffect;
        [SerializeField] private SceneTransitionManager _sceneTransitionManager;
        [SerializeField] private SaveSystem.SaveManager _saveManager;

        public UnityEvent OnNoSaveData;

        private void Start() {
            if (_saveManager.SaveDataPresent == false) {
                OnNoSaveData?.Invoke();
            }
        }

        public void StartNewGame() {
            _screenTransitionEffect.PlayTransition(false);
            _saveManager.ResetSaveData();
            StartCoroutine(LoadScene(_inHouseSceneName));
        }

        public void LoadSavedData() {
            _screenTransitionEffect.PlayTransition(false);
            _saveManager.SaveGameState();
            StartCoroutine(LoadScene(_sceneTransitionManager.LoadedSceneName));
        }

        private IEnumerator LoadScene(string sceneName) {
            yield return new WaitForSeconds(0.4f);
            _sceneTransitionManager.LoadScene(sceneName);
        }
    }
}
