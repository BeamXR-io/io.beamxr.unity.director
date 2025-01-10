using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class PortraitControl : BeamCheckboxElement
    {
        protected override void Checkbox(bool value)
        {
            _cameraController.ChangeCameraRotation(value);
        }

        protected override void UpdateSettings()
        {
            base.UpdateSettings();
            _checkbox.SetIsOnWithoutNotify(_cameraController.RotateCameraToPortrait);
        }

        protected override string UpdateText()
        {
            return "";
        }
    }
}