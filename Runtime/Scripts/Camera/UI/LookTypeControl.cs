using BeamXR.Streaming.Core.Media;
using TMPro;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class LookTypeControl : BeamCameraControlElement
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

            if (_dropdown != null)
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
                    settings.lookType = CameraLookType.Direction;
                    break;
                case 1:
                    settings.lookType = CameraLookType.Player;
                    break;
                case 2:
                    settings.lookType = CameraLookType.LookPosition;
                    break;
            }
            _cameraController.UpdateCameraSettings(settings);
        }

        protected override void UpdateSettings()
        {
            _dropdown.interactable = _cameraController.CurrentSettings.cameraView != CameraView.FirstPerson;
            _dropdown.SetValueWithoutNotify((int)_cameraController.CurrentSettings.lookType);
        }
    }
}