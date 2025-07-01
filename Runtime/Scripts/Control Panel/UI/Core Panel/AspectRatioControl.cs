using BeamXR.Streaming.Core;
using BeamXR.Streaming.Core.Media;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class AspectRatioControl : ButtonBind
    {
        private BeamUnityEvents _unityEvents;

        [SerializeField]
        private Image _icon;

        private void Awake()
        {
            Bind(ChangeAspectRatio);
            if (_unityEvents == null)
            {
                _unityEvents = FindFirstObjectByType<BeamUnityEvents>(FindObjectsInactive.Include);
            }
        }

        private void OnEnable()
        {
            if(_unityEvents == null)
            {
                _unityEvents = FindFirstObjectByType<BeamUnityEvents>(FindObjectsInactive.Include);
            }
            if (_unityEvents != null)
            {
                _unityEvents.OnAspectRatioChanged.AddListener(OnAspectRatioChanged);
                _unityEvents.OnCapturingStarted.AddListener(CapturingStarted);
                _unityEvents.OnCapturingEnded.AddListener(CapturingEnded);
                OnAspectRatioChanged(BeamManager.Instance.AspectRatio);
            }
        }

        private void OnDisable()
        {
            if (_unityEvents != null)
            {
                _unityEvents.OnAspectRatioChanged.RemoveListener(OnAspectRatioChanged);
            }
        }

        public void ChangeAspectRatio()
        {
            BeamAspectRatio ratio = BeamManager.Instance.AspectRatio == BeamAspectRatio.Landscape ? BeamAspectRatio.Portrait : BeamAspectRatio.Landscape;
            BeamManager.Instance.SetAspectRatio(ratio);
        }

        private void OnAspectRatioChanged(BeamAspectRatio ratio)
        {
            switch (ratio)
            {
                case BeamAspectRatio.Landscape:
                    _icon.transform.localRotation = Quaternion.Euler(0, 0, -45);
                    break;
                case BeamAspectRatio.Portrait:
                    _icon.transform.localRotation = Quaternion.Euler(0, 0, 45);
                    break;
            }
        }

        private void CapturingStarted()
        {
            _button.interactable = false;
        }

        private void CapturingEnded()
        {
            _button.interactable = true;
        }
    }
}