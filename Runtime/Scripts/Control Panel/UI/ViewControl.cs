using BeamXR.Streaming.Core.Media;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class ViewControl : BeamCameraControlElement
    {
        [SerializeField]
        private MenuGroup _menuGroup;

        [SerializeField]
        private ButtonAnimator _firstPerson, _thirdPerson, _selfie;

        protected override void Awake()
        {
            base.Awake();

            _firstPerson.OnClick += () => ViewChanged(CameraView.FirstPerson);
            _thirdPerson.OnClick += () => ViewChanged(CameraView.ThirdPerson);
            _selfie.OnClick += () => ViewChanged(CameraView.Selfie);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            UpdateSettings();
        }


        private void ViewChanged(CameraView view)
        {
            CameraSettings settings = _streamingCamera.CurrentCameraSettings;
            settings.cameraView = view;
            _streamingCamera.UpdateCameraSettings(settings);
        }

        private void OnValidate()
        {
            if(_menuGroup == null)
            {
                _menuGroup = GetComponentInChildren<MenuGroup>(true);
            }
        }

        protected override void UpdateSettings()
        {
            if(_menuGroup != null)
            {
                switch (_streamingCamera.CurrentCameraSettings.cameraView)
                {
                    case CameraView.FirstPerson:
                        _menuGroup.ChangeSelected(_firstPerson);
                        break;
                    case CameraView.ThirdPerson:
                        _menuGroup.ChangeSelected(_thirdPerson);
                        break;
                    case CameraView.Selfie:
                        _menuGroup.ChangeSelected(_selfie);
                        break;
                    case CameraView.ObjectAttached:
                        _menuGroup.ChangeSelected(null);
                        break;
                }
            }
        }
    }
}