using BeamXR.Streaming.Core;
using TMPro;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class RecordButton : BeamButton
    {
        [SerializeField]
        private BeamControlPanel _controlPanel = null;

        [SerializeField]
        private TextMeshProUGUI _buttonText = null;

        private Color _recordingColor;

        private bool _liveInteractable = true, _recordingInteractable = true;

        private RecordingState _oldState;

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
            ColorUtility.TryParseHtmlString(BeamControlPanel.RecordingColorHex, out _recordingColor);
            Bind(ToggleRecording);
        }

        private void OnEnable()
        {
            _unityEvents.OnStreamingStateChanged.AddListener(LiveStatusChanged);
            _unityEvents.OnRecordingStateChanged.AddListener(RecordingStateChanged);
            LiveStatusChanged(_beamManager.StreamingState);
            RecordingStateChanged(_beamManager.RecordingType, _beamManager.RecordingState);
            UpdateStatus();
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
                case StreamingState.Disconnected:
                case StreamingState.Error:
                case StreamingState.Streaming:
                    _liveInteractable = true;
                    break;
                default:
                    _liveInteractable = false;
                    break;
            }
            UpdateStatus();
        }

        private void RecordingStateChanged(CaptureType type, RecordingState state)
        {
            switch (state)
            {
                case RecordingState.Unsupported:
                case RecordingState.Idle:
                case RecordingState.Recording:
                    _recordingInteractable = true;
                    break;
                case RecordingState.Starting:
                case RecordingState.Saving:
                    _recordingInteractable = false;
                    break;
            }

            if (state == RecordingState.Recording)
            {
                _button.SetAlternativeColor(_recordingColor, _recordingColor);
                _buttonText.text = "Stop Record";
            }
            else
            {
                _button.ResetAlternativeColor();
                _buttonText.text = "Record";
            }

            UpdateStatus();
            _oldState = state;
        }

        private void UpdateStatus()
        {
            _button.Selectable.interactable = _liveInteractable && _recordingInteractable;
        }

        private void ToggleRecording()
        {
            if (_beamManager.IsRecording)
            {
                _beamManager.StopRecording();
                
            }
            else
            {
                _beamManager.StartRecording();
                _recordingInteractable = false;
            }
        }
    }
}