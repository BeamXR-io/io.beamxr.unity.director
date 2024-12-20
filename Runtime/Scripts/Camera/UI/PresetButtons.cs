using BeamXR.Streaming.Core.Media;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class PresetButtons : BeamCameraControlElement
    {
        [SerializeField, HideInInspector]
        private BeamCameraPresets _presets = null;

        protected override void Awake()
        {
            base.Awake();
            FindParts();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            FindParts();
        }

        private void FindParts()
        {
            if(_presets == null)
            {
                _presets = FindFirstObjectByType<BeamCameraPresets>(FindObjectsInactive.Include);
            }
        }

        protected override void UpdateSettings()
        {

        }

        public void ApplyDefaultPreset()
        {
            _cameraController.SetToDefaultCamera();
        }

        public void ApplyPreset(int preset)
        {
            _presets.LoadPreset(preset);
        }

        public void SavePreset(int preset)
        {
            _presets.SaveCurrentCamera(preset);
        }
    }
}