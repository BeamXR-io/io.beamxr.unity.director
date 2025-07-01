using BeamXR.Streaming.Core;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class BeamToaster : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private BeamUnityEvents _unityEvents;

        [SerializeField]
        private int _maxToasts = 3;
        private int _currentToast = 0;

        [SerializeField]
        private BeamToast _toastPrefab;

        private BeamToast[] _toastPlate = null;

        [SerializeField]
        private float _distanceFromPlayer = 0.5f;

        private StreamingState _oldStreamState;
        private RecordingState _oldRecordState;

        private void Awake()
        {
            if (_toastPrefab == null)
                return;

            GameObject beamToasts = new GameObject("Beam Toasts");
            beamToasts.transform.SetParent(transform);

            _toastPlate = new BeamToast[_maxToasts];
            for (int i = 0; i < _maxToasts; i++)
            {
                BeamToast toast = Instantiate<BeamToast>(_toastPrefab, beamToasts.transform);
                toast.gameObject.SetActive(false);
                _toastPlate[i] = toast;
            }

            if (_unityEvents == null)
            {
                _unityEvents = FindFirstObjectByType<BeamUnityEvents>(FindObjectsInactive.Include);
            }
            if(_unityEvents != null)
            {
                _unityEvents.OnStreamingStateChanged.AddListener(StreamingStateChanged);
                _unityEvents.OnRecordingStateChanged.AddListener(RecordingStateChanged);
            }
        }

        private void OnValidate()
        {
            if(_unityEvents == null)
            {
                _unityEvents = FindFirstObjectByType<BeamUnityEvents>(FindObjectsInactive.Include);
            }
        }

        public void SendToast(string text, float time = 3)
        {
            if (!Application.isPlaying)
                return;

            _toastPlate[_currentToast].SetupToast(text, _distanceFromPlayer, time);
            _currentToast = (_currentToast + 1) % _maxToasts;
        }

        public void ClearAllToasts()
        {
            foreach (var toast in _toastPlate)
            {
                toast.gameObject.SetActive(false);
            }
        }

        private void StreamingStateChanged(StreamingState state)
        {
            if (state == StreamingState.Streaming)
            {
                if (_oldStreamState != StreamingState.Streaming)
                {
                    SendToast("Your stream is live");
                }
            }
            else
            {
                if (_oldStreamState == StreamingState.Streaming)
                {
                    SendToast("Stream stopped");
                }
            }
            _oldStreamState = state;
        }

        private void RecordingStateChanged(CaptureType captureType, RecordingState state)
        {
            if (state == RecordingState.Recording && _oldRecordState == RecordingState.Starting)
            {
                SendToast($"Recording");
            }
            if (_oldRecordState == RecordingState.Saving && state == RecordingState.Idle)
            {
                SendToast($"Recording saved {(captureType == CaptureType.Local ? "to your headset" : "to your stream")}");
            }
            _oldRecordState = state;
        }
    }
}