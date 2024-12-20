using BeamXR.Streaming.Core.Media;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class CameraHeightControl : BeamPlusMinusElement
    {
        [SerializeField]
        private float _changeAmount = 0.1f;

        protected override void Minus()
        {
            ChangeHeight(-_changeAmount);
        }

        protected override void Plus()
        {
            ChangeHeight(_changeAmount);
        }

        private void ChangeHeight(float increment)
        {
            CameraSettings settings = _cameraController.CurrentSettings;

            settings.cameraHeight = Mathf.Clamp(settings.cameraHeight + increment, -2f, 2f);

            settings.cameraHeight = Mathf.Round(settings.cameraHeight * 10f) / 10f;

            _cameraController.UpdateCameraSettings(settings);
        }

        protected override string UpdateText()
        {
            return "";
        }
    }
}