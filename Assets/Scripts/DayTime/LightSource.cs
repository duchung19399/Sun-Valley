using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;


namespace FarmGame.DayNight {
    public class LightSource : MonoBehaviour {

        [SerializeField]
        private Light2D _light;
        public void ToggleLight(bool isNightTime) {
            _light.enabled = isNightTime;
        }
    }
}
