using BeamXR.Streaming.Core.Media;
using UnityEngine;

namespace BeamXR.Director.Camera
{
    public class BeamSelfieStick : BeamCameraControllerComponent
    {
        [SerializeField]
        private Transform _spawnPoint = null;

        [SerializeField]
        private Transform _cameraTransform = null;

        protected override void Awake()
        {
            base.Awake();
            gameObject.SetActive(_streamingCamera.CurrentCameraSettings.cameraView == CameraView.ObjectAttached);
        }

        protected override void CameraSettingsChanged()
        {
            bool wasActive = gameObject.activeInHierarchy;
            bool enable = _streamingCamera.CurrentCameraSettings.cameraView == CameraView.ObjectAttached;
            gameObject.SetActive(enable);
            if (wasActive != enable)
            {
                if (enable)
                {
                    _streamingCamera.AddTransformFollow(_cameraTransform, 0);
                    if (_spawnPoint != null)
                    {
                        transform.position = _spawnPoint.position;
                        transform.rotation = _spawnPoint.rotation;
                    }
                }
                else
                {
                    _streamingCamera.RemoveTransformFollow(_cameraTransform);
                }
            }
        }
    }
}