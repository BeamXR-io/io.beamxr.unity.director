using UnityEngine;
using BeamXR.Streaming.Core;
using BeamXR.Streaming.Core.Media;

namespace BeamXR.Director
{
    public class BeamComponent : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        protected BeamStreamingManager _streamingManager;

        [SerializeField, HideInInspector]
        protected BeamStreamingCamera _streamingCamera;

        [SerializeField, HideInInspector]
        protected BeamUnityEvents _unityEvents;

        protected virtual void OnValidate()
        {
            FindParts();
        }

        protected virtual void Awake()
        {
            FindParts();
        }

        private void FindParts()
        {
            if (_unityEvents == null)
            {
                _unityEvents = FindFirstObjectByType<BeamUnityEvents>(FindObjectsInactive.Include);
            }

            if (_streamingManager == null)
            {
                _streamingManager = FindFirstObjectByType<BeamStreamingManager>(FindObjectsInactive.Include);
            }

            if (_streamingCamera == null)
            {
                _streamingCamera = FindFirstObjectByType<BeamStreamingCamera>(FindObjectsInactive.Include);
            }
        }
    }
}