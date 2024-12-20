using BeamXR.Streaming.Core.Media;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeamXR.Director.Camera
{
    public class BeamObjectHider : MonoBehaviour
    {
        [SerializeField]
        private bool _hideFromCamera = true;

        private int _originalLayer = -1;

        private Renderer[] _renderers = null;
        private int[] _rendererInts = null;

        private Canvas[] _canvases = null;
        private int[] _canvasInts = null;

        private void Awake()
        {
            _originalLayer = gameObject.layer;
            _renderers = GetComponentsInChildren<Renderer>(true);
            _rendererInts = new int[_renderers.Length];

            for (int i = 0; i < _rendererInts.Length; i++)
            {
                _rendererInts[i] = _renderers[i].gameObject.layer;
            }

            _canvases = GetComponentsInChildren<Canvas>(true);
            _canvasInts = new int[_canvases.Length];

            for (int i = 0; i < _canvasInts.Length; i++)
            {
                _canvasInts[i] = _canvases[i].gameObject.layer;
            }
        }

        private void OnEnable()
        {
            ChangeLayers(_hideFromCamera);
        }

        public void ChangeHideFromCamera(bool hideFromCamera)
        {
            _hideFromCamera = hideFromCamera;
            ChangeLayers(_hideFromCamera);
        }

        public void ToggleHideFromCamera()
        {
            _hideFromCamera = !_hideFromCamera;
            ChangeLayers(_hideFromCamera);
        }

        private void ChangeLayers(bool enable)
        {
            if(BeamStreamingCamera.Instance == null)
            {
                return;
            }

            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].gameObject.layer = enable ? BeamStreamingCamera.Instance.BeamIgnoreLayer : _rendererInts[i];
            }

            for (int i = 0; i < _canvases.Length; i++)
            {
                _canvases[i].gameObject.layer = enable ? BeamStreamingCamera.Instance.BeamIgnoreLayer : _canvasInts[i];
            }
        }
    }
}