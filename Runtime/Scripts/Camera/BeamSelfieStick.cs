using BeamXR.Streaming.Core.Media;
using System.Collections;
using System.Collections.Generic;
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
            gameObject.SetActive(_cameraController.CurrentSettings.cameraView == CameraView.ObjectAttached);
        }

        protected override void CameraSettingsChanged()
        {
            bool wasActive = gameObject.activeInHierarchy;
            bool enable = _cameraController.CurrentSettings.cameraView == CameraView.ObjectAttached;
            gameObject.SetActive(enable);
            if (wasActive != enable)
            {
                if (enable)
                {
                    _cameraController.AddFollowTransform(_cameraTransform, 0);
                    if (_spawnPoint != null)
                    {
                        transform.position = _spawnPoint.position;
                        transform.rotation = _spawnPoint.rotation;
                    }
                }
                else
                {
                    _cameraController.RemoveTransform(_cameraTransform);
                }
            }
        }
    }
}