using BeamXR.Streaming.Core.Media;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public abstract class BeamCameraControlElement : MonoBehaviour
    {
        protected BeamCameraController _cameraController;

        protected virtual void Awake()
        {
            FindParts();
        }

        private void FindParts()
        {
            if(_cameraController == null)
            {
                _cameraController = FindFirstObjectByType<BeamCameraController>(FindObjectsInactive.Include);
            }
        }

        protected virtual void OnEnable()
        {
            UpdateSettings();
            if (_cameraController != null)
            {
                _cameraController.OnCameraSettingsChanged.AddListener(UpdateSettings);
            }
        }

        protected virtual void OnDisable()
        {
            if (_cameraController != null)
            {
                _cameraController.OnCameraSettingsChanged.RemoveListener(UpdateSettings);
            }
        }

        protected abstract void UpdateSettings();
    }
}