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
        // XRI

        [MenuItem("BeamXR/Prefabs/Director (XRI)/Add Control Panel", priority = 2)]
        private static void AddControlPanel()
        {
            PrefabAddStreamingPrefab.SpawnPrefab("BeamXR Control Panel - XRI", FindFirstObjectByType<ControlPanel.BeamControlPanel>(FindObjectsInactive.Include), PrefabAddStreamingPrefab.GetParentObject().transform);
        }

        [MenuItem("BeamXR/Prefabs/Director (XRI)/Add Hand Menu", priority = 3)]
        private static void AddHandMenu()
        {
            PrefabAddStreamingPrefab.SpawnPrefab("BeamXR Hand Menu - XRI", GameObject.Find("BeamXR Hand Menu - XRI"), PrefabAddStreamingPrefab.GetParentObject().transform);
        }

        [MenuItem("BeamXR/Prefabs/Director (XRI)/Add Camera Model", priority = 4)]
        private static void AddCameraModel()
        {
            PrefabAddStreamingPrefab.SpawnPrefab("BeamXR Camera Model - XRI", FindFirstObjectByType<CameraModelController>(FindObjectsInactive.Include), PrefabAddStreamingPrefab.GetParentObject().transform);
        }

        // Meta

        [MenuItem("BeamXR/Prefabs/Director (Meta)/Add Control Panel", priority = 2)]
        private static void AddMetaControlPanel()
        {
            PrefabAddStreamingPrefab.SpawnPrefab("BeamXR Control Panel - Meta", FindFirstObjectByType<ControlPanel.BeamControlPanel>(FindObjectsInactive.Include), PrefabAddStreamingPrefab.GetParentObject().transform);
        }
    }
}