using BeamXR.Streaming.Core.Media;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class LookTypeControl : BeamCameraControlElement
    {
        [SerializeField]
        private MenuGroup _menuGroup;

        [SerializeField]
        private ButtonAnimator _direction, _inverted, _player, _lookPosition;

        protected override void Awake()
        {
            base.Awake();

            _direction.OnClick += () => LookTypeChanged(CameraLookType.Direction);
            _inverted.OnClick += () => LookTypeChanged(CameraLookType.Inverted);
            _player.OnClick += () => LookTypeChanged(CameraLookType.Player);
            _lookPosition.OnClick += () => LookTypeChanged(CameraLookType.LookPosition);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateSettings();
        }


        private void LookTypeChanged(CameraLookType look)
        {
            CameraSettings settings = _streamingCamera.CurrentCameraSettings;
            settings.lookType = look;
            _streamingCamera.UpdateCameraSettings(settings);
        }

        private void OnValidate()
        {
            if (_menuGroup == null)
            {
                _menuGroup = GetComponentInChildren<MenuGroup>(true);
            }
        }

        protected override void UpdateSettings()
        {
            if(_streamingCamera.CurrentCameraSettings.cameraView == CameraView.FirstPerson)
            {
                _menuGroup.ChangeSelected(_direction);
                _menuGroup.ToggleInteractable(false);
            }
            else if (_streamingCamera.CurrentCameraSettings.cameraView == CameraView.ThirdPerson)
            {
                _menuGroup.ToggleInteractable(true);
                _inverted.Selectable.interactable = false;
            }
            else
            {
                _menuGroup.ToggleInteractable(true);
            }

            if (_menuGroup != null)
            {
                switch (_streamingCamera.CurrentCameraSettings.lookType)
                {
                    case CameraLookType.Direction:
                        _menuGroup.ChangeSelected(_direction);
                        break;
                    case CameraLookType.Inverted:
                        _menuGroup.ChangeSelected(_inverted);
                        break;
                    case CameraLookType.Player:
                        _menuGroup.ChangeSelected(_player);
                        break;
                    case CameraLookType.LookPosition:
                        _menuGroup.ChangeSelected(_lookPosition);
                        break;
                }
            }
        }
    }
}