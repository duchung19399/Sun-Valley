using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.Agent;
using FarmGame.Tools;
using FarmGame.UI;
using UnityEngine;
using UnityEngine.Events;

namespace FarmGame.Interact {
    public class SleepInteraction : MonoBehaviour, IInteractable {
        public List<ToolType> UsableTools { get; set; } = new() { ToolType.Hand };

        public UnityEvent OnAfterFinishedSleeping, OnMoveToNextDay;

        [SerializeField]
        private ScreenTransitionEffect _screenTransitionEffect;

        private void Awake() {
            _screenTransitionEffect = FindObjectOfType<ScreenTransitionEffect>(true);
        }

        public bool CanInteract(IAgent agent)
            => UsableTools.Contains(agent.ToolsBag.CurrentTool.ToolType);
        public void Interact(IAgent agent) {
            Debug.Log("Going to sleep");
            StartCoroutine(SleepTransition(agent));
        }

        private IEnumerator SleepTransition(IAgent agent) {
            if(_screenTransitionEffect != null) {
                _screenTransitionEffect.PlayTransition(false);
            }
            agent.BlockedInput = true;
            yield return new WaitForSecondsRealtime(1);
            OnMoveToNextDay?.Invoke();
            if(_screenTransitionEffect != null) {
                _screenTransitionEffect.PlayTransition(true);
            }
            yield return new WaitForSecondsRealtime(1);
            agent.BlockedInput = false;
            OnAfterFinishedSleeping?.Invoke();
            Debug.Log("Woke up!");
        }
    }
}
