using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BeamXR.Director.Camera;
using BeamXR.Streaming.Editor;

namespace BeamXR.Director.Editor
{
    public class DirectorPrefabMenu : MonoBehaviour
    {
        [MenuItem("BeamXR/Prefabs/Director/Add Control Panel", priority = 2)]
        private static void AddControlPanel()
        {
            PrefabAddStreamingPrefab.SpawnPrefab("BeamXR Control Panel", FindFirstObjectByType<ControlPanel.BeamControlPanel>(FindObjectsInactive.Include), PrefabAddStreamingPrefab.GetParentObject().transform);
        }

        [MenuItem("BeamXR/Prefabs/Director/Add Hand Menu", priority = 3)]
        private static void AddHandMenu()
        {
            PrefabAddStreamingPrefab.SpawnPrefab("BeamXR Hand Menu", GameObject.Find("BeamXR Hand Menu"), PrefabAddStreamingPrefab.GetParentObject().transform);
        }

        [MenuItem("BeamXR/Prefabs/Director/Add Camera Model", priority = 4)]
        private static void AddCameraModel()
        {
            PrefabAddStreamingPrefab.SpawnPrefab("BeamXR Camera Model", FindFirstObjectByType<CameraModelController>(FindObjectsInactive.Include), PrefabAddStreamingPrefab.GetParentObject().transform);
        }
    }
}