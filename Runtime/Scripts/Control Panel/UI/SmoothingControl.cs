using BeamXR.Streaming.Core.Media;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class SmoothingControl : BeamPlusMinusElement
    {
        protected override void Minus()
        {
            ChangeSmoothing(-1);
        }

        protected override void Plus()
        {
            ChangeSmoothing(1);
        }

        private void ChangeSmoothing(int increment)
        {
            CameraSettings settings = _streamingCamera.CurrentCameraSettings;
            settings.smoothingAmount = Mathf.Clamp(settings.smoothingAmount + increment, 0, 10);
            settings.smoothingAmount = Mathf.Round(settings.smoothingAmount);
            
            _streamingCamera.UpdateCameraSettings(settings);
        }

        protected override string UpdateText()
        {
            return _streamingCamera.CurrentCameraSettings.smoothingAmount.ToString();
        }
    }
}