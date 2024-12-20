using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeamXR.Streaming.Core;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class BeamCameraPreview : BeamComponent
    {
        [SerializeField]
        private RawImage _cameraPreviewImage;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            if(_unityEvents != null)
            {
                _unityEvents.OnStreamStarted.AddListener(StreamStartEnd);
                _unityEvents.OnStreamEnded.AddListener(StreamStartEnd);
            }

            _streamingCamera?.RequestTemporaryCamera(this);
            StreamStartEnd();
        }

        private void OnDisable()
        {
            if(_streamingCamera != null)
            {
                _streamingCamera?.ReleaseTemporaryCamera(this);
            }
            if (_unityEvents != null)
            {
				_unityEvents.OnStreamStarted.RemoveListener(StreamStartEnd);
				_unityEvents.OnStreamEnded.RemoveListener(StreamStartEnd);
			}
        }

        private void StreamStartEnd()
        {
            _cameraPreviewImage.texture = _streamingCamera.Camera.targetTexture;
        }
    }
}