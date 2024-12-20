using UnityEngine;
using BeamXR.Streaming.Core;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

namespace BeamXR.Director.ControlPanel
{
    public class BeamStatus : BeamComponent
    {
        [SerializeField]
        private Sprite _idleIcon, _activeIcon;

        [SerializeField]
        private Color _idleColor = Color.white, _progressColor = Color.yellow, _streamingColor = Color.green, _recordingColor = Color.red, _errorColor = Color.red;

        [SerializeField, Space]
        private StatusIcon _statusIcon;

        [SerializeField]
        private TextMeshProUGUI _statusText;

        [SerializeField, Space]
        private StatusIcon _recordingStatusIcon;

        [SerializeField]
        private TextMeshProUGUI _recordingStatusText;

        [SerializeField, Space]
        private TextMeshProUGUI _fpsText;

        private void OnEnable()
        {
            if (_unityEvents != null)
            {
                _unityEvents.OnStreamingStateChanged.AddListener(StreamStateChange);
                _unityEvents.OnRecordingStarted.AddListener(RecordingStarted);
                _unityEvents.OnRecordingEnded.AddListener(RecordingEnded);
            }
            StreamStateChange(_streamingManager.StreamingState);

            if (_streamingManager.IsRecording)
            {
                RecordingStarted();
            }
            else
            {
                RecordingEnded();
            }
        }

        private void OnDisable()
        {
            if (_unityEvents != null)
            {
                _unityEvents.OnStreamingStateChanged.RemoveListener(StreamStateChange);
                _unityEvents.OnRecordingStarted.RemoveListener(RecordingStarted);
                _unityEvents.OnRecordingEnded.RemoveListener(RecordingEnded);
            }
        }

        private void StreamStateChange(StreamingState state)
        {
            switch (state)
            {
                default:
                case StreamingState.Disconnected:
                    _statusIcon.SetColor(_idleColor);
                    _statusIcon.SetSprite(_idleIcon);
                    _statusIcon.SetPulsing(false);
                    break;
                case StreamingState.Streaming:
                    _statusIcon.SetColor(_streamingColor);
                    _statusIcon.SetSprite(_activeIcon);
                    _fpsText.text = BeamStreamingManager.Instance.SessionState.FrameRate.ToString() + " FPS";
                    _statusIcon.SetPulsing(true);
                    break;
                case StreamingState.CreatingSession:
                case StreamingState.Connecting:
                case StreamingState.Connected:
                case StreamingState.Disconnecting:
                    _statusIcon.SetColor(_progressColor);
                    _statusIcon.SetSprite(_activeIcon);
                    _statusIcon.SetPulsing(true);
                    break;
                case StreamingState.Error:
                    _statusIcon.SetColor(_errorColor);
                    _statusIcon.SetSprite(_idleIcon);
                    _statusIcon.SetPulsing(false);
                    break;
            }

            _statusText.text = ParseStreamingState(state);
        }

        private string ParseStreamingState(StreamingState state)
        {
            switch (state)
            {
                case StreamingState.Disconnected:
                    return "Offline";
                case StreamingState.Streaming:
                    return "Live";
                case StreamingState.CreatingSession:
                case StreamingState.Connecting:
                    return "Connecting";
                case StreamingState.Connected:
                    return "Connected";
                case StreamingState.Disconnecting:
                    return "Disconnecting";
                case StreamingState.Error:
                    return "Error";
            }
            return "";
        }

        private void RecordingStarted()
        {
            _recordingStatusIcon.SetColor(_recordingColor);
            _recordingStatusIcon.SetSprite(_activeIcon);
            _recordingStatusIcon.SetPulsing(true);
            _recordingStatusText.text = "Recording";
        }

        private void RecordingEnded()
        {
            _recordingStatusIcon.SetPulsing(false);
            _recordingStatusIcon.SetSprite(_idleIcon);
            _recordingStatusIcon.SetColor(_idleColor);
        }
    }
}