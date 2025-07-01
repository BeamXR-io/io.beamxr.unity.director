using System.Collections;
using System.Collections.Generic;
using BeamXR.Streaming.Core.Media;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class BeamCorePanel : BeamComponent
    {
        [SerializeField]
        private float _animationTime = 0.2f;
        [SerializeField, Tooltip("X = canvas width, Y = canvas height, Z = aspect ratio value")]
        private Vector3 _landscapeSize = new Vector3(326, 208, 1.777f), _portraitSize = new Vector3(200, 312, 0.5625f);
        private Vector3 _currentAspectRatioLerp;
        private float _currentLerp = 0;

        [SerializeField]
        private SkinnedMeshRenderer _panelBackground = null;

        [SerializeField]
        private Canvas _coreCanvas = null;
        [SerializeField,HideInInspector]
        private RectTransform _coreRectTransform = null;

        [SerializeField]
        private BeamCameraPreview _cameraPreview = null;

        [SerializeField]
        private AspectRatioFitter _aspectRatioFitter = null;

        private IEnumerator _animation = null;

        private bool _followPlayer = false, _grabbed = false;
        [SerializeField]
        private Transform _selfieTransform;

        [SerializeField]
        private Transform _dockTransform;

        [SerializeField, Tooltip("The local pose of the dock transform when in the respective aspect ratio.")]
        private Pose _landscapeDockPose, _portraitDockPose;

        protected override void FindParts()
        {
            base.FindParts();
            if (_panelBackground == null)
            {
                _panelBackground = GetComponentInChildren<SkinnedMeshRenderer>(true);
            }
            if (_coreCanvas == null)
            {
                _coreCanvas = GetComponentInChildren<Canvas>(true);
            }
            if(_coreRectTransform == null && _coreCanvas != null)
            {
                _coreRectTransform = _coreCanvas.GetComponent<RectTransform>();
            }
            if (_cameraPreview == null)
            {
                _cameraPreview = GetComponentInChildren<BeamCameraPreview>(true);
            }
            if(_cameraPreview != null)
            {
                _aspectRatioFitter = _cameraPreview.GetComponent<AspectRatioFitter>();
            }
        }

        private void OnEnable()
        {
            FindParts();
            ChangeAspectRatio(_beamManager.AspectRatio, true);
            _unityEvents.OnAspectRatioChanged.AddListener(ChangeAspectRatio);
            _beamCamera.OnCameraSettingsChanged.AddListener(CameraSettingsChanged);
            _beamCamera.SetSelfieTransform(_selfieTransform);
            CameraSettingsChanged();
            _grabbed = false;
        }

        private void OnDisable()
        {
            _unityEvents.OnAspectRatioChanged.RemoveListener(ChangeAspectRatio);
            _beamCamera.OnCameraSettingsChanged.RemoveListener(CameraSettingsChanged);
            _beamCamera.SetSelfieTransform(null);
            _grabbed = false;
        }

        private void ChangeAspectRatio(BeamAspectRatio ratio)
        {
            ChangeAspectRatio(ratio, false);
        }

        private void ChangeAspectRatio(BeamAspectRatio ratio, bool immediate = false)
        {
            if (immediate)
            {
                _currentLerp = ratio == BeamAspectRatio.Landscape ? 0 : 1;
                InterpolateControls(_currentLerp);
            }
            else
            {
                if(_animation != null)
                {
                    StopCoroutine(_animation);
                }
                _animation = AnimateAspectRatio(ratio);
                StartCoroutine(_animation);
            }
        }

        private IEnumerator AnimateAspectRatio(BeamAspectRatio ratio)
        {
            int direction = 1, result = 1;
            switch (ratio)
            {
                case BeamAspectRatio.Landscape:
                    direction = -1;
                    result = 0;
                    break;
            }

            while (_currentLerp != result)
            {
                InterpolateControls(_currentLerp);
                _currentLerp += (Time.deltaTime * (1f / _animationTime) * direction);

                if(result == 1)
                {
                    if(_currentLerp >= result)
                    {
                        _currentLerp = result;
                    }
                }
                else
                {
                    if(_currentLerp <= result)
                    {
                        _currentLerp = result;
                    }
                }
                yield return null;
            }
            InterpolateControls(result);

            yield return null;
            _animation = null;
        }

        private void Update()
        {
            if (!_grabbed && _followPlayer)
            {
                transform.forward = -BeamCamera.Instance.transform.forward;
            }
        }

        public void SetGrabbed(bool grabbed)
        {
            _grabbed = grabbed;
        }

        private void InterpolateControls(float amount)
        {
            _currentAspectRatioLerp = Vector3.Lerp(_landscapeSize, _portraitSize, amount);
            _panelBackground.SetBlendShapeWeight(0, amount * 100f);
            _coreRectTransform.sizeDelta = _currentAspectRatioLerp;
            _aspectRatioFitter.aspectRatio = _currentAspectRatioLerp.z;
            _dockTransform.localPosition = Vector3.Lerp(_landscapeDockPose.position, _portraitDockPose.position, amount);
            _dockTransform.localRotation = Quaternion.Euler(Vector3.Lerp(_landscapeDockPose.rotation.eulerAngles, _portraitDockPose.rotation.eulerAngles, amount));
        }

        private void CameraSettingsChanged()
        {
            _followPlayer = (_beamCamera.CurrentCameraSettings.lookType == CameraLookType.Player || _beamCamera.CurrentCameraSettings.lookType == CameraLookType.LookPosition)
                && _beamCamera.CurrentCameraSettings.cameraView == CameraView.Selfie;
        }

#if UNITY_EDITOR
        [ContextMenu("Set Portrait Dock Pose")]
        private void PortraitDockPose()
        {
            _portraitDockPose.position = _dockTransform.localPosition;
            _portraitDockPose.rotation = _dockTransform.localRotation;
        }

        [ContextMenu("Set Landscape Dock Pose")]
        private void LandscapeDockPose()
        {
            _landscapeDockPose.position = _dockTransform.localPosition;
            _landscapeDockPose.rotation = _dockTransform.localRotation;
        }
#endif
    }
}