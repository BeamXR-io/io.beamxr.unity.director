using UnityEngine;
using BeamXR.Streaming.Core;
using BeamXR.Streaming.Core.Media;

namespace BeamXR.Director
{
    public class BeamComponent : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        protected BeamManager _beamManager;

        [SerializeField, HideInInspector]
        protected BeamCamera _beamCamera;

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

        protected virtual void FindParts()
        {
            if (_unityEvents == null)
            {
                _unityEvents = FindFirstObjectByType<BeamUnityEvents>(FindObjectsInactive.Include);
            }

            if (_beamManager == null)
            {
                _beamManager = FindFirstObjectByType<BeamManager>(FindObjectsInactive.Include);
            }

            if (_beamCamera == null)
            {
                _beamCamera = FindFirstObjectByType<BeamCamera>(FindObjectsInactive.Include);
            }
        }
    }
}