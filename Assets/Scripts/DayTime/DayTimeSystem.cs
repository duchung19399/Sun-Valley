using System;
using System.Collections;
using System.Collections.Generic;
using FarmGame.DayNight;
using FarmGame.TimeSystem;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FarmGame {
    public class DayTimeSystem : MonoBehaviour {
        [SerializeField]
        private Light2D _globalLight;

        TimeManager _timeManager;

        [SerializeField]
        private int _sunRiseHour = 6, _sunUpHour = 7, _sunSetHour = 18, _sunDownHour = 19;

        [SerializeField]
        private float _sunMaxIntensity = 1.0f, _sunMinIntensity = 0.05f;
        [SerializeField]
        private TimeOfDay _currentTimeOfDay = TimeOfDay.Day;

        public event Action<bool> OnNightTime;

        private void Awake() {
            _timeManager = FindObjectOfType<TimeManager>(true);
            if(_timeManager != null) {
                _timeManager.OnClockProgress += AffectLight;
            }

            foreach(LightSource light in FindObjectsOfType<LightSource>()) {
                OnNightTime += light.ToggleLight;
            }
        }

        private void AffectLight(object sender, TimeManager.TimeEventArgs e) {
            int currentHour = e.CurrentTime.Hours;
            TimeOfDay tempTimeOfDay = TimeOfDay.Day;
            float valueToSet = 1;
            if(currentHour >= _sunRiseHour && currentHour < _sunUpHour) {
                tempTimeOfDay = TimeOfDay.SunRise;
                valueToSet = Mathf.Clamp(e.CurrentTime.Minutes/60f, _sunMinIntensity, _sunMaxIntensity);
            } else if(currentHour>= _sunUpHour && currentHour < _sunSetHour) {
                tempTimeOfDay = TimeOfDay.Day;
                valueToSet = _sunMaxIntensity;
            } else if(currentHour>= _sunSetHour && currentHour < _sunDownHour) {
                tempTimeOfDay = TimeOfDay.SunSet;
                valueToSet = Mathf.Clamp(1 - e.CurrentTime.Minutes/60f, _sunMinIntensity, _sunMaxIntensity);
            } else {
                tempTimeOfDay = TimeOfDay.Night;
                valueToSet = _sunMinIntensity;
            }

            if(tempTimeOfDay != _currentTimeOfDay || tempTimeOfDay == TimeOfDay.SunRise || tempTimeOfDay == TimeOfDay.SunSet) {
                if(tempTimeOfDay == TimeOfDay.Day) {
                    OnNightTime?.Invoke(false);
                } else if(tempTimeOfDay == TimeOfDay.Night) {
                    OnNightTime?.Invoke(true);
                } 
                _currentTimeOfDay = tempTimeOfDay;
                Debug.Log($"Setting light intensity to {valueToSet} because it is {_currentTimeOfDay} ");
                _globalLight.intensity = valueToSet;
            }
        }
    }

    public enum TimeOfDay {
        Day,
        Night,
        SunSet,
        SunRise
    }
}
