using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.TimeSystem {
    [CreateAssetMenu(fileName = "PauseTimeControllerSO", menuName = "Time System/Pause Time Controller")]
    public class PauseTimeControllerSO : ScriptableObject {
        public void SetTimePause(bool timeFreeze) {
            if (timeFreeze) {
                Debug.Log($"<b><size=15> Time </size></b> paused <color=red> {timeFreeze} </color>");
            } else {
                Debug.Log($"<b><size=15> Time </size></b> paused <color=green> {timeFreeze} </color>");

            }
            Time.timeScale = timeFreeze ? 0 : 1;
        }
    }
}
