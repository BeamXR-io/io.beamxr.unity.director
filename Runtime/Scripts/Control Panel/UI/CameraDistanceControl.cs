using BeamXR.Streaming.Core.Media;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class CameraDistanceControl : BeamPlusMinusElement
    {
        [SerializeField]
        private float _distanceChangeAmount = 0.1f, _minimumDistance = 0.1f, _maximumDistance = 5f;

        protected override void UpdateSettings()
        {
            base.UpdateSettings();
            _minus.interactable = _streamingCamera.CurrentCameraSettings.cameraView != CameraView.FirstPerson
                && _streamingCamera.CurrentCameraSettings.lookType != CameraLookType.Direction
                && _streamingCamera.CurrentCameraSettings.lookType != CameraLookType.Inverted;
            _plus.interactable = _streamingCamera.CurrentCameraSettings.cameraView != CameraView.FirstPerson
                && _streamingCamera.CurrentCameraSettings.lookType != CameraLookType.Direction
                && _streamingCamera.CurrentCameraSettings.lookType != CameraLookType.Inverted;
        }

        protected override string UpdateText()
        {
            return _streamingCamera.CurrentCameraSettings.zDistance.ToString() + "m";
        }

        protected override void Minus()
        {
            _streamingCamera.UpdateCameraSettings(ChangeDistance(-_distanceChangeAmount));
        }

        protected override void Plus()
        {
            _streamingCamera.UpdateCameraSettings(ChangeDistance(_distanceChangeAmount));
        }

        private CameraSettings ChangeDistance(float amount)
        {
            CameraSettings settings = _streamingCamera.CurrentCameraSettings;

            settings.zDistance = Mathf.Clamp(settings.zDistance + amount, _minimumDistance, _maximumDistance);

            settings.zDistance = Mathf.Round(settings.zDistance * 10f) / 10f;

            return settings;
        }
    }
}