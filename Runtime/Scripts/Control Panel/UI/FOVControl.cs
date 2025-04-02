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
            return _streamingCamera.CurrentCameraSettings.fieldOfView.ToString();
        }

        protected override void Minus()
        {
            _streamingCamera.UpdateCameraSettings(ChangeFOV(-_fovChangeAmount));
        }

        protected override void Plus()
        {
            _streamingCamera.UpdateCameraSettings(ChangeFOV(_fovChangeAmount));
        }

        private CameraSettings ChangeFOV(int amount)
        {
            CameraSettings settings = _streamingCamera.CurrentCameraSettings;

            settings.fieldOfView = Mathf.Clamp(settings.fieldOfView + amount, _minimumFov, _maximumFov);

            return settings;
        }
    }
}