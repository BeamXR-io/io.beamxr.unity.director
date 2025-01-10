using BeamXR.Streaming.Core.Media;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace BeamXR.Director.ControlPanel
{
    public class CameraPositionControl : BeamCameraControlElement
    {
        [SerializeField]
        private List<Toggle> _toggles = new List<Toggle>();

        private List<int> _degrees = new List<int>();

        [SerializeField]
        private int _degreesPerButton = 45;

        private ColorBlock _normalBlock, _selectedBlock;

        protected override void Awake()
        {
            base.Awake();
            _normalBlock = _toggles[0].colors;
            _selectedBlock = _normalBlock;
            _selectedBlock.normalColor = _normalBlock.selectedColor;

            int deg = 0;
            for (int i = 0; i < _toggles.Count; i++)
            {
                int deg2 = deg;
                _toggles[i].onValueChanged.AddListener((val) => { SetAngle(val, deg2); });
                _degrees.Add(deg2);
                deg += _degreesPerButton;
            }
        }

        protected override void UpdateSettings()
        {
            bool interactable = _cameraController.CurrentSettings.cameraView == CameraView.ThirdPerson;
            int ind = _degrees.IndexOf((int)_cameraController.CurrentSettings.yAngle);
            for (int i = 0; i < _toggles.Count; i++)
            {
                _toggles[i].interactable = interactable;
                _toggles[i].SetIsOnWithoutNotify(i == ind);
                _toggles[i].colors = i == ind ? _selectedBlock : _normalBlock;
            }
        }

        private void SetAngle(bool on, float angle)
        {
            if (on)
            {
                CameraSettings settings = _cameraController.CurrentSettings;
                settings.yAngle = angle;
                _cameraController.UpdateCameraSettings(settings);
            }
        }
    }
}