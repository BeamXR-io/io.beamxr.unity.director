using BeamXR.Streaming.Core;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class GameAudioControl : BeamAudioControlElement
    {
        [SerializeField]
        private float _gameAudioChangeAmount = 0.1f, _minimumAudio = 0.1f, _maximumAudio = 1f;

        protected override void Minus()
        {
            BeamStreamingManager.Instance.GameVolume = Mathf.Round(Mathf.Clamp(BeamStreamingManager.Instance.GameVolume - _gameAudioChangeAmount, _minimumAudio, _maximumAudio) * 100f) / 100f;
        }

        protected override void Plus()
        {
            BeamStreamingManager.Instance.GameVolume = Mathf.Round(Mathf.Clamp(BeamStreamingManager.Instance.GameVolume + _gameAudioChangeAmount, _minimumAudio, _maximumAudio) * 100f) / 100f;
        }

        protected override void Toggle(bool value)
        {
            BeamStreamingManager.Instance.RecordGameAudio = value;
        }

        protected override void UpdateVisual()
        {
            if (BeamStreamingManager.Instance != null)
            {
                UpdateText(Mathf.Round(BeamStreamingManager.Instance.GameVolume * 100f) + "%");
                _enabledToggle.SetIsOnWithoutNotify(BeamStreamingManager.Instance.RecordGameAudio);
            }
        }
    }
}