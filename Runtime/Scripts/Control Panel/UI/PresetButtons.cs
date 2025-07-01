using BeamXR.Streaming.Core.Media;

namespace BeamXR.Director.ControlPanel
{
    public class PresetButtons : BeamCameraControlElement
    {
        protected override void UpdateSettings()
        {

        }

        public void ApplyDefaultPreset()
        {
            _streamingCamera.SetToDefaultCamera();
        }

        public void ApplyPreset(int preset)
        {
            _streamingCamera.LoadPreset(preset);
        }

        public void SavePreset(int preset)
        {
            _streamingCamera.SaveCurrentCamera(preset);
        }

        public void ResetPresets()
        {
            BeamCamera.ResetAllPresets();
        }
    }
}