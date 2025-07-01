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
            BeamManager.Instance.MicrophoneVolume = Mathf.Round(Mathf.Clamp(BeamManager.Instance.MicrophoneVolume - _micChangeAmount, _minimumMic, _maximumMic) * 100f) / 100f;
        }

        protected override void Plus()
        {
            BeamManager.Instance.MicrophoneVolume = Mathf.Round(Mathf.Clamp(BeamManager.Instance.MicrophoneVolume + _micChangeAmount, _minimumMic, _maximumMic) * 100f) / 100f;
        }

        protected override void Toggle(bool value)
        {
            BeamManager.Instance.RecordMicrophoneAudio = value;
        }

        protected override void UpdateVisual()
        {
            if (BeamManager.Instance != null)
            {
                UpdateText(Mathf.Round(BeamManager.Instance.MicrophoneVolume * 100f) + "%");
                _enabledToggle.SetToggle(BeamManager.Instance.RecordMicrophoneAudio, true);
            }
        }
    }
}