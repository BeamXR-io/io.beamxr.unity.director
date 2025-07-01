using UnityEngine;

namespace BeamXR.Director.ControlPanel
{
    public class PhotoButton : BeamButton
    {
        [SerializeField]
        private float _countdownTime = 3;

        [SerializeField]
        private PhotoCountdown _countdown = null;

        protected override void Awake()
        {
            base.Awake();
            _unityEvents.OnPhotoCapturedResult.AddListener(CapturedPhoto);
            Bind(CapturePhoto);
        }

        private void CapturePhoto()
        {
            _button.Selectable.interactable = false;
            _beamManager.SavePhoto(_countdownTime);
            _countdown.StartCountdown(_countdownTime);
        }

        private void CapturedPhoto(bool result)
        {
            _button.Selectable.interactable = true;
        }

    }
}