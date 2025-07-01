using BeamXR.Streaming.Core.Media;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public abstract class BeamCameraControlElement : MonoBehaviour
    {
        protected BeamCamera _streamingCamera;

        protected virtual void Awake()
        {
            FindParts();
        }

        private void FindParts()
        {
            if(_streamingCamera == null)
            {
                _streamingCamera = FindFirstObjectByType<BeamCamera>(FindObjectsInactive.Include);
            }
        }

        protected virtual void OnEnable()
        {
            UpdateSettings();
            if (_streamingCamera != null)
            {
                _streamingCamera.OnCameraSettingsChanged.AddListener(UpdateSettings);
            }
        }

        protected virtual void OnDisable()
        {
            if (_streamingCamera != null)
            {
                _streamingCamera.OnCameraSettingsChanged.RemoveListener(UpdateSettings);
            }
        }

        protected abstract void UpdateSettings();
    }
}