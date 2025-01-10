using BeamXR.Streaming.Core;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class MicControl : BeamAudioControlElement
    {
        [SerializeField]
        private float _micChangeAmount = 0.1f, _minimumMic = 0.1f, _maximumMic = 2f;

        protected override void Minus()
        {
            BeamStreamingManager.Instance.MicrophoneVolume = Mathf.Round(Mathf.Clamp(BeamStreamingManager.Instance.MicrophoneVolume - _micChangeAmount, _minimumMic, _maximumMic) * 100f) / 100f;
        }

        protected override void Plus()
        {
            BeamStreamingManager.Instance.MicrophoneVolume = Mathf.Round(Mathf.Clamp(BeamStreamingManager.Instance.MicrophoneVolume + _micChangeAmount, _minimumMic, _maximumMic) * 100f) / 100f;
        }

        protected override void Toggle(bool value)
        {
            BeamStreamingManager.Instance.RecordMicrophoneAudio = value;
        }

        protected override void UpdateVisual()
        {
            UpdateText(Mathf.Round(BeamStreamingManager.Instance.MicrophoneVolume * 100f) + "%");
            _enabledToggle.SetIsOnWithoutNotify(BeamStreamingManager.Instance.RecordMicrophoneAudio);
        }
    }
}