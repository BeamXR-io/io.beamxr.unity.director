using UnityEditor;
using UnityEngine;
using BeamXR.Director.Camera;
using BeamXR.Streaming.Editor;
using BeamXR.Director.ControlPanel;
using BeamXR.Director.HandMenu;

namespace BeamXR.Director.Editor
{
    public class DirectorPrefabMenu : MonoBehaviour
    {
        // XRI

        [MenuItem("BeamXR/Prefabs/Director/Add Control Panel (XRI)", false, priority = 1)]
        private static void AddControlPanel()
        {
            PrefabAddStreamingPrefab.SpawnPrefab("BeamXR Control Panel - XRI", FindFirstObjectByType<BeamControlPanel>(FindObjectsInactive.Include), PrefabAddStreamingPrefab.GetParentObject().transform, typeof(BeamControlPanel));
        }

        [MenuItem("BeamXR/Prefabs/Director/Add Hand Menu (XRI)", false, priority = 2)]
        private static void AddHandMenu()
        {
            PrefabAddStreamingPrefab.SpawnPrefab("BeamXR Hand Menu - XRI", FindFirstObjectByType<BeamHandMenu>(FindObjectsInactive.Include), PrefabAddStreamingPrefab.GetParentObject().transform, typeof(BeamHandMenu));
        }

        [MenuItem("BeamXR/Prefabs/Director/Add Camera Model", false, priority = 200)]
        private static void AddCameraModel()
        {
            PrefabAddStreamingPrefab.SpawnPrefab("BeamXR Camera Model", FindFirstObjectByType<BeamCameraModel>(FindObjectsInactive.Include), PrefabAddStreamingPrefab.GetParentObject().transform, typeof(BeamCameraModel));
        }

        // Meta

        [MenuItem("BeamXR/Prefabs/Director/Add Control Panel (Meta)", false, priority = 100)]
        private static void AddMetaControlPanel()
        {
            PrefabAddStreamingPrefab.SpawnPrefab("BeamXR Control Panel - Meta", FindFirstObjectByType<BeamControlPanel>(FindObjectsInactive.Include), PrefabAddStreamingPrefab.GetParentObject().transform, typeof(BeamControlPanel));
        }

        [MenuItem("BeamXR/Prefabs/Director/Add Hand Menu (Meta)", false, priority = 101)]
        private static void AddMetaHandMenu()
        {
            PrefabAddStreamingPrefab.SpawnPrefab("BeamXR Hand Menu - Meta", FindFirstObjectByType<BeamHandMenu>(FindObjectsInactive.Include), PrefabAddStreamingPrefab.GetParentObject().transform, typeof(BeamHandMenu));
        }
    }
}