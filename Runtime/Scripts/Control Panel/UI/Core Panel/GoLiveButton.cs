using BeamXR.Streaming.Core;
using TMPro;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class GoLiveButton : BeamButton
    {
        [SerializeField]
        private BeamControlPanel _controlPanel = null;

        [SerializeField]
        private TextMeshProUGUI _buttonText = null;

        private Color _liveColor;

        private bool _liveInteractable = true, _recordingInteractable = true;

        private StreamingState _oldState;

        protected override void FindParts()
        {
            base.FindParts();
            if (_controlPanel == null)
            {
                _controlPanel = GetComponentInParent<BeamControlPanel>(true);
            }
            if (_buttonText == null)
            {
                _buttonText = GetComponentInChildren<TextMeshProUGUI>(true);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            ColorUtility.TryParseHtmlString(BeamControlPanel.LiveColorHex, out _liveColor);
            Bind(ToggleStreaming);
        }

        private void OnEnable()
        {
            _unityEvents.OnStreamingStateChanged.AddListener(LiveStatusChanged);
            _unityEvents.OnRecordingStateChanged.AddListener(RecordingStateChanged);
            LiveStatusChanged(_beamManager.StreamingState);
            RecordingStateChanged(_beamManager.RecordingType, _beamManager.RecordingState);
        }

        private void OnDisable()
        {
            _unityEvents.OnStreamingStateChanged.RemoveListener(LiveStatusChanged);
            _unityEvents.OnRecordingStateChanged.RemoveListener(RecordingStateChanged);
        }

        private void LiveStatusChanged(StreamingState state)
        {
            switch (state)
            {
                case StreamingState.CreatingSession:
                case StreamingState.Connecting:
                case StreamingState.Disconnecting:
                case StreamingState.Connected:
                    _liveInteractable = false;
                    break;
                default:
                    _liveInteractable = true;
                    break;
            }

            if (state == StreamingState.Streaming)
            {
                _button.SetAlternativeColor(_liveColor, _liveColor);
                _buttonText.text = "Stop Live";
            }
            else
            {
                _button.ResetAlternativeColor();
                _buttonText.text = "Go Live";
            }

            UpdateStatus();
            _oldState = state;
        }

        private void RecordingStateChanged(CaptureType type, RecordingState state)
        {
            if (type == CaptureType.Local)
            {
                switch (state)
                {
                    case RecordingState.Unsupported:
                    case RecordingState.Idle:
                        _recordingInteractable = true;
                        break;
                    case RecordingState.Starting:
                    case RecordingState.Saving:
                    case RecordingState.Recording:
                        _recordingInteractable = false;
                        break;
                }
            }
            else
            {
                _recordingInteractable = true;
            }
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            _button.Selectable.interactable = _liveInteractable && _recordingInteractable;
        }

        private void ToggleStreaming()
        {
            if (_beamManager.AuthState != Streaming.AuthenticationState.Authenticated)
            {
                _controlPanel.OpenUserAccount();
                return;
            }

            _button.Selectable.interactable = false;
            if (_beamManager.IsStreaming)
            {
                _beamManager.StopStreaming();
                _buttonText.text = "Stop Live";
            }
            else
            {
                _beamManager.StartStreaming();
                _buttonText.text = "Go Live";
            }
        }
    }
}