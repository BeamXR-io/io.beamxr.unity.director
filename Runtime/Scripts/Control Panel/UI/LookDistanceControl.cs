using BeamXR.Streaming.Core.Media;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class LookDistanceControl : BeamPlusMinusElement
    {
        [SerializeField]
        private float _distanceChangeAmount = 0.1f, _minimumDistance = 0.1f, _maximumDistance = 5f;

        protected override void UpdateSettings()
        {
            base.UpdateSettings();
            _minus.interactable = _streamingCamera.CurrentCameraSettings.cameraView != CameraView.FirstPerson
                && _streamingCamera.CurrentCameraSettings.lookType == CameraLookType.LookPosition;
            _plus.interactable = _streamingCamera.CurrentCameraSettings.cameraView != CameraView.FirstPerson
                && _streamingCamera.CurrentCameraSettings.lookType == CameraLookType.LookPosition;
        }

        protected override string UpdateText()
        {
            return _streamingCamera.CurrentCameraSettings.zLookDistance.ToString() + "m";
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

            settings.zLookDistance = Mathf.Clamp(settings.zLookDistance + amount, _minimumDistance, _maximumDistance);

            settings.zLookDistance = Mathf.Round(settings.zLookDistance * 10f) / 10f;

            return settings;
        }
    }
}