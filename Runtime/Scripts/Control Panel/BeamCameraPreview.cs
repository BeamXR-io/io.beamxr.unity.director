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
            _beamCamera.OnRenderTextureChanged.AddListener(RenderTextureChanged);

            _cameraPreviewImage.texture = _beamCamera.RenderTexture;

            _beamCamera?.RequestTemporaryCamera(this);
        }

        private void OnDisable()
        {
            if(_beamCamera != null)
            {
                _beamCamera?.ReleaseTemporaryCamera(this);
            }
            if (_unityEvents != null)
            {
                _beamCamera.OnRenderTextureChanged.RemoveListener(RenderTextureChanged);
            }
        }

        private void RenderTextureChanged(RenderTexture texture)
        {
            _cameraPreviewImage.texture = texture;
        }
    }
}