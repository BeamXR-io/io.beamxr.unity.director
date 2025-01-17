using System.Collections.Generic;
using BeamXR.Streaming.Core.Media;
using UnityEngine;

namespace BeamXR.Director.Camera
{
    public class BeamObjectHider : MonoBehaviour
    {
        public enum HideType
        {
            Visible = 0,
            Hidden = 1,
            HideInFirstPersonOnly = 2
        }

        [SerializeField, HideInInspector]
        private BeamCameraController _cameraController;

        [SerializeField]
        private HideType _hideType = HideType.Hidden;

        [SerializeField, Tooltip("If set to true, the object layer will be changed. If false, the layers the renderers are on will be removed from the streaming camera's culling mask. This may also hide everything else that rests on the layers." +
            " This can be useful when working with mirrors, or when you have objects on the player's face such as glasses.")]
        private bool _changeObjectLayer = true;
        private bool _wasChangingObjectLayer = false, _cameraInFirstPerson = false;

        private Renderer[] _renderers = null;
        private int[] _rendererInts = null;

        private Canvas[] _canvases = null;
        private int[] _canvasInts = null;

        private HashSet<int> _layers = new HashSet<int>();

        private void Awake()
        {
            _wasChangingObjectLayer = _changeObjectLayer;
            _renderers = GetComponentsInChildren<Renderer>(true);
            _rendererInts = new int[_renderers.Length];

            for (int i = 0; i < _rendererInts.Length; i++)
            {
                _rendererInts[i] = _renderers[i].gameObject.layer;
                _layers.Add(_renderers[i].gameObject.layer);
            }

            _canvases = GetComponentsInChildren<Canvas>(true);
            _canvasInts = new int[_canvases.Length];

            for (int i = 0; i < _canvasInts.Length; i++)
            {
                _canvasInts[i] = _canvases[i].gameObject.layer;
                _layers.Add(_canvases[i].gameObject.layer);
            }

            FindParts();
            if(_cameraController != null)
            {
                CameraSettingsChanged();
                _cameraController.OnCameraSettingsChanged.AddListener(CameraSettingsChanged);
            }
        }

        private void OnValidate()
        {
            FindParts();
            if (Application.isPlaying)
            {
                UpdateHide();
            }
        }

        private void FindParts()
        {
            if (_cameraController == null)
            {
                _cameraController = FindFirstObjectByType<BeamCameraController>(FindObjectsInactive.Include);
            }
        }

        private void OnEnable()
        {
            UpdateHide();
        }

        public void ChangeHideFromCamera(HideType hideType)
        {
            _hideType = hideType;
            UpdateHide();
        }

        public void ToggleHideFromCamera()
        {
            _hideType = _hideType == HideType.Visible ? HideType.Hidden : HideType.Visible;
            UpdateHide();
        }

        private void UpdateHide()
        {
            if (BeamStreamingCamera.Instance == null)
            {
                return;
            }

            ChangeObject(_hideType == HideType.Hidden || (_hideType == HideType.HideInFirstPersonOnly && _cameraInFirstPerson));
            _wasChangingObjectLayer = _changeObjectLayer;
        }

        private void ChangeObject(bool hide)
        {
            if (_changeObjectLayer)
            {
                ChangeObjectLayers(hide);
            }
            else
            {
                ChangeCullingMask(hide);
            }
        }

        private void ChangeCullingMask(bool hide)
        {
            if (_wasChangingObjectLayer)
            {
                ChangeObjectLayers(false);
            }

            foreach (var layer in _layers)
            {
                if (hide)
                {
                    // Remove from culling mask
                    BeamStreamingCamera.Instance.Camera.cullingMask &= ~(1 << layer);
                }
                else
                {
                    // Add to culling mask
                    BeamStreamingCamera.Instance.Camera.cullingMask |= (1 << layer);
                }
            }
        }

        private void ChangeObjectLayers(bool hide)
        {
            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].gameObject.layer = hide ? BeamStreamingCamera.Instance.BeamIgnoreLayer : _rendererInts[i];
            }

            for (int i = 0; i < _canvases.Length; i++)
            {
                _canvases[i].gameObject.layer = hide ? BeamStreamingCamera.Instance.BeamIgnoreLayer : _canvasInts[i];
            }
        }

        private void CameraSettingsChanged()
        {
            _cameraInFirstPerson = _cameraController.CurrentSettings.cameraView == CameraView.FirstPerson;
            UpdateHide();
        }
    }
}