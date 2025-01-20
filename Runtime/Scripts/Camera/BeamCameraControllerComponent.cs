using BeamXR.Streaming.Core.Media;
using UnityEngine;

namespace BeamXR.Director.Camera
{
    public abstract class BeamCameraControllerComponent : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        protected BeamCameraController _cameraController = null;

        protected virtual void Awake()
        {
            FindParts();
            if (_cameraController != null)
            {
                _cameraController.OnCameraSettingsChanged.AddListener(CameraSettingsChanged);
            }
        }

        protected virtual void OnValidate()
        {
            FindParts();
        }

        protected void FindParts()
        {
            if (_cameraController == null)
            {
                _cameraController = FindFirstObjectByType<BeamCameraController>(FindObjectsInactive.Include);
            }
        }

        protected abstract void CameraSettingsChanged();
    }
}