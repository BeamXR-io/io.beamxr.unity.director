using BeamXR.Streaming.Core.Media;
using BeamXR.Director.ControlPanel;
using System.Collections;
using UnityEngine;

namespace BeamXR.Director.Camera
{
    public class CameraModelController : BeamComponent
    {
        [SerializeField]
        private BeamControlPanel _controlPanel;

        [SerializeField]
        private BeamCameraController _cameraController;

        [SerializeField]
        private Renderer _streamingStatus, _recordingStatus;

        [SerializeField]
        private Color _streamingColor, _recordingColor;
        private Color _streamingDarkColor, _recordingDarkColor;
        [SerializeField]
        private float _statusRate = 1f;

        private bool _pausedTracking = false, _notFirstPerson = false, _controlPanelOpen = false;

        [SerializeField]
        private string _materialColorName = "_BaseColor";
        private MaterialPropertyBlock _streaming, _recording, _idle;
        private Coroutine _streamingCoroutine, _recordingCoroutine;

        protected override void Awake()
        {
            base.Awake();

            FindParts();

            _cameraController.OnCameraSettingsChanged.AddListener(ChangeViewStatus);
            _controlPanel.OnControlPanelVisible.AddListener(ChangeControlPanelState);

            _idle = new MaterialPropertyBlock();
            _idle.SetColor(_materialColorName, _streamingStatus.material.color);

            _streaming = new MaterialPropertyBlock();
            _streaming.SetColor(_materialColorName, _streamingColor);
            _streamingDarkColor = DarkenColor(_streamingColor, 0.25f);

            _recording = new MaterialPropertyBlock();
            _recording.SetColor(_materialColorName, _recordingColor);
            _recordingDarkColor = DarkenColor(_recordingColor, 0.25f);

            ChangeControlPanelState(_controlPanel.gameObject.activeInHierarchy);
            ChangeViewStatus();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            FindParts();
        }

        private void FindParts()
        {
            if (_controlPanel == null)
            {
                _controlPanel = FindFirstObjectByType<BeamControlPanel>(FindObjectsInactive.Include);
            }
            if (_cameraController == null)
            {
                _cameraController = FindFirstObjectByType<BeamCameraController>(FindObjectsInactive.Include);
            }
        }

        private Color DarkenColor(Color color, float v)
        {
            float h, s;
            Color.RGBToHSV(color, out h, out s, out var origV);
            origV *= v;
            return Color.HSVToRGB(h, s, origV);
        }

        private void OnEnable()
        {
            _unityEvents.OnStreamStarted.AddListener(StartStreaming);
            _unityEvents.OnStreamEnded.AddListener(StopStreaming);
            if (_streamingManager.StreamingState == Streaming.Core.StreamingState.Streaming)
            {
                StartStreaming();
            }
            else
            {
                StopStreaming();
            }

            _unityEvents.OnRecordingStarted.AddListener(StartRecording);
            _unityEvents.OnRecordingEnded.AddListener(StopRecording);
            if (_streamingManager.IsRecording)
            {
                StartRecording();
            }
            else
            {
                StopRecording();
            }
        }

        private void OnDisable()
        {
            _unityEvents.OnStreamStarted.RemoveListener(StartStreaming);
            _unityEvents.OnStreamEnded.RemoveListener(StopStreaming);

            _unityEvents.OnRecordingStarted.RemoveListener(StartRecording);
            _unityEvents.OnRecordingEnded.RemoveListener(StopRecording);
        }

        private void Update()
        {
            if (_streamingCamera.TransformToFollow != transform && !_pausedTracking)
            {
                transform.position = _streamingCamera.transform.position;
            }
            transform.rotation = _streamingCamera.transform.rotation;
        }

        private void ChangeViewStatus()
        {
            _notFirstPerson = _cameraController.CurrentSettings.cameraView != CameraView.FirstPerson;
            ChangeEnabled();
        }

        private void ChangeControlPanelState(bool open)
        {
            _controlPanelOpen = open;
            ChangeEnabled();
        }

        private void ChangeEnabled()
        {
            gameObject.SetActive(_notFirstPerson && _controlPanelOpen);
        }

        private void StartStreaming()
        {
            _streamingCoroutine = StartCoroutine(ShowStreamingStatus());
        }
        private void StopStreaming()
        {
            if (_streamingCoroutine != null)
            {
                StopCoroutine(_streamingCoroutine);
            }
            _streamingStatus.SetPropertyBlock(_idle);
        }

        private IEnumerator ShowStreamingStatus()
        {
            for (; ; )
            {
                _streaming.SetColor(_materialColorName, Color.Lerp(_streamingColor, _streamingDarkColor, (Time.unscaledTime * _statusRate) % 1f));
                _streamingStatus.SetPropertyBlock(_streaming);
                yield return null;
            }
        }

        private void StartRecording()
        {
            _recordingCoroutine = StartCoroutine(ShowRecordingStatus());
        }
        private void StopRecording()
        {
            if (_recordingCoroutine != null)
            {
                StopCoroutine(_recordingCoroutine);
            }
            _recordingStatus.SetPropertyBlock(_idle);
        }

        private IEnumerator ShowRecordingStatus()
        {
            for (; ; )
            {
                _recording.SetColor(_materialColorName, Color.Lerp(_recordingColor, _recordingDarkColor, (Time.unscaledTime * _statusRate) % 1f));
                _recordingStatus.SetPropertyBlock(_recording);
                yield return null;
            }
        }


    }
}
