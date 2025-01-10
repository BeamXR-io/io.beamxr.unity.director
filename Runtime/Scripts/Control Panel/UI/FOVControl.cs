using BeamXR.Streaming.Core.Media;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class FOVControl : BeamPlusMinusElement
    {
        [SerializeField]
        private int _fovChangeAmount = 5, _minimumFov = 40, _maximumFov = 120;

        protected override string UpdateText()
        {
            return _cameraController.CurrentSettings.fieldOfView.ToString();
        }

        protected override void Minus()
        {
            _cameraController.UpdateCameraSettings(ChangeFOV(-_fovChangeAmount));
        }

        protected override void Plus()
        {
            _cameraController.UpdateCameraSettings(ChangeFOV(_fovChangeAmount));
        }

        private CameraSettings ChangeFOV(int amount)
        {
            CameraSettings settings = _cameraController.CurrentSettings;

            settings.fieldOfView = Mathf.Clamp(settings.fieldOfView + amount, _minimumFov, _maximumFov);

            return settings;
        }
    }
}