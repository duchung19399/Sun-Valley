using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Agent {
    public class PlayerAudio : AgentAudio {
        [SerializeField] private AudioCueSO _footStep;
        public void PlayFootStep() => PlayAudio(_footStep, _audioConfig, transform.position);
    }
}
