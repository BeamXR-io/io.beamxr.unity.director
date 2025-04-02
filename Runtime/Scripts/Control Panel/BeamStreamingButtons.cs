using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BeamXR.Streaming.Core;
using System.Collections;

namespace BeamXR.Director.ControlPanel
{
    public class BeamStreamingButtons : BeamComponent
    {
        [SerializeField]
        private Button _streamingButton;

        [SerializeField]
        private TextMeshProUGUI _streamingButtonText;

        [SerializeField]
        private Button _recordingButton;

        [SerializeField]
        private TextMeshProUGUI _recordingButtonText;

        protected override void Awake()
        {
            base.Awake();
            _streamingButton.onClick.AddListener(StreamingClick);
            _recordingButton.onClick.AddListener(RecordingClick);
        }

        private void OnEnable()
        {
            _unityEvents.OnStreamingStateChanged.AddListener(UpdateStreamingButton);
            UpdateStreamingButton(_streamingManager.StreamingState);

            _unityEvents.OnStreamingStateChanged.AddListener(UpdateRecordingButton);
            _unityEvents.OnRecordingStarted.AddListener(StartRecording);
            _unityEvents.OnRecordingEnded.AddListener(EndRecording);
            UpdateRecordingButton(_streamingManager.StreamingState);
        }

        private void OnDisable()
        {
            _unityEvents.OnStreamingStateChanged.RemoveListener(UpdateStreamingButton);

            _unityEvents.OnStreamingStateChanged.RemoveListener(UpdateRecordingButton);
            _unityEvents.OnRecordingStarted.RemoveListener(StartRecording);
            _unityEvents.OnRecordingEnded.RemoveListener(EndRecording);
        }

        private void StreamingClick()
        {
            switch (_streamingManager.StreamingState)
            {
                case StreamingState.Disconnected:
                case StreamingState.Error:
                    _streamingManager.StartStreaming();
                    break;
                case StreamingState.Streaming:
                    _streamingManager.StopStreaming();
                    break;
            }
        }

        private void UpdateStreamingButton(StreamingState state)
        {
            _streamingButton.interactable = state == StreamingState.Disconnected
                || state == StreamingState.Streaming
                || state == StreamingState.Error;

            switch (state)
            {
                case StreamingState.Disconnected:
                case StreamingState.Error:
                    _streamingButtonText.text = "Start Streaming";
                    break;
                case StreamingState.Streaming:
                    _streamingButtonText.text = "Stop Streaming";
                    break;
                case StreamingState.CreatingSession:
                case StreamingState.Connecting:
                case StreamingState.Connected:
                    _streamingButtonText.text = "Starting Stream...";
                    break;
                case StreamingState.Disconnecting:
                    _streamingButtonText.text = "Stopping Stream...";
                    break;
            }
        }

        private void RecordingClick()
        {
            if(_streamingManager.StreamingState == StreamingState.Streaming)
            {
                if(_streamingManager.IsRecording)
                {
                    _streamingManager.StopRecording();
                    _recordingButtonText.text = "Stopping Recording...";
                }
                else
                {
                    _streamingManager.StartRecording();
                    _recordingButtonText.text = "Starting Recording...";
                }
                _recordingButton.interactable = false;
            }
        }

        private void StartRecording()
        {
            UpdateRecordingButton(_streamingManager.StreamingState);
            _recordingButtonText.text = "Stop Recording";
        }

        private void EndRecording()
        {
            UpdateRecordingButton(_streamingManager.StreamingState);
            _recordingButtonText.text = "Start Recording";
        }

        private void UpdateRecordingButton(StreamingState state)
        {
            _recordingButton.interactable = state == StreamingState.Streaming && _streamingManager.SessionState.CanRecord;
            
        }

    }
}