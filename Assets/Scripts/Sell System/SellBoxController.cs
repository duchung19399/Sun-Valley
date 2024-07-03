using System;
using FarmGame.DataStorage.Inventory;
using FarmGame.TimeSystem;
using UnityEngine;

namespace FarmGame.SellSystem {
    public class SellBoxController : MonoBehaviour {
        [SerializeField]
        private InputReader _inputReader;
        [SerializeField]
        private GameObject _sellBoxCanvas;
        [SerializeField]
        private PauseTimeControllerSO _pauseTimeControllerSO;
        public void PrepareSellBox(Inventory inventory) {
            _inputReader.EnableUIInput();
            _inputReader.UIExitEvent += ExitUI;
            _sellBoxCanvas.SetActive(true);
            _pauseTimeControllerSO.SetTimePause(true);
        }

        private void ExitUI() {
            _sellBoxCanvas.SetActive(false);
            _pauseTimeControllerSO.SetTimePause(false);
            _inputReader.EnablePlayerInput();
            _inputReader.UIExitEvent -= ExitUI;
        }
    }
}
