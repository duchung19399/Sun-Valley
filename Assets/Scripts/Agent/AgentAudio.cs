using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FarmGame.Agent {
    public class AgentAudio : MonoBehaviour {
        [SerializeField] protected AudioCueEventChannelSO _sfxEventChannel = default;
        [SerializeField] protected AudioConfigurationSO _audioConfig = default;

        protected void PlayAudio(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace = default) {
            _sfxEventChannel.RaisePlayEvent(audioCue, audioConfiguration, positionInSpace);
        }
    }
}
