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
            BeamManager.Instance.GameVolume = Mathf.Round(Mathf.Clamp(BeamManager.Instance.GameVolume - _gameAudioChangeAmount, _minimumAudio, _maximumAudio) * 100f) / 100f;
        }

        protected override void Plus()
        {
            BeamManager.Instance.GameVolume = Mathf.Round(Mathf.Clamp(BeamManager.Instance.GameVolume + _gameAudioChangeAmount, _minimumAudio, _maximumAudio) * 100f) / 100f;
        }

        protected override void Toggle(bool value)
        {
            
        }

        protected override void UpdateVisual()
        {
            if (BeamManager.Instance != null)
            {
                UpdateText(Mathf.Round(BeamManager.Instance.GameVolume * 100f) + "%");
            }
        }
    }
}