using BeamXR.Streaming.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class BeamCameraPreview : BeamComponent
    {
        [SerializeField]
        private RawImage _cameraPreviewImage;

        private void OnEnable()
        {
            if(_unityEvents != null)
            {
                _unityEvents.OnStreamStarted.AddListener(StreamingView);
                _unityEvents.OnStreamEnded.AddListener(TempView);
            }

            _streamingCamera?.RequestTemporaryCamera(this);
            if(BeamStreamingManager.Instance.StreamingState == StreamingState.Streaming)
            {
                StreamingView();
            }
            else
            {
                TempView();
            }
        }

        private void OnDisable()
        {
            if(_streamingCamera != null)
            {
                _streamingCamera?.ReleaseTemporaryCamera(this);
            }
            if (_unityEvents != null)
            {
				_unityEvents.OnStreamStarted.RemoveListener(StreamingView);
				_unityEvents.OnStreamEnded.RemoveListener(TempView);
			}
        }

        private void StreamingView()
        {
            _cameraPreviewImage.texture = _streamingCamera.RenderTexture;
        }

        private void TempView()
        {
            _cameraPreviewImage.texture = _streamingCamera.TempTexture;
        }
    }
}