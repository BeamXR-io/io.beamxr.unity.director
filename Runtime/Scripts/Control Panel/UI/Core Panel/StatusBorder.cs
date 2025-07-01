using BeamXR.Streaming.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class StatusBorder : BeamComponent
    {
        [SerializeField]
        private Image _border, _logoBorder;

        private Color _idleColor, _liveColor, _recordingColor;

        protected override void Awake()
        {
            base.Awake();
            _idleColor = _border.color;
            ColorUtility.TryParseHtmlString(BeamControlPanel.LiveColorHex, out _liveColor);
            ColorUtility.TryParseHtmlString(BeamControlPanel.RecordingColorHex, out _recordingColor);
        }

        private void OnEnable()
        {
            if(_unityEvents != null)
            {
                _unityEvents.OnStreamStarted.AddListener(CheckStatus);
                _unityEvents.OnStreamEnded.AddListener(CheckStatus);
                _unityEvents.OnRecordingStarted.AddListener(CheckStatus);
                _unityEvents.OnRecordingEnded.AddListener(CheckStatus);
            }
            CheckStatus();
        }

        private void OnDisable()
        {
            if (_unityEvents != null)
            {
                _unityEvents.OnStreamStarted.RemoveListener(CheckStatus);
                _unityEvents.OnStreamEnded.RemoveListener(CheckStatus);
                _unityEvents.OnRecordingStarted.RemoveListener(CheckStatus);
                _unityEvents.OnRecordingEnded.RemoveListener(CheckStatus);
            }
        }

        private void CheckStatus()
        {
            CheckStatus(CaptureType.None);
        }

        private void CheckStatus(CaptureType type)
        {
            if (_beamManager.IsStreaming)
            {
                if (_beamManager.IsRecording)
                {
                    ApplyColor(_recordingColor);
                }
                else
                {
                    ApplyColor(_liveColor);
                }
            }
            else if (_beamManager.IsLocalRecording)
            {
                ApplyColor(_recordingColor);
            }
            else
            {
                ApplyColor(_idleColor);
            }
        }

        private void ApplyColor(Color color)
        {
            _border.color = color;
            _logoBorder.color = color;
        }
    }
}