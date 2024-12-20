using BeamXR.Streaming.Core.Media;
using TMPro;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class ViewDropdown : BeamCameraControlElement
    {
        [SerializeField]
        private TMP_Dropdown _dropdown;

        protected override void OnValidate()
        {
            base.OnValidate();
            if (_dropdown == null)
            {
                _dropdown = GetComponentInChildren<TMP_Dropdown>(true);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (_dropdown == null)
            {
                _dropdown = GetComponentInChildren<TMP_Dropdown>(true);
            }

            if(_dropdown != null)
            {
                _dropdown.onValueChanged.AddListener(ViewChanged);
            }
        }

        private void ViewChanged(int index)
        {
            CameraSettings settings = _cameraController.CurrentSettings;
            switch (index)
            {
                case 0:
                    settings.cameraView = CameraView.FirstPerson;
                    break;
                case 1:
                    settings.cameraView = CameraView.ThirdPerson;
                    break;
                case 2:
                    settings.cameraView = CameraView.Static;
                    break;
                case 3:
                    settings.cameraView = CameraView.ObjectAttached;
                    settings.lookType = CameraLookType.Player;
                    break;
            }
            _cameraController.UpdateCameraSettings(settings);
        }

        protected override void UpdateSettings()
        {
            _dropdown.SetValueWithoutNotify((int)_cameraController.CurrentSettings.cameraView);
        }
    }
}