using BeamXR.Streaming.Core.Media;
using UnityEngine;

namespace BeamXR.Director.Camera
{
    public abstract class BeamCameraControllerComponent : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        protected BeamStreamingCamera _streamingCamera = null;

        protected virtual void Awake()
        {
            FindParts();
            if (_streamingCamera != null)
            {
                _streamingCamera.OnCameraSettingsChanged.AddListener(CameraSettingsChanged);
            }
        }

        protected virtual void OnValidate()
        {
            FindParts();
        }

        protected void FindParts()
        {
            if (_streamingCamera == null)
            {
                _streamingCamera = FindFirstObjectByType<BeamStreamingCamera>(FindObjectsInactive.Include);
            }
        }

        protected abstract void CameraSettingsChanged();
    }
}